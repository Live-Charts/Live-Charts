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

        /// <summary>
        /// Gets the collection of points displayed in the chart current view
        /// </summary>
        public IEnumerable<ChartPoint> Points
        {
            get
            {
                if (Series == null) return Enumerable.Empty<ChartPoint>();

                var config = (Series.Configuration ?? Series.SeriesCollection.Configuration) as SeriesConfiguration<T>;

                if (config == null) return Enumerable.Empty<ChartPoint>();

                var q = IndexData(config);

                return q.Select(t => new ChartPoint
                {
                    X = config.XValueMapper(t.Value, t.Key),
                    Y = config.YValueMapper(t.Value, t.Key),
                    Instance = t.Value,
                    Key = t.Key
                });
            }
        }

        public LvcPoint MaxChartPoint { get; private set; }
        public LvcPoint MinChartPoint { get; private set; }
        public ISeriesModel Series { get; set; }
        public SeriesConfiguration SeriesConfiguration { get; set; }

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
            EvaluateLimits();
        }

        #endregion

        #region Private Methods

        private void EvaluateLimits()
        {
            var config = (Series.Configuration ?? Series.SeriesCollection.Configuration) as SeriesConfiguration<T>;
            if (config == null) return;

            var q = IndexData(config).ToArray();

            var xs = q.Select(t => config.XValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var xMax = xs.Where(x => !double.IsNaN(x)).Max();
            var xMin = xs.Where(x => !double.IsNaN(x)).Min();

            var ys = q.Select(t => config.YValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var yMax = ys.Where(x => !double.IsNaN(x)).Max();
            var yMin = ys.Where(x => !double.IsNaN(x)).Min();

            MaxChartPoint = new LvcPoint(xMax, yMin);
            MinChartPoint = new LvcPoint(xMin, yMin);
        }

        private IEnumerable<KeyValuePair<int, T>> IndexData(SeriesConfiguration<T> config)
        {
            var f = config.Chart.PivotZoomingAxis == AxisTags.X
                ? config.XValueMapper
                : config.YValueMapper;

            var isObservable = typeof(IObservableChartPoint).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo());

            var i = 0;

            foreach (var t in this)
            {
                if (isObservable)
                {
                    var observable = t as IObservableChartPoint;
                    if (observable != null)
                    {
                        observable.PointChanged -= ObservableOnPointChanged;
                        observable.PointChanged += ObservableOnPointChanged;
                    }
                }
                yield return new KeyValuePair<int, T>(i, t);
                i++;
            }
        }

        private void ObservableOnPointChanged(object caller)
        {
            //var mapper = Series.SeriesCollection.Chart.ShapesMapper;
            //var updatedPoint = mapper.FirstOrDefault(x => x.ChartPoint.Instance == caller);
            //if (updatedPoint != null)
            //{
            //    var config = (Series.Configuration ?? Series.Collection.Configuration) as SeriesConfiguration<T>;
            //    if (config != null)
            //    {
            //        updatedPoint.ChartPoint.X = config.XValueMapper((T)caller, updatedPoint.ChartPoint.Key);
            //        updatedPoint.ChartPoint.Y = config.YValueMapper((T)caller, updatedPoint.ChartPoint.Key);

            //        //test
            //        var mapedPoint = Series.Collection.Chart.ShapesMapper
            //            .FirstOrDefault(x => updatedPoint.ChartPoint == x.ChartPoint);
            //    }
            //}
            Series.Chart.Updater.Run(false);
        }

        #endregion

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}