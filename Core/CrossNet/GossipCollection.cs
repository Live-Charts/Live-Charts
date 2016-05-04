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
using System.Collections.Generic;

namespace LiveCharts.CrossNet
{
    public class GossipCollection<T> : List<T>
    {
        public event Action CollectionChanged;

        public new void Add(T item)
        {
            base.Add(item);
            OnCollectionChanged();
        }

        public new void AddRange(IEnumerable<T> items)
        {
            base.AddRange(items);
            OnCollectionChanged();
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnCollectionChanged();
        }
        
        public new void InsertRange(int index, IEnumerable<T> items)
        {
            base.InsertRange(index, items);
            OnCollectionChanged();
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            OnCollectionChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnCollectionChanged();
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            OnCollectionChanged();
        }

        public new void RemoveAll(Predicate<T> predicate)
        {
            base.RemoveAll(predicate);
            OnCollectionChanged();
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged();
        }

        private void OnCollectionChanged()
        {
            if (CollectionChanged != null) CollectionChanged.Invoke();
        }
    }
}
