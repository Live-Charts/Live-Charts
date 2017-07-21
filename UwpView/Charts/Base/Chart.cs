//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LiveCharts.Charts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Events;
using LiveCharts.Helpers;
using LiveCharts.Uwp.Components;
using LiveCharts.Uwp.Points;
using System.Windows.Input;
using Windows.UI.Xaml.Controls.Primitives;

namespace LiveCharts.Uwp.Charts.Base
{
    /// <summary>
    /// Base chart class
    /// </summary>
    public abstract class Chart : UserControl, IChartView
    {
        /// <summary>
        /// Chart core model, the model calculates the chart.
        /// </summary>
        protected ChartCore ChartCoreModel;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Chart class
        /// </summary>
        protected Chart()
        {
            Canvas = new Canvas();
            Content = Canvas;

            DrawMargin = new Canvas();

            Canvas.Children.Add(DrawMargin);

            TooltipTimeoutTimer = new DispatcherTimer();

            /*Current*/
            SetValue(MinHeightProperty, 50d);
            /*Current*/
            SetValue(MinWidthProperty, 80d);

            /*Current*/
            SetValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(300));
            /*Current*/
            SetValue(TooltipTimeoutProperty, TimeSpan.FromMilliseconds(800));

            /*Current*/
            SetValue(AxisXProperty, new AxesCollection());
            /*Current*/
            SetValue(AxisYProperty, new AxesCollection());

            /*Current*/
            SetValue(ChartLegendProperty, new DefaultLegend());
            /*Current*/
            SetValue(DataTooltipProperty, new DefaultTooltip());

            Colors = new List<Color>
            {
                Color.FromArgb(255, 33, 149, 242),
                Color.FromArgb(255, 243, 67, 54),
                Color.FromArgb(255, 254, 192, 7),
                Color.FromArgb(255, 96, 125, 138),
                Color.FromArgb(255, 0, 187, 211),
                Color.FromArgb(255, 232, 30, 99),
                Color.FromArgb(255, 254, 87, 34),
                Color.FromArgb(255, 63, 81, 180),
                Color.FromArgb(255, 204, 219, 57),
                Color.FromArgb(255, 0, 149, 135),
                Color.FromArgb(255, 76, 174, 80)
            };

            SizeChanged += OnSizeChanged;
            RegisterPropertyChangedCallback(VisibilityProperty, OnIsVisibleChanged);
            PointerWheelChanged += OnPointerWheelChanged;
            Loaded += OnLoaded;
            TooltipTimeoutTimer.Tick += TooltipTimeoutTimerOnTick;

            DrawMargin.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            // if this line is not set, then it does not detect mouse down event...
            DrawMargin.PointerPressed += OnDraggingStart;
            DrawMargin.PointerReleased += OnDraggingEnd;
            //DrawMargin.MouseMove += DragSection;
            DrawMargin.PointerMoved += PanOnMouseMove;
            //MouseUp += DisableSectionDragMouseUp;

            Unloaded += (sender, args) =>
            {
                //remove updater timer from memory
                var updater = (Components.ChartUpdater)Model.Updater;
                if (updater.Timer == null) return;
                updater.Timer.Tick -= updater.OnTimerOnTick;
                updater.Timer.Stop();
                //updater.Timer.IsEnabled = false;
                updater.Timer = null;
            };
        }

        static Chart()
        {
            Randomizer = new Random();
        }

        #endregion

        #region Private and internal methods

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            IsControlLoaded = true;
            Model.DrawMargin.Height = Canvas.ActualHeight;
            Model.DrawMargin.Width = Canvas.ActualWidth;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            Model.ControlSize = new CoreSize(ActualWidth, ActualHeight);
            if (!(this is IPieChart))
                Canvas.Clip = new RectangleGeometry
                {
                    Rect = new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight))
                };
            Model.Updater.Run();
        }

        private void OnIsVisibleChanged(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            Model.ControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.Updater.Run();
            //PrepareScrolBar();
        }

        private static void OnSeriesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var chart = (Chart) o;

            if (chart.Series != null)
            {
                chart.Series.Chart = chart.Model;
                foreach (var series in chart.Series) series.Model.Chart = chart.Model;
            }

            if (chart.LastKnownSeriesCollection != chart.Series && chart.LastKnownSeriesCollection != null)
            {
                foreach (var series in chart.LastKnownSeriesCollection)
                {
                    series.Erase(true);
                }
            }

            CallChartUpdater()(o, e);
            chart.LastKnownSeriesCollection = chart.Series;
        }

        internal void ChartUpdated()
        {
            UpdaterTick?.Invoke(this);
            if (UpdaterTickCommand != null && UpdaterTickCommand.CanExecute(this)) UpdaterTickCommand.Execute(this);
        }

        #endregion

        #region Events

        /// <summary>
        /// The DataClick event is fired when a user click any data point
        /// </summary>
        public event DataClickHandler DataClick;

        /// <summary>
        /// The DataHover event is fired when a user hovers over any data point
        /// </summary>
        public event DataHoverHandler DataHover;

        /// <summary>
        /// This event is fired every time the chart updates.
        /// </summary>
        public event UpdaterTickHandler UpdaterTick;

        #endregion

        #region Commands        

        /// <summary>
        /// The data click command property
        /// </summary>
        public static readonly DependencyProperty DataClickCommandProperty = DependencyProperty.Register(
            "DataClickCommand", typeof(ICommand), typeof(Chart), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the data click command.
        /// </summary>
        /// <value>
        /// The data click command.
        /// </value>
        public ICommand DataClickCommand
        {
            get { return (ICommand) GetValue(DataClickCommandProperty); }
            set { SetValue(DataClickCommandProperty, value); }
        }

        /// <summary>
        /// The data hover command property
        /// </summary>
        public static readonly DependencyProperty DataHoverCommandProperty = DependencyProperty.Register(
            "DataHoverCommand", typeof(ICommand), typeof(Chart), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the data hover command.
        /// </summary>
        /// <value>
        /// The data hover command.
        /// </value>
        public ICommand DataHoverCommand
        {
            get { return (ICommand) GetValue(DataHoverCommandProperty); }
            set { SetValue(DataHoverCommandProperty, value); }
        }

        /// <summary>
        /// The updater tick command property
        /// </summary>
        public static readonly DependencyProperty UpdaterTickCommandProperty = DependencyProperty.Register(
            "UpdaterTickCommand", typeof(ICommand), typeof(Chart), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the updater tick command.
        /// </summary>
        /// <value>
        /// The updater tick command.
        /// </value>
        public ICommand UpdaterTickCommand
        {
            get { return (ICommand) GetValue(UpdaterTickCommandProperty); }
            set { SetValue(UpdaterTickCommandProperty, value); }
        }

        #endregion

        #region Properties

        private AxesCollection PreviousXAxis { get; set; }
        private AxesCollection PreviousYAxis { get; set; }
        private static Random Randomizer { get; }
        private SeriesCollection LastKnownSeriesCollection { get; set; }

        /// <summary>
        /// Gets or sets the chart current canvas
        /// </summary>
        protected Canvas Canvas { get; set; }

        internal Canvas DrawMargin { get; set; }
        internal Popup TooltipContainer { get; set; }

        /// <summary>
        /// Gets or sets whether charts must randomize the starting default series color.
        /// </summary>
        public static bool RandomizeStartingColor { get; set; }

        /// <summary>
        /// This property need to be true when unit testing
        /// </summary>
        public bool IsMocked { get; set; }

        /// <summary>
        /// Gets or sets the application level default series color list
        /// </summary>
        public static List<Color> Colors { get; set; }

        /// <summary>
        /// The series colors property
        /// </summary>
        public static readonly DependencyProperty SeriesColorsProperty = DependencyProperty.Register(
            "SeriesColors", typeof(ColorsCollection), typeof(Chart), new PropertyMetadata(default(ColorsCollection)));

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public ColorsCollection SeriesColors
        {
            get { return (ColorsCollection) GetValue(SeriesColorsProperty); }
            set { SetValue(SeriesColorsProperty, value); }
        }

        /// <summary>
        /// The axis y property
        /// </summary>
        public static readonly DependencyProperty AxisYProperty = DependencyProperty.Register(
            "AxisY", typeof(AxesCollection), typeof(Chart),
            new PropertyMetadata(null, AxisInstancechanged(AxisOrientation.Y)));

        /// <summary>
        /// Gets or sets vertical axis
        /// </summary>
        public AxesCollection AxisY
        {
            get { return (AxesCollection) GetValue(AxisYProperty); }
            set { SetValue(AxisYProperty, value); }
        }

        /// <summary>
        /// The axis x property
        /// </summary>
        public static readonly DependencyProperty AxisXProperty = DependencyProperty.Register(
            "AxisX", typeof(AxesCollection), typeof(Chart),
            new PropertyMetadata(null, AxisInstancechanged(AxisOrientation.X)));

        /// <summary>
        /// Gets or sets horizontal axis
        /// </summary>
        public AxesCollection AxisX
        {
            get { return (AxesCollection) GetValue(AxisXProperty); }
            set { SetValue(AxisXProperty, value); }
        }

        /// <summary>
        /// The chart legend property
        /// </summary>
        public static readonly DependencyProperty ChartLegendProperty = DependencyProperty.Register(
            "ChartLegend", typeof(UserControl), typeof(Chart),
            new PropertyMetadata(null, CallChartUpdater()));

        /// <summary>
        /// Gets or sets the control to use as chart legend for this chart.
        /// </summary>
        public UserControl ChartLegend
        {
            get { return (UserControl) GetValue(ChartLegendProperty); }
            set { SetValue(ChartLegendProperty, value); }
        }

        /// <summary>
        /// The zoom property
        /// </summary>
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof(ZoomingOptions), typeof(Chart),
            new PropertyMetadata(default(ZoomingOptions)));

        /// <summary>
        /// Gets or sets chart zoom behavior
        /// </summary>
        public ZoomingOptions Zoom
        {
            get { return (ZoomingOptions) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        /// <summary>
        /// The pan property
        /// </summary>
        public static readonly DependencyProperty PanProperty = DependencyProperty.Register(
            "Pan", typeof(PanningOptions), typeof(Chart), new PropertyMetadata(PanningOptions.Unset));

        /// <summary>
        /// Gets or sets the chart pan, default is Unset, which bases the behavior according to Zoom property
        /// </summary>
        /// <value>
        /// The pan.
        /// </value>
        public PanningOptions Pan
        {
            get { return (PanningOptions) GetValue(PanProperty); }
            set { SetValue(PanProperty, value); }
        }

        /// <summary>
        /// The legend location property
        /// </summary>
        public static readonly DependencyProperty LegendLocationProperty = DependencyProperty.Register(
            "LegendLocation", typeof(LegendLocation), typeof(Chart),
            new PropertyMetadata(LegendLocation.None, CallChartUpdater()));

        /// <summary>
        /// Gets or sets where legend is located
        /// </summary>
        public LegendLocation LegendLocation
        {
            get { return (LegendLocation) GetValue(LegendLocationProperty); }
            set { SetValue(LegendLocationProperty, value); }
        }

        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof(SeriesCollection), typeof(Chart),
            new PropertyMetadata(default(SeriesCollection), OnSeriesChanged));

        /// <summary>
        /// Gets or sets chart series collection to plot.
        /// </summary>
        public SeriesCollection Series
        {
            get { return (SeriesCollection) GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// The animations speed property
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(default(TimeSpan), UpdateChartFrequency));

        /// <summary>
        /// Gets or sets the default animation speed for this chart, you can override this speed for each element (series and axes)
        /// </summary>
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        /// <summary>
        /// The disable animations property
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof(bool), typeof(Chart),
            new PropertyMetadata(default(bool), UpdateChartFrequency));

        /// <summary>
        /// Gets or sets if the chart is animated or not.
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        /// <summary>
        /// The data tooltip property
        /// </summary>
        public static readonly DependencyProperty DataTooltipProperty = DependencyProperty.Register(
            "DataTooltip", typeof(UserControl), typeof(Chart), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the chart data tooltip.
        /// </summary>
        public UserControl DataTooltip
        {
            get { return (UserControl) GetValue(DataTooltipProperty); }
            set { SetValue(DataTooltipProperty, value); }
        }

        /// <summary>
        /// The hoverable property
        /// </summary>
        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof(bool), typeof(Chart), new PropertyMetadata(true));

        /// <summary>
        /// gets or sets whether chart should react when a user moves the mouse over a data point.
        /// </summary>
        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        /// <summary>
        /// The scroll mode property
        /// </summary>
        public static readonly DependencyProperty ScrollModeProperty = DependencyProperty.Register(
            "ScrollMode", typeof(ScrollMode), typeof(Chart),
            new PropertyMetadata(ScrollMode.None, ScrollModeOnChanged));

        /// <summary>
        /// Gets or sets chart scroll mode
        /// </summary>
        public ScrollMode ScrollMode
        {
            get { return (ScrollMode) GetValue(ScrollModeProperty); }
            set { SetValue(ScrollModeProperty, value); }
        }

        /// <summary>
        /// The scroll horizontal from property
        /// </summary>
        public static readonly DependencyProperty ScrollHorizontalFromProperty = DependencyProperty.Register(
            "ScrollHorizontalFrom", typeof(double), typeof(Chart),
            new PropertyMetadata(default(double), ScrollLimitOnChanged));

        /// <summary>
        /// Gets or sets the scrolling horizontal start value
        /// </summary>
        public double ScrollHorizontalFrom
        {
            get { return (double) GetValue(ScrollHorizontalFromProperty); }
            set { SetValue(ScrollHorizontalFromProperty, value); }
        }

        /// <summary>
        /// The scroll horizontal to property
        /// </summary>
        public static readonly DependencyProperty ScrollHorizontalToProperty = DependencyProperty.Register(
            "ScrollHorizontalTo", typeof(double), typeof(Chart),
            new PropertyMetadata(default(double), ScrollLimitOnChanged));

        /// <summary>
        /// Gets or sets the scrolling horizontal end value
        /// </summary>
        public double ScrollHorizontalTo
        {
            get { return (double) GetValue(ScrollHorizontalToProperty); }
            set { SetValue(ScrollHorizontalToProperty, value); }
        }

        /// <summary>
        /// The scroll vertical from property
        /// </summary>
        public static readonly DependencyProperty ScrollVerticalFromProperty = DependencyProperty.Register(
            "ScrollVerticalFrom", typeof(double), typeof(Chart), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the scrolling vertical start value
        /// </summary>
        public double ScrollVerticalFrom
        {
            get { return (double) GetValue(ScrollVerticalFromProperty); }
            set { SetValue(ScrollVerticalFromProperty, value); }
        }

        /// <summary>
        /// The scroll vertical to property
        /// </summary>
        public static readonly DependencyProperty ScrollVerticalToProperty = DependencyProperty.Register(
            "ScrollVerticalTo", typeof(double), typeof(Chart), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the scrolling vertical end value
        /// </summary>
        public double ScrollVerticalTo
        {
            get { return (double) GetValue(ScrollVerticalToProperty); }
            set { SetValue(ScrollVerticalToProperty, value); }
        }

        /// <summary>
        /// The scroll bar fill property
        /// </summary>
        public static readonly DependencyProperty ScrollBarFillProperty = DependencyProperty.Register(
            "ScrollBarFill", typeof(Brush), typeof(Chart),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(30, 30, 30, 30))));

        /// <summary>
        /// Gets or sets the scroll bar fill brush
        /// </summary>
        public Brush ScrollBarFill
        {
            get { return (Brush) GetValue(ScrollBarFillProperty); }
            set { SetValue(ScrollBarFillProperty, value); }
        }

        /// <summary>
        /// The zooming speed property
        /// </summary>
        public static readonly DependencyProperty ZoomingSpeedProperty = DependencyProperty.Register(
            "ZoomingSpeed", typeof(double), typeof(Chart), new PropertyMetadata(0.8d));

        /// <summary>
        /// Gets or sets zooming speed, goes from 0.95 (slow) to 0.1 (fast), default is 0.8, it means the current axis range percentage that will be draw in the next zooming step
        /// </summary>
        public double ZoomingSpeed
        {
            get { return (double) GetValue(ZoomingSpeedProperty); }
            set { SetValue(ZoomingSpeedProperty, value); }
        }

        /// <summary>
        /// The updater state property
        /// </summary>
        public static readonly DependencyProperty UpdaterStateProperty = DependencyProperty.Register(
            "UpdaterState", typeof(UpdaterState), typeof(Chart),
            new PropertyMetadata(default(UpdaterState), CallChartUpdater()));

        /// <summary>
        /// Gets or sets chart's updater state
        /// </summary>
        public UpdaterState UpdaterState
        {
            get { return (UpdaterState) GetValue(UpdaterStateProperty); }
            set { SetValue(UpdaterStateProperty, value); }
        }

        /// <summary>
        /// Gets the chart model, the model is who calculates everything, is the engine of the chart
        /// </summary>
        public ChartCore Model => ChartCoreModel;

        /// <summary>
        /// Gets whether the chart has an active tooltip.
        /// </summary>
        public bool HasTooltip => DataTooltip != null;

        /// <summary>
        /// Gets whether the chart has a DataClick event attacked.
        /// </summary>
        public bool HasDataClickEventAttached => DataClick != null;

        /// <summary>
        /// Gets whether the chart has a DataHover event attached
        /// </summary>
        public bool HasDataHoverEventAttached => DataHover != null;

        /// <summary>
        /// Gets whether the chart is already loaded in the view.
        /// </summary>
        public bool IsControlLoaded { get; private set; }

        /// <summary>
        /// Gets whether the control is in design mode
        /// </summary>
        public bool IsInDesignMode => Windows.ApplicationModel.DesignMode.DesignModeEnabled;

        /// <summary>
        /// Gets the visible series in the chart
        /// </summary>
        public IEnumerable<ISeriesView> ActualSeries
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled && Series == null)
                    SetValue(SeriesProperty, GetDesignerModeCollection());

                return (Series ?? Enumerable.Empty<ISeriesView>())
                    .Where(x => x.IsSeriesVisible);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the draw margin top.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDrawMarginTop(double value)
        {
            Canvas.SetTop(DrawMargin, value);
        }

        /// <summary>
        /// Sets the draw margin left.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDrawMarginLeft(double value)
        {
            Canvas.SetLeft(DrawMargin, value);
        }

        /// <summary>
        /// Sets the height of the draw margin.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDrawMarginHeight(double value)
        {
            DrawMargin.Height = value;
            SetClip();
        }

        /// <summary>
        /// Sets the width of the draw margin.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDrawMarginWidth(double value)
        {
            DrawMargin.Width = value;
            SetClip();
        }

        private void SetClip()
        {
            DrawMargin.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, DrawMargin.Width, DrawMargin.Height)
            };
        }

        /// <summary>
        /// Adds to view.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddToView(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            Canvas.Children.Add(wpfElement);
        }

        /// <summary>
        /// Adds to draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddToDrawMargin(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            DrawMargin.Children.Add(wpfElement);
        }

        /// <summary>
        /// Removes from view.
        /// </summary>
        /// <param name="element">The element.</param>
        public void RemoveFromView(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            Canvas.Children.Remove(wpfElement);
        }

        /// <summary>
        /// Removes from draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        public void RemoveFromDrawMargin(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            DrawMargin.Children.Remove(wpfElement);
        }

        /// <summary>
        /// Ensures the element belongs to current view.
        /// </summary>
        /// <param name="element">The element.</param>
        public void EnsureElementBelongsToCurrentView(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            var p = (Canvas) wpfElement.Parent;
            if (p == null) AddToView(wpfElement);
        }

        /// <summary>
        /// Ensures the element belongs to current draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        public void EnsureElementBelongsToCurrentDrawMargin(object element)
        {
            var wpfElement = (FrameworkElement) element;
            if (wpfElement == null) return;
            var p = (Canvas) wpfElement.Parent;
            p?.Children.Remove(wpfElement);
            AddToDrawMargin(wpfElement);
        }

        /// <summary>
        /// Shows the legend.
        /// </summary>
        /// <param name="at">At.</param>
        public void ShowLegend(CorePoint at)
        {
            if (ChartLegend == null) return;

            if (ChartLegend.Parent == null)
            {
                AddToView(ChartLegend);
                Canvas.SetLeft(ChartLegend, 0d);
                Canvas.SetTop(ChartLegend, 0d);
            }

            ChartLegend.Visibility = Visibility.Visible;

            Canvas.SetLeft(ChartLegend, at.X);
            Canvas.SetTop(ChartLegend, at.Y);
        }

        /// <summary>
        /// Hides the legend.
        /// </summary>
        public void HideLegend()
        {
            if (ChartLegend != null)
                ChartLegend.Visibility = Visibility.Collapsed; //Visibility.Hidden;
        }

        /// <summary>
        /// Forces the chart to update
        /// </summary>
        /// <param name="restartView">Indicates whether the update should restart the view, animations will run again if true.</param>
        /// <param name="force">Force the updater to run when called, without waiting for the next updater step.</param>
        public void Update(bool restartView = false, bool force = false)
        {
            Model?.Updater.Run(restartView, force);
        }

        /// <summary>
        /// Maps the x axes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public List<AxisCore> MapXAxes(ChartCore chart)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled && AxisX == null)
                AxisX = DefaultAxes.DefaultAxis;

            if (AxisX.Count == 0)
                AxisX.AddRange(DefaultAxes.CleanAxis);

            AxisX.Chart = this;

            return AxisX.Select(x =>
            {
                if (x.Parent == null)
                {
                    x.AxisOrientation = AxisOrientation.X;
                    if (x.Separator != null) chart.View.AddToView(x.Separator);
                    chart.View.AddToView(x);
                    x.AxisOrientation = AxisOrientation.X;
                }
                return x.AsCoreElement(Model, AxisOrientation.X);
            }).ToList();
        }

        /// <summary>
        /// Maps the y axes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public List<AxisCore> MapYAxes(ChartCore chart)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled && AxisY == null)
                AxisY = DefaultAxes.DefaultAxis;

            if (AxisY.Count == 0)
                AxisY.AddRange(DefaultAxes.DefaultAxis);

            AxisX.Chart = this;

            return AxisY.Select(y =>
            {
                if (y.Parent == null)
                {
                    y.AxisOrientation = AxisOrientation.Y;
                    if (y.Separator != null) chart.View.AddToView(y.Separator);
                    chart.View.AddToView(y);
                    y.AxisOrientation = AxisOrientation.Y;
                }
                return y.AsCoreElement(Model, AxisOrientation.Y);
            }).ToList();
        }

        /// <summary>
        /// Gets the default color of the next.
        /// </summary>
        /// <returns></returns>
        public Color GetNextDefaultColor()
        {
            if (Series.CurrentSeriesIndex == short.MaxValue) Series.CurrentSeriesIndex = 0;
            var i = Series.CurrentSeriesIndex;
            Series.CurrentSeriesIndex++;

            if (SeriesColors != null)
            {
                var rsc = RandomizeStartingColor
                    ? Randomizer.Next(0, SeriesColors.Count)
                    : 0;
                return SeriesColors[(i + rsc)%SeriesColors.Count];
            }

            var r = RandomizeStartingColor
                ? Randomizer.Next(0, Colors.Count)
                : 0;
            return Colors[(i + r)%Colors.Count];
        }

        #endregion

        #region Tooltip and legend

        internal DispatcherTimer TooltipTimeoutTimer { get; set; }

        /// <summary>
        /// The tooltip timeout property
        /// </summary>
        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            "TooltipTimeout", typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(default(TimeSpan), TooltipTimeoutCallback));


        /// <summary>
        /// Gets or sets the time a tooltip takes to hide when the user leaves the data point.
        /// </summary>
        public TimeSpan TooltipTimeout
        {
            get { return (TimeSpan) GetValue(TooltipTimeoutProperty); }
            set { SetValue(TooltipTimeoutProperty, value); }
        }

        internal void AttachHoverableEventTo(FrameworkElement element)
        {
            element.PointerPressed -= DataMouseDown;
            element.PointerEntered -= DataMouseEnter;
            element.PointerExited -= DataMouseLeave;

            element.PointerPressed += DataMouseDown;
            element.PointerEntered += DataMouseEnter;
            element.PointerExited += DataMouseLeave;
        }

        private void DataMouseDown(object sender, PointerRoutedEventArgs e)
        {
            var result = ActualSeries.SelectMany(x => x.ActualValues.GetPoints(x))
                .FirstOrDefault(x =>
                {
                    var pointView = x.View as PointView;
                    return pointView != null && Equals(pointView.HoverShape, sender);
                });
            OnDataClick(sender, result);
        }

        internal void OnDataClick(object sender, ChartPoint point)
        {
            DataClick?.Invoke(sender, point);
            if (DataClickCommand != null && DataClickCommand.CanExecute(point)) DataClickCommand.Execute(point);
        }

        private void DataMouseEnter(object sender, PointerRoutedEventArgs e)
        {
            TooltipTimeoutTimer.Stop();

            var source = ActualSeries.SelectMany(x => x.ActualValues.GetPoints(x)).ToList();
            var senderPoint = source.FirstOrDefault(x => x.View != null &&
                                                         Equals(((PointView) x.View).HoverShape, sender));

            if (senderPoint == null) return;

            if (Hoverable) senderPoint.View.OnHover(senderPoint);

            if (DataTooltip != null)
            {
                if (DataTooltip.Parent == null)
                {
                    Canvas.SetZIndex(DataTooltip, short.MaxValue);
                    TooltipContainer = new Popup();
                    AddToView(TooltipContainer);
                    TooltipContainer.Child = DataTooltip;
                    Canvas.SetTop(DataTooltip, 0d);
                    Canvas.SetLeft(DataTooltip, 0d);
                }

                var lcTooltip = DataTooltip as IChartTooltip;
                if (lcTooltip == null)
                    throw new LiveChartsException(
                        "The current tooltip is not valid, ensure it implements IChartsTooltip");

                if (lcTooltip.SelectionMode == TooltipSelectionMode.Auto)
                    lcTooltip.SelectionMode = senderPoint.SeriesView.Model.PreferredSelectionMode;

                var coreModel = ChartFunctions.GetTooltipData(senderPoint, Model, lcTooltip.SelectionMode);

                lcTooltip.Data = new TooltipData
                {
                    XFormatter = coreModel.XFormatter,
                    YFormatter = coreModel.YFormatter,
                    SharedValue = coreModel.Shares,
                    SelectionMode =
                        lcTooltip.SelectionMode == TooltipSelectionMode.Auto
                            ? TooltipSelectionMode.OnlySender
                            : lcTooltip.SelectionMode,
                    Points = coreModel.Points.Select(x => new DataPointViewModel
                    {
                        Series = new SeriesViewModel
                        {
                            PointGeometry = ((Series) x.SeriesView).PointGeometry == DefaultGeometries.None
                                ? new PointGeometry("M0,0 L1,0").Parse()
                                : ((Series) x.SeriesView).PointGeometry.Parse(),
                            Fill = ((Series) x.SeriesView) is IFondeable &&
                                   !(x.SeriesView is IVerticalStackedAreaSeriesView ||
                                     x.SeriesView is IStackedAreaSeriesView)
                                ? ((IFondeable) x.SeriesView).PointForeround
                                : ((Series) x.SeriesView).Fill,
                            Stroke = ((Series) x.SeriesView).Stroke,
                            StrokeThickness = ((Series) x.SeriesView).StrokeThickness,
                            Title = ((Series) x.SeriesView).Title,
                        },
                        ChartPoint = x
                    }).ToList()
                };

                TooltipContainer.IsOpen = true;
                DataTooltip.UpdateLayout();

                var location = GetTooltipPosition(senderPoint);
                location = new Point(Canvas.GetLeft(DrawMargin) + location.X, Canvas.GetTop(DrawMargin) + location.Y);

                if (DisableAnimations)
                {
                    TooltipContainer.VerticalOffset = location.Y;
                    TooltipContainer.HorizontalOffset = location.X;
                }
                else
                {
                    TooltipContainer.BeginDoubleAnimation(nameof(Popup.VerticalOffset),
                        location.Y, TimeSpan.FromMilliseconds(200));
                    TooltipContainer.BeginDoubleAnimation(nameof(Popup.HorizontalOffset),
                        location.X, TimeSpan.FromMilliseconds(200));
                }
            }

            OnDataHover(sender, senderPoint);

        }

        internal void OnDataHover(object sender, ChartPoint point)
        {
            DataHover?.Invoke(sender, point);
            if (DataHoverCommand != null && DataHoverCommand.CanExecute(point)) DataHoverCommand.Execute(point);
        }

        private void DataMouseLeave(object sender, PointerRoutedEventArgs e)
        {
            TooltipTimeoutTimer.Stop();
            TooltipTimeoutTimer.Start();

            var source = ActualSeries.SelectMany(x => x.ActualValues.GetPoints(x));
            var senderPoint = source.FirstOrDefault(x => x.View != null &&
                                                         Equals(((PointView) x.View).HoverShape, sender));

            if (senderPoint == null) return;

            if (Hoverable) senderPoint.View.OnHoverLeave(senderPoint);
        }

        private void TooltipTimeoutTimerOnTick(object sender, object args)
        {
            TooltipTimeoutTimer.Stop();
            if (TooltipContainer == null) return;
            TooltipContainer.IsOpen = false;
        }

        /// <summary>
        /// Loads the legend.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LiveCharts.Helpers.LiveChartsException">The current legend is not valid, ensure it implements IChartLegend</exception>
        public CoreSize LoadLegend()
        {
            if (ChartLegend == null || LegendLocation == LegendLocation.None)
                return new CoreSize();

            if (ChartLegend.Parent == null)
                Canvas.Children.Add(ChartLegend);

            var l = new List<SeriesViewModel>();

            foreach (var t in ActualSeries)
            {
                var item = new SeriesViewModel();

                var series = (Series) t;

                item.Title = series.Title;
                item.StrokeThickness = series.StrokeThickness;
                item.Stroke = series.Stroke;
                item.Fill = ((Series) t) is IFondeable &&
                            !(t is IVerticalStackedAreaSeriesView ||
                              t is IStackedAreaSeriesView)
                    ? ((IFondeable) t).PointForeround
                    : ((Series) t).Fill;
                item.PointGeometry = series.PointGeometry == DefaultGeometries.None
                    ? new PointGeometry("M0,0 L1,0").Parse()
                    : series.PointGeometry.Parse();

                l.Add(item);
            }

            var iChartLegend = ChartLegend as IChartLegend;
            if (iChartLegend == null)
                throw new LiveChartsException("The current legend is not valid, ensure it implements IChartLegend");

            iChartLegend.Series = l;

            var defaultLegend = ChartLegend as DefaultLegend;
            if (defaultLegend != null)
            {
                defaultLegend.InternalOrientation = LegendLocation == LegendLocation.Bottom ||
                                                    LegendLocation == LegendLocation.Top
                    ? Orientation.Horizontal
                    : Orientation.Vertical;

                defaultLegend.MaxWidth = defaultLegend.InternalOrientation == Orientation.Horizontal
                    ? ActualWidth
                    : double.PositiveInfinity;

                defaultLegend.MaxHeight = defaultLegend.InternalOrientation == Orientation.Vertical
                    ? ActualHeight
                    : double.PositiveInfinity;
            }

            ChartLegend.UpdateLayout();
            ChartLegend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return new CoreSize(ChartLegend.DesiredSize.Width,
                ChartLegend.DesiredSize.Height);
        }

        private static void TooltipTimeoutCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var chart = (Chart) dependencyObject;

            if (chart == null) return;

            chart.TooltipTimeoutTimer.Interval = chart.TooltipTimeout;
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        public void HideTooltip()
        {
            if (TooltipContainer == null) return;

            TooltipContainer.IsOpen = false;
        }

        /// <summary>
        /// Gets the tooltip position.
        /// </summary>
        /// <param name="senderPoint">The sender point.</param>
        /// <returns></returns>
        protected internal virtual Point GetTooltipPosition(ChartPoint senderPoint)
        {
            var xt = senderPoint.ChartLocation.X;
            var yt = senderPoint.ChartLocation.Y;

            xt = xt > DrawMargin.Width/2 ? xt - DataTooltip.ActualWidth - 5 : xt + 5;
            yt = yt > DrawMargin.Height/2 ? yt - DataTooltip.ActualHeight - 5 : yt + 5;

            return new Point(xt, yt);
        }

        internal SeriesCollection GetDesignerModeCollection()
        {
            var r = new Random();
            SeriesCollection mockedCollection;

            Func<IChartValues> getValuesForPies = () =>
            {
                var gvt = Type.GetType("LiveCharts.Geared.GearedValues`1, LiveCharts.Geared");
                gvt = gvt?.MakeGenericType(typeof(ObservableValue));

                var obj = gvt != null
                    ? (IChartValues) Activator.CreateInstance(gvt)
                    : new ChartValues<ObservableValue>();

                obj.Add(new ObservableValue(r.Next(0, 100)));

                return obj;
            };

            if (this is PieChart)
            {
                mockedCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Values = getValuesForPies()
                    },
                    new PieSeries
                    {
                        Values = getValuesForPies()
                    },
                    new PieSeries
                    {
                        Values = getValuesForPies()
                    },
                    new PieSeries
                    {
                        Values = getValuesForPies()
                    }
                };
            }
            else
            {
                Func<IChartValues> getRandomValues = () =>
                {
                    var gvt = Type.GetType("LiveCharts.Geared.GearedValues`1, LiveCharts.Geared");
                    gvt = gvt?.MakeGenericType(typeof(ObservableValue));

                    var obj = gvt != null
                        ? (IChartValues) Activator.CreateInstance(gvt)
                        : new ChartValues<ObservableValue>();

                    obj.Add(new ObservableValue(r.Next(0, 100)));
                    obj.Add(new ObservableValue(r.Next(0, 100)));
                    obj.Add(new ObservableValue(r.Next(0, 100)));
                    obj.Add(new ObservableValue(r.Next(0, 100)));
                    obj.Add(new ObservableValue(r.Next(0, 100)));

                    return obj;
                };

                mockedCollection = new SeriesCollection
                {
                    new LineSeries {Values = getRandomValues()},
                    new LineSeries {Values = getRandomValues()},
                    new LineSeries {Values = getRandomValues()}
                };
            }

            var goWild = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            goWild.Tick += (sender, args) =>
            {
                foreach (var series in mockedCollection)
                {
                    foreach (ObservableValue chartValue in series.Values)
                    {
                        chartValue.Value = r.Next(10, 100);
                    }
                }
            };
            //not today... maybe later
            //goWild.Start();

            return mockedCollection;
        }

        #endregion

        #region Zooming and Panning

        private Point DragOrigin { get; set; }
        private bool IsPanning { get; set; }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (Zoom == ZoomingOptions.None) return;

            var p = e.GetCurrentPoint(this);

            var corePoint = new CorePoint(p.Position.X, p.Position.Y);

            e.Handled = true;

            if (p.Properties.MouseWheelDelta > 0)
                Model.ZoomIn(corePoint);
            else
                Model.ZoomOut(corePoint);
        }

        private void OnDraggingStart(object sender, PointerRoutedEventArgs e)
        {
            if (Model?.AxisX == null || Model.AxisY == null) return;

            DragOrigin = e.GetCurrentPoint(this).Position;
            IsPanning = true;
        }

        private void PanOnMouseMove(object sender, PointerRoutedEventArgs e)
        {
            if (!IsPanning) return;

            if (Pan == PanningOptions.Unset && Zoom == ZoomingOptions.None ||
                Pan == PanningOptions.None) return;

            var end = e.GetCurrentPoint(this).Position;
            
            Model.Drag(new CorePoint(DragOrigin.X - end.X, DragOrigin.Y - end.Y));
            DragOrigin = end;
        }

        private void OnDraggingEnd(object sender, PointerRoutedEventArgs e)
        {
            if (!IsPanning) return;
            IsPanning = false;
        }

        #endregion

        //#region ScrollBar functionality
        //private bool _isDragging;
        //private Point _previous;
        //private Rectangle ScrollBar { get; set; }

        internal void PrepareScrolBar()
        {
            //    if (!IsControlLoaded) return;

            //    if (ScrollMode == ScrollMode.None)
            //    {
            //        RemoveFromDrawMargin(ScrollBar);
            //        ScrollBar = null;

            //        return;
            //    }

            //    if (ScrollBar == null)
            //    {
            //        ScrollBar = new Rectangle();

            //        ScrollBar.SetBinding(Shape.FillProperty,
            //            new Binding {Path = new PropertyPath(ScrollBarFillProperty), Source = this});

            //        EnsureElementBelongsToCurrentView(ScrollBar);
            //        ScrollBar.MouseDown += ScrollBarOnMouseDown;
            //        MouseMove += ScrollBarOnMouseMove;
            //        ScrollBar.MouseUp += ScrollBarOnMouseUp;
            //    }

            //    ScrollBar.SetBinding(HeightProperty,
            //        new Binding {Path = new PropertyPath(ActualHeightProperty), Source = this});
            //    ScrollBar.SetBinding(WidthProperty,
            //        new Binding {Path = new PropertyPath(ActualWidthProperty), Source = this});

            //    var f = this.ConvertToPixels(new Point(ScrollHorizontalFrom, ScrollVerticalFrom));
            //    var t = this.ConvertToPixels(new Point(ScrollHorizontalTo, ScrollVerticalTo));

            //    if (ScrollMode == ScrollMode.X || ScrollMode == ScrollMode.XY)
            //    {
            //        Canvas.SetLeft(ScrollBar, f.X);
            //        if (t.X - f.X >= 0) ScrollBar.Width = t.X - f.X > 8 ? t.X - f.X : 8;
            //    }

            //    if (ScrollMode == ScrollMode.Y || ScrollMode == ScrollMode.XY)
            //    {
            //        Canvas.SetTop(ScrollBar, t.Y);
            //        if (f.Y - t.Y >= 0) ScrollBar.Height = f.Y - t.Y > 8 ? f.Y - t.Y : 8;
            //    }
        }

        //private void ScrollBarOnMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    _isDragging = false;
        //    ((UIElement) sender).ReleaseMouseCapture();
        //}

        //private void ScrollBarOnMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!_isDragging) return;

        //    var d = e.GetPosition(this);

        //    var dp = new Point(d.X - _previous.X, d.Y - _previous.Y);
        //    var d0 = this.ConvertToChartValues(new Point());
        //    var d1 = this.ConvertToChartValues(dp);
        //    var dv = new Point(d0.X - d1.X, d0.Y - d1.Y);

        //    _previous = d;

        //    if (ScrollMode == ScrollMode.X || ScrollMode == ScrollMode.XY)
        //    {
        //        if (Math.Abs(dp.X) < 0.1) return;
        //        ScrollHorizontalFrom -= dv.X;
        //        ScrollHorizontalTo -= dv.X;
        //    }

        //    if (ScrollMode == ScrollMode.Y || ScrollMode == ScrollMode.XY)
        //    {
        //        if (Math.Abs(dp.Y) < 0.1) return;
        //        ScrollVerticalFrom += dv.Y;
        //        ScrollVerticalTo += dv.Y;
        //    }
        //}

        //private void ScrollBarOnMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    _isDragging = true;
        //    _previous = e.GetPosition(this);
        //    ((UIElement) sender).CaptureMouse();
        //}

        private static void ScrollModeOnChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var uwpChart = (Chart) o;
            if (o == null) return;
            //uwpfChart.PrepareScrolBar();
        }

        private static void ScrollLimitOnChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var uwpChart = (Chart) o;
            if (o == null) return;
            //uwpfChart.PrepareScrolBar();
        }

        //#endregion

        //#region Dragging Sections

        //internal static Point? Ldsp;

        //private void DragSection(object sender, MouseEventArgs e)
        //{
        //    var ax = AxisSection.Dragging;

        //    if (ax == null) return;

        //    if (Ldsp == null)
        //    {
        //        Ldsp = e.GetPosition(this);
        //        return;
        //    }

        //    var p = e.GetPosition(this);
        //    double delta;

        //    if (ax.Model.Source == AxisOrientation.X)
        //    {
        //        delta = this.ConvertToChartValues(new Point(Ldsp.Value.X, 0), ax.Model.AxisIndex).X -
        //                this.ConvertToChartValues(new Point(p.X, 0), ax.Model.AxisIndex).X;
        //        Ldsp = p;
        //        ax.FromValue -= delta;
        //        ax.ToValue -= delta;
        //    }
        //    else
        //    {
        //        delta = this.ConvertToChartValues(new Point(0, Ldsp.Value.Y), 0, ax.Model.AxisIndex).Y -
        //                this.ConvertToChartValues(new Point(0, p.Y), 0, ax.Model.AxisIndex).Y;
        //        Ldsp = p;
        //        ax.FromValue -= delta;
        //        ax.ToValue -= delta;
        //    }
        //}

        //private void DisableSectionDragMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    AxisSection.Dragging = null;
        // }
        // #endregion

        #region Property Changed

        /// <summary>
        /// Calls the chart updater
        /// </summary>
        /// <param name="animate">if true, the series view will be removed and added again, this restarts animations also.</param>
        /// <param name="updateNow">forces the updater to run as this function is called.</param>
        /// <returns></returns>
        protected static PropertyChangedCallback CallChartUpdater(bool animate = false, bool updateNow = false)
        {
            return (o, args) =>
            {
                var uwpChart = o as Chart;
                if (uwpChart == null) return;
                if (uwpChart.Model != null) uwpChart.Model.Updater.Run(animate, updateNow);
            };
        }

        private static void UpdateChartFrequency(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var uwpChart = (Chart) o;

            var freq = uwpChart.DisableAnimations ? TimeSpan.FromMilliseconds(10) : uwpChart.AnimationsSpeed;

            if (uwpChart.Model == null || uwpChart.Model.Updater == null) return;

            uwpChart.Model.Updater.UpdateFrequency(freq);

            CallChartUpdater(true)(o, e);
        }

        private static PropertyChangedCallback AxisInstancechanged(AxisOrientation orientation)
        {
            return (o, a) =>
            {
                var chart = (Chart) o;
                var ax = orientation == AxisOrientation.X ? chart.PreviousXAxis : chart.PreviousYAxis;

                if (ax != null)
                    foreach (var axis in ax)
                    {
                        axis.Clean();
                    }
                if (orientation == AxisOrientation.X)
                {
                    chart.PreviousXAxis = chart.AxisX;
                }
                else
                {
                    chart.PreviousYAxis = chart.AxisY;
                }
                CallChartUpdater()(o, a);
            };
        }

        #endregion
    }
}
