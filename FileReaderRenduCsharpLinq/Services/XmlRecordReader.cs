using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileReaderRenduCsharpLinq.Services
{
    /// <summary>
    /// Lit un fichier XML contenant une collection d'éléments T (wrapper List<T>).
    /// </summary>
    public class XmlRecordReader<T> : IRecordReader<T>
    {
        public IEnumerable<T> ReadRecords(string path)
        {
            using var stream = File.OpenRead(path);
            var serializer = new XmlSerializer(typeof(List<T>));
            if (serializer.Deserialize(stream) is List<T> list)
                return list;
            return new List<T>();
        }
    }
}
