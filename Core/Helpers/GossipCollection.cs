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
    public delegate void GossipCollectionCollectionChanged<in T>(
        IEnumerable<T> oldItems, IEnumerable<T> newItems);
    
    public interface IGossipCollection : IEnumerable
    {
        event GossipCollectionCollectionChanged<object> CollectionChanged;

        object this[int index] { get; set; }
        int Count { get; }

        int Add(object item);
        int AddRange(IEnumerable<object> items);
        int Insert(int index, object item);
        int InsertRange(int index, IEnumerable<object> items);
        int Remove(object item);
        int RemoveAt(int index);
        int RemoveRange(int index, int count);
        void Clear();

        bool Contains(object item);
        int IndexOf(object item);}

    public class GossipCollection<T> : IGossipCollection, IEnumerable<T>
    {
        private readonly List<T> _source;

        #region Constrctors

        public GossipCollection()
        {
            _source = new List<T>();
        }

        public GossipCollection(int capacity)
        {
            _source = new List<T>(capacity);
        }

        public GossipCollection(IEnumerable<T> source)
        {
            _source = new List<T>(source);
        }

        #endregion

        #region Events

        event GossipCollectionCollectionChanged<object> IGossipCollection.CollectionChanged
        {
            add { CollectionChanged += value as GossipCollectionCollectionChanged<T>; }
            remove { CollectionChanged -= value as GossipCollectionCollectionChanged<T>; }
        }

        public event GossipCollectionCollectionChanged<T> CollectionChanged;

#endregion

        public T this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = value; }
        }
        object IGossipCollection.this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = (T) value; }
        }

        public int Count { get { return _source.Count; } }

#region Add

        public int Add(T item)
        {
            _source.Add(item);
            OnCollectionChanged(null, new List<T> {item});
            return _source.Count;
        }

        int IGossipCollection.Add(object item)
        {
            _source.Add((T) item);
            OnCollectionChanged(null, new List<T> {(T) item});
            return _source.Count;
        }

        public int AddRange(IEnumerable<T> items)
        {
            //resharper says possible multiple enumeration, I guess this is what we need
            //literally a possible multple enumeration, but we could actually discard multiple enumeration
            //if the derived class does nothing with the new items.
            _source.AddRange(items);
            OnCollectionChanged(null, items);
            return _source.Count;
        }

        int IGossipCollection.AddRange(IEnumerable<object> items)
        {
            var enumerable = items.Cast<T>();
            _source.AddRange(enumerable);
            OnCollectionChanged(null, enumerable);
            return _source.Count;
        }

        public int Insert(int index, T item)
        {
            _source.Insert(index, item);
            OnCollectionChanged(null, new List<T> {item});
            return _source.Count;
        }

        int IGossipCollection.Insert(int index, object item)
        {
            _source.Insert(index, (T) item);
            OnCollectionChanged(null, new List<T> {(T) item});
            return _source.Count;
        }

        public int InsertRange(int index, IEnumerable<T> items)
        {
            _source.InsertRange(index, items);
            OnCollectionChanged(null, items);
            return _source.Count;
        }
        int IGossipCollection.InsertRange(int index, IEnumerable<object> items)
        {
            var enumerable = items.Cast<T>();
            _source.InsertRange(index, enumerable);
            OnCollectionChanged(null, enumerable);
            return _source.Count;
        }

#endregion

#region Remove

        public int Remove(T item)
        {
            _source.Remove(item);
            OnCollectionChanged(new List<T> {item}, null);
            return _source.Count;
        }

        int IGossipCollection.Remove(object item)
        {
            _source.Remove((T) item);
            OnCollectionChanged(new List<T> {(T) item}, null);
            return _source.Count;
        }

        public int RemoveAt(int index)
        {
            var removedItem = new List<T> {_source[index]};
            _source.RemoveAt(index);
            OnCollectionChanged(removedItem, null);
            return _source.Count;
        }

        public int RemoveRange(int index, int count)
        {
            var range = _source.Skip(index).Take(count).ToList();
            _source.RemoveRange(index, count);
            OnCollectionChanged(range, null);
            return _source.Count;
        }

        public int RemoveAll(Predicate<T> predicate)
        {
            var matches = _source.Where(x => predicate(x)).ToList();
            _source.RemoveAll(predicate);
            OnCollectionChanged(matches, null);
            return _source.Count;
        }

        public void Clear()
        {
            var removed = _source.Select(x => x).ToList();
            _source.Clear();
            OnCollectionChanged(removed, null);
        }

#endregion

        public bool Contains(T item)
        {
            return _source.Contains(item);
        }
        bool IGossipCollection.Contains(object item)
        {
            return _source.Contains((T) item);
        }

        public int IndexOf(T item)
        {
            return _source.IndexOf(item);
        }
        int IGossipCollection.IndexOf(object item)
        {
            return _source.IndexOf((T) item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnCollectionChanged(IEnumerable<T> olditems, IEnumerable<T> newItems)
        {
            if (CollectionChanged != null)
                CollectionChanged.Invoke(olditems, newItems);
        }
    }
}
