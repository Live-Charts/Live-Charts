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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LiveChartsCore
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : ObservableCollection<T>, IChartValues
    {
        #region Properties

        public ChartValues()
        {
            Primitives = new Dictionary<int, ChartPoint>();
            Generics = new Dictionary<T, ChartPoint>();
            CollectionChanged += (sender, eventArgs) =>
            {
                if (Series != null && Series.Chart != null) Series.Chart.Updater.Run();
            };
        }

        /// <summary>
        /// Gets the collection of points displayed in the chart current view
        /// </summary>
        public IEnumerable<ChartPoint> Points
        {
            get { return GetPointsIterator(); }
        }

        public LvcPoint MaxChartPoint { get; private set; }
        public LvcPoint MinChartPoint { get; private set; }
        public SeriesCore Series { get; set; }
        public SeriesConfiguration SeriesConfiguration { get; set; }

        internal int GarbageCollectorIndex { get; set; }
        internal Dictionary<int, ChartPoint> Primitives { get; set; }
        internal Dictionary<T, ChartPoint> Generics { get; set; }

        #endregion

        #region Public Methods

        public ChartValues<T> AddRange(IEnumerable<T> collection)
        {
            CheckReentrancy();
            foreach (var item in collection) Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return this;
        }

        public void GetLimits()
        {
            var config = GetConfig();
            if (config == null) return;

            var q = IndexData().ToArray();

            var xs = q.Select(t => config.XValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var xMax = xs.Where(x => !double.IsNaN(x)).Max();
            var xMin = xs.Where(x => !double.IsNaN(x)).Min();

            var ys = q.Select(t => config.YValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var yMax = ys.Where(x => !double.IsNaN(x)).Max();
            var yMin = ys.Where(x => !double.IsNaN(x)).Min();

            MaxChartPoint = new LvcPoint(xMax, yMax);
            MinChartPoint = new LvcPoint(xMin, yMin);
        }

        public void InitializeGarbageCollector()
        {
            ValidateGarbageCollector();
            GarbageCollectorIndex++;
        }

        public void CollectGarbage()
        {
            var isPrimitive = typeof (T).GetTypeInfo().IsPrimitive;
            foreach (var garbage in GetGarbagePoints().ToList())
            {
                Series.View.RemovePointView(garbage.View);
                if (isPrimitive) Primitives.Remove(garbage.Key);
                else Generics.Remove((T) garbage.Instance);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<KeyValuePair<int, T>> IndexData()
        {
            var i = 0;
            foreach (var t in this)
            {
                yield return new KeyValuePair<int, T>(i, t);
                i++;
            }
        }

        private IEnumerable<ChartPoint> GetPointsIterator()
        {
            if (Series == null) yield break;

            var config = GetConfig();

            var isPrimitive = typeof (T).GetTypeInfo().IsPrimitive;
            var isObservable = !isPrimitive &&
                               typeof (IObservableChartPoint).GetTypeInfo().IsAssignableFrom(typeof (T).GetTypeInfo());

            var garbageCollectorIndex = GarbageCollectorIndex;
            var i = 0;

            foreach (var value in this)
            {
                if (isObservable)
                {
                    var observable = value as IObservableChartPoint;
                    if (observable != null)
                    {
                        observable.PointChanged -= ObservableOnPointChanged;
                        observable.PointChanged += ObservableOnPointChanged;
                    }
                }

                ChartPoint cp;

                if (isPrimitive)
                {
                    if (!Primitives.TryGetValue(i, out cp))
                    {
                        cp = new ChartPoint
                        {
                            X = config.XValueMapper(value, i),
                            Y = config.YValueMapper(value, i),
                            Instance = value,
                            Key = i
                        };
                        Primitives[i] = cp;
                    }
                }
                else
                {
                    if (!Generics.TryGetValue(value, out cp))
                    {
                        cp = new ChartPoint
                        {
                            X = config.XValueMapper(value, i),
                            Y = config.YValueMapper(value, i),
                            Instance = value,
                            Key = i
                        };
                        Generics[value] = cp;
                    }
                }

                cp.GarbageCollectorIndex = garbageCollectorIndex;

                yield return cp;
                i++;
            }
        }

        private IEnumerable<ChartPoint> GetGarbagePoints()
        {
            return Generics.Values.Where(x => x.GarbageCollectorIndex < GarbageCollectorIndex - 1)
                .Concat(Primitives.Values.Where(y => y.GarbageCollectorIndex < GarbageCollectorIndex - 1));
        }

        private SeriesConfiguration<T> GetConfig()
        {
            //the null propagator is ugly but necessary for livecharts starters
            //this enables any chart by default to support any primitive type
            //this should run ok if you dont have many points
            //if you are facing perfomrnace issues then you must configure your type
            //if you are not using chart values of double ChartValues<double> then you must:
            //mySeriesConfiguration.Setup(new SeriesConfiguration<int>().Y(v => (double) v));
            return (Series.View.Configuration ?? Series.SeriesCollection.Configuration) as SeriesConfiguration<T>
                   ?? new SeriesConfiguration<T>().Y(t => (double) (object) t);
        }

        private void ValidateGarbageCollector()
        {
            //just in case!
            if (GarbageCollectorIndex != int.MaxValue) return;
            GarbageCollectorIndex = 0;
            foreach (var point in Generics.Values.Concat(Primitives.Values))
            {
                point.GarbageCollectorIndex = 0;
            }
        }

        private void ObservableOnPointChanged(object caller)
        {
            Series.Chart.Updater.Run();
        }

        #endregion

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}