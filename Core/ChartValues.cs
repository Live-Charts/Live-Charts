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
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of chart values
    /// </summary>
    /// <typeparam name="T">Type to plot, notice you could need to configure the type.</typeparam>
    public class ChartValues<T> : NoisyCollection<T>, IChartValues
    {
        private IPointEvaluator<T> DefaultConfiguration { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of chart values
        /// </summary>
        public ChartValues()
        {
            Trackers = new Dictionary<ISeriesView, PointTracker>();
            CollectionChanged += OnChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the series that is firing the ChartValus
        /// </summary>
        public Dictionary<ISeriesView, PointTracker> Trackers { get; internal set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates the limits in the chart values
        /// </summary>
        public void GetLimits(ISeriesView seriesView)
        {
            var config = GetConfig(seriesView);
            if (config == null) return;

            var xMin = double.PositiveInfinity;
            var xMax = double.NegativeInfinity;
            var yMin = double.PositiveInfinity;
            var yMax = double.NegativeInfinity;
            var wMin = double.PositiveInfinity;
            var wMax = double.NegativeInfinity;

            var tracker = GetTracker(seriesView);

            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                var pair = new KeyValuePair<int, T>(index, item);
                var xyw = config.GetEvaluation(pair);

                if (xyw[0].X < xMin) xMin = xyw[0].X;
                if (xyw[1].X > xMax) xMax = xyw[1].X;

                if (xyw[0].Y < yMin) yMin = xyw[0].Y;
                if (xyw[1].Y > yMax) yMax = xyw[1].Y;

                if (xyw[0].W < wMin) wMin = xyw[0].W;
                if (xyw[1].W > wMax) wMax = xyw[1].W;
            }

            tracker.Limit1 = new CoreLimit(double.IsInfinity(xMin)
                ? 0
                : xMin, double.IsInfinity(yMin) ? 1 : xMax);
            tracker.Limit2 = new CoreLimit(double.IsInfinity(yMin)
                ? 0
                : yMin, double.IsInfinity(yMax) ? 1 : yMax);
            tracker.Limit3 = new CoreLimit(double.IsInfinity(wMin)
                ? 0
                : wMin, double.IsInfinity(wMax) ? 1 : wMax);
        }

        /// <summary>
        /// Gets the current chart points in the view, the view is required as an argument, because an instance of IChartValues could hold many ISeriesView instances.
        /// </summary>
        /// <param name="seriesView">The series view</param>
        /// <returns></returns>
        public IEnumerable<ChartPoint> GetPoints(ISeriesView seriesView)
        {
            if (seriesView == null) yield break;

            var config = GetConfig(seriesView);

            var isClass = typeof(T).IsClass;
            var isObservable = isClass && typeof(IObservableChartPoint).IsAssignableFrom(typeof(T));

            var tracker = GetTracker(seriesView);
            var gci = tracker.Gci;
            
            for (var index = 0; index < Count; index++)
            {
                var value = this[index];
                if (isObservable)
                {
                    var observable = (IObservableChartPoint)value;
                    if (observable != null)
                    {
                        observable.PointChanged -= ObservableOnPointChanged;
                        observable.PointChanged += ObservableOnPointChanged;
                    }
                }

                ChartPoint cp;

                if (!isClass)
                {
                    if (!tracker.Indexed.TryGetValue(index, out cp))
                    {
                        cp = new ChartPoint
                        {
                            Instance = value,
                            Key = index
                        };
                        tracker.Indexed[index] = cp;
                    }
                }
                else
                {
                    if (!tracker.Referenced.TryGetValue(value, out cp))   
                    {
                        cp = new ChartPoint
                        {
                            Instance = value,
                            Key = index
                        };
                        tracker.Referenced[value] = cp;
                    }
                }

                cp.Gci = gci;

                cp.Instance = value;
                cp.Key = index;
                cp.SeriesView = seriesView;

                config.SetAll(new KeyValuePair<int, T>(index, value), cp);

                yield return cp;
            }
        }

        /// <summary>
        /// Initializes the garbage collector
        /// </summary>
        public void InitializeGarbageCollector(ISeriesView series)
        {
            ValidateGarbageCollector(series);
            GetTracker(series).Gci++;
        }

        /// <summary>
        /// Collects the unnecessary values 
        /// </summary>
        public void CollectGarbage(ISeriesView seriesView)
        {
            var isclass = typeof (T).IsClass;

            var tracker = GetTracker(seriesView);

            foreach (var garbage in GetGarbagePoints(seriesView).ToList())
            {
                if (garbage.View != null) //yes null, double.Nan Values, will generate null views.
                    garbage.View.RemoveFromView(seriesView.Model.Chart);

                if (!isclass)
                {
                    tracker.Indexed.Remove(garbage.Key);
                }
                else
                {
                    tracker.Referenced.Remove(garbage.Instance);
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<ChartPoint> GetGarbagePoints(ISeriesView view)
        {
            var tracker = GetTracker(view);

            return tracker.Indexed.Values.Where(x => IsGarbage(x, tracker)).Concat(
                tracker.Referenced.Values.Where(x => IsGarbage(x, tracker)));
        }

        private IPointEvaluator<T> GetConfig(ISeriesView view)
        {
            //Trying to get the user defined configuration...

            //series == null means that chart values are null, and LiveCharts
            //could not set the Series Instance tho the current chart values...
            if (view == null) return null;

            var config =
                (view.Configuration ?? view.Model.SeriesCollection.Configuration) as IPointEvaluator<T>;
            
            if (config != null) return config;

            return DefaultConfiguration ??
                   (DefaultConfiguration =
                       ChartCore.Configurations.GetConfig<T>(view.Model.SeriesOrientation) as IPointEvaluator<T>);
        }

        private void ValidateGarbageCollector(ISeriesView view)
        {
            var tracker = GetTracker(view);

            if (tracker.Gci != int.MaxValue) return;

            tracker.Gci = 0;

            foreach (var point in tracker.Indexed.Values.Concat(tracker.Referenced.Values))
                point.Gci = 0;
        }

        private void ObservableOnPointChanged()
        {
            Trackers.Keys.ForEach(x => x.Model.Chart.Updater.Run());
        }

        private bool IsGarbage(ChartPoint point, PointTracker tracker)
        {
            return point.Gci < tracker.Gci
                   || double.IsNaN(point.X) || double.IsNaN(point.Y);
        }

        private PointTracker GetTracker(ISeriesView view)
        {
            PointTracker tracker;

            if (Trackers.TryGetValue(view, out tracker)) return tracker;

            tracker = new PointTracker();
            Trackers[view] = tracker;

            return tracker;
        }

        private void OnChanged(IEnumerable<T> oldItems, IEnumerable<T> newItems)
        {
            if (Trackers.Keys.All(x => x != null && x.Model.Chart != null))
                Trackers.Keys.ForEach(x => x.Model.Chart.Updater.Run());
        }

        #endregion
    }
}