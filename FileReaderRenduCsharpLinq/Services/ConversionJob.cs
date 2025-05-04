using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static FileReaderRenduCsharpLinq.Services.IDataProcessor;
using static FileReaderRenduCsharpLinq.Services.IRecordWriter;

namespace FileReaderRenduCsharpLinq.Services
{
    /// <summary>
    /// Orchestrateur générique :
    /// lit des T In, applique filtres/tri/groupage,
    /// transforme chaque item en T Out, puis écrit.
    /// </summary>
    public class ConversionJob<TIn, TOut>
    {
        readonly IRecordReader<TIn> _reader;
        readonly IDataProcessor<TIn> _processor;
        readonly ConsoleUi<TIn> _ui;
        readonly Func<TIn, IEnumerable<string>, TOut> _mapper;
        readonly IRecordWriter<TOut> _writer;

        public ConversionJob(
                        IRecordReader<TIn> reader,
                        IDataProcessor<TIn> processor,
                        ConsoleUi<TIn> ui,
                        Func<TIn, IEnumerable<string>, TOut> mapper,
                        IRecordWriter<TOut> writer)
        {
            _reader = reader;
            _processor = processor;
            _ui = ui;
            _mapper = mapper;
            _writer = writer;
        }

        public void Run()
        {
            // 1) Chemin du fichier source
            Console.Write("Chemin du fichier source : ");
            var sourcePath = Console.ReadLine();

            // 2) Lecture
            var items = _reader.ReadRecords(sourcePath).ToList();

            // 3) Saisie des critères
            var options = _ui.AskQueryOptions();

            // 4) Filtre + tri
            var afterFilter = _processor.ApplyFilters(items, options.Filters);
            var afterSort = _processor.ApplySort(afterFilter, options.Sorts);

            // 5) Aperçu groupé ou non
            if (options.Group != null)
                _ui.ShowPreview(_processor.ApplyGroup(afterSort, options.Group), options.Group);
            else
                _ui.ShowPreview(afterSort);

            // 6) Choix des champs
            var fieldsToExport = _ui.AskFieldsToExport();

            // 7) Projection en T Out
            var outputItems = afterSort
                .Select(item => _mapper(item, fieldsToExport))
                .ToList();

            // 8) Chemin du fichier de sortie (fichier ou dossier)
            Console.Write("Chemin du dossier ou du fichier de sortie : ");
            var input = Console.ReadLine()?.Trim();
            string destPath;

            // Si l’entrée est un dossier (ou qu’il n’y a pas d’extension), on génère un nom de fichier par défaut
            if (Directory.Exists(input) || !Path.HasExtension(input))
            {
                // Nom par défaut selon writer (ici on part sur "export.ext")
                // On peut déduire l’extension de T Out : 
                //   .json si JsonRecordWriter, .csv si CsvRecordWriter  
                // Pour simplifier .json quand TOut est Dictionary<,>, sinon .csv
                var ext = typeof(TOut) == typeof(Dictionary<string, object>) ? ".json" : ".csv";
                destPath = Path.Combine(input, "export" + ext);
                Console.WriteLine($"→ Vous avez choisi un dossier, j'écris : {destPath}");
            }
            else
            {
                // Sinon c’est un chemin de fichier complet
                destPath = input;
            }

            // Créer le dossier parent si besoin
            var parent = Path.GetDirectoryName(destPath);
            if (!string.IsNullOrEmpty(parent) && !Directory.Exists(parent))
                Directory.CreateDirectory(parent);

            // 9) Écriture
            _writer.WriteRecords(outputItems, destPath);
            Console.WriteLine($"✅ Conversion terminée : {destPath}");


            Console.WriteLine("✅ Conversion terminée !");
        }
    }
}
