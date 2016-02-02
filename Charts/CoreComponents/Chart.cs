//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts.Tooltip;
using LiveCharts.Viewers;

namespace LiveCharts.CoreComponents
{
    public abstract class Chart : UserControl
    {
        public event Action<Chart> Plot;
        public event Action<ChartPoint> DataClick;

        internal Rect PlotArea;
        internal Canvas DrawMargin;
        internal Point Max;
        internal Point Min;
        internal Point S;
        internal int ColorStartIndex;
        internal bool RequiresScale;
        internal List<Series> EraseSerieBuffer = new List<Series>();
        internal bool SeriesInitialized;
        internal double From = double.MinValue;
        internal double To = double.MaxValue;
        internal AxisTags ZoomingAxis = AxisTags.None;
        internal bool SupportsMultipleSeries = true;

        protected double CurrentScale;
        protected ShapeHoverBehavior ShapeHoverBehavior;
        protected bool AlphaLabel;
        protected readonly DispatcherTimer TooltipTimer;
        protected double DefaultFillOpacity = 0.35;
        
        private static readonly Random Randomizer;
        private readonly DispatcherTimer _resizeTimer;
        private readonly DispatcherTimer _serieValuesChanged;
        private readonly DispatcherTimer _seriesChanged;
        private Point _panOrigin;
        private bool _isDragging;
        private UIElement _dataToolTip;
        private int _colorIndexer;
        private UIElement _dataTooltip;

        static Chart()
        {
            Colors = new List<Color>
            {
                Color.FromRgb(33, 149, 242),
                Color.FromRgb(243, 67, 54),
                Color.FromRgb(254, 192, 7),
                Color.FromRgb(96, 125, 138),
                Color.FromRgb(155, 39, 175),
                Color.FromRgb(0, 149, 135),
                Color.FromRgb(76, 174, 80),
                Color.FromRgb(121, 85, 72),
                Color.FromRgb(157, 157, 157),
                Color.FromRgb(232, 30, 99),
                Color.FromRgb(63, 81, 180),
                Color.FromRgb(0, 187, 211),
                Color.FromRgb(254, 234, 59),
                Color.FromRgb(254, 87, 34)
            };
            Randomizer = new Random();
        }

        protected Chart()
        {
            var b = new Border();
            Canvas = new Canvas();
            b.Child = Canvas;
            Content = b;

            if (RandomizeStartingColor) ColorStartIndex = Randomizer.Next(0, Colors.Count - 1);

            AnimatesNewPoints = false;
            CurrentScale = 1;

            var defaultConfig = new SeriesConfiguration<double>().Y(x => x);
            SetCurrentValue(SeriesProperty, new SeriesCollection(defaultConfig));
            DataTooltip = new DefaultIndexedTooltip();
            Shapes = new List<FrameworkElement>();
            HoverableShapes = new List<HoverableShape>();
            PointHoverColor = System.Windows.Media.Colors.White; 

            Background = Brushes.Transparent;

            SizeChanged += Chart_OnsizeChanged;
            MouseWheel += MouseWheelOnRoll;
            MouseLeftButtonDown += MouseDownForPan;
            MouseLeftButtonUp += MouseUpForPan;
            MouseMove += MouseMoveForPan;

            _resizeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _resizeTimer.Tick += (sender, e) =>
            {
                _resizeTimer.Stop();
                ClearAndPlot();
            };
            TooltipTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            TooltipTimer.Tick += TooltipTimerOnTick;

            _serieValuesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _serieValuesChanged.Tick += UpdateModifiedDataSeries;

            _seriesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _seriesChanged.Tick += UpdateSeries;
        }

        #region StaticProperties
        /// <summary>
        /// List of Colors series will use, yu can change this list to your own colors.
        /// </summary>
        public static List<Color> Colors { get; set; }

        /// <summary>
        /// indicates wether each instance of chart you create needs to randomize starting color
        /// </summary>
        public static bool RandomizeStartingColor { get; set; }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty AxisYProperty = DependencyProperty.Register(
            "AxisY", typeof (Axis), typeof (Chart), new PropertyMetadata(default(Axis)));

        public Axis AxisY
        {
            get { return (Axis) GetValue(AxisYProperty); }
            set { SetValue(AxisYProperty, value); }
        }

        public static readonly DependencyProperty AxisXProperty = DependencyProperty.Register(
            "AxisX", typeof (Axis), typeof (Chart), new PropertyMetadata(default(Axis)));

        public Axis AxisX
        {
            get { return (Axis) GetValue(AxisXProperty); }
            set { SetValue(AxisXProperty, value); }
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof (ZoomingOptions), typeof (Chart), new PropertyMetadata(default(ZoomingOptions)));

        public ZoomingOptions Zoom
        {
            get { return (ZoomingOptions) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(
            "Legend", typeof (ChartLegend), typeof (Chart), new PropertyMetadata(null));

        public ChartLegend Legend
        {
            get { return (ChartLegend) GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        public static readonly DependencyProperty LegendLocationProperty = DependencyProperty.Register(
            "LegendLocation", typeof (LegendLocation), typeof (Chart), new PropertyMetadata(LegendLocation.None));

        public LegendLocation LegendLocation
        {
            get { return (LegendLocation) GetValue(LegendLocationProperty); }
            set { SetValue(LegendLocationProperty, value); }
        }

        public static readonly DependencyProperty InvertProperty = DependencyProperty.Register(
            "Invert", typeof (bool), typeof (Chart), new PropertyMetadata(default(bool)));

        public bool Invert
        {
            get { return (bool) GetValue(InvertProperty); }
            set { SetValue(InvertProperty, value); }
        }

        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof (bool), typeof (Chart));

        /// <summary>
        /// Indicates weather chart is hoverable or not
        /// </summary>
        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        public static readonly DependencyProperty PointHoverColorProperty = DependencyProperty.Register(
            "PointHoverColor", typeof (Color), typeof (Chart));

        /// <summary>
        /// Indicates Point hover color.
        /// </summary>
        public Color PointHoverColor
        {
            get { return (Color) GetValue(PointHoverColorProperty); }
            set { SetValue(PointHoverColorProperty, value); }
        }

        public static readonly DependencyProperty DisableAnimationProperty = DependencyProperty.Register(
            "DisableAnimation", typeof (bool), typeof (Chart));

        /// <summary>
        /// Indicates weather to show animation or not.
        /// </summary>
        public bool DisableAnimation
        {
            get { return (bool) GetValue(DisableAnimationProperty); }
            set { SetValue(DisableAnimationProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof (SeriesCollection), typeof (Chart),
            new PropertyMetadata(null, SeriesChangedCallback ));
        /// <summary>
        /// Gets or sets chart series to plot
        /// </summary>
        public SeriesCollection Series
        {
            get { return (SeriesCollection)  GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets current DataTooltip
        /// </summary>
        public UIElement DataTooltip
        {
            get { return _dataTooltip; }
            set
            {
                _dataTooltip = value;
                if (value == null) return;
                Panel.SetZIndex(DataTooltip, int.MaxValue);
                Canvas.SetLeft(DataTooltip, 0);
                Canvas.SetTop(DataTooltip, 0);
                DataTooltip.Visibility = Visibility.Hidden;
                Canvas.Children.Add(DataTooltip);
            }
        }

        /// <summary>
        /// Gets chart canvas
        /// </summary>
        public Canvas Canvas { get; internal set; }
        /// <summary>
        /// Gets chart point offset
        /// </summary>
        internal double XOffset { get; set; }
        /// <summary>
        /// Gets charts point offset
        /// </summary>
        internal double YOffset { get; set; }
        /// <summary>
        /// Gets current set of shapes added to canvas by LiveCharts
        /// </summary>
        public List<FrameworkElement> Shapes { get; internal set; }
        /// <summary>
        /// Gets collection of shapes that fires tooltip on hover
        /// </summary>
        public List<HoverableShape> HoverableShapes { get; internal set; }
        #endregion

        #region ProtectedProperties
        protected bool AnimatesNewPoints { get; set; }

        internal bool HasInvalidArea
        {
            get { return PlotArea.Width < 15 || PlotArea.Height < 15; }
        }

        internal bool HasValidRange
        {
            get { return Math.Abs(Max.X - Min.X) > S.X*.01 || Math.Abs(Max.Y - Min.Y) > S.Y*.01; }
        }

        #endregion

        #region Public Methods
        public void ClearAndPlot(bool animate = true)
        {
            if (_seriesChanged == null) return;
            _seriesChanged.Stop();
            _seriesChanged.Start();
            PrepareCanvas(animate);
        }

        public void ZoomIn(Point pivot)
        {
            if (DataTooltip != null) DataTooltip.Visibility = Visibility.Hidden;

            var dataPivot = new Point(FromDrawMargin(pivot.X, AxisTags.X), FromDrawMargin(pivot.Y, AxisTags.Y));

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                var max = AxisX.MaxValue ?? Max.X;
                var min = AxisX.MinValue ?? Min.X;
                var l = max - min;
                var rMin = (dataPivot.X - min)/l;
                var rMax = 1 - rMin;

                AxisX.MinValue = min + rMin*S.X;
                AxisX.MaxValue = max - rMax*S.X;
            }
            else
            {
                AxisX.MinValue = null;
                AxisX.MaxValue = null;
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                var max = AxisY.MaxValue ?? Max.Y;
                var min = AxisY.MinValue ?? Min.Y;
                var l = max - min;
                var rMin = (dataPivot.Y - min)/l;
                var rMax = 1 - rMin;

                AxisY.MinValue = min + rMin*S.Y;
                AxisY.MaxValue = max - rMax*S.Y;
            }
            else
            {
                AxisY.MinValue = null;
                AxisY.MaxValue = null;
            }

            foreach (var series in Series) series.Values.RequiresEvaluation = true;

            ForceRedrawNow();
        }

        public void ZoomOut(Point pivot)
        {
            if (DataTooltip != null) DataTooltip.Visibility = Visibility.Hidden;

            var dataPivot = new Point(FromDrawMargin(pivot.X, AxisTags.X), FromDrawMargin(pivot.Y, AxisTags.Y));

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                var max = AxisX.MaxValue ?? Max.X;
                var min = AxisX.MinValue ?? Min.X;
                var l = max - min;
                var rMin = (dataPivot.X - min) / l;
                var rMax = 1 - rMin;

                AxisX.MinValue = min - rMin * S.X;
                AxisX.MaxValue = max + rMax * S.X;
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                var max = AxisY.MaxValue ?? Max.Y;
                var min = AxisY.MinValue ?? Min.Y;
                var l = max - min;
                var rMin = (dataPivot.Y - min) / l;
                var rMax = 1 - rMin;

                AxisY.MinValue = min - rMin * S.Y;
                AxisY.MaxValue = max + rMax * S.Y;
            }

            foreach (var series in Series)
                series.Values.RequiresEvaluation = true;

            ForceRedrawNow();
        }

        public void ClearZoom()
        {
            AxisX.MinValue = null;
            AxisX.MaxValue = null;
            AxisY.MinValue = null;
            AxisY.MaxValue = null;
            ForceRedrawNow();
        }

        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double ToPlotArea(double value, AxisTags axis)
        {
            return Methods.ToPlotArea(value, axis, this);
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Point ToPlotArea(Point value)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X), ToPlotArea(value.Y, AxisTags.Y));
        }

        public double FromPlotArea(double value, AxisTags axis)
        {
            return Methods.FromPlotArea(value, axis, this);
        }

        public double FromDrawMargin(double value, AxisTags axis)
        {
            return Methods.FromDrawMargin(value, axis, this);
        }

        public double LenghtOf(double value, AxisTags axis)
        {
            var isX = axis == AxisTags.X;
            var m = isX ? Min.X : Min.Y;
            var o = isX ? PlotArea.X : PlotArea.Y;
            return Methods.ToPlotArea(m + value, axis, this) - o;
        }
        #endregion

        #region ProtectedMethods
        internal double CalculateSeparator(double range, AxisTags axis)
        {
            //based on:
            //http://stackoverflow.com/questions/361681/algorithm-for-nice-grid-line-intervals-on-a-graph

            var ft = axis == AxisTags.Y
                ? new FormattedText(
                    "A label",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(AxisY.FontFamily, AxisY.FontStyle, AxisY.FontWeight,
                        AxisY.FontStretch), AxisY.FontSize, Brushes.Black)
                : new FormattedText(
                    "A label",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(AxisX.FontFamily, AxisX.FontStyle, AxisX.FontWeight,
                        AxisX.FontStretch), AxisX.FontSize, Brushes.Black);

            var separations = axis == AxisTags.Y
                ? Math.Round(PlotArea.Height / ((ft.Height) * AxisY.CleanFactor), 0)
                : Math.Round(PlotArea.Width / ((ft.Width) * AxisX.CleanFactor), 0);

            separations = separations < 2 ? 2 : separations;

            var minimum = range / separations;
            var magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));
            var residual = minimum / magnitude;
            double tick;
            if (residual > 5)
                tick = 10 * magnitude;
            else if (residual > 2)
                tick = 5 * magnitude;
            else if (residual > 1)
                tick = 2 * magnitude;
            else
                tick = magnitude;
            return tick;
        }

        protected void ConfigureXAsIndexed()
        {
            if (AxisX.Labels == null && AxisX.LabelFormatter == null) AxisX.ShowLabels = false;
            var f = GetFormatter(AxisX);
            var d = AxisX.Labels == null
                ? Max.X
                : AxisX.Labels.IndexOf(AxisX.Labels.OrderBy(x => x.Length).Reverse().First());
            var longestYLabel = new FormattedText(HasValidRange ? f(d) : "", CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(AxisX.FontFamily, AxisX.FontStyle, AxisX.FontWeight, AxisX.FontStretch), AxisX.FontSize,
                Brushes.Black);
            AxisX.Separator.Step = (longestYLabel.Width*Max.X)*1.25 > PlotArea.Width
                ? null
                : (int?) 1;
            if (AxisX.Separator.Step != null) S.X = (int) AxisX.Separator.Step;
            if (Zoom != ZoomingOptions.None) ZoomingAxis = AxisTags.X;
        }

        protected void ConfigureYAsIndexed()
        {
            if (AxisY.Labels == null && AxisY.LabelFormatter == null) AxisY.ShowLabels = false;
            var f = GetFormatter(AxisY);
            var d = AxisY.Labels == null
                ? Max.Y
                : AxisY.Labels.IndexOf(AxisY.Labels.OrderBy(x => x.Length).Reverse().First());
            var longestYLabel = new FormattedText(HasValidRange ? f(d) : "", CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(AxisY.FontFamily, AxisY.FontStyle, AxisY.FontWeight, AxisY.FontStretch), AxisY.FontSize,
                Brushes.Black);
            AxisY.Separator.Step = (longestYLabel.Width*Max.Y)*1.25 > PlotArea.Width
                ? null
                : (int?) 1;
            if (AxisY.Separator.Step != null) S.Y = (int) AxisY.Separator.Step;
            if (Zoom != ZoomingOptions.None) ZoomingAxis = AxisTags.Y;
        }

        protected Point GetLongestLabelSize(Axis axis)
        {
            if (!axis.ShowLabels) return new Point(0, 0);
            var label = "";
            var from = Equals(axis, AxisY) ? Min.Y : Min.X;
            var to = Equals(axis, AxisY) ? Max.Y : Max.X;
            var s = Equals(axis, AxisY) ? S.Y : S.X;
            var f = GetFormatter(axis);
            for (var i = from; i <= to; i += s)
            {
                var iL = f(i);
                if (label.Length < iL.Length)
                {
                    label = iL;
                }
            }
            var longestLabel = new FormattedText(label, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight,
                axis.FontStretch), axis.FontSize, Brushes.Black);
            return new Point(longestLabel.Width, longestLabel.Height);
        }

        protected Point GetLabelSize(Axis axis, double value)
        {
            if (!axis.ShowLabels) return new Point(0, 0);

            var labels = axis.Labels != null ? axis.Labels.ToArray() : null;
            var fomattedValue = labels == null
                ? (AxisX.LabelFormatter == null
                    ? Min.X.ToString(CultureInfo.InvariantCulture)
                    : AxisX.LabelFormatter(value))
                : (labels.Length > value && value>=0
                    ? labels[(int)value]
                    : "");
            var uiLabelSize = new FormattedText(fomattedValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch),
                axis.FontSize, Brushes.Black);
            return new Point(uiLabelSize.Width, uiLabelSize.Height);
        }

        protected Point GetLabelSize(Axis axis, string value)
        {
            if (!axis.ShowLabels) return new Point(0, 0);
            var fomattedValue = value;
            var uiLabelSize = new FormattedText(fomattedValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch),
                axis.FontSize, Brushes.Black);
            return new Point(uiLabelSize.Width, uiLabelSize.Height);
        }

        protected virtual void Scale()
        {
            InitializeComponents();

            Max = new Point(
                AxisX.MaxValue ??
                Series.Where(x => x.Values != null).Select(x => x.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max(),
                AxisY.MaxValue ??
                Series.Where(x => x.Values != null).Select(x => x.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max());

            Min = new Point(
                AxisX.MinValue ??
                Series.Where(x => x.Values != null).Select(x => x.Values.MinChartPoint.X).DefaultIfEmpty(0).Min(),
                AxisY.MinValue ??
                Series.Where(x => x.Values != null).Select(x => x.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min());

            if (ZoomingAxis == AxisTags.X)
            {
                From = Min.X;
                To = Max.X;
            }
            if (ZoomingAxis == AxisTags.Y)
            {
                From = Min.Y;
                To = Max.Y;
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void DrawAxes()
        {
            if (!HasValidRange) return;

            var c = Canvas.Children.Count;
            foreach (var l in Shapes) Canvas.Children.Remove(l);
            var c1 = Canvas.Children.Count;

            //legend
            var legend = Legend ?? new ChartLegend();
            LoadLegend(legend);

            if (LegendLocation != LegendLocation.None)
            {
                Canvas.Children.Add(legend);
                Shapes.Add(legend);
                legend.UpdateLayout();
                legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            switch (LegendLocation)
            {
                case LegendLocation.None:
                    break;
                case LegendLocation.Top:
                    var top = new Point(ActualWidth*.5 - legend.DesiredSize.Width*.5, 0);
                    PlotArea.Y += top.Y + legend.DesiredSize.Height;
                    PlotArea.Height -= legend.DesiredSize.Height;
                    Canvas.SetTop(legend, top.Y);
                    Canvas.SetLeft(legend, top.X);
                    break;
                case LegendLocation.Bottom:
                    var bot = new Point(ActualWidth*.5 - legend.DesiredSize.Width*.5, ActualHeight - legend.DesiredSize.Height);
                    PlotArea.Height -= legend.DesiredSize.Height;
                    Canvas.SetTop(legend, Canvas.ActualHeight - legend.DesiredSize.Height);
                    Canvas.SetLeft(legend, bot.X);
                    break;
                case LegendLocation.Left:
                    PlotArea.X += legend.DesiredSize.Width;
                    PlotArea.Width -= legend.DesiredSize.Width;
                    Canvas.SetTop(legend, Canvas.ActualHeight * .5 - legend.DesiredSize.Height * .5);
                    Canvas.SetLeft(legend, 0);
                    break;
                case LegendLocation.Right:
                    PlotArea.Width -= legend.DesiredSize.Width;
                    Canvas.SetTop(legend, Canvas.ActualHeight*.5 - legend.DesiredSize.Height*.5);
                    Canvas.SetLeft(legend, ActualWidth - legend.DesiredSize.Width);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //draw axes titles
            var longestY = GetLongestLabelSize(AxisY);
            var longestX = GetLongestLabelSize(AxisX);

            if (!string.IsNullOrWhiteSpace(AxisY.Title))
            {
                var ty = GetLabelSize(AxisY, AxisY.Title);
                var yLabel = AxisY.BuildATextBlock(-90);
                var binding = new Binding {Path = new PropertyPath("Title"), Source = AxisY};
                BindingOperations.SetBinding(yLabel, TextBlock.TextProperty, binding);
                Shapes.Add(yLabel);
                Canvas.Children.Add(yLabel);
                if (AxisY.Title.Trim().Length > 0)
                {
                    PlotArea.X += ty.Y;
                    PlotArea.Width -= ty.Y;
                }
                Canvas.SetLeft(yLabel, PlotArea.X - ty.Y -(AxisY.ShowLabels ? longestY.X +5 : 0) -5);
                Canvas.SetTop(yLabel, Canvas.DesiredSize.Height * .5 + ty.X * .5);
            }
            if (!string.IsNullOrWhiteSpace(AxisX.Title))
            {
                var tx = GetLabelSize(AxisX, AxisX.Title);
                var yLabel = AxisY.BuildATextBlock(0);
                var binding = new Binding {Path = new PropertyPath("Title"), Source = AxisX};
                BindingOperations.SetBinding(yLabel, TextBlock.TextProperty, binding);
                Shapes.Add(yLabel);
                Canvas.Children.Add(yLabel);
                if (AxisX.Title.Trim().Length > 0) PlotArea.Height -= tx.Y;
                Canvas.SetLeft(yLabel, Canvas.DesiredSize.Width*.5 - tx.X*.5);
                Canvas.SetTop(yLabel, PlotArea.Y + PlotArea.Height + (AxisX.ShowLabels ? tx.Y +5 : 0));
            }

            //YAxis
            DrawAxis(AxisY, longestY);
            //XAxis
            DrawAxis(AxisX, longestX);
            //drawing ceros.
            if (Max.Y >= 0 && Min.Y <= 0 && AxisY.IsEnabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush {Color = AxisY.Color}, StrokeThickness = AxisY.Thickness, X1 = ToPlotArea(Min.X, AxisTags.X), Y1 = ToPlotArea(0, AxisTags.Y), X2 = ToPlotArea(Max.X, AxisTags.X), Y2 = ToPlotArea(0, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                Shapes.Add(l);
            }
            if (Max.X >= 0 && Min.X <= 0 && AxisX.IsEnabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush {Color = AxisX.Color}, StrokeThickness = AxisX.Thickness, X1 = ToPlotArea(0, AxisTags.X), Y1 = ToPlotArea(Min.Y, AxisTags.Y), X2 = ToPlotArea(0, AxisTags.X), Y2 = ToPlotArea(Max.Y, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                Shapes.Add(l);
            }

            if (DrawMargin.Parent == null) Canvas.Children.Add(DrawMargin);
            Canvas.SetLeft(DrawMargin, PlotArea.X);
            Canvas.SetTop(DrawMargin, PlotArea.Y);
            DrawMargin.Height = PlotArea.Height;
            DrawMargin.Width = PlotArea.Width;
        }

        protected virtual void LoadLegend(ChartLegend legend)
        {
            legend.Series = Series.Select(x => new SeriesStandin
            {
                Fill = x.Fill,
                Stroke = x.Stroke,
                Title = x.Title
            });
            legend.Orientation = LegendLocation == LegendLocation.Bottom || LegendLocation == LegendLocation.Top
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        internal virtual void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataTooltip == null || !Hoverable) return;

            DataTooltip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;
            var sibilings = Invert ? HoverableShapes.Where(s => Math.Abs(s.Value.Y - senderShape.Value.Y) < S.Y*.01).ToList() : HoverableShapes.Where(s => Math.Abs(s.Value.X - senderShape.Value.X) < S.X*.01).ToList();

            var first = sibilings.Count > 0 ? sibilings[0] : null;
            var vx = first != null ? (Invert ? first.Value.Y : first.Value.X) : 0;

            foreach (var sibiling in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    sibiling.Target.Stroke = sibiling.Series.Stroke;
                    sibiling.Target.Fill = new SolidColorBrush {Color = PointHoverColor};
                }
                else sibiling.Target.Opacity = .8;
            }

            var indexedToolTip = DataTooltip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                var fh = GetFormatter(Invert ? AxisY : AxisX);
                var fs = GetFormatter(Invert ? AxisX : AxisY);
                indexedToolTip.Header = fh(vx);
                indexedToolTip.Data = sibilings.Select(x => new IndexedTooltipData
                {
                    Index = Series.IndexOf(x.Series), Series = x.Series, Stroke = x.Series.Stroke, Fill = x.Series.Fill, Point = x.Value, Value = fs(Invert ? x.Value.X : x.Value.Y)
                }).ToArray();
            }

            var p = GetToolTipPosition(senderShape, sibilings);

            DataTooltip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X, Duration = TimeSpan.FromMilliseconds(200)
            });
            DataTooltip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y, Duration = TimeSpan.FromMilliseconds(200)
            });
        }

        internal virtual void DataMouseLeave(object sender, MouseEventArgs e)
        {
            if (!Hoverable) return;

            var s = sender as Shape;
            if (s == null) return;

            var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
            if (shape == null) return;

            var sibilings = HoverableShapes.Where(x => Math.Abs(x.Value.X - shape.Value.X) < .001*S.X).ToList();

            foreach (var p in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    p.Target.Fill = p.Series.Stroke;
                    p.Target.Stroke = new SolidColorBrush {Color = PointHoverColor};
                }
                else
                {
                    p.Target.Opacity = 1;
                }
            }
            TooltipTimer.Stop();
            TooltipTimer.Start();
        }

        internal virtual void DataMouseDown(object sender, MouseEventArgs e)
        {
            var shape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (shape == null) return;
            if (DataClick != null) DataClick.Invoke(shape.Value);
        }

        protected virtual Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
        {
            DataTooltip.UpdateLayout();
            DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = sender.Value.X > (Min.X + Max.X)/2 ? ToPlotArea(sender.Value.X, AxisTags.X) - 10 - DataTooltip.DesiredSize.Width : ToPlotArea(sender.Value.X, AxisTags.X) + 10;
            var y = ToPlotArea(sibilings.Select(s => s.Value.Y).DefaultIfEmpty(0).Sum()/sibilings.Count, AxisTags.Y);
            y = y + DataTooltip.DesiredSize.Height > ActualHeight ? y - (y + DataTooltip.DesiredSize.Height - ActualHeight) - 5 : y;
            return new Point(x, y);
        }

        #endregion

        #region Internal Methods

        internal void OnDataSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _serieValuesChanged.Stop();
            _serieValuesChanged.Start();
        }

        #endregion

        #region Private Methods

        private void DrawAxis(Axis axis, Point longestLabel)
        {
            var isX = Equals(axis, AxisX);
            var max = isX ? Max.X : Max.Y;
            var min = isX ? Min.X : Min.Y;
            var s = isX ? S.X : S.Y;

            var maxval = axis.Separator.IsEnabled || axis.ShowLabels ? max + (axis.IgnoresLastLabel ? -1 : 0) : min - 1;

            var formatter = GetFormatter(axis);

            for (var i = min; i <= maxval; i += s)
            {
                if (axis.Separator.IsEnabled)
                {
                    var l = new Line
                    {
                        Stroke = new SolidColorBrush {Color = axis.Separator.Color}, StrokeThickness = axis.Separator.Thickness
                    };
                    if (isX)
                    {
                        var x = ToPlotArea(i, AxisTags.X);
                        l.X1 = x;
                        l.X2 = x;
                        l.Y1 = ToPlotArea(Max.Y, AxisTags.Y);
                        l.Y2 = ToPlotArea(Min.Y, AxisTags.Y);
                    }
                    else
                    {
                        var y = ToPlotArea(i, AxisTags.Y);
                        l.X1 = ToPlotArea(Min.X, AxisTags.X);
                        l.X2 = ToPlotArea(Max.X, AxisTags.X);
                        l.Y1 = y;
                        l.Y2 = y;
                    }

                    Canvas.Children.Add(l);
                    Shapes.Add(l);
                }

                if (axis.ShowLabels)
                {
                    var text = formatter(i);
                    var label = axis.BuildATextBlock(0);
                    label.Text = text;
                    var fl = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch), axis.FontSize, Brushes.Black);
                    Canvas.Children.Add(label);
                    Shapes.Add(label);

                    var top = 0;
                    var left = 0;

                    if (isX)
                    {
                        Canvas.SetLeft(label, ToPlotArea(i, AxisTags.X) - fl.Width*.5 + XOffset);
                        Canvas.SetTop(label, PlotArea.Y + PlotArea.Height + 5);
                    }
                    else
                    {
                        Canvas.SetLeft(label, PlotArea.X - fl.Width -5);
                        Canvas.SetTop(label, ToPlotArea(i, AxisTags.Y) - longestLabel.Y*.5 + YOffset);
                    }
                }
            }
        }

        internal Func<double, string> GetFormatter(Axis axis)
        {
            var labels = axis.Labels != null ? axis.Labels : null;

            return x => labels == null
                ? (axis.LabelFormatter == null ? x.ToString(CultureInfo.InvariantCulture) : axis.LabelFormatter(x))
                : (labels.Count > x && x >= 0 ? labels[(int) x] : "");
        }

        private void ForceRedrawNow()
        {
            PrepareCanvas();
            UpdateSeries(null, null);
        }

        private void PrepareCanvas(bool animate = false)
        {
            if (Series == null) return;
            if (!SeriesInitialized) InitializeSeries(this);

            if (AxisY.Parent == null) Canvas.Children.Add(AxisY);
            if (AxisX.Parent == null) Canvas.Children.Add(AxisX);

            if (DrawMargin == null)
            {
                DrawMargin = new Canvas {ClipToBounds = true};
                Panel.SetZIndex(DrawMargin, 1);
            }

            foreach (var series in Series)
            {
                var p = series.Parent as Canvas;
                if (p != null) p.Children.Remove(series);
                DrawMargin.Children.Add(series);
                EraseSerieBuffer.Add(series);
                series.RequiresAnimation = animate;
                series.RequiresPlot = true;
            }

            Canvas.Width = ActualWidth*CurrentScale;
            Canvas.Height = ActualHeight*CurrentScale;
            PlotArea = new Rect(0, 0, ActualWidth*CurrentScale, ActualHeight*CurrentScale);
            RequiresScale = true;
        }

        protected void InitializeComponents()
        {
            Series.Chart = this;
            Series.Configuration.Chart = this;
            foreach (var series in Series)
            {
                series.Collection = Series;
                if (series.Values == null) continue;
                if (series.Configuration != null) series.Configuration.Chart = this;
                series.Values.Series = series;
                series.Values.GetLimits();
            }
        }

        private void Chart_OnsizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private static void SeriesChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs eventArgs)
        {
            var chart = o as Chart;

            if (chart == null || chart.Series == null) return;
            if (chart.Series.Any(x => x == null)) return;

            if (chart.Series.Count > 0 && !chart.HasInvalidArea) chart.Scale();
        }

        private void InitializeSeries(Chart chart)
        {
#if DEBUG
            Trace.WriteLine("Chart was initialized (" + DateTime.Now.ToLongTimeString() + ")");
#endif
            chart.SeriesInitialized = true;
            foreach (var series in chart.Series)
            {
                var index = _colorIndexer++;
                series.Chart = chart;
                series.Collection = Series;
                series.Stroke = series.Stroke ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))]);
                series.Fill = series.Fill ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))])
                {
                    Opacity = DefaultFillOpacity
                };
                series.RequiresPlot = true;
                series.RequiresAnimation = true;
                var observable = series.Values as INotifyCollectionChanged;
                if (observable != null)
                    observable.CollectionChanged += chart.OnDataSeriesChanged;
            }

            chart.ClearAndPlot();
            var anim = new DoubleAnimation
            {
                From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(1000)
            };
            if (!chart.DisableAnimation) chart.Canvas.BeginAnimation(OpacityProperty, anim);

            chart.Series.CollectionChanged += (sender, args) =>
            {
                chart._seriesChanged.Stop();
                chart._seriesChanged.Start();

                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    chart.Canvas.Children.Clear();
                    chart.Shapes.Clear();
                    chart.HoverableShapes.Clear();
                }

                if (args.OldItems != null)
                    foreach (var series in args.OldItems.Cast<Series>())
                    {
                        chart.EraseSerieBuffer.Add(series);
                    }

                var newElements = args.NewItems != null ? args.NewItems.Cast<Series>() : new List<Series>();

                chart.RequiresScale = true;
                foreach (var serie in chart.Series.Where(x => !newElements.Contains(x)))
                {
                    chart.EraseSerieBuffer.Add(serie);
                    serie.RequiresPlot = true;
                }

                if (args.NewItems != null)
                    foreach (var series in newElements)
                    {
                        var index = _colorIndexer++;
                        series.Chart = chart;
                        series.Collection = Series;
                        series.Stroke = series.Stroke ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))]);
                        series.Fill = series.Fill ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))])
                        {
                            Opacity = DefaultFillOpacity
                        };
                        series.RequiresPlot = true;
                        series.RequiresAnimation = true;
                        var observable = series.Values as INotifyCollectionChanged;
                        if (observable != null)
                            observable.CollectionChanged += chart.OnDataSeriesChanged;
#if DEBUG
                        if (observable == null) Trace.WriteLine("series do not implements INotifyCollectionChanged");
#endif
                    }
            };
        }

        private void UpdateSeries(object sender, EventArgs e)
        {
            _seriesChanged.Stop();

            EreaseSeries();

            if (Series == null || Series.Count == 0) return;
            if (HasInvalidArea) return;

            foreach (var shape in Shapes) Canvas.Children.Remove(shape);
            
            HoverableShapes = new List<HoverableShape>();
            Shapes = new List<FrameworkElement>();

            if (RequiresScale)
            {
                Scale();
                RequiresScale = false;
            }

            var toPlot = Series.Where(x => x.RequiresPlot);
            foreach (var series in toPlot)
            {
                if (series.Values.Count > 0) series.Plot(series.RequiresAnimation);
                series.RequiresPlot = false;
                series.RequiresAnimation = false;
            }

            if (Plot != null) Plot(this);
#if DEBUG
            Trace.WriteLine("Series Updated (" + DateTime.Now.ToLongTimeString() + ")");
            if (DrawMargin != null) Trace.WriteLine("Draw Margin Objects " + DrawMargin.Children.Count);
            Trace.WriteLine("Canvas Children " + Canvas.Children.Count);
            Trace.WriteLine("Shapes " +Shapes.Count);
            Trace.WriteLine("Hoverable Shapes " + HoverableShapes.Count);
#endif
        }

        private void EreaseSeries()
        {
            foreach (var serie in EraseSerieBuffer.GroupBy(x => x)) serie.First().Erase();
            EraseSerieBuffer.Clear();
        }

        private void UpdateModifiedDataSeries(object sender, EventArgs e)
        {
#if DEBUG
            Trace.WriteLine("Primary Values Updated (" + DateTime.Now.ToLongTimeString() + ")");
#endif
            _serieValuesChanged.Stop();
            Scale();
            foreach (var serie in Series)
            {
                serie.Erase();
                serie.Plot(AnimatesNewPoints);
            }
        }

        private void MouseWheelOnRoll(object sender, MouseWheelEventArgs e)
        {
            if (ZoomingAxis == AxisTags.None) return;
            e.Handled = true;
            if (e.Delta > 0) ZoomIn(e.GetPosition(this));
            else ZoomOut(e.GetPosition(this));
        }

        private void MouseDownForPan(object sender, MouseEventArgs e)
        {
            if (ZoomingAxis == AxisTags.None) return;
            var p = e.GetPosition(this);
            _panOrigin = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            _isDragging = true;
        }

        private void MouseMoveForPan(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            //var p = e.GetPosition(this);
            //var movePoint = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            //var dif = _panOrigin - movePoint;

            //var maxX = AxisX.MaxValue ?? Max.X;
            //var minX = AxisX.MinValue ?? Min.X;
            //var maxY = AxisY.MaxValue ?? Max.Y;
            //var minY = AxisY.MinValue ?? Min.Y;

            //var dx =dif.X;
            //var dy = dif.Y;

            //AxisX.MaxValue = maxX + dx;
            //AxisX.MinValue = minX - dx;

            //foreach (var series in Series) series.Values.RequiresEvaluation = true;

            //ForceRedrawNow();

            //_panOrigin = movePoint;
        }

        private void MouseUpForPan(object sender, MouseEventArgs e)
        {
            if (ZoomingAxis == AxisTags.None) return;
            var p = e.GetPosition(this);
            var movePoint = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            var dif = _panOrigin - movePoint;

            var maxX = AxisX.MaxValue ?? Max.X;
            var minX = AxisX.MinValue ?? Min.X;
            var maxY = AxisY.MaxValue ?? Max.Y;
            var minY = AxisY.MinValue ?? Min.Y;

            var dx = dif.X;
            var dy = dif.Y;

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                AxisX.MaxValue = maxX + dx;
                AxisX.MinValue = minX + dx;
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                AxisY.MaxValue = maxY + dy;
                AxisY.MinValue = minY + dy;
            }

            foreach (var series in Series) series.Values.RequiresEvaluation = true;

            ForceRedrawNow();
            _isDragging = false;
        }

        private void TooltipTimerOnTick(object sender, EventArgs e)
        {
            DataTooltip.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}