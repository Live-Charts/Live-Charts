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
            get
            {
                var index = 0;
                var collection = Series.SeriesCollection as SeriesCollection<T>;
                if (collection == null) return Enumerable.Empty<Point>();
                return this.Select(x => new Point(index++, collection.XValueMapper()));
            }
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
            _points = Points.ToArray();

            var xs = _points.Select(x => x.X).ToArray();

            var xMax = xs.Max();
            var xMin = xs.Min();

            _min.X = xMin;
            _max.X = xMax;

            var ys = _points.Select(x => x.Y).ToArray();

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

    public class IndexedChartValues : ObservableCollection<double>, IChartValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);

        public IndexedChartValues()
        {
            CollectionChanged += OnCollectionChanged;
        }

        public IEnumerable<KeyValuePair<double, string>> XLabels { get; }
        public IEnumerable<KeyValuePair<double, string>> YLabels { get; }

        public IEnumerable<Point> Points
        {
            get
            {
                var index = 0;
                return this.Select(x => new Point(index++, x));
            }
        }

        public Point MaxChartPoint { get { return _max; } }
        public Point MinChartPoint { get { return _min; } }

        public Chart Chart { get; set; }

        public void AddRange(IEnumerable<double> collection)
        {
            CheckReentrancy();
            foreach (var item in collection) Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public sealed override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                var values = sender as IEnumerable<double>;
                if (values == null) return;
                var valArr = values.ToArray();
                var max = valArr.Max();
                var min = valArr.Min();
                _min.Y = min < _min.Y ? min : _min.Y;
                _max.Y = max > _max.Y ? max : _max.Y;
                _min.X = 0;
                _max.X = Count;
                return;
            }

            if (args.NewItems != null)
            {
                var values = args.NewItems.Cast<double>().Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).ToArray();
                var max = values.Max();
                var min = values.Min();
                if (_max.Y < max) _max.Y = max;
                if (_min.Y > min) _min.Y = min;
            }
            if (args.OldItems != null)
            {
                var values = args.OldItems.Cast<double>().Where(x => !double.IsNaN(x)).DefaultIfEmpty(0).ToArray();
                var max = values.Max();
                var min = values.Min();
                if (Math.Abs(_max.Y - max) < Chart.S.Y * .01) _max.Y = Points.Select(x => x.Y).DefaultIfEmpty().Max();
                if (Math.Abs(_min.Y - min) < Chart.S.Y * .01) _min.Y = Points.Select(x => x.Y).DefaultIfEmpty().Min();
            }
            _min.X = 0;
            _max.X = Count;
        }
    }
}