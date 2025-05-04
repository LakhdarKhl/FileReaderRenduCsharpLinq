using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static FileReaderRenduCsharpLinq.Services.IRecordWriter;

namespace FileReaderRenduCsharpLinq.Services
{
    /// <summary>
    /// Écrit une collection d'objets T dans un fichier CSV.
    /// </summary>
    public class CsvRecordWriter<T> : IRecordWriter<T>
    {
        public void WriteRecords(IEnumerable<T> records, string path)
        {
            var sb = new StringBuilder();
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 1) Ligne d'en-tête
            var header = string.Join(",", props.Select(p => p.Name));
            sb.AppendLine(header);

            // 2) Chaque ligne de données
            foreach (var item in records)
            {
                var values = props.Select(p =>
                {
                    var raw = p.GetValue(item)?.ToString() ?? "";
                    // Échapper les virgules et guillemets
                    if (raw.Contains(',') || raw.Contains('"'))
                    {
                        raw = "\"" + raw.Replace("\"", "\"\"") + "\"";
                    }
                    return raw;
                });
                sb.AppendLine(string.Join(",", values));
            }

            // 3) Écrire dans le fichier, en créant le dossier parent si besoin
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            File.WriteAllText(path, sb.ToString());
        }
    }
}
