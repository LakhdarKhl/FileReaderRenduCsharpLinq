using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FileReaderRenduCsharpLinq.Services.IRecordWriter;

namespace FileReaderRenduCsharpLinq.Services
{
    public class JsonRecordWriter<T> : IRecordWriter<T>
    {
        public void WriteRecords(IEnumerable<T> records, string path)
        {
            // 1) Sérialisation en JSON indenté
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(records, options);

            // 2) Création du dossier parent si besoin
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // 3) Écriture dans le fichier
            File.WriteAllText(path, json);
        }
    }
}