using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace LiveCharts.Uwp.Components.MultiBinding
{
    /// <summary>
    /// Represents a collection of <see cref="DependencyObject"/> instances of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class DependencyObjectCollection<T> : DependencyObjectCollection, INotifyCollectionChanged
        where T : DependencyObject
    {
        /// <summary>
        /// Occurs when items in the collection are added, removed, or replaced.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly List<T> _oldItems = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyObjectCollection{T}" /> class.
        /// </summary>
        public DependencyObjectCollection()
        {
            VectorChanged += DependencyObjectCollectionVectorChanged;
        }

        private void DependencyObjectCollectionVectorChanged(IObservableVector<DependencyObject> sender, IVectorChangedEventArgs e)
        {
            var index = (int)e.Index;

            switch (e.CollectionChange)
            {
                case CollectionChange.Reset:
                    foreach (var item in this)
                    {
                        VerifyType(item);
                    }

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                    _oldItems.Clear();

                    break;

                case CollectionChange.ItemInserted:
                    VerifyType(this[index]);

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this[index], index));

                    _oldItems.Insert(index, (T)this[index]);

                    break;

                case CollectionChange.ItemRemoved:
                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, _oldItems[index], index));

                    _oldItems.RemoveAt(index);

                    break;

                case CollectionChange.ItemChanged:
                    VerifyType(this[index]);

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, this[index], _oldItems[index]));

                    _oldItems[index] = (T)this[index];

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event with the provided event data.
        /// </summary>
        /// <param name="eventArgs">The event data.</param>
        protected void RaiseCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            var eventHandler = CollectionChanged;

            if (eventHandler != null)
            {
                eventHandler(this, eventArgs);
            }
        }

        private void VerifyType(DependencyObject item)
        {
            if (!(item is T))
            {
                throw new InvalidOperationException("Invalid item type added to collection");
            }
        }
    }
}
