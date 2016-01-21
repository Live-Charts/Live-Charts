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

namespace LiveCharts.Charts
{
    public abstract class Chart : UserControl
    {
        public event Action<Chart> Plot;

        internal Rect PlotArea;
        internal Point Max;
        internal Point Min;
        internal Point S;
        internal int ColorStartIndex;
        internal bool RequiresScale;
        internal List<Series> EraseSerieBuffer = new List<Series>();
        internal bool SeriesInitialized;

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

        static Chart()
        {
            Colors = new List<Color>
            {
                Color.FromRgb(41, 127, 184),
                Color.FromRgb(230, 76, 60),
                Color.FromRgb(240, 195, 15),
                Color.FromRgb(26, 187, 155),
                Color.FromRgb(87, 213, 140),
                Color.FromRgb(154, 89, 181),
                Color.FromRgb(92, 109, 126),
                Color.FromRgb(22, 159, 132),
                Color.FromRgb(39, 173, 96),
                Color.FromRgb(92, 171, 225),
                Color.FromRgb(141, 68, 172),
                Color.FromRgb(229, 126, 34),
                Color.FromRgb(210, 84, 0),
                Color.FromRgb(191, 57, 43)
            };
            Randomizer = new Random();
        }

        protected Chart()
        {
            var b = new Border {ClipToBounds = true};
            Canvas = new Canvas {RenderTransform = new TranslateTransform(0, 0)};
            b.Child = Canvas;
            Content = b;

            if (RandomizeStartingColor) ColorStartIndex = Randomizer.Next(0, Colors.Count - 1);

            AnimatesNewPoints = false;
            CurrentScale = 1;

            var defaultConfig = new SeriesConfiguration<double>().Y(x => x);
            SetCurrentValue(SeriesProperty, new SeriesCollection(defaultConfig));
            DataToolTip = new DefaultIndexedTooltip();
            Shapes = new List<FrameworkElement>();
            HoverableShapes = new List<HoverableShape>();
            PointHoverColor = System.Windows.Media.Colors.White; 

            //it requieres a background so it detect mouse down/up events.
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
        /// Gets chart canvas
        /// </summary>
        public Canvas Canvas { get; internal set; }
        /// <summary>
        /// Gets chart point offset
        /// </summary>
        public double XOffset { get; internal set; }
        /// <summary>
        /// Gets current set of shapes added to canvas by LiveCharts
        /// </summary>
        public List<FrameworkElement> Shapes { get; internal set; }
        /// <summary>
        /// Gets collection of shapes that fires tooltip on hover
        /// </summary>
        public List<HoverableShape> HoverableShapes { get; internal set; }

        /// <summary>
        /// Gets or sets X Axis
        /// </summary>
        public Axis AxisY { get; set; }

        /// <summary>
        /// Gets or sets Y Axis
        /// </summary>
        public Axis AxisX { get; set; }
        /// <summary>
        /// Gets or sets current tooltip when mouse is over a hoverable shape
        /// </summary>
        public UIElement DataToolTip
        {
            get { return _dataToolTip; }
            set
            {
                _dataToolTip = value;
                if (value == null) return;
                Panel.SetZIndex(_dataToolTip, int.MaxValue);
                Canvas.SetLeft(_dataToolTip,0);
                Canvas.SetTop(_dataToolTip, 0);
                _dataToolTip.Visibility = Visibility.Hidden;
                Canvas.Children.Add(_dataToolTip);
            }
        }
        /// <summary>
        /// Gets or sets if chart allows zooming or not
        /// </summary>
        public bool Zooming { get; set; }
        #endregion

        #region ProtectedProperties
        protected bool AnimatesNewPoints { get; set; }
        #endregion

        #region Public Methods
        public void ClearAndPlot(bool animate = true)
        {
            if (_seriesChanged == null) return;
            _seriesChanged.Stop();
            _seriesChanged.Start();
            PrepareCanvas(animate);
        }

        public void ZoomIn()
        {
            CurrentScale += .1;
            ForceRedrawNow();
            PreventPlotAreaToBeVisible();
        }

        public void ZoomOut()
        {
            CurrentScale -= .1;
            if (CurrentScale <= 1) CurrentScale = 1;
            ForceRedrawNow();
            PreventPlotAreaToBeVisible();
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
        #endregion

        #region ProtectedMethods
        internal double CalculateSeparator(double range, AxisTags axis)
        {
            //based on:
            //http://stackoverflow.com/questions/361681/algorithm-for-nice-grid-line-intervals-on-a-graph

            var m = axis == AxisTags.Y ? Min.Y : Min.X;
            if (Math.Abs(range) < m * .01) range = m;

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

        protected void ConfigureSmartAxis(Axis axis)
        {
            axis.ShowLabels = axis.Labels != null;
            if (axis.Labels == null || !axis.Labels.Any() || !axis.ShowLabels) return;
            var m = axis.Labels.OrderByDescending(x => x.Length);
            var longestYLabel = new FormattedText(m.First(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch), axis.FontSize,
                Brushes.Black);
            axis.Separator.Step = (longestYLabel.Width * Max.X) * 1.25 > PlotArea.Width
                ? null
                : (int?)1;
            if (AxisX.Separator.Step != null) S.X = (int)AxisX.Separator.Step;
        }

        protected Point GetLongestLabelSize(Axis axis)
        {
            if (!axis.ShowLabels) return new Point(0, 0);
            var label = "";
            var from = Equals(axis, AxisY) ? Min.Y : Min.X;
            var to = Equals(axis, AxisY) ? Max.Y : Max.X;
            var s = Equals(axis, AxisY) ? S.Y : S.X;
            for (var i = from; i <= to; i += s)
            {
                var iL = axis.LabelFormatter == null
                    ? i.ToString(CultureInfo.InvariantCulture)
                    : axis.LabelFormatter(i);
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
            Series.Chart = this;
            foreach (var series in Series)
            {
                series.Collection = Series;
                if (series.Values == null) continue;
                series.Values.Series = series;
                series.Values.Evaluate();
            }

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
        }
        #endregion

        #region Virtual Methods
        protected virtual void DrawAxes()
        {
            foreach (var l in Shapes) Canvas.Children.Remove(l);
            //draw axes titles
            var titleY = 0d;
            if (!string.IsNullOrWhiteSpace(AxisY.Title))
            {
                var ty = GetLabelSize(AxisY, AxisY.Title);
                var yLabel = AxisY.BuildATextBlock(-90);
                var binding = new Binding {Path = new PropertyPath("Title"), Source = AxisY};
                BindingOperations.SetBinding(yLabel, TextBlock.TextProperty, binding);
                Shapes.Add(yLabel);
                Canvas.Children.Add(yLabel);
                Canvas.SetLeft(yLabel, 5);
                Canvas.SetTop(yLabel, Canvas.DesiredSize.Height * .5 + ty.X * .5);
                titleY += ty.Y + 5;
            }
            var titleX = 0d;
            if (!string.IsNullOrWhiteSpace(AxisX.Title))
            {
                var tx = GetLabelSize(AxisX, AxisX.Title);
                var yLabel = AxisY.BuildATextBlock(0);
                var binding = new Binding {Path = new PropertyPath("Title"), Source = AxisX};
                BindingOperations.SetBinding(yLabel, TextBlock.TextProperty, binding);
                Shapes.Add(yLabel);
                Canvas.Children.Add(yLabel);
                Canvas.SetLeft(yLabel, Canvas.DesiredSize.Width * .5 - tx.X * .5);
                Canvas.SetTop(yLabel, Canvas.DesiredSize.Height - tx.Y - 5);
                titleX += tx.Y;
            }
            PlotArea.X += titleY;
            PlotArea.Width -= titleY;
            PlotArea.Height -= titleX;

            //now lets draw axis
            var longestY = GetLongestLabelSize(AxisY);
            var longestX = GetLongestLabelSize(AxisX);
            //YAxis
            DrawAxis(AxisY, longestY, longestX, titleY, 0);
            //XAxis
            DrawAxis(AxisX, longestX, longestX, 0, 0);
            //drawing ceros.
            if (Max.Y >= 0 && Min.Y <= 0 && AxisY.IsEnabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush { Color = AxisY.Color },
                    StrokeThickness = AxisY.Thickness,
                    X1 = ToPlotArea(Min.X, AxisTags.X),
                    Y1 = ToPlotArea(0, AxisTags.Y),
                    X2 = ToPlotArea(Max.X, AxisTags.X),
                    Y2 = ToPlotArea(0, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                Shapes.Add(l);
            }
            if (Max.X >= 0 && Min.X <= 0 && AxisX.IsEnabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush { Color = AxisX.Color },
                    StrokeThickness = AxisX.Thickness,
                    X1 = ToPlotArea(0, AxisTags.X),
                    Y1 = ToPlotArea(Min.Y, AxisTags.Y),
                    X2 = ToPlotArea(0, AxisTags.X),
                    Y2 = ToPlotArea(Max.Y, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                Shapes.Add(l);
            }
        }

        public virtual void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataToolTip == null) return;

            DataToolTip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;
            var sibilings = HoverableShapes
                .Where(s => Math.Abs(s.Value.X - senderShape.Value.X) < S.X*.001).ToList();

            var first = sibilings.Count > 0 ? sibilings[0] : null;
            var labels = AxisX.Labels != null ? AxisX.Labels.ToArray() : null;
            var vx = first != null ? first.Value.X : 0;
            vx = AlphaLabel ? (int) (vx/(360d/Series.First().Values.Count)) : vx;

            foreach (var sibiling in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    sibiling.Target.Stroke = sibiling.Series.Stroke;
                    sibiling.Target.Fill = new SolidColorBrush {Color = PointHoverColor};
                }
                else
                {
                    sibiling.Target.Opacity = .8;
                }
            }

            var indexedToolTip = DataToolTip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                indexedToolTip.Header = labels == null
                        ? (AxisX.LabelFormatter == null
                            ? vx.ToString(CultureInfo.InvariantCulture)
                            : AxisX.LabelFormatter(vx))
                        : (labels.Length > vx
                            ? labels[(int) vx]
                            : "");
                indexedToolTip.Data = sibilings.Select(x => new IndexedTooltipData
                {
                    Index = Series.IndexOf(x.Series),
                    Series = x.Series,
                    Stroke = x.Series.Stroke,
                    Fill = x.Series.Fill,
                    Point = x.Value,
                    Value = AxisY.LabelFormatter == null
                        ? x.Value.Y.ToString(CultureInfo.InvariantCulture)
                        : AxisY.LabelFormatter(x.Value.Y)
                }).ToArray();
            }

            var p = GetToolTipPosition(senderShape, sibilings);

            DataToolTip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X,
                Duration = TimeSpan.FromMilliseconds(200)
            });
            DataToolTip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y,
                Duration = TimeSpan.FromMilliseconds(200)
            });
        }

        public virtual void DataMouseLeave(object sender, MouseEventArgs e)
        {
            var s = sender as Shape;
            if (s == null) return;

            var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
            if (shape == null) return;

            var sibilings = HoverableShapes
                .Where(x => Math.Abs(x.Value.X - shape.Value.X) < .001 * S.X).ToList();

            foreach (var p in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    p.Target.Fill = p.Series.Stroke;
                    p.Target.Stroke = new SolidColorBrush { Color = PointHoverColor };
                }
                else
                {
                    p.Target.Opacity = 1;
                }
            }
            TooltipTimer.Stop();
            TooltipTimer.Start();
        }

        protected virtual Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
        {
            DataToolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = sender.Value.X > (Min.X + Max.X) / 2
                ? ToPlotArea(sender.Value.X, AxisTags.X) - 10 - DataToolTip.DesiredSize.Width
                : ToPlotArea(sender.Value.X, AxisTags.X) + 10;
            var y = ToPlotArea(sibilings.Select(s => s.Value.Y).DefaultIfEmpty(0).Sum()
                               / sibilings.Count, AxisTags.Y);
            y = y + DataToolTip.DesiredSize.Height > ActualHeight
                ? y - (y + DataToolTip.DesiredSize.Height - ActualHeight) - 5
                : y;
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

        private void DrawAxis(Axis axis, Point longestY, Point longestX, double offsetY, double offsetX)
        {
            var isX = Equals(axis, AxisX);
            var max = isX ? Max.X : Max.Y;
            var min = isX ? Min.X : Min.Y;
            var s = isX ? S.X : S.Y;

            var maxval = axis.Separator.IsEnabled || axis.ShowLabels
                ? max + (axis.IgnoresLastLabel ? -1 : 0)
                : min - 1;

            var formatter = GetFormatter(axis);

            for (var i = min; i <= maxval; i += s)
            {
                if (axis.Separator.IsEnabled)
                {
                    var l = new Line
                    {
                        Stroke = new SolidColorBrush { Color = axis.Separator.Color },
                        StrokeThickness = axis.Separator.Thickness
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
                    var fl = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                        new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight,
                            axis.FontStretch), axis.FontSize, Brushes.Black);
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
                        Canvas.SetLeft(label, offsetY + (5 + longestY.X) - fl.Width);
                        Canvas.SetTop(label, ToPlotArea(i, AxisTags.Y) - longestY.Y * .5);
                    }
                }
            }
        }

        private Func<double, string> GetFormatter(Axis axis)
        {
            var labels = axis.Labels != null ? axis.Labels : null;

            return x => labels == null
                ? (axis.LabelFormatter == null
                    ? x.ToString(CultureInfo.InvariantCulture)
                    : axis.LabelFormatter(x))
                : (labels.Count > x && x >= 0
                    ? labels[(int) x]
                    : "");
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

            foreach (var series in Series)
            {
                Canvas.Children.Remove(series);
                Canvas.Children.Add(series);
                EraseSerieBuffer.Add(series);
                series.RequiresAnimation = animate;
                series.RequiresPlot = true;
            }

            Canvas.Width = ActualWidth * CurrentScale;
            Canvas.Height = ActualHeight * CurrentScale;
            PlotArea = new Rect(0, 0, ActualWidth * CurrentScale, ActualHeight * CurrentScale);
            RequiresScale = true;
        }

        private void PreventPlotAreaToBeVisible()
        {
            var tt = Canvas.RenderTransform as TranslateTransform;
            if (tt == null) return;
            var eX = tt.X;
            var eY = tt.Y;
            var xOverflow = -tt.X + ActualWidth - Canvas.Width;
            var yOverflow = -tt.Y + ActualHeight - Canvas.Height;

            if (eX > 0)
            {
                //Cant understand why with I cant animate this...
                //Pan stops working when I do animation on overflow

                //var y = new DoubleAnimation(tt.Y, 0, TimeSpan.FromMilliseconds(150));
                //var x = new DoubleAnimation(tt.X, 0, TimeSpan.FromMilliseconds(150));

                //I even try this... but nope
                //y.Completed += (o, args) => { tt.Y = 0; };
                //x.Completed += (o, args) => { tt.X = 0; };

                //Canvas.RenderTransform.BeginAnimation(TranslateTransform.YProperty, y);
                //Canvas.RenderTransform.BeginAnimation(TranslateTransform.XProperty, x);
                tt.X = 0;
            }

            if (eY > 0)
            {
                tt.Y = 0;
            }

            if (xOverflow > 0)
            {
                tt.X = tt.X + xOverflow;
            }

            if (yOverflow > 0)
            {
                tt.Y = tt.Y + yOverflow;
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

            chart.InitializeSeries(chart);

            if (chart.Series.Any(x => x == null)) return;

            foreach (var series in chart.Series)
            {
                series.Collection = chart.Series;
                if (series.Values == null) continue;
                series.Values.Series = series;
                series.Values.Evaluate();
            }

            var xs = chart.Series.SelectMany(x => x.Values.Points.Select(pt => pt.X)).DefaultIfEmpty(0).ToArray();
            var ys = chart.Series.SelectMany(x => x.Values.Points.Select(pt => pt.Y)).DefaultIfEmpty(0).ToArray();

            chart.Min = new Point(xs.Min(), ys.Min());
            chart.Max = new Point(xs.Max(), ys.Max());
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
                series.Stroke = series.Stroke ??
                                new SolidColorBrush(
                                    Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))]);
                series.Fill = series.Fill ??
                              new SolidColorBrush(
                                  Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))])
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
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(1000)
            };
            if (!chart.DisableAnimation) chart.Canvas.BeginAnimation(OpacityProperty, anim);

            chart.Series.CollectionChanged += (sender, args) =>
            {
                chart._seriesChanged.Stop();
                chart._seriesChanged.Start();

                if (args.OldItems != null)
                    foreach (var serie in args.OldItems.Cast<Series>())
                    {
                        chart.EraseSerieBuffer.Add(serie);
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
                        series.Stroke = series.Stroke ??
                                new SolidColorBrush(
                                    Colors[(int)(index - Colors.Count * Math.Truncate(index / (decimal)Colors.Count))]);
                        series.Fill = series.Fill ??
                                      new SolidColorBrush(
                                          Colors[(int)(index - Colors.Count * Math.Truncate(index / (decimal)Colors.Count))])
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

            if (Series == null) return;
            if (PlotArea.Width < 15 || PlotArea.Height < 15) return;

            foreach (var shape in Shapes) Canvas.Children.Remove(shape);
            foreach (var shape in HoverableShapes.Select(x => x.Shape).ToList()) Canvas.Children.Remove(shape);
            HoverableShapes = new List<HoverableShape>();
            Shapes = new List<FrameworkElement>();

            if (RequiresScale)
            {
                Scale();
                RequiresScale = false;
            }
            foreach (var serie in EraseSerieBuffer.GroupBy(x => x)) serie.First().Erase();
            
            EraseSerieBuffer.Clear();

            var toPlot = Series.Where(x => x.RequiresPlot);
            foreach (var series in toPlot)
            {
                series.Plot(series.RequiresAnimation);
                series.RequiresPlot = false;
                series.RequiresAnimation = false;
            }

            if (Plot != null) Plot(this);
#if DEBUG
            Trace.WriteLine("Series Updated (" + DateTime.Now.ToLongTimeString() + ")");
#endif
        }

        private void UpdateModifiedDataSeries(object sender, EventArgs e)
        {
#if DEBUG
            Trace.WriteLine("Primary Values Updated (" + DateTime.Now.ToLongTimeString() + ")");
#endif
            _serieValuesChanged.Stop();
            //if (ScaleChanged) Scale();
            //this if can be safely removed without making this library to redraw multiple times
            //because now implemented a buffer, and performance optimizations, so it should be fine 
            //if we force redraw everytime we make a change.
            //this could be a future improvemnt,
            //by now this is perfectly fine and should not impact in performance.
            Scale();
            foreach (var serie in Series)
            {
                serie.Erase();
                serie.Plot(AnimatesNewPoints);
            }
        }

        private void MouseWheelOnRoll(object sender, MouseWheelEventArgs e)
        {
            if (!Zooming) return;
            e.Handled = true;
            if (e.Delta > 0) ZoomIn();
            else ZoomOut();
        }

        private void MouseDownForPan(object sender, MouseEventArgs e)
        {
            if (!Zooming) return;
            _panOrigin = e.GetPosition(this);
            _isDragging = true;
        }

        private void MouseMoveForPan(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            var tt = Canvas.RenderTransform as TranslateTransform;
            if (tt == null) return;

            var movePoint = e.GetPosition(this);
            var dif = _panOrigin - movePoint;

            tt.X = tt.X - dif.X;
            tt.Y = tt.Y - dif.Y;

            _panOrigin = movePoint;
        }

        private void MouseUpForPan(object sender, MouseEventArgs e)
        {
            if (!Zooming) return;
            _isDragging = false;
            PreventPlotAreaToBeVisible();
        }

        private void TooltipTimerOnTick(object sender, EventArgs e)
        {
            DataToolTip.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}