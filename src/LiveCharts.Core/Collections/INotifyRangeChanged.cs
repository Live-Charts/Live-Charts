using System.Collections.Generic;
using System.Collections.Specialized;

namespace LiveCharts.Core.Collections
{
    /// <summary>
    /// An extended <see cref="INotifyCollectionChanged"/> that implements AddRange and RemoveRange methods.
    /// </summary>
    public interface INotifyRangeChanged<in T> : INotifyCollectionChanged
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        void RemoveRange(IEnumerable<T> items);
    }
}