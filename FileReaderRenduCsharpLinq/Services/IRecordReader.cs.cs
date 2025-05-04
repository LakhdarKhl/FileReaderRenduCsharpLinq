using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderRenduCsharpLinq.Services
{
    interface IRecordReader
    {
        /// <summary>
        /// Lit un fichier en entrée et retourne une séquence d'objet T
        /// </summary>
        /// <typeparam name="T">Type du modèle (User, Car…)</typeparam>
    }
    public interface IRecordReader<T>
    {
        /// Lit Toutes les lignes du fichier et les convertit en instances T
        /// <param name="path">Chemin du fichier à lire.</param>
        /// /// <returns>Énumérable d'objets T.</returns>
        IEnumerable<T> ReadRecords(string path);
    }
}
