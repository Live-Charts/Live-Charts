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

namespace LiveCharts.CrossNet
{
    //To support multuple .net versions and platforms, lets create our "own" specialized observablecollection
    public interface IGossipCollection
    {
        event Action CollectionChanged;

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
        int IndexOf(object item);
    }

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

        public event Action CollectionChanged;

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

        public int Add(T item)
        {
            _source.Add(item);
            NotifyCollectionChanged();
            return _source.Count;
        }

        int IGossipCollection.Add(object item)
        {
            _source.Add((T) item);
            NotifyCollectionChanged();
            return _source.Count;
        }


        public int AddRange(IEnumerable<T> items)
        {
           _source.AddRange(items);
            NotifyCollectionChanged();
            return _source.Count;
        }

        int IGossipCollection.AddRange(IEnumerable<object> items)
        {
            _source.AddRange(items.Cast<T>());
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int Insert(int index, T item)
        {
            _source.Insert(index, item);
            NotifyCollectionChanged();
            return _source.Count;
        }

        int IGossipCollection.Insert(int index, object item)
        {
            _source.Insert(index, (T) item);
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int InsertRange(int index, IEnumerable<T> items)
        {
            _source.InsertRange(index, items);
            NotifyCollectionChanged();
            return _source.Count;
        }
        int IGossipCollection.InsertRange(int index, IEnumerable<object> items)
        {
            _source.InsertRange(index, items.Cast<T>());
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int Remove(T item)
        {
            _source.Remove(item);
            NotifyCollectionChanged();
            return _source.Count;
        }

        int IGossipCollection.Remove(object item)
        {
            _source.Remove((T) item);
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int RemoveAt(int index)
        {
            _source.RemoveAt(index);
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int RemoveRange(int index, int count)
        {
            _source.RemoveRange(index, count);
            NotifyCollectionChanged();
            return _source.Count;
        }

        public int RemoveAll(Predicate<T> preicate)
        {
            _source.RemoveAll(preicate);
            NotifyCollectionChanged();
            return _source.Count;
        }

        public void Clear()
        {
            _source.Clear();
            NotifyCollectionChanged();
        }

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

        private void NotifyCollectionChanged()
        {
            if (CollectionChanged != null)
                CollectionChanged.Invoke();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
