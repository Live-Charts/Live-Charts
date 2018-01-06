using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    public class CartesianChart : Control, IChartView
    {
        static CartesianChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CartesianChart),
                new FrameworkPropertyMetadata(typeof(CartesianChart)));
        }

        public CartesianChart()
        {
            _chartModel = new CartesianChartModel(this);
            SetValue(SeriesProperty, new ObservableCollection<IChartSeries>());
            SetValue(XAxisProperty, new ObservableCollection<Plane>());
            SetValue(XAxisProperty, new ObservableCollection<Plane>());
            Initialized += OnInitialized;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public IEnumerable<IChartSeries> Series
        {
            get => (IEnumerable<IChartSeries>) GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Gets or sets the x axis.
        /// </summary>
        /// <value>
        /// The x axis.
        /// </value>
        public IEnumerable<Plane> XAxis
        {
            get => (IEnumerable<Plane>) GetValue(XAxisProperty);
            set => SetValue(XAxisProperty, value);
        }

        /// <summary>
        /// Gets or sets the y axis.
        /// </summary>
        /// <value>
        /// The y axis.
        /// </value>
        public IEnumerable<Plane> YAxis
        {
            get => (IEnumerable<Plane>) GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        /// <inheritdoc cref="IChartView.DisableAnimations"/>
        public bool DisableAnimations
        {
            get => (bool) GetValue(DisableAnimationsProperty);
            set => SetValue(DisableAnimationsProperty, value);
        }

        /// <inheritdoc cref="IChartView.AnimationsSpeed"/>
        public TimeSpan AnimationsSpeed
        {
            get => (TimeSpan) GetValue(AnimationsSpeedProperty);
            set => SetValue(AnimationsSpeedProperty, value);
        }

        /// <inheritdoc cref="IChartView.Legend"/>
        public IChartLegend ChartLegend { get; set; }
        IChartLegend IChartView.Legend => ChartLegend;

        #endregion

        #region Dependency properties

        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series), typeof(IEnumerable<IChartSeries>), typeof(CartesianChart),
            new PropertyMetadata(null, GenerateInstanceChanged(p => p.Series, nameof(Series))));

        /// <summary>
        /// The x axis property.
        /// </summary>
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(
            nameof(XAxis), typeof(IEnumerable<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, GenerateInstanceChanged(p => p.XAxis, nameof(XAxis))));

        /// <summary>
        /// The y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(
            nameof(YAxis), typeof(IEnumerable<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, GenerateInstanceChanged(p => p.YAxis, nameof(YAxis))));

        /// <summary>
        /// The disable animations property, default value is true.
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            nameof(DisableAnimations), typeof(bool), typeof(CartesianChart),
            new PropertyMetadata(false, ChartUpdaterFreqChangedCallback));

        /// <summary>
        /// The animations speed property, default value is 250 milliseconds.
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            nameof(AnimationsSpeed), typeof(TimeSpan), typeof(CartesianChart),
            new PropertyMetadata(TimeSpan.FromMilliseconds(250), ChartUpdaterFreqChangedCallback));

        #endregion

        #region Dependency properties callbacks

        private static void ChartUpdaterFreqChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = (CartesianChart) sender;
            chart.UpdaterFrequencyChanged?.Invoke(chart.AnimationsSpeed);
        }

        private static PropertyChangedCallback GenerateInstanceChanged(
            Func<CartesianChart, object> getter, string propertyName)
        {
            return (sender, args) =>
            {
                var chart = (CartesianChart) sender;
                chart.PropertyInstanceChanged?.Invoke(getter(chart), propertyName);
            };
        }

        #endregion

        #region IChartView Implementation

        private readonly ChartModel _chartModel;
        ChartModel IChartView.ChartModel => _chartModel;
        Size IChartView.ControlSize => new Size((int) ActualWidth, (int) ActualHeight);
        Margin IChartView.DrawMargin { get; set; }
        private object _dimensionsUpdateId;
        private IList<IList<Plane>> _dimensions;
        IList<IList<Plane>> IChartView.AxisArrayByDimension
        {
            get
            {
                if (_chartModel.UpdateId == _dimensionsUpdateId) return _dimensions;
                _dimensions = new List<IList<Plane>>
                {
                    XAxis.Select(axis => new Plane(PlaneTypes.X)).ToList(),
                    YAxis.Select(axis => new Plane(PlaneTypes.Y)).ToList()
                };
                _dimensionsUpdateId = _chartModel.UpdateId;
                return _dimensions;
            }
        }

        private object _seriesUpdateId;
        private IList<IChartSeries> _series;
        IEnumerable<IChartSeries> IChartView.Series
        {
            get
            {
                if (_chartModel.UpdateId == _seriesUpdateId) return _series;
                _series = new List<IChartSeries>(Series);
                _seriesUpdateId = _chartModel.UpdateId;
                return _series;
            }
        }

        TimeSpan IChartView.AnimationsSpeed => AnimationsSpeed;
        public event Action ChartViewInitialized;
        public event PropertyInstanceChangedHandler PropertyInstanceChanged;
        public event ChartUpdaterfrequencyChangedHandler UpdaterFrequencyChanged;

        #endregion

        #region Private methods

        private void OnInitialized(object sender, EventArgs eventArgs)
        {
            ChartViewInitialized?.Invoke();
        }

        #endregion
    }
}
