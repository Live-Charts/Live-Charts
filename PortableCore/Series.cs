using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class Series : ISeriesModel
    {
        public Series()
        {
        }

        public Series(SeriesConfiguration configuration)
        {
            
        }
    }

    public interface ISeriesModel
    {
        
    }

    public interface ISeriesView
    {
        ISeriesModel Model { get; }
    }

    public interface IChartValues : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets series points to draw.
        /// </summary>
        IEnumerable<ChartPoint> Points { get; }
        /// <summary>
        /// Gets max X and Y values
        /// </summary>
        Point MaxChartPoint { get; }
        /// <summary>
        /// Gets min X and Y values
        /// </summary>
        Point MinChartPoint { get; }
        /// <summary>
        /// Gets or sets series that owns the values
        /// </summary>
        IChartSeries Series { get; set; }
        /// <summary>
        /// Forces values to calculate max, min and index data.
        /// </summary>
        void GetLimits();
        /// <summary>
        /// Gets or sets if values requires to calculate max, min and index data.
        /// </summary>
        bool RequiresEvaluation { get; set; }
    }

    public struct Point
    {
        public Point(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ChartPoint
    {
        /// <summary>
        /// Gets the X point value
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Gets the Y point value
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Gets the coordinate where the value is placed at chart
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Gets the object where the chart pulled the point
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// Gets the index of this point in the chart
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// Gets is point is mocked
        /// </summary>
        public bool IsMocked { get; set; }
    }

    public interface IChartSeries
    {
        /// <summary>
        /// Gets or sets series values to plot.
        /// </summary>
        IChartValues Values { get; set; }
        /// <summary>
        /// Collection that owns the series.
        /// </summary>
        SeriesCollection Collection { get; }
        /// <summary>
        /// Gets or sets Series Configuration
        /// </summary>
        SeriesConfiguration Configuration { get; set; }
    }

    public class SeriesCollection : ObservableCollection<IChartSeries>
    {
        public SeriesCollection()
        {
            Configuration = new SeriesConfiguration<double>().X((v, i) => i).Y(v => v);
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (var series in args.NewItems.Cast<Series>())
                        series.Collection = this;
            };
        }

        public SeriesCollection(SeriesConfiguration configuration)
        {
            Configuration = configuration;
        }

        public SeriesConfiguration Configuration { get; set; }

        /// <summary>
        /// Setup a configuration for this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public SeriesCollection Setup<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                base.CollectionChanged += value;

            }
            remove { base.CollectionChanged -= value; }
        }
    }

    public class SeriesConfiguration
    {
        
    }

    public class SeriesConfiguration<T> : SeriesConfiguration
    {
        public SeriesConfiguration()
        {
            XValueMapper = (value, index) => index;
            YValueMapper = (value, index) => index;
        }

        /// <summary>
        /// Gets or sets optimization method
        /// </summary>
        public IDataOptimization<T> DataOptimization { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls X value from T
        /// </summary>
        internal Func<T, int, double> XValueMapper { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls Y value from T
        /// </summary>
        internal Func<T, int, double> YValueMapper { get; set; }

        /// <summary>
        /// Maps X value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, double> predicate)
        {
            XValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, int, double> predicate)
        {
            XValueMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, double> predicate)
        {
            YValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Max X Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, int, double> predicate)
        {
            YValueMapper = predicate;
            return this;
        }

        public SeriesConfiguration<T> HasHighPerformanceMethod(IDataOptimization<T> optimization)
        {
            DataOptimization = optimization;
            return this;
        }
    }

    public interface IDataOptimization<T>
    {
        Func<T, int, double> XMapper { get; set; }
        Func<T, int, double> YMapper { get; set; }
        IEnumerable<ChartPoint> Run(IEnumerable<KeyValuePair<int, T>> data);
    }
}
