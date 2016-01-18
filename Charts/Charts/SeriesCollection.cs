using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using lvc.Charts;

namespace lvc
{
    public class SeriesCollection<T> : ObservableCollection<Series>, ISeriesCollection
    {
        private int _xIndexer;
        private int _yIndexer;

        public SeriesCollection()
        {
            XValueMapper = (value, index) => index;
            OptimizationMethod = values =>
            {
                _xIndexer = 0;
                _yIndexer = 0;
                return values.Select(v => new Point(XValueMapper(v, _xIndexer), YValueMapper(v, _yIndexer)));
            };
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (var series in args.NewItems.Cast<Series>())
                        series.Collection = this;
            };
        }

        /// <summary>
        /// Gets max chart point
        /// </summary>
        public Point MaxChartPoint { get; }
        /// <summary>
        /// Gets min chart point
        /// </summary>
        public Point MinChartPoint { get; }
        /// <summary>
        /// Gets or sets chart
        /// </summary>
        public Chart Chart { get; set; }
        /// <summary>
        /// Gets or sets optimization method
        /// </summary>
        public Func<IEnumerable<T>, IEnumerable<Point>> OptimizationMethod { get; set; }

        /// <summary>
        /// Gets X labels
        /// </summary>
        public IEnumerable<KeyValuePair<double, string>> XLabels
        {
            get
            {
                for (var i = MinChartPoint.X; i <= MaxChartPoint.X; i += Chart.S.X)
                    yield return new KeyValuePair<double, string>(i, XLabelMapper(i));
            }
        }
        /// <summary>
        /// Gets Y labels
        /// </summary>
        public IEnumerable<KeyValuePair<double, string>> YLabels
        {
            get
            {
                for (var i = MinChartPoint.Y; i <= MaxChartPoint.Y; i += Chart.S.Y)
                    yield return new KeyValuePair<double, string>(i, YLabelMapper(i));
            }
        }

        /// <summary>
        /// Gets or sets the current function that pulls X value from T
        /// </summary>
        public Func<T, int, double> XValueMapper { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls Y value from T
        /// </summary>
        public Func<T, int, double> YValueMapper { get; set; }

        /// <summary>
        /// Get or sets a function that pulls X labels
        /// </summary>
        public Func<double, string> XLabelMapper { get; set; }

        /// <summary>
        /// Gets or sets a functions that pulls Y labels
        /// </summary>
        public Func<double, string> YLabelMapper { get; set; }

        /// <summary>
        /// Maps X value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> X(Func<T, double> predicate)
        {
            XValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> X(Func<T, int, double> predicate)
        {
            XValueMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> Y(Func<T, double> predicate)
        {
            YValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Max X Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> Y(Func<T, int, double> predicate)
        {
            YValueMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps X Labels
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> XLabel(Func<double, string> predicate)
        {
            XLabelMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps X Labels
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> XLabel(Func<T, string> predicate)
        {
            XLabelMapper = x =>
            {
                //var p = _points.FirstOrDefault(pt => Math.Abs(pt.X - x) < Chart.S.X);
                //var list = this as IList;
                //var index = (int)Math.Round(x);
                //var t = (T)list[index];
                //if (t == null) return null;
                return null;
            };
            return this;
        }

        /// <summary>
        /// Maps Y labels
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> YLabel(Func<double, string> predicate)
        {
            YLabelMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Labels
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesCollection<T> YLabel(Func<T, string> predicate)
        {
            XLabelMapper = x =>
            {
                //var list = this as IList;
                //var index = (int)Math.Round(x);
                //var t = (T)list[index];
                //if (t == null) return null;
                //return predicate(t);
                return null;
            };
            return this;
        }

        public SeriesCollection<T> HasOptimization(Func<IEnumerable<T>, IEnumerable<Point>> predicate)
        {
            OptimizationMethod = predicate;
            return this;
        } 

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}

