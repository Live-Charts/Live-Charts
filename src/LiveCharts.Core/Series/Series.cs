using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// Defines a data series in a chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public abstract class Series<TModel> : IChartSeries
    {
        private object _chartPointsUpdateId;
        private IEnumerable<ChartPoint> _chartPoints;
        private IEnumerable<TModel> _values;
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel}"/> class.
        /// </summary>
        /// <param name="chartPointType">Type of the chart point.</param>
        /// <param name="defaultFillOpacity">the series default fill opacity.</param>
        /// <param name="skipCriteria">the skip criteria.</param>
        protected Series(
            ChartPointTypes chartPointType, double defaultFillOpacity, SeriesSkipCriteria skipCriteria)
        {
            _chartPointType = chartPointType;
            _defaultFillOpacity = defaultFillOpacity;
            _skipCriteria = skipCriteria;
            Fill = Color.Empty;
            Stroke = Color.Empty;
            TrackingMode = SeriesTrackingModes.ByReference;
        }

        /// <summary>
        /// Gets or sets the values to plot.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<TModel> Values
        {
            get => _values;
            set
            {
                // unsubscribe previous instance from INCC.
                if (_values is INotifyCollectionChanged previousIncc)
                {
                    previousIncc.CollectionChanged -= OnValuesCollectionChanged;
                }

                // assign the new instance
                _values = value;

                // subscribe new instance to INCC.
                if (_values is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged += OnValuesCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets the tracking mode.
        /// </summary>
        /// <value>
        /// The tracking mode.
        /// </value>
        public SeriesTrackingModes TrackingMode { get; set; }

        private readonly double _defaultFillOpacity;
        double IChartSeries.DefaultFillOpacity => _defaultFillOpacity;

        private readonly SeriesSkipCriteria _skipCriteria;
        SeriesSkipCriteria IChartSeries.SkipCriteria => _skipCriteria;

        private readonly ChartPointTypes _chartPointType;
        ChartPointTypes IChartSeries.ChartPointType => _chartPointType;

        /// <summary>
        /// Gets the charts that are using this series.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public IEnumerable<ChartModel> UsedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Series"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the type builder, it defines the way the type is mapped to the chart.
        /// </summary>
        /// <value>
        /// The type builder.
        /// </value>
        public IChartingTypeBuilder TypeBuilder { get; set; }

        /// <inheritdoc cref="IChartSeries.Stroke"/>
        public Color Stroke { get; set; }

        /// <inheritdoc cref="IChartSeries.Fill"/>
        public Color Fill { get; set; }

        /// <inheritdoc cref="IChartSeries.ScaleAtByDimension"/>
        protected int[] ScalesAt { get; set; }
        int[] IChartSeries.ScaleAtByDimension => ScalesAt;

        /// <inheritdoc cref="IChartSeries.ScaleAtByDimension"/>
        public DimensionRange DataRange { get; } = new DimensionRange(double.PositiveInfinity, double.NegativeInfinity);

        Dictionary<ChartModel, Dictionary<object, ChartPoint>> IChartSeries.ValuePointDictionary { get; }
            = new Dictionary<ChartModel, Dictionary<object, ChartPoint>>();

        /// <summary>
        /// Fetches the data for a given chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public IEnumerable<ChartPoint> FetchData(ChartModel chart)
        {
            // returned cached points if this method was called from the same updateId.
            if (_chartPointsUpdateId == chart.UpdateId) return _chartPoints;
            _chartPointsUpdateId = chart.UpdateId;
        
            // Assign a color if the user did not set it.
            if (Stroke == Color.Empty || Fill == Color.Empty)
            {
                var nextColor = chart.GetNextColor();
                if (Stroke == Color.Empty)
                    Stroke = nextColor;
                if (Fill == Color.Empty)
                    Fill = nextColor.SetOpacity(((IChartSeries) this).DefaultFillOpacity);
            }

            // call the factory to fetch our data.
            // Fetch() has 2 main tasks.
            // 1. Calculate each ChartPoint required by the series.
            // 2. Evaluate every dimension and every axis to get Max and Min limits.
            _chartPoints = LiveCharts.Options.PointFactory
                .Fetch(new DataFactoryArgs
                {
                    Series = this,
                    Chart = chart,
                    Collection = new List<TModel>(Values),
                    CollectionItemsType = typeof(TModel),
                    PropertyChangedEventHandler = OnValuesItemPropertyChanged
                });
            return _chartPoints;
        }

        /// <param name="chart"></param>
        /// <inheritdoc cref="IChartSeries.UpdateView"/>
        void IChartSeries.UpdateView(ChartModel chart)
        {
            OnUpdateView(chart);
        }

        /// <inheritdoc cref="IChartSeries.UpdateView"/>
        protected virtual void OnUpdateView(ChartModel chart)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_values is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged -= OnValuesCollectionChanged;
            }

            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TModel));
            if (!notifiesChange) return;
            foreach (var value in _values)
            {
                var npc = (INotifyPropertyChanged) value;
                npc.PropertyChanged -= OnValuesItemPropertyChanged;
            }
            _values = null;
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
            ((IChartSeries) this).ValuePointDictionary.Add(chart, new Dictionary<object, ChartPoint>());
        }

        private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (var chartModel in UsedBy)
            {
                chartModel.Invalidate();
            }
        }

        private void OnValuesItemPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            foreach (var chartModel in UsedBy)
            {
                chartModel.Invalidate();
            }
        }
    }
}
