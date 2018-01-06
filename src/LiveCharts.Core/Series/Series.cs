using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Data.Points;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;

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
        /// <param name="type">the series type.</param>
        protected Series(string type)
        {
            Defaults = LiveCharts.Options.HasSeriesDefault(type);
            Geometry = Geometry.Empty;
            Type = type;
            Fill = Color.Empty;
            Stroke = Color.Empty;
            TrackingMode = SeriesTrackingModes.ByReference;
        }

        /// <summary>
        /// Gets or sets the values.
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

        /// <inheritdoc cref="IChartSeries.Defaults"/>
        public SeriesMetadata Defaults { get; set; }

        /// <summary>
        /// Gets the charts that are using this series.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public IEnumerable<ChartModel> UsedBy { get; internal set; }

        /// <inheritdoc cref="IChartSeries.Geometry"/>
        public Geometry Geometry { get; set; }

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

        /// <inheritdoc cref="IChartSeries.Title"/>
        public string Title { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <inheritdoc cref="IChartSeries.StrokeThickness"/>
        public double StrokeThickness { get; set; }

        /// <inheritdoc cref="IChartSeries.Stroke"/>
        public Color Stroke { get; set; }

        /// <inheritdoc cref="IChartSeries.Fill"/>
        public Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the index of the z.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        public int ZIndex { get; set; }

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
                    Fill = nextColor.SetOpacity(((IChartSeries) this).Defaults.FillOpacity);
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
