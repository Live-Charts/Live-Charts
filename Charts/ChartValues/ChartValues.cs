using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using lvc.Charts;

namespace lvc
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : ObservableCollection<T>, IChartValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);
        private string _title;
        private Point[] _points;

        public ChartValues()
        {
            CollectionChanged += OnCollectionChanged;
        }

        #region Properties

        /// <summary>
        /// Chart that owns the values
        /// </summary>
        public Chart Chart { get; set; }

        public IChartSeries Series { get; internal set; }

        /// <summary>
        /// Gets the collection of points displayed in the chart current view
        /// </summary>
        public IEnumerable<Point> Points
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

        /// <summary>
        /// Gets or sets chart Title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

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

        

        public ChartValues<T> WithTitle(string title)
        {
            Title = Title;
            return this;
        }

        #endregion

        #region Private Methods

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            EvaluateAllPoints();
        }

        private void EvaluateAllPoints()
        {
            var collection = Series == null ? null : Series.Collection as SeriesCollection<T>;
            _points = collection == null
                ? new Point[] {}
                : collection.OptimizationMethod(this).ToArray();

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