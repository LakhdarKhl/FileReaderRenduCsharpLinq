using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static FileReaderRenduCsharpLinq.Services.IDataProcessor;

namespace FileReaderRenduCsharpLinq.Services
{
    public class LinqDataProcessor<T> : IDataProcessor<T>
    {
        public IEnumerable<T> ApplyFilters(IEnumerable<T> source, IEnumerable<FilterOption> filters)
        {
            var query = source;
            foreach (var f in filters)
            {
                var prop = typeof(T)
                    .GetProperty(f.PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                query = query.Where(item =>
                {
                    var val = prop.GetValue(item);
                    // on convertit la valeur de comparaison dans le bon type
                    object cmpValue = Convert.ChangeType(f.Value, prop.PropertyType);

                    return f.Operator switch
                    {
                        ">" when val is IComparable c => c.CompareTo(cmpValue) > 0,
                        "<" when val is IComparable c => c.CompareTo(cmpValue) < 0,
                        "==" => Equals(val, cmpValue),
                        "Contains" => val?.ToString()?.Contains(f.Value.ToString(), StringComparison.OrdinalIgnoreCase) == true,
                        _ => true
                    };
                });
            }
            return query;
        }

        public IEnumerable<T> ApplySort(IEnumerable<T> source, IEnumerable<SortOption> sorts)
        {
            IOrderedEnumerable<T> ordered = null;
            bool first = true;

            foreach (var s in sorts)
            {
                var prop = typeof(T)
                    .GetProperty(s.PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                if (first)
                {
                    ordered = s.Descending
                        ? source.OrderByDescending(item => prop.GetValue(item))
                        : source.OrderBy(item => prop.GetValue(item));
                    first = false;
                }
                else
                {
                    ordered = s.Descending
                        ? ordered.ThenByDescending(item => prop.GetValue(item))
                        : ordered.ThenBy(item => prop.GetValue(item));
                }
            }

            return ordered ?? source;
        }

        public IEnumerable<IGrouping<object, T>> ApplyGroup(IEnumerable<T> source, GroupOption group)
        {
            if (group == null || string.IsNullOrWhiteSpace(group.PropertyName))
                return Enumerable.Empty<IGrouping<object, T>>();

            var prop = typeof(T)
                .GetProperty(group.PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                return Enumerable.Empty<IGrouping<object, T>>();

            return source.GroupBy(item => prop.GetValue(item));
        }
    }
}
