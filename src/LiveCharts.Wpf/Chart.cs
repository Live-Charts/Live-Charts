
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Wpf.Controls;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    public abstract class Chart : Canvas, IChartView
    {
        protected Chart()
        {
            DrawMargin = Core.Drawing.Margin.Empty;
            DrawArea = new Canvas
            {
                Background = Brushes.Transparent // to detect events
            };
            TooltipPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.RelativePoint
            };
            Children.Add(DrawArea);
            Children.Add(TooltipPopup);
            SetValue(LegendProperty, new ChartLegend());
            SetValue(DataTooltipProperty, new ChartToolTip());
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        #region Dependency properties

        /// <summary>
        /// The series property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series), typeof(IEnumerable<BaseSeries>), typeof(Chart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(Series))));

        /// <summary>
        /// The animations speed property, default value is 250 milliseconds.
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            nameof(AnimationsSpeed), typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(TimeSpan.FromMilliseconds(250), RaiseOnPropertyChanged(nameof(AnimationsSpeed))));

        /// <summary>
        /// The tooltip timeout property
        /// </summary>
        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            nameof(TooltipTimeOut), typeof(TimeSpan), typeof(Chart), new PropertyMetadata(TimeSpan.FromMilliseconds(150)));

        /// <summary>
        /// The legend property, default is DefaultLegend class.
        /// </summary>
        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(
            nameof(Legend), typeof(ILegend), typeof(Chart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(Legend))));

        /// <summary>
        /// The legend position property
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            nameof(LegendPosition), typeof(LegendPosition), typeof(Chart),
            new PropertyMetadata(LegendPosition.None, RaiseOnPropertyChanged(nameof(LegendPosition))));

        /// <summary>
        /// The data tooltip property.
        /// </summary>
        public static readonly DependencyProperty DataTooltipProperty = DependencyProperty.Register(
            nameof(DataToolTip), typeof(IDataToolTip), typeof(Chart),
            new PropertyMetadata(null, OnDataTooltipPropertyChanged));

        #endregion

        #region Properties

        /// <summary>
        /// Gets the draw area.
        /// </summary>
        /// <value>
        /// The draw area.
        /// </value>
        public Canvas DrawArea { get; }

        /// <summary>
        /// Gets or sets the tooltip popup.
        /// </summary>
        /// <value>
        /// The tooltip popup.
        /// </value>
        public Popup TooltipPopup { get; set; }

        #endregion

        #region private and protected methods

        /// <summary>
        /// Gets the planes in the current chart.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual IList<IList<Plane>> GetPlanes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notifies that the specified property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected static PropertyChangedCallback RaiseOnPropertyChanged(string propertyName)
        {
            return (sender, eventArgs) =>
            {
                var chart = (Chart) sender;
                chart.OnPropertyChanged(propertyName);
            };
        }

        private void OnLoaded(object sender, EventArgs eventArgs)
        {
            ChartViewLoaded?.Invoke();
        }

        private void OnSizeChanged(object o, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ChartViewResized?.Invoke();
        }

        private static void OnDataTooltipPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = (Chart) sender;
            if (chart.DataToolTip != null)
            {
                chart.MouseMove += chart.OnMouseMove;
            }
            else
            {
                chart.MouseMove -= chart.OnMouseMove;
            }
        }

        private void OnMouseMove(object o, MouseEventArgs args)
        {
            if (DataToolTip == null) return;
            var point = args.GetPosition(DrawArea);
            PointerMoved?.Invoke(new Core.Drawing.Point(point.X, point.Y), DataToolTip.SelectionMode, point.X, point.Y);
        }

        #endregion

        #region IChartView implementation

        private event Action ChartViewLoaded;
        event Action IChartView.ChartViewLoaded
        {
            add => ChartViewLoaded += value;
            remove => ChartViewLoaded -= value;
        }

        private event Action ChartViewResized;
        event Action IChartView.ChartViewResized
        {
            add => ChartViewResized += value;
            remove => ChartViewResized -= value;
        }

        private event PointerMovedHandler PointerMoved;
        event PointerMovedHandler IChartView.PointerMoved
        {
            add => PointerMoved += value;
            remove => PointerMoved -= value;
        }

        /// <inheritdoc cref="IChartView.Model"/>
        public ChartModel Model { get; protected set; }

        /// <inheritdoc cref="IChartView.ControlSize"/>
        Size IChartView.ControlSize => new Size((int) ActualWidth, (int) ActualHeight);

        /// <inheritdoc cref="IChartView.DrawMargin"/>
        public Margin DrawMargin { get; set; }

        /// <inheritdoc />
        IList<IList<Plane>> IChartView.Dimensions => GetPlanes();

        /// <inheritdoc cref="IChartView.Series"/>
        public IEnumerable<BaseSeries> Series
        {
            get => (IEnumerable<BaseSeries>) GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <inheritdoc cref="IChartView.AnimationsSpeed"/>
        public TimeSpan AnimationsSpeed
        {
            get => (TimeSpan) GetValue(AnimationsSpeedProperty);
            set => SetValue(AnimationsSpeedProperty, value);
        }

        /// <summary>
        /// Gets or sets the tooltip time out.
        /// </summary>
        /// <value>
        /// The tooltip time out.
        /// </value>
        public TimeSpan TooltipTimeOut
        {
            get => (TimeSpan)GetValue(TooltipTimeoutProperty);
            set => SetValue(TooltipTimeoutProperty, value);
        }

        /// <inheritdoc cref="IChartView.Legend"/>
        public ILegend Legend
        {
            get => (ILegend) GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }

        /// <inheritdoc cref="IChartView.LegendPosition"/>
        public LegendPosition LegendPosition
        {
            get => (LegendPosition) GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        /// <inheritdoc cref="IChartView.DataToolTip"/>
        public IDataToolTip DataToolTip
        {
            get => (IDataToolTip)GetValue(DataTooltipProperty);
            set => SetValue(DataTooltipProperty, value);
        }

        /// <inheritdoc cref="IChartView.UpdateDrawArea"/>
        public void UpdateDrawArea(Rectangle model)
        {
            SetLeft(DrawArea, model.Left);
            SetTop(DrawArea, model.Top);
            DrawArea.Width = model.Width;
            DrawArea.Height = model.Height;
        }

        void IChartView.InvokeOnUiThread(Action action)
        {
            Dispatcher.Invoke(action);
        }

        #endregion

        #region INPC implementation

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
