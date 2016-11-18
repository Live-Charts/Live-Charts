//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiveCharts.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="oldItems">The old items.</param>
    /// <param name="newItems">The new items.</param>
    public delegate void NoisyCollectionCollectionChanged<in T>(
        IEnumerable<T> oldItems, IEnumerable<T> newItems);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.IList" />
    public interface INoisyCollection : IList
    {
        /// <summary>
        /// Occurs when [noisy collection changed].
        /// </summary>
        event NoisyCollectionCollectionChanged<object> NoisyCollectionChanged;
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<object> items);
        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection.</param>
        void InsertRange(int index, IEnumerable<object> collection);
    }

    /// <summary>
    /// A collection that notifies every time a value is added or removed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NoisyCollection<T> : INoisyCollection, IList<T>
    {
        private readonly List<T> _source;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of NoisyCollection class
        /// </summary>
        public NoisyCollection()
        {
            _source = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of NoisyCollection class with a given collection
        /// </summary>
        /// <param name="collection">given collection</param>
        public NoisyCollection(IEnumerable<T> collection)
        {
            _source = new List<T>(collection);
        }

        /// <summary>
        /// Initializes a new instance of NoisiCollection class with a given capacity
        /// </summary>
        /// <param name="capacity">given capacity</param>
        public NoisyCollection(int capacity)
        {
            _source = new List<T>(capacity);
        }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when [collection reset].
        /// </summary>
        public event Action CollectionReset;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an item from/in a specific index
        /// </summary>
        /// <param name="index">index to get/set</param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = value; }
        }

        /// <summary>
        /// Gets or sets an item from/in a specific index
        /// </summary>
        /// <param name="index">index to get/set</param>
        /// <returns></returns>
        object IList.this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = (T) value; }
        }

        /// <summary>
        /// Enumerates the collection
        /// </summary>
        /// <returns>collection enumeration</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        /// <summary>
        /// Enumerates the collection
        /// </summary>
        /// <returns>collection enumeration</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of items in the array
        /// </summary>
        /// <returns>items count</returns>
        public int Count
        {
            get { return _source.Count; }
        }

        /// <summary>
        /// Gets whether the collection is read only
        /// </summary>
        /// <returns>result</returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>) _source).IsReadOnly; }
        }
        /// <summary>
        /// Gets the number of items in the array
        /// </summary>
        /// <returns>result</returns>
        bool IList.IsReadOnly
        {
            get { return ((IList) _source).IsReadOnly; }
        }

        /// <summary>
        /// Gets whether the collection is synchronized
        /// </summary>
        /// <returns>result</returns>
        public bool IsSynchronized
        {
            get { return ((ICollection) _source).IsSynchronized; }
        }

        /// <summary>
        /// Gets the collections's sync root
        /// </summary>
        public object SyncRoot
        {
            get { return ((ICollection) _source).SyncRoot; }
        }

        /// <summary>
        /// Gets whether the collection is fixed
        /// </summary>
        public bool IsFixedSize { get { return ((IList) _source).IsFixedSize; } }

        #endregion

        /// <summary>
        /// Adds an object to the collection, and notifies the change
        /// </summary>
        /// <param name="value">item to add</param>
        /// <returns>number of items in the collection</returns>
        int IList.Add(object value)
        {
            var v = (T) value;
            Add(v);
            return _source.IndexOf(v);
        }

        /// <summary>
        /// Add an item to the collection, and notifies the change
        /// </summary>
        /// <returns>number of items in the collection</returns>
        public void Add(T item)
        {
            _source.Add(item);
            OnCollectionChanged(null, new[] {item});
        }

        /// <summary>
        /// Adds many items to the collection, and notifies the change
        /// </summary>
        /// <param name="items">collection to add</param>
        public void AddRange(IEnumerable<object> items)
        {
            AddRange(items.Cast<T>());
        }

        /// <summary>
        /// Adds many items to the collection, and notifies the change
        /// </summary>
        /// <param name="items">collection to add</param>
        public void AddRange(IEnumerable<T> items)
        {
            var newItems = items as T[] ?? items.ToArray();
            _source.AddRange(newItems);
            OnCollectionChanged(null, newItems);
        }

        /// <summary>
        /// Insert an item in a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="item">item to insert</param>
        public void Insert(int index, T item)
        {
            _source.Insert(index, item);
            OnCollectionChanged(null, new[] { item });
        }

        /// <summary>
        /// Insert an item in a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="value">item to insert</param>
        public void Insert(int index, object value)
        {
            Insert(index, (T) value);
        }

        /// <summary>
        /// Insert a range of values, starting in a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to start at</param>
        /// <param name="collection">collection to insert</param>
        public void InsertRange(int index, IEnumerable<object> collection)
        {
            InsertRange(index, collection.Cast<T>());
        }

        /// <summary>
        /// Insert a range of values, starting in a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to start at</param>
        /// <param name="collection">collection to insert</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            var newItems = collection as T[] ?? collection.ToArray();
            _source.InsertRange(index, newItems);
            OnCollectionChanged(null, newItems);
        }

        /// <summary>
        /// Removes an item from a collection, then notifies the change
        /// </summary>
        /// <param name="value">item to remove</param>
        public void Remove(object value)
        {
            Remove((T) value);
        }

        /// <summary>
        /// Remove an item from a collection, then notifies the change
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns>number of items in the collection</returns>
        public bool Remove(T item)
        {
            var ans = _source.Remove(item);
            OnCollectionChanged(new[] { item }, null);
            return ans;
        }

        /// <summary>
        /// Removes an item at a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to remove at</param>
        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        /// <summary>
        /// Removes an item at a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to remove at</param>
        void IList<T>.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        /// <summary>
        /// Removes an item at a specific index, then notifies the change
        /// </summary>
        /// <param name="index">index to remove at</param>
        public void RemoveAt(int index)
        {
            var i = _source[index];
            _source.RemoveAt(index);
            OnCollectionChanged(new[] {i}, null);
        }

        /// <summary>
        /// Removes all the items from the collection, then notifies the change
        /// </summary>
        void IList.Clear()
        {
            Clear();
        }

        /// <summary>
        /// Removes all the items from the collection, then notifies the change
        /// </summary>
        void ICollection<T>.Clear()
        {
            Clear();
        }

        /// <summary>
        /// Removes all the items from the collection, then notifies the change
        /// </summary>
        public void Clear()
        {
            var backup = _source.ToArray();
            _source.Clear();
            OnCollectionChanged(backup, null);
            if (CollectionReset != null) CollectionReset.Invoke();
        }

        /// <summary>
        /// Evaluates whether an item is in this collection
        /// </summary>
        /// <param name="value">object to look for</param>
        /// <returns>evaluation</returns>
        public bool Contains(object value)
        {
            return Contains((T) value);
        }

        /// <summary>
        /// Evaluates whether an item is in this collection
        /// </summary>
        /// <param name="item">item to look for</param>
        /// <returns>evaluation</returns>
        public bool Contains(T item)
        {
            return _source.Contains(item);
        }

        /// <summary>
        /// Copies the collection to another array
        /// </summary>
        /// <param name="array">backup array</param>
        /// <param name="index">array index</param>
        public void CopyTo(Array array, int index)
        {
            CopyTo(array.Cast<T>().ToArray(), index);
        }

        /// <summary>
        /// Copies the collection to another array
        /// </summary>
        /// <param name="array">backup array</param>
        /// <param name="index">array index</param>
        public void CopyTo(T[] array, int index)
        {
            _source.CopyTo(array, index);
        }

        /// <summary>
        /// Returns the index of an item in the collection
        /// </summary>
        /// <param name="value">item to look for</param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            return IndexOf((T) value);
        }

        /// <summary>
        /// Returns the index of an item in the collection
        /// </summary>
        /// <param name="item">item to look for</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _source.IndexOf(item);
        }

        event NoisyCollectionCollectionChanged<object> INoisyCollection.NoisyCollectionChanged
        {
            add { NoisyCollectionChanged += value as NoisyCollectionCollectionChanged<T>; }
            remove { NoisyCollectionChanged -= value as NoisyCollectionCollectionChanged<T>; }
        }
        /// <summary>
        /// Occurs when [noisy collection changed].
        /// </summary>
        public event NoisyCollectionCollectionChanged<T> NoisyCollectionChanged;

        private void OnCollectionChanged(IEnumerable<T> olditems, IEnumerable<T> newItems)
        {
            if (NoisyCollectionChanged != null)
                NoisyCollectionChanged.Invoke(olditems, newItems);
        }
    }
}
