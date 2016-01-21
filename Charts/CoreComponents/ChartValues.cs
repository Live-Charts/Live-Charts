//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.Windows;
using LiveCharts.Charts;
namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : ObservableCollection<T>, IChartValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);
        private ChartPoint[] _points;

        public ChartValues()
        {
            CollectionChanged += (sender, args) =>
            {
                RequiresEvaluation = true;
            };
        }

        #region Properties

        /// <summary>
        /// Chart that owns the values
        /// </summary>
        public Chart Chart { get; set; }

        public IChartSeries Series { get; set; }
       
        /// <summary>
        /// Gets the collection of points displayed in the chart current view
        /// </summary>
        public IEnumerable<ChartPoint> Points
        {
            get { return _points; }
        }

        /// <summary>
        /// Gets Max X and Y coordinates in chart
        /// </summary>
        public Point MaxChartPoint
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets Min X and Y coordintes in chart
        /// </summary>
        public Point MinChartPoint
        {
            get { return _min; }
        }
        public bool RequiresEvaluation { get; set; }

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
        public void Evaluate()
        {
            if (RequiresEvaluation)
            {
                EvalueateValues();
                RequiresEvaluation = false;
            }
        }

        #endregion

        #region Private Methods

        private void EvalueateValues()
        {
            var config = (Series.Configuration ?? Series.Collection.Configuration) as SeriesConfiguration<T>;
            if (config == null) return;

            var collection = Series == null ? null : Series.Collection;
            _points = collection == null
                ? new ChartPoint[] {}
                : config.OptimizationMethod(this).ToArray();

            var xs = _points.Select(x => x.X).DefaultIfEmpty(0).ToArray();

            var xMax = xs.Max();
            var xMin = xs.Min();

            _min.X = xMin;
            _max.X = xMax;

            var ys = _points.Select(x => x.Y).DefaultIfEmpty(0).ToArray();

            var yMax = ys.Max();
            var yMin = ys.Min();

            _min.Y = yMin;
            _max.Y = yMax;
        }

        #endregion

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}