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
    public class ChartValues<T> : ObservableCollection<T>, IChartValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);
        private int _xIndexer;
        private int _yIndexer;
        private bool _hasChanged;
        private string _title;
        private Func<T, int, double> _xMapper;
        private Func<T, int, double> _yMapper;

        public ChartValues()
        {
            CollectionChanged += OnCollectionChanged;
            XMapper = (x, i) => i;
        }

        #region Properties
        internal Chart Chart { get; set; }
        public IEnumerable<Point> Points
        {
            get
            {
                return Optimize();
            }
        }
        public Point MaxChartPoint { get { return _max; } }
        public Point MinChartPoint { get { return _min; } }

        public Func<T, int, double> XMapper
        {
            get { return _xMapper; }
            set
            {
                _xMapper = value;
                if (_hasChanged && value != null && YMapper != null) EvaluateAllPoints();
            }
        }

        public Func<T, int, double> YMapper
        {
            get { return _yMapper; }
            set
            {
                _yMapper = value;
                if (_hasChanged && value != null && XMapper != null) EvaluateAllPoints();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        protected Func<ChartValues<T>, IEnumerable<Point>> Optimization { get; set; }
        #endregion

        public ChartValues<T> AddRange(IEnumerable<T> collection)
        {
            CheckReentrancy();
            foreach (var item in collection) Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return this;
        }

        public ChartValues<T> X(Func<T, double> predicate)
        {
            XMapper = (x, i) => predicate(x);
            return this;
        }
        public ChartValues<T> X(Func<T, int, double> predicate)
        {
            XMapper = predicate;
            return this;
        }
        public ChartValues<T> Y(Func<T, double> predicate)
        {
            YMapper = (x,i) => predicate(x);
            return this;
        }
        public ChartValues<T> Y(Func<T, int, double> predicate)
        {
            YMapper = predicate;
            return this;
        }

        public ChartValues<T> HasOptimization(Func<ChartValues<T>, IEnumerable<Point>> optimization)
        {
            Optimization = optimization;
            return this;
        }

        public ChartValues<T> WithTitle(string title)
        {
            Title = Title;
            return this;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (XMapper == null || YMapper == null)
            {
                _hasChanged = true;
                return;
            }
            EvaluateAllPoints();   
        }

        private IEnumerable<Point> Optimize()
        {
            _xIndexer = 0;
            _yIndexer = 0;
            return Optimization == null
                ? this.Select(value => new Point(XMapper(value, _xIndexer++), YMapper(value, _yIndexer++)))
                : Optimization(this);
        }

        private void EvaluateAllPoints()
        {
            var optimized = Optimize().ToArray();

            var xs = optimized.Select(x => x.X).ToArray();

            var xMax = xs.Max();
            var xMin = xs.Min();

            _min.X = xMin;
            _max.X = xMax;

            var ys = optimized.Select(x => x.Y).ToArray();

            var yMax = ys.Max();
            var yMin = ys.Min();

            _min.Y = yMin;
            _max.Y = yMax;
        }

        public sealed override event NotifyCollectionChangedEventHandler CollectionChanged
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

        internal Chart Chart { get; set; }

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