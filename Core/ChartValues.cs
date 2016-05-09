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
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.CrossNet;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : GossipCollection<T>, IChartValues
    {
        #region Contructors

        public ChartValues()
        {
            ReportOldItems = false;
            Primitives = new Dictionary<int, ChartPoint>();
            Generics = new Dictionary<T, ChartPoint>();
            CollectionChanged += oldItems =>
            {
                if (Series != null && Series.Chart != null)
                    Series.Chart.Updater.Run();
            };
        }

        #endregion

        #region Properties

        public IEnumerable<ChartPoint> Points
        {
            get { return GetPointsIterator(); }
        }

        public CoreLimit Value1CoreLimit { get; private set; }
        public CoreLimit Value2CoreLimit { get; private set; }
        public CoreLimit Value3CoreLimit { get; private set; }
        public SeriesAlgorithm Series { get; set; }
        public ISeriesConfiguration ConfigurableElement { get; set; }

        internal int GarbageCollectorIndex { get; set; }
        internal Dictionary<int, ChartPoint> Primitives { get; set; }
        internal Dictionary<T, ChartPoint> Generics { get; set; }
        #endregion

        #region Public Methods

        public void GetLimits()
        {
            var config = GetConfig();
            if (config == null) return;

            var q = IndexData().ToArray();

            if (config.Value1 != null)
            {
                var v = q.Select(t => config.Value1(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
                var max = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Max();
                var min = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Min();

                Value1CoreLimit = new CoreLimit(min, max);
            }

            if (config.Value2 != null)
            {
                var v = q.Select(t => config.Value2(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
                var max = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Max();
                var min = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Min();

                Value2CoreLimit = new CoreLimit(min, max);
            }

            if (config.Value3 != null)
            {
                var v = q.Select(t => config.Value3(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
                var max = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Max();
                var min = v.Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).Min();

                Value3CoreLimit = new CoreLimit(min, max);
            }
        }

        public void InitializeGarbageCollector()
        {
            ValidateGarbageCollector();
            GarbageCollectorIndex++;
        }

        public void CollectGarbage()
        {
            var isPrimitive = typeof (T).AsCrossNet().IsPrimitive();
            foreach (var garbage in GetGarbagePoints().ToList())
            {
                if (garbage.View != null) //yes null, double.Nan Values, will generate null views.
                    garbage.View.RemoveFromView(Series.Chart);
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

            var isPrimitive = typeof (T).AsCrossNet().IsPrimitive();
            var isObservable = !isPrimitive &&
                               typeof (IObservableChartPoint).AsCrossNet().IsAssignableFrom(typeof (T));

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
                            Instance = value,
                            Key = i
                        };
                        Generics[value] = cp;
                    }
                }

                cp.GarbageCollectorIndex = garbageCollectorIndex;
                cp.Instance = value;
                cp.X = config.Value1(value, i);
                cp.Y = config.Value2(value, i);
                if (config.Value3 != null)
                    cp.Weight = config.Value3(value, i);

                yield return cp;
                i++;
            }
        }

        private IEnumerable<ChartPoint> GetGarbagePoints()
        {
            return Generics.Values.Where(IsGarbage)
                .Concat(Primitives.Values.Where(IsGarbage));
        }

        private SeriesConfiguration<T> GetConfig()
        {
            //Trying to ge the user defined configuration...
            var config =
                (Series.View.Configuration ?? Series.SeriesCollection.Configuration) as SeriesConfiguration<T>;

#if DEBUG
            Debug.WriteLine("Series Configuration not found, trying to get one from the defaults configurations...");
#endif

            return config
                   ?? (ChartCore.Configurations[typeof (T), Series.SeriesConfigurationType] as SeriesConfiguration<T>
                       ?? ForceConfiguration(Series.SeriesConfigurationType));
        }

        private static SeriesConfiguration<T> ForceConfiguration(SeriesConfigurationType type)
        {
            return type == SeriesConfigurationType.IndexedX
                ? new SeriesConfiguration<T>().X((v, i) => i).Y(v => (double) (object) v)
                : new SeriesConfiguration<T>().X(v => (double) (object) v).Y((v, i) => i);
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

        protected void ObservableOnPointChanged()
        {
            Series.Chart.Updater.Run();
        }

        private bool IsGarbage(ChartPoint point)
        {
            return point.GarbageCollectorIndex < GarbageCollectorIndex
                   || double.IsNaN(point.X) || double.IsNaN(point.Y);
        }

        #endregion
    }
}