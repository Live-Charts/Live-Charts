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
    public class IndexedChartValues : ObservableCollection<double>, IChartsValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);

        public IndexedChartValues()
        {
            CollectionChanged += OnCollectionChanged;
        }

        internal Chart Chart { get; set; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
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
                if (Math.Abs(_max.Y - max) < Chart.S.Y*.01) _max.Y = Points.Select(x => x.Y).DefaultIfEmpty().Max();
                if (Math.Abs(_min.Y - min) < Chart.S.Y*.01) _min.Y = Points.Select(x => x.Y).DefaultIfEmpty().Min();
            }
            _min.X = 0;
            _max.X = Count;
        }

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
    }

    public class SeriesValues<T> : ObservableCollection<T>, IChartsValues where T : class
    {
        public Func<T, double> XMapper { get; set; }
        public Func<T, double> YMapper { get; set; }

        public IEnumerable<Point> Points
        {
            get
            {
                //by default X value will be current index;
                var index = 0;

                if (YMapper == null)
                    throw new InvalidEnumArgumentException("You need to specify XMapper property before ploting");

                return XMapper == null
                    ? this.Select(x => new Point(index++, YMapper(x)))
                    : this.Select(x => new Point(XMapper(x), YMapper(x)));
            }
        }

        public Point Max { get; }
        public Point Min { get; }
        public Point S { get; }

        public SeriesValues<T> MapX(Func<T, double> mapper)
        {
            XMapper = mapper;
            return this;
        }

        public SeriesValues<T> MapY(Func<T, double> mapper)
        {
            YMapper = mapper;
            return this;
        }
    }
}