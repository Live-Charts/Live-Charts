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
    public delegate void NoisyCollectionCollectionChanged<in T>(
        IEnumerable<T> oldItems, IEnumerable<T> newItems);
    
    public interface INoisyCollection : IList
    {
        event NoisyCollectionCollectionChanged<object> CollectionChanged;
    }

    public class NoisyCollection<T> : INoisyCollection, IList<T>
    {
        private readonly List<T> _source;

        #region Contructors

        public NoisyCollection()
        {
            _source = new List<T>();
        }

        public NoisyCollection(IEnumerable<T> collection)
        {
            _source = new List<T>(collection);
        }

        public NoisyCollection(int capacity)
        {
            _source = new List<T>(capacity);
        }

        #endregion


        #region Properties

        public T this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = value; }
        }

        object IList.this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = (T) value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _source.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>) _source).IsReadOnly; }
        }
        bool IList.IsReadOnly
        {
            get { return ((IList) _source).IsReadOnly; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection) _source).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection) _source).SyncRoot; }
        }

        public bool IsFixedSize { get { return ((IList) _source).IsFixedSize; } }

        #endregion


        public int Add(object value)
        {
            _source.Add((T)value);
            OnCollectionChanged(null, new[] {(T) value});
            return _source.Count;
        }

        public void Add(T item)
        {
            _source.Add(item);
            OnCollectionChanged(null, new [] {item});
        }

        public void AddRange(IEnumerable<T> items)
        {
            _source.AddRange(items);
            OnCollectionChanged(null, items);
        }

        public void Insert(int index, object value)
        {
            _source.Insert(index, (T) value);
            OnCollectionChanged(null, new[] {(T) value});
        }

        public void Insert(int index, T item)
        {
            _source.Insert(index, item);
            OnCollectionChanged(null, new[] {item});
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _source.InsertRange(index, collection);
            OnCollectionChanged(null, collection);
        }

        public void Remove(object value)
        {
            _source.Remove((T) value);
            OnCollectionChanged(new[] {(T) value}, null);
        }

        public bool Remove(T item)
        {
            var ans = _source.Remove(item);
            OnCollectionChanged(new[] { item }, null);
            return ans;
        }

        void IList.RemoveAt(int index)
        {
            var i = _source[index];
            _source.RemoveAt(index);
            OnCollectionChanged(new[] { i }, null);
        }

        void IList<T>.RemoveAt(int index)
        {
            var i = _source[index];
            _source.RemoveAt(index);
            OnCollectionChanged(new[] { i }, null);
        }

        public void RemoveAt(int index)
        {
            var i = _source[index];
            _source.RemoveAt(index);
            OnCollectionChanged(new[] { i }, null);
        }


        void IList.Clear()
        {
            var backup = _source.ToArray();
            _source.Clear();
            OnCollectionChanged(backup, null);
        }

        void ICollection<T>.Clear()
        {
            var backup = _source.ToArray();
            _source.Clear();
            OnCollectionChanged(backup, null);
        }

        public void Clear()
        {
            var backup = _source.ToArray();
            _source.Clear();
            OnCollectionChanged(backup, null);
        }

        public bool Contains(object value)
        {
            return _source.Contains((T) value);
        }

        public bool Contains(T item)
        {
            return _source.Contains(item);
        }


        public void CopyTo(Array array, int index)
        {
            _source.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _source.CopyTo(array, arrayIndex);
        }


        public int IndexOf(object value)
        {
            return _source.IndexOf((T) value);
        }
        
        public int IndexOf(T item)
        {
            return _source.IndexOf(item);
        }

        event NoisyCollectionCollectionChanged<object> INoisyCollection.CollectionChanged
        {
            add { CollectionChanged += value as NoisyCollectionCollectionChanged<T>; }
            remove { CollectionChanged -= value as NoisyCollectionCollectionChanged<T>; }
        }
        public event NoisyCollectionCollectionChanged<T> CollectionChanged;

        private void OnCollectionChanged(IEnumerable<T> olditems, IEnumerable<T> newItems)
        {
            if (CollectionChanged != null)
                CollectionChanged.Invoke(olditems, newItems);
        }
    }
}
