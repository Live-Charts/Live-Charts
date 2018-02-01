using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LiveCharts.Core.Collections
{
    /// <summary>
    /// An extended observable collection that implements AddRange, RemoveRange methods optimized for LiveCharts.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    public class PlotableCollection<T> : ObservableCollection<T>, INotifyRangeChanged<T>
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        public PlotableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public PlotableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotableCollection{T}"/> class.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public PlotableCollection(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Adds a range of items, then notifies only once that the collection changed.
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            CheckReentrancy();

            foreach (var item in items)
            {
                Items.Add(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnPropertyChanged(new PropertyChangedEventArgs(CountString));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        /// <summary>
        /// Removes a range of items, then notifies only once that the collection changed.
        /// </summary>
        /// <param name="items">The items.</param>
        public void RemoveRange(IEnumerable<T> items)
        {
            CheckReentrancy();

            foreach (var item in items)
            {
                Items.Remove(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnPropertyChanged(new PropertyChangedEventArgs(CountString));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
        }
    }
}
