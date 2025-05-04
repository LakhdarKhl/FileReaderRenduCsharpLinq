using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FileReaderRenduCsharpLinq.Services
{
    public class ConsoleUi<T>
    {
        /// <summary>
        /// Demande à l'utilisateur ses critères de filtre, tri et groupage.
        /// </summary>
        public QueryOptions<T> AskQueryOptions()
        {
            var options = new QueryOptions<T>();

            // --- filtres ---
            Console.Write("Voulez-vous filtrer les résultats ? (o/n) : ");
            if (Console.ReadLine()?.Trim().ToLower() == "o")
            {
                while (true)
                {
                    Console.Write("Propriété à filtrer (ex. Age) : ");
                    var prop = Console.ReadLine()?.Trim();
                    Console.Write("Opérateur (>, <, ==, Contains) : ");
                    var op = Console.ReadLine()?.Trim();
                    Console.Write("Valeur comparée : ");
                    var val = Console.ReadLine()?.Trim();

                    options.Filters.Add(new FilterOption
                    {
                        PropertyName = prop,
                        Operator = op,
                        Value = val
                    });

                    Console.Write("Ajouter un autre filtre ? (o/n) : ");
                    if (Console.ReadLine()?.Trim().ToLower() != "o")
                        break;
                }
            }

            // --- tri ---
            Console.Write("Voulez-vous trier les résultats ? (o/n) : ");
            if (Console.ReadLine()?.Trim().ToLower() == "o")
            {
                while (true)
                {
                    Console.Write("Propriété à trier (ex. Name) : ");
                    var prop = Console.ReadLine()?.Trim();
                    Console.Write("Ordre décroissant ? (o/n) : ");
                    var desc = Console.ReadLine()?.Trim().ToLower() == "o";

                    options.Sorts.Add(new SortOption
                    {
                        PropertyName = prop,
                        Descending = desc
                    });

                    Console.Write("Ajouter un autre critère de tri ? (o/n) : ");
                    if (Console.ReadLine()?.Trim().ToLower() != "o")
                        break;
                }
            }

            // --- groupage ---
            Console.Write("Voulez-vous grouper les résultats ? (o/n) : ");
            if (Console.ReadLine()?.Trim().ToLower() == "o")
            {
                Console.Write("Propriété de groupage (ex. City) : ");
                var prop = Console.ReadLine()?.Trim();
                options.Group = new GroupOption { PropertyName = prop };
            }

            return options;
        }

        /// <summary>
        /// Affiche un aperçu (possiblement groupé → maj dans une autre finalement) dans la console
        /// </summary>
        //public void ShowPreview(IEnumerable<T> data, GroupOption group)
        //{
        //    if (group != null && !string.IsNullOrWhiteSpace(group.PropertyName))
        //    {
        //        //  si group != null
        //        foreach (dynamic grp in data)
        //        {
        //            Console.WriteLine($"==> {group.PropertyName} = {grp.Key}");
        //            foreach (T item in grp)
        //                Console.WriteLine("   " + FormatItem(item));
        //        }
        //    }
        //    else
        //    {
        //        foreach (var item in data)
        //            Console.WriteLine("   " + FormatItem(item));
        //    }
        //}

        /// <summary>
        /// Demande à l'utilisateur quels champs il veut exporte
        /// </summary>
        public IEnumerable<string> AskFieldsToExport()
        {
            // lister les propriétés de T
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();

            Console.WriteLine("Champs disponibles : " + string.Join(", ", props));
            Console.Write("Sélectionnez les champs à exporter (séparés par des virgules) : ");
            var line = Console.ReadLine() ?? "";
            return line
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => props.Contains(s, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Convertit un objet T en ligne texte pour affichage console
        /// </summary>
        private string FormatItem(T item)
        {
            var values = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => $"{p.Name}={p.GetValue(item)}");
            return string.Join(" | ", values);
        }

        /// <summary>
        /// Affichage simple (liste plate)
        /// </summary>
        public void ShowPreview(IEnumerable<T> data)
        {
            foreach (var item in data)
                Console.WriteLine("   " + FormatItem(item));
        }

        /// <summary>
        /// Affichage groupé (IGrouping)
        /// </summary>
        public void ShowPreview(IEnumerable<IGrouping<object, T>> groupedData, GroupOption group)
        {
            foreach (var grp in groupedData)
            {
                Console.WriteLine($"==> {group.PropertyName} = {grp.Key}");
                foreach (var item in grp)
                    Console.WriteLine("   " + FormatItem(item));
            }
        }
    }
}
