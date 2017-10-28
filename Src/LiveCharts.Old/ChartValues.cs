//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.ComponentModel;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
#if NET45
using System.Reflection;
#endif

namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of chart values
    /// </summary>
    /// <typeparam name="T">Type to plot, notice you could need to configure the type.</typeparam>
    public class ChartValues<T> : NoisyCollection<T>, IChartValues
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of chart values
        /// </summary>
        public ChartValues()
        {
            Trackers = new Dictionary<ISeriesView, PointTracker>();
            NoisyCollectionChanged += OnChanged;
        }

        /// <summary>
        /// Initializes a new instance of chart values, with a given collection
        /// </summary>
        public ChartValues(IEnumerable<T> collection) : base(collection)
        {
            Trackers = new Dictionary<ISeriesView, PointTracker>();
            NoisyCollectionChanged += OnChanged;
        }

        #endregion

        #region Properties
        private IPointEvaluator<T> DefaultConfiguration { get; set; }
        private Dictionary<ISeriesView, PointTracker> Trackers { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates the limits in the chart values
        /// </summary>
        public void Initialize(ISeriesView seriesView)
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

            var cp = new ChartPoint();

            var ax = seriesView.Model.Chart.AxisX[seriesView.ScalesXAt];
            var ay = seriesView.Model.Chart.AxisY[seriesView.ScalesYAt];
            double fx = double.IsNaN(ax.MinValue) ? double.NegativeInfinity : ax.MinValue,
                tx = double.IsNaN(ax.MaxValue) ? double.PositiveInfinity : ax.MaxValue,
                fy = double.IsNaN(ay.MinValue) ? double.NegativeInfinity : ay.MinValue,
                ty = double.IsNaN(ay.MaxValue) ? double.PositiveInfinity : ay.MaxValue;

            var isHorizontal = seriesView.Model.SeriesOrientation == SeriesOrientation.Horizontal;

            var index = 0;
            foreach(var item in this)
            {
                config.Evaluate(index, item, cp);
                index++;

                if (isHorizontal)
                {
                    if (cp.X < fx || cp.X > tx) continue;
                }
                else
                {
                    if (cp.Y < fy || cp.Y > ty) continue;
                }

                if (seriesView is IFinancialSeriesView)
                {
                    if (cp.X < xMin) xMin = cp.X;
                    if (cp.X > xMax) xMax = cp.X;

                    if (cp.Low < yMin) yMin = cp.Low;
                    if (cp.High > yMax) yMax = cp.High;

                    if (cp.Weight < wMin) wMin = cp.Weight;
                    if (cp.Weight > wMax) wMax = cp.Weight;

                }
                else if (seriesView is IScatterSeriesView || seriesView is IHeatSeriesView)
                {
                    if (cp.X < xMin) xMin = cp.X;
                    if (cp.X > xMax) xMax = cp.X;

                    if (cp.Y < yMin) yMin = cp.Y;
                    if (cp.Y > yMax) yMax = cp.Y;

                    if (cp.Weight < wMin) wMin = cp.Weight;
                    if (cp.Weight > wMax) wMax = cp.Weight;
                }
                else
                {
                    if (cp.X < xMin) xMin = cp.X;
                    if (cp.X > xMax) xMax = cp.X;

                    if (cp.Y < yMin) yMin = cp.Y;
                    if (cp.Y > yMax) yMax = cp.Y;
                }
            }

            tracker.XLimit = new CoreLimit(double.IsInfinity(xMin)
                ? 0
                : xMin, double.IsInfinity(yMin) ? 1 : xMax);
            tracker.YLimit = new CoreLimit(double.IsInfinity(yMin)
                ? 0
                : yMin, double.IsInfinity(yMax) ? 1 : yMax);
            tracker.WLimit = new CoreLimit(double.IsInfinity(wMin)
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

#if NET40
            var isClass = typeof(T).IsClass;
            var isObservable = isClass && typeof(IObservableChartPoint).IsAssignableFrom(typeof(T));
            var notifies = isClass && typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T));

#endif
#if NET45
            var isClass = typeof(T).GetTypeInfo().IsClass;
            var isObservable = isClass &&
                               typeof(IObservableChartPoint).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo());
            var notifies = isClass && typeof(INotifyPropertyChanged).GetTypeInfo()
                               .IsAssignableFrom(typeof(T).GetTypeInfo());
#endif

            var tracker = GetTracker(seriesView);
            var gci = tracker.Gci;

            var index = 0;
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

                if (notifies)
                {
                    var notify = (INotifyPropertyChanged) value;
                    if (notify != null)
                    {
                        notify.PropertyChanged -= NotifyOnPropertyChanged;
                        notify.PropertyChanged += NotifyOnPropertyChanged;
                    }
                }

                var cp = GetChartPoint(isClass, tracker, index, value);

                cp.Gci = gci;
                cp.Instance = value;
                cp.Key = index;
                cp.SeriesView = seriesView;

                config.Evaluate(index, value, cp);
                index++;

                yield return cp;
            }
        }

        /// <summary>
        /// Initializes the garbage collector
        /// </summary>
        public void InitializeStep(ISeriesView series)
        {
            ValidateGarbageCollector(series);
            GetTracker(series).Gci++;
        }

        /// <summary>
        /// Collects the unnecessary values 
        /// </summary>
        public void CollectGarbage(ISeriesView seriesView)
        {
#if NET40
            var isclass = typeof(T).IsClass;
#endif
#if NET45
            var isclass = typeof(T).GetTypeInfo().IsClass;
#endif

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

        /// <summary>
        /// Gets series that owns the values
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public PointTracker GetTracker(ISeriesView view)
        {
            PointTracker tracker;

            if (Trackers.TryGetValue(view, out tracker)) return tracker;

            tracker = new PointTracker();
            Trackers[view] = tracker;

            return tracker;
        }

        #endregion

        #region Privates

        private IPointEvaluator<T> GetConfig(ISeriesView view)
        {
            //Trying to get the user defined configuration...

            //series == null means that chart values are null, and LiveCharts
            //could not set the Series Instance tho the current chart values...
            if (view == null || view.Model.SeriesCollection == null) return null;

            var config =
                (view.Configuration ?? view.Model.SeriesCollection.Configuration) as IPointEvaluator<T>;

            if (config != null) return config;

            return DefaultConfiguration ??
                   (DefaultConfiguration =
                       ChartCore.Configurations.GetConfig<T>(view.Model.SeriesOrientation) as IPointEvaluator<T>);
        }

        private static ChartPoint GetChartPoint(bool isClass, PointTracker tracker, int index, T value)
        {
            ChartPoint cp;

            if (!isClass)
            {
                if (tracker.Indexed.TryGetValue(index, out cp)) return cp;
                cp = new ChartPoint
                {
                    Instance = value,
                    Key = index
                };
                tracker.Indexed[index] = cp;
            }
            else
            {
                if (tracker.Referenced.TryGetValue(value, out cp)) return cp;
                cp = new ChartPoint
                {
                    Instance = value,
                    Key = index
                };
                tracker.Referenced[value] = cp;
            }
            return cp;
        }

        private void ObservableOnPointChanged()
        {
            Trackers.Keys.ForEach(x => x.Model.Chart.Updater.Run());
        }

        private void NotifyOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Trackers.Keys.ForEach(x => x.Model.Chart.Updater.Run());
        }

        private IEnumerable<ChartPoint> GetGarbagePoints(ISeriesView view)
        {
            var tracker = GetTracker(view);

            return tracker.Indexed.Values.Where(x => IsGarbage(x, tracker)).Concat(
                tracker.Referenced.Values.Where(x => IsGarbage(x, tracker)));
        }

        private void ValidateGarbageCollector(ISeriesView view)
        {
            var tracker = GetTracker(view);

            if (tracker.Gci != int.MaxValue) return;

            tracker.Gci = 0;

            foreach (var point in tracker.Indexed.Values.Concat(tracker.Referenced.Values))
                point.Gci = 0;
        }

        private static bool IsGarbage(ChartPoint point, PointTracker tracker)
        {
            return point.Gci < tracker.Gci
                   || double.IsNaN(point.X) || double.IsNaN(point.Y);
        }

        private void OnChanged(IEnumerable<T> oldItems, IEnumerable<T> newItems)
        {
            if (Trackers.Keys.All(x => x != null && x.Model.Chart != null))
                Trackers.Keys.ForEach(x => x.Model.Chart.Updater.Run());
        }

        #endregion
    }
}