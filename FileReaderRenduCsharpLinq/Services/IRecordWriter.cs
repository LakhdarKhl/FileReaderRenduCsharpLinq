using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderRenduCsharpLinq.Services
{
    public interface IRecordWriter
    {
        /// <summary>
        /// Sérialiste une srie d'objet T dans un fichier de sortie
        /// </summary>
        /// <typeparam name="T">Type du modèle (User, Car…)</typeparam>
        /// 
        public interface IRecordWriter<T>
        {
            /// <summary>
            /// Écrit tous les enregistrements dans le fichier spécifié
            /// </summary>
            /// /// <param name="records">Les objets à sérialiser.</param>
            /// <param name="path">Chemin du fichier de sortie.</param>
            void WriteRecords(IEnumerable<T> records, string path);
        }
    }
}
