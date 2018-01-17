using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Series;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    public abstract class Chart : UserControl, IChartView
    {
        protected Chart()
        {
            DrawArea = new Canvas();
            Canvas = new Canvas();
            Canvas.Children.Add(DrawArea);
            Initialized += OnInitialized;
        }

        public Canvas Canvas { get; set; }
        public Canvas DrawArea { get; }

        #region Dependency properties

        /// <summary>
        /// The series property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series), typeof(IEnumerable<Series>), typeof(Chart),
            new PropertyMetadata(null, BuildInstanceChangedCallback(p => p.Series, nameof(Series))));

        /// <summary>
        /// The animations speed property, default value is 250 milliseconds.
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            nameof(AnimationsSpeed), typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(TimeSpan.FromMilliseconds(250), ChartUpdaterFreqChangedCallback));

        /// <summary>
        /// The legend property, default is DefaultLegend class.
        /// </summary>
        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(
            nameof(Legend), typeof(ILegend), typeof(Chart),
            new PropertyMetadata(null));

        /// <summary>
        /// The legend position property
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            nameof(LegendPosition), typeof(LegendPositions), typeof(Chart),
            new PropertyMetadata(LegendPositions.None));

        #endregion

        #region private and protected methods

        /// <summary>
        /// Builds the instance changed callback.
        /// </summary>
        /// <param name="getter">The getter.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected static PropertyChangedCallback BuildInstanceChangedCallback(
            Func<CartesianChart, object> getter, string propertyName)
        {
            return (sender, args) =>
            {
                var chart = (CartesianChart) sender;
                chart.DataInstanceChanged?.Invoke(getter(chart), propertyName);
            };
        }

        /// <summary>
        /// The updater freq changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void ChartUpdaterFreqChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = (CartesianChart) sender;
            chart.UpdaterFrequencyChanged?.Invoke(chart.AnimationsSpeed);
        }

        /// <summary>
        /// Gets the planes in the current chart.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual IList<IList<Plane>> GetPlanes()
        {
            throw new NotImplementedException();
        }

        private void OnInitialized(object sender, EventArgs eventArgs)
        {
            ChartViewInitialized?.Invoke();
        }

        #endregion

        #region IChartView implementation

        /// <inheritdoc cref="IChartView.ChartViewInitialized"/>
        public event Action ChartViewInitialized;

        /// <inheritdoc cref="IChartView.DataInstanceChanged"/>
        public event PropertyInstanceChangedHandler DataInstanceChanged;

        /// <inheritdoc cref="IChartView.UpdaterFrequencyChanged"/>
        public event ChartUpdaterfrequencyChangedHandler UpdaterFrequencyChanged;

        /// <inheritdoc cref="IChartView.Model"/>
        public ChartModel Model { get; protected set; }

        /// <inheritdoc cref="IChartView.ControlSize"/>
        Size IChartView.ControlSize => new Size((int) ActualWidth, (int) ActualHeight);

        /// <inheritdoc cref="IChartView.DrawMargin"/>
        public Margin DrawMargin { get; set; }

        private object _dimensionsUpdateId;
        private IList<IList<Plane>> _dimensions;

        IList<IList<Plane>> IChartView.PlanesArrayByDimension
        {
            get
            {
                if (Model.UpdateId == _dimensionsUpdateId) return _dimensions;
                _dimensions = GetPlanes();
                _dimensionsUpdateId = Model.UpdateId;
                return _dimensions;
            }
        }

        /// <inheritdoc cref="IChartView.Series"/>
        public IEnumerable<Series> Series
        {
            get => (IEnumerable<Series>) GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <inheritdoc cref="IChartView.AnimationsSpeed"/>
        public TimeSpan AnimationsSpeed
        {
            get => (TimeSpan) GetValue(AnimationsSpeedProperty);
            set => SetValue(AnimationsSpeedProperty, value);
        }

        /// <inheritdoc cref="IChartView.Legend"/>
        public ILegend Legend
        {
            get => (ILegend) GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }

        /// <inheritdoc cref="IChartView.LegendPosition"/>
        public LegendPositions LegendPosition
        {
            get => (LegendPositions) GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        /// <inheritdoc cref="IChartView.UpdateDrawArea"/>
        public void UpdateDrawArea(Rectangle model)
        {
            Canvas.SetLeft(DrawArea, model.Left);
            Canvas.SetTop(DrawArea, model.Top);
            DrawArea.Width = model.Width;
            DrawArea.Height = model.Height;
        }

        #endregion
    }
}
