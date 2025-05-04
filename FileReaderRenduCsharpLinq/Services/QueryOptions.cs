using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderRenduCsharpLinq.Services
{
    /// <summary>
    /// Critère de filtrage : on compare la propriété PropertyName via Operator à la Value
    /// </summary>
    public class FilterOption
    {
        public string PropertyName { get; set; }   // ex "Age"
        public string Operator { get; set; }   // ">",    "<",    "==",    "Contains"
        public object Value { get; set; }   // ex 30, "Paris"
    }

    /// <summary>
    /// Critère de tri : on trie par PropertyName, Asc ou Desc
    /// </summary>
    public class SortOption
    {
        public string PropertyName { get; set; }   // ex "Name"
        public bool Descending { get; set; }   // true = ordre décroissant
    }

    /// <summary>
    /// Critère d groupage : on regroupe par PropertyName
    /// </summary>
    public class GroupOption
    {
        public string PropertyName { get; set; }   // ex "city"
    }

    /// <summary>
    /// Regroupe tous les critères pr une requête
    /// </summary>
    public class QueryOptions<T>
    {
        public List<FilterOption> Filters { get; set; } = new();
        public List<SortOption> Sorts { get; set; } = new();
        public GroupOption Group { get; set; }
    }
}

