using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileReaderRenduCsharpLinq.Services
{
    /// <summary>
    /// Lit un fichier JSON contenant un tableau d'objets T et retourne IEnumerable<T>.
    /// </summary>
    public class JsonRecordReader<T> : IRecordReader<T>
    {
        public IEnumerable<T> ReadRecords(string path)
        {
            // 1) Lire tt le texte
            var json = File.ReadAllText(path);

            // 2) Désérialiser en IEnumerable<T>
            //    →  le JSON est un tableau : [ {...}, {...}, ... ]
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var list = JsonSerializer.Deserialize<IEnumerable<T>>(json, options);

            return list ?? new List<T>();
        }
    }
}
