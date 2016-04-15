using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class ChartModel : IChartModel
    {
        private readonly IChartView _view;

        public ChartModel(IChartView view)
        {
            _view = view;
        }

        public int ColorIndex { get; set; }
        public SeriesCollection Series { get; set; }

        public void Update(bool restartsAnimation = false)
        {
            DataStep.RestartAnimation = DataStep.RestartAnimation || restartsAnimation;
            if (DataStep.IsRunning) return;

            Task.Delay(100).ContinueWith(t =>
            {
                _view.PrepareCanvas(DataStep.RestartAnimation);
                _view.UpdateSeries();
            });
        }

        public DataStep DataStep { get; set; }
    }

    /// <summary>
    /// Defines the chart core model
    /// </summary>
    public interface IChartModel
    {
        int ColorIndex { get; set; }

        SeriesCollection Series { get; set; }



        void Update(bool restartsAnimation = false);
    }

    /// <summary>
    /// Defines the chart core view
    /// </summary>
    public interface IChartView
    {
        void PrepareCanvas(bool restartAnimation);
        void UpdateSeries();
    }

    public class DataStep
    {
        public bool RestartAnimation { get; set; }
        public bool IsRunning { get; set; }
    }

    public interface ISeriesModel
    {
        /// <summary>
        /// Gets or sets the chart that own the Series
        /// </summary>
        IChartModel Chart { get; set; }
        /// <summary>
        /// Gets or sets series values to plot.
        /// </summary>
        IChartValues Values { get; set; }
        /// <summary>
        /// Collection that owns the series.
        /// </summary>
        SeriesCollection Collection { get; set; }
        /// <summary>
        /// Gets or sets Series Configuration
        /// </summary>
        ISeriesConfiguration Configuration { get; set; }
    }

    public class SeriesCollection : ObservableCollection<ISeriesModel>
    {
        public SeriesCollection(ISeriesConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets owner chart.
        /// </summary>
        public IChartModel Chart { get; set; }

        public ISeriesConfiguration Configuration { get; set; }

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

    public class SeriesConfiguration<T> : ISeriesConfiguration
    {
        public SeriesConfiguration()
        {
            XValueMapper = (value, index) => index;
            YValueMapper = (value, index) => index;
        }

        /// <summary>
        /// Gets or Sets the chart 
        /// </summary>
        public IChartModel Chart { get; set; }

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
        IChartModel Chart { get; set; }
        Func<T, int, double> XMapper { get; set; }
        Func<T, int, double> YMapper { get; set; }
        IEnumerable<ChartPoint> Run(IEnumerable<KeyValuePair<int, T>> data);
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
        ISeriesModel Series { get; set; }
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

    public interface ISeriesConfiguration
    {
        IChartModel Chart { get; set; }
    }
}
