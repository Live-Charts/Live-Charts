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

        public ChartValues()
        {
            CollectionChanged += OnCollectionChanged;
        }

        internal Chart Chart { get; set; }
        public IEnumerable<Point> Points
        {
            get
            {
                return Optimize();
            }
        }
        public Point Max { get { return _max; } }
        public Point Min { get { return _min; } }
        public Func<T, int, double> XMapp { get; set; }
        public Func<T, int, double> YMapp { get; set; }
        protected Func<ChartValues<T>, IEnumerable<Point>> Optimization { get; set; } 

        public void AddRange(IEnumerable<T> collection)
        {
            CheckReentrancy();
            foreach (var item in collection) Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public ChartValues<T> PullX(Func<T, double> predicate)
        {
            XMapp = (x, i) => predicate(x);
            return this;
        }
        public ChartValues<T> PullX(Func<T, int, double> predicate)
        {
            XMapp = predicate;
            return this;
        }
        public ChartValues<T> PullY(Func<T, double> predicate)
        {
            YMapp = (x,i) => predicate(x);
            return this;
        }
        public ChartValues<T> PullY(Func<T, int, double> predicate)
        {
            YMapp = predicate;
            return this;
        }

        public ChartValues<T> HasOptimization(Func<ChartValues<T>, IEnumerable<Point>> optimization)
        {
            Optimization = optimization;
            return this;
        } 

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (XMapp == null || YMapp == null)
                throw new InvalidEnumArgumentException("XMapp and YMapp properties can not be null");

            var optimized = Optimize().ToArray();

            var xs = optimized.Select(x => x.X).ToArray();

            var xMax = xs.Max();
            var xMin = xs.Min();

            _min.X = xMin < _min.X ? xMin : _min.X;
            _max.X = xMax > _max.X ? xMax : _max.X;

            var ys = optimized.Select(x => x.Y).ToArray();

            var yMax = ys.Max();
            var yMin = ys.Min();

            _min.Y = yMin < _min.Y ? yMin : _min.Y;
            _max.Y = yMax > _max.Y ? yMax : _max.Y;
        }

        private IEnumerable<Point> Optimize()
        {
            _xIndexer = 0;
            _yIndexer = 0;
            return Optimization == null
                ? this.Select(value => new Point(XMapp(value, _xIndexer++), YMapp(value, _yIndexer++)))
                : Optimization(this);
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

        public Point Max { get { return _max; } }
        public Point Min { get { return _min; } }

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