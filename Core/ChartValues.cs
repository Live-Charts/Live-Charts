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
using LiveCharts.Configurations;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : GossipCollection<T>, IChartValues
    {
        private IPointEvaluator<T> DefaultConfiguration { get; set; }

        #region Contructors

        public ChartValues()
        {
            Primitives = new Dictionary<int, ChartPoint>();
            Generics = new Dictionary<T, ChartPoint>();
            CollectionChanged += OnChanged;
        }

        #endregion

        #region Properties

        public IEnumerable<ChartPoint> Points
        {
            get { return GetPointsIterator(); }
        }

        public CoreLimit Limit1 { get; private set; }
        public CoreLimit Limit2 { get; private set; }
        public CoreLimit Limit3 { get; private set; }

        public SeriesAlgorithm Series { get; set; }
        public object ConfigurableElement { get; set; }

        internal int GarbageCollectorIndex { get; set; }
        internal Dictionary<int, ChartPoint> Primitives { get; set; }
        internal Dictionary<T, ChartPoint> Generics { get; set; }
        #endregion

        #region Public Methods

        public void GetLimits()
        {
            var config = GetConfig();
            if (config == null) return;

            var xMin = double.PositiveInfinity;
            var xMax = double.NegativeInfinity;
            var yMin = double.PositiveInfinity;
            var yMax = double.NegativeInfinity;
            var wMin = double.PositiveInfinity;
            var wMax = double.NegativeInfinity;
            
            foreach (var xyw in IndexData().Select(data => config.GetEvaluation(data)))
            {
                if (xyw[0].X < xMin) xMin = xyw[0].X;
                if (xyw[1].X > xMax) xMax = xyw[1].X;

                if (xyw[0].Y < yMin) yMin = xyw[0].Y;
                if (xyw[1].Y > yMax) yMax = xyw[1].Y;

                if (xyw[0].W < wMin) wMin = xyw[0].W;
                if (xyw[1].W > wMax) wMax = xyw[1].W;
            }

            Limit1 = new CoreLimit(double.IsInfinity(xMin) ? 0 : xMin, double.IsInfinity(yMin) ? 1 : xMax);
            Limit2 = new CoreLimit(double.IsInfinity(yMin) ? 0 : yMin, double.IsInfinity(yMax) ? 1 : yMax);
            Limit3 = new CoreLimit(double.IsInfinity(wMin) ? 0 : wMin, double.IsInfinity(wMax) ? 1 : wMax);
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
                    var observable = (IObservableChartPoint) value;
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
                cp.Key = i;

                config.SetAll(new KeyValuePair<int, T>(i, value), cp);
                //ToDo: this feels bad, when indexing the data, this is already done...
                //Also the index will break every run...

                yield return cp;
                i++;
            }
        }

        private IEnumerable<ChartPoint> GetGarbagePoints()
        {
            return Generics.Values.Where(IsGarbage)
                .Concat(Primitives.Values.Where(IsGarbage));
        }

        private IPointEvaluator<T> GetConfig()
        {
            //Trying to ge the user defined configuration...
            var config =
                (Series.View.Configuration ?? Series.SeriesCollection.Configuration) as IPointEvaluator<T>;

#if DEBUG
            Debug.WriteLine("Series Configuration not found, trying to get one from the defaults configurations...");
#endif

            if (config != null) return config;

            return DefaultConfiguration ??
                   (DefaultConfiguration =
                       ChartCore.Configurations.GetConfig<T>(Series.SeriesOrientation) as IPointEvaluator<T>);
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

        private void OnChanged(object oldItems, object newItems)
        {
            if (Series != null && Series.Chart != null) Series.Chart.Updater.Run();
        }

        #endregion
    }
}