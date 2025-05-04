using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FileReaderRenduCsharpLinq.Models;
using FileReaderRenduCsharpLinq.Services;
using static FileReaderRenduCsharpLinq.Services.CsvRecordReader;

namespace FileReaderRenduCsharpLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) Instanciation des services génériques pour User
            var processor = new LinqDataProcessor<User>();
            var ui = new ConsoleUi<User>();

            // 2) Menu de choix de conversion
            Console.WriteLine("1) CSV → JSON");
            Console.WriteLine("2) JSON → CSV");
            Console.WriteLine("3) XML → JSON");
            Console.Write("Votre choix (1, 2 ou 3) : ");
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    // CSV → JSON ( projette vers Dictionary<string, object>)
                    var csvToJsonJob = new ConversionJob<User, Dictionary<string, object>>(
                        reader: new csvRecordReader<User>(),
                        processor: processor,
                        ui: ui,
                        mapper: (user, fields) => fields.ToDictionary(name => name,name => typeof(User).GetProperty(name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                                    .GetValue(user)),
                        writer: new JsonRecordWriter<Dictionary<string, object>>()
                    );
                    Console.WriteLine("\n=== Conversion CSV → JSON ===");
                    csvToJsonJob.Run();
                    break;

                case "2":
                    // JSON → CSV (on garde tout l'objet User)
                    var jsonToCsvJob = new ConversionJob<User, User>(
                        reader: new JsonRecordReader<User>(),
                        processor: processor,
                        ui: ui,
                        mapper: (user, fields) => user,
                        writer: new CsvRecordWriter<User>()
                    );
                    Console.WriteLine("\n=== Conversion JSON → CSV ===");
                    jsonToCsvJob.Run();
                    break;

                case "3":
                    // XML → JSON (idem CSV→JSON, projette en dictionnaires filtrés)
                    var xmlToJsonJob = new ConversionJob<User, Dictionary<string, object>>(
                        reader: new XmlRecordReader<User>(),
                        processor: processor,
                        ui: ui,
                        mapper: (user, fields) => fields.ToDictionary(
                                        name => name,
                                        name => typeof(User)
                                                    .GetProperty(name,
                                                        BindingFlags.Public
                                                      | BindingFlags.Instance
                                                      | BindingFlags.IgnoreCase)
                                                    .GetValue(user)
                                    ),
                        writer: new JsonRecordWriter<Dictionary<string, object>>()
                    );
                    Console.WriteLine("\n=== Conversion XML → JSON ===");
                    xmlToJsonJob.Run();
                    break;


                default:
                    Console.WriteLine("Choix invalide, opération terminée.");
                    break;
            }
        }
    }
}
