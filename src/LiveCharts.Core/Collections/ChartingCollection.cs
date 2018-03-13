#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

#endregion

namespace LiveCharts.Core.Collections
{
    /// <summary>
    /// An extended observable collection that implements AddRange, RemoveRange methods optimized for LiveCharts.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    public class ChartingCollection<T> : ObservableCollection<T>, INotifyRangeChanged<T>
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingCollection{T}"/> class.
        /// </summary>
        public ChartingCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public ChartingCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingCollection{T}"/> class.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public ChartingCollection(IList<T> list)
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
