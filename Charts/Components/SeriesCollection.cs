using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using lvc.Charts;

namespace lvc
{
    public class SeriesCollection : ObservableCollection<Series>
    {
        public SeriesCollection()
        {
            Configuration = new SeriesConfiguration<double>();
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (var series in args.NewItems.Cast<Series>())
                        series.Collection = this;
            };
        }

        public SeriesCollection(ISeriesConfiguration configuration)
        {
            Configuration = configuration;
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

        public ISeriesConfiguration Configuration { get; set; }
        /// <summary>
        /// Gets X labels
        /// </summary>
        public IEnumerable<KeyValuePair<double, string>> XLabels
        {
            get
            {
                for (var i = MinChartPoint.X; i <= MaxChartPoint.X; i += Chart.S.X)
                    yield return new KeyValuePair<double, string>(1,""); // new KeyValuePair<double, string>(i, Configuration.XLabelMapper(i));
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
                    yield return new KeyValuePair<double, string>(1,""); //new KeyValuePair<double, string>(i, Configuration.YLabelMapper(i));
            }
        }

        public SeriesCollection For<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}

