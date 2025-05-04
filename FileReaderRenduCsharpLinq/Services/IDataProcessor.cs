using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderRenduCsharpLinq.Services
{
    public interface IDataProcessor
    {
        /// <summary>
        /// Applique filtrage tri et grouapge sur une collection de mémoire.
        /// </summary>

        public interface IDataProcessor<T>
        {
            IEnumerable<T> ApplyFilters(IEnumerable<T> source, IEnumerable<FilterOption> filters);
            IEnumerable<T> ApplySort(IEnumerable<T> source, IEnumerable<SortOption> sorts);
            IEnumerable<IGrouping<object, T>> ApplyGroup(IEnumerable<T> source, GroupOption group);
        }
    }

}
