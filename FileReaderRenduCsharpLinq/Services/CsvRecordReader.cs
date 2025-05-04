using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FileReaderRenduCsharpLinq.Services;

namespace FileReaderRenduCsharpLinq.Services
{
    class CsvRecordReader
    {
        public class csvRecordReader<T> : IRecordReader<T> where T : new()
        {
            public IEnumerable<T> ReadRecords (string path)
            {
                // 1- Lire toutes les lignes du fichier
                var lines = File.ReadLines(path).ToList();
                if (lines.Count < 2)
                    yield break; //bah ya rien

                // 2) Récup les en-têtes
                var headers = lines[0].Split(',');

                // 3) Préparer la liste des prop du type T
                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Construire le mapping 
                
                var map = headers
                              .Select((h, i) => new { Name = h.Trim(), Index = i }) // On énumère chaque entête avec son indice I
                              .Where(x => props.Any(p => p.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))) // On ne garde que ceux qui correspondent au nom d'une propté de T
                              .ToDictionary(
                                            x => x.Index,
                                            x => props.First(p => p.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)));// On construit un dictionnaire où la clé est l’indice de la colonne CSV et la valeur est le PropertyInfo correspondant.


                // 4) Parcourir chaque ligne de données
                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = line.Split(',');
                    var obj = new T();

                    // Pour chaque colonne : retrouver la propInfo, convertir value ver s type de prop et affecter prop
                    foreach (var kv in map)
                    {
                        int colIndex = kv.Key;
                        var propInfo = kv.Value;
                        string rawValue = values[colIndex];

                        // 1) Tenter la conversion de chaîne vers le type de la propriété
                        object converted;
                        try
                        {
                            converted = Convert.ChangeType(rawValue, propInfo.PropertyType); // Convert.chngeType gère les conversions simples (string → int, double..)
                        }
                        catch
                        {
                            // Si la conversion échoue, on met la valeur par défaut du type
                            converted = propInfo.PropertyType.IsValueType
                                ? Activator.CreateInstance(propInfo.PropertyType)
                                : null;
                        }

                        // 2) Affecter à l’objet
                        propInfo.SetValue(obj, converted);

                        yield return obj;
                    }

                }
            }
        }
    }
}
