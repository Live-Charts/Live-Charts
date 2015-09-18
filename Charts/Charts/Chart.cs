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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Charts.Series;

namespace Charts.Charts
{
    public abstract class Chart : UserControl
    {
        public Rect PlotArea;
        public Canvas Canvas;
        public Point Max;
        public Point Min;
        public Point S;
        protected Border CurrentToolTip;
        protected List<Shape> AxisShapes = new List<Shape>();
        protected List<TextBlock> AxisLabels = new List<TextBlock>();
        protected double CurrentScale;
        protected ShapeHoverBehavior ShapeHoverBehavior;
        protected double LabelOffset;
        public List<HoverableShape> HoverableShapes = new List<HoverableShape>(); 
        private Point _panOrigin;
        private bool _isDragging;

        //these timers are to avoid multiple graph ploting, graph will only plot when timer ticks.
        private readonly DispatcherTimer _resizeTimer;
        private readonly DispatcherTimer _seriesCangedTimer;
        
        static Chart()
        {
            Colors = new List<Color>
            {
                Color.FromRgb(41,127,184),
                Color.FromRgb(230,76,60),
                Color.FromRgb(240,195,15),
                Color.FromRgb(26,187,155),
                Color.FromRgb(87,213,140),
                Color.FromRgb(154,89,181),
                Color.FromRgb(92,109,126),
                Color.FromRgb(22,159,132),
                Color.FromRgb(39,173,96),
                Color.FromRgb(92,171,225),
                Color.FromRgb(141,68,172),
                Color.FromRgb(229,126,34),
                Color.FromRgb(210,84,0),
                Color.FromRgb(191,57,43)
            };
        }

        protected Chart()
        {
            var b = new Border {ClipToBounds = true};
            Canvas = new Canvas {RenderTransform = new TranslateTransform(0,0)};
            b.Child = Canvas;
            Content = b;

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
                ClearAndPlot();
                _resizeTimer.Stop();
            };

            _seriesCangedTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            _seriesCangedTimer.Tick += UpdateModifiedDataSeries;
            
            CurrentScale = 1;
        }

        abstract protected void Scale();
        abstract protected bool ScaleChanged { get; }

        public static List<Color> Colors { get; set; }

        public static readonly DependencyProperty ZoomingProperty = DependencyProperty.Register(
            "Zooming", typeof(bool), typeof(Chart));
        /// <summary>
        /// Indicates weather user can zoom graph with mouse wheel.
        /// </summary>
        public bool Zooming
        {
            get { return (bool)GetValue(ZoomingProperty); }
            set { SetValue(ZoomingProperty, value); }
        }

        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof(bool), typeof(Chart));
        /// <summary>
        /// Indicates weather points should be visible or not.
        /// </summary>
        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        public static readonly DependencyProperty PointHoverColorProperty = DependencyProperty.Register(
            "PointHoverColor", typeof(Color), typeof(Chart));
        /// <summary>
        /// Indicates Point hover color.
        /// </summary>
        public Color PointHoverColor
        {
            get { return (Color)GetValue(PointHoverColorProperty); }
            set { SetValue(PointHoverColorProperty, value); }
        }

        public static readonly DependencyProperty PrimaryAxisProperty = DependencyProperty.Register(
           "PrimaryAxis", typeof(Axis), typeof(Chart));
        /// <summary>
        /// Configures Horizontal Axes and its labels.
        /// </summary>
        public Axis PrimaryAxis
        {
            get { return (Axis)GetValue(PrimaryAxisProperty); }
            set { SetValue(PrimaryAxisProperty, value); }
        }

        public static readonly DependencyProperty SecondaryAxisProperty = DependencyProperty.Register(
            "SecondaryAxis", typeof(Axis), typeof(Chart));
        /// <summary>
        /// Configures Vertical Axes and its labels.
        /// </summary>
        public Axis SecondaryAxis
        {
            get { return (Axis)GetValue(SecondaryAxisProperty); }
            set { SetValue(SecondaryAxisProperty, value); }
        }

        public static readonly DependencyProperty DisableAnimationProperty = DependencyProperty.Register(
            "DisableAnimation", typeof(bool), typeof(Chart));
        /// <summary>
        /// Indicates weather to show animation or not.
        /// </summary>
        public bool DisableAnimation
        {
            get { return (bool)GetValue(DisableAnimationProperty); }
            set { SetValue(DisableAnimationProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof(IEnumerable<Serie>), typeof(Chart));
        /// <summary>
        /// Collection of series to be drawn.
        /// </summary>
        public ObservableCollection<Serie> Series
        {
            get { return GetValue(SeriesProperty) as ObservableCollection<Serie>; }
            set
            {
                SetValue(SeriesProperty, value);
                value.CollectionChanged += OnSeriesCollectionChanged;
                var index = 0;
                foreach (var serie in value)
                {
                    serie.ColorId = index;
                    serie.PrimaryValues.CollectionChanged += OnDataSeriesChanged;
                    serie.Chart = this;
                    index++;
                }
                ClearAndPlot();
            }
        }

        /// <summary>
        /// Axis Y: is Horizontal axis, Axis X: Vertical, confusing but this is how it is!
        /// </summary>
        /// <param name="range"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected double CalculateSeparator(double range, AxisTags axis)
        {
            //based on:
            //http://stackoverflow.com/questions/361681/algorithm-for-nice-grid-line-intervals-on-a-graph

            var ft = axis == AxisTags.Y
                ? new FormattedText(
                    "A label",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight,
                        PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black)
                : new FormattedText(
                    "A label",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(SecondaryAxis.FontFamily, SecondaryAxis.FontStyle, SecondaryAxis.FontWeight,
                        SecondaryAxis.FontStretch), SecondaryAxis.FontSize, Brushes.Black);

            var separations = axis == AxisTags.Y
                ? Math.Round(PlotArea.Height/((ft.Height)*PrimaryAxis.CleanFactor), 0)
                : Math.Round(PlotArea.Width /( (ft.Width) * SecondaryAxis.CleanFactor), 0);

            separations = separations < 2 ? 2 : separations;

            var minimum = range/separations;
            var magnitude =Math.Pow(10,Math.Floor(Math.Log(minimum)/Math.Log(10)));
            var residual = minimum/magnitude;
            double tick;
            if (residual > 5)
                tick = 10*magnitude;
            else if (residual > 2)
                tick = 5*magnitude;
            else if (residual > 1)
                tick = 2*magnitude;
            else
                tick = magnitude;
            return tick;
        }

        protected virtual void Plot(bool animate = true)
        {
            if (Series == null ||
                ActualHeight < 10 ||
                ActualWidth < 10)
                return;

            Scale();
            foreach (var serie in Series)
            {
                serie.Plot(animate);
            }
#if DEBUG
            Trace.WriteLine("Plot called at " + DateTime.Now.ToString("mm:ss.fff"));
#endif
        }

        protected virtual void DrawAxis()
        {
            foreach (var l in AxisLabels) Canvas.Children.Remove(l);
            foreach (var s in AxisShapes) Canvas.Children.Remove(s);

            AxisLabels.Clear();
            AxisShapes.Clear();

            //Calculating labels length
            var yLabel = "";
            if (PrimaryAxis.PrintLabels)
                for (var i = Min.Y; i <= Max.Y; i += S.Y)
                {
                    var iL = PrimaryAxis.LabelFormatter == null
                        ? i.ToString(CultureInfo.InvariantCulture)
                        : PrimaryAxis.LabelFormatter(i);
                    if (yLabel.Length < iL.Length)
                    {
                        yLabel = iL;
                    }
                }

            var longestYLabel = new FormattedText(yLabel, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(SecondaryAxis.FontFamily, SecondaryAxis.FontStyle, SecondaryAxis.FontWeight, SecondaryAxis.FontStretch), SecondaryAxis.FontSize, Brushes.Black);

            var labels = SecondaryAxis.Labels?.ToArray();
            var fx = labels == null
                ? (SecondaryAxis.LabelFormatter == null
                    ? Min.X.ToString(CultureInfo.InvariantCulture)
                    : SecondaryAxis.LabelFormatter(Min.X))
                : (labels.Length > Min.X
                    ? labels[(int) Min.X]
                    : "");
            fx = SecondaryAxis.PrintLabels ? fx : "";
            var vx = Math.Truncate((Max.X - Min.X)/S.X)*S.X ;
            var labx = labels == null
                ? (SecondaryAxis.LabelFormatter == null
                    ? vx.ToString(CultureInfo.InvariantCulture)
                    : SecondaryAxis.LabelFormatter(vx))
                : (labels.Length > vx
                    ? labels[(int) vx]
                    : "");
            labx = SecondaryAxis.PrintLabels ? labx : "";
            var firstXLabel = new FormattedText(fx, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight, PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black);
            var lastXLabel = new FormattedText(labx, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight, PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black);

            Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            PlotArea.X = 5 + (longestYLabel.Width > firstXLabel.Width*.5 ? longestYLabel.Width : firstXLabel.Width*.5) + 5;
            var distanceToEnd = ToPlotArea(Max.X - vx, AxisTags.X) - PlotArea.X;
            var lastXLabelOverFlow = distanceToEnd > lastXLabel.Width*.5
                ? (lastXLabel.Width*.5 - distanceToEnd > 0
                    ? lastXLabel.Width*.5 - distanceToEnd
                    : 0)
                : lastXLabel.Width*.5;
            var w = Canvas.DesiredSize.Width - PlotArea.X - lastXLabelOverFlow - 5;
            PlotArea.Width = w > 0 ? w : 10;
            PlotArea.Y = longestYLabel.Height * .5 + 5;
            var h = Canvas.DesiredSize.Height - PlotArea.Y - firstXLabel.Height - 10;
            PlotArea.Height = h > 0 ? h : 10;

            //drawing primary axis
            var ly = PrimaryAxis.Separator.Enabled || PrimaryAxis.PrintLabels
                ? Max.Y
                : Min.Y - 1;
            for (var i = Min.Y; i <= ly; i += S.Y)
            {
                var y = ToPlotArea(i, AxisTags.Y);
                if (PrimaryAxis.Separator.Enabled)
                {
                    var l = new Line
                    {
                        Stroke = new SolidColorBrush {Color = PrimaryAxis.Separator.Color},
                        StrokeThickness = PrimaryAxis.Separator.Thickness,
                        X1 = ToPlotArea(Min.X, AxisTags.X),
                        Y1 = y,
                        X2 = ToPlotArea(Max.X, AxisTags.X),
                        Y2 = y
                    };
                    Canvas.Children.Add(l);
                    AxisShapes.Add(l);
                }
                    
                if (PrimaryAxis.PrintLabels)
                {
                    var t = PrimaryAxis.LabelFormatter == null
                        ? i.ToString(CultureInfo.InvariantCulture)
                        : PrimaryAxis.LabelFormatter(i);
                    var label = new TextBlock
                    {
                        FontFamily = PrimaryAxis.FontFamily,
                        FontSize = PrimaryAxis.FontSize,
                        FontStretch = PrimaryAxis.FontStretch,
                        FontStyle = PrimaryAxis.FontStyle,
                        FontWeight = PrimaryAxis.FontWeight,
                        Foreground = new SolidColorBrush {Color = PrimaryAxis.TextColor},
                        Text = t
                    };
                    var fl = new FormattedText(t, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                        new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight,
                            PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black);
                    Canvas.Children.Add(label);
                    AxisLabels.Add(label);
                    Canvas.SetLeft(label, (5 + longestYLabel.Width) - fl.Width);
                    Canvas.SetTop(label, ToPlotArea(i, AxisTags.Y) - longestYLabel.Height*.5);
                }
            }

            //drawing secondary axis
            var lx = SecondaryAxis.Separator.Enabled || SecondaryAxis.PrintLabels
                ? Max.X
                : Min.X - 1;
            
            for (var i = Min.X; i <= lx; i += S.X)
            {
                var x = ToPlotArea(i, AxisTags.X);
                if (SecondaryAxis.Separator.Enabled)
                {
                    var l = new Line
                    {
                        Stroke = new SolidColorBrush {Color = SecondaryAxis.Separator.Color},
                        StrokeThickness = SecondaryAxis.Separator.Thickness,
                        X1 = x,
                        Y1 = ToPlotArea(Max.Y, AxisTags.Y),
                        X2 = x,
                        Y2 = ToPlotArea(Min.Y, AxisTags.Y)
                    };
                    Canvas.Children.Add(l);
                    AxisShapes.Add(l);
                }
                    
                if (SecondaryAxis.PrintLabels)
                {
                    var t = labels == null
                        ? (SecondaryAxis.LabelFormatter == null
                            ? i.ToString(CultureInfo.InvariantCulture)
                            : SecondaryAxis.LabelFormatter(i))
                        : (labels.Length > i
                            ? labels[(int) i]
                            : "");
                    var label = new TextBlock
                    {
                        FontFamily = SecondaryAxis.FontFamily,
                        FontSize = SecondaryAxis.FontSize,
                        FontStretch = SecondaryAxis.FontStretch,
                        FontStyle = SecondaryAxis.FontStyle,
                        FontWeight = SecondaryAxis.FontWeight,
                        Foreground = new SolidColorBrush { Color = SecondaryAxis.TextColor },
                        Text = t
                    };
                    var fl = new FormattedText(t, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                        new Typeface(SecondaryAxis.FontFamily, SecondaryAxis.FontStyle, SecondaryAxis.FontWeight,
                            SecondaryAxis.FontStretch), SecondaryAxis.FontSize, Brushes.Black);
                    Canvas.Children.Add(label);
                    AxisLabels.Add(label);
                    Canvas.SetLeft(label, ToPlotArea(i, AxisTags.X) - fl.Width * .5 + LabelOffset);
                    Canvas.SetTop(label, PlotArea.Y + PlotArea.Height + 5);
                }
            }
            
            //drawing ceros.
            if (Max.Y >= 0 && Min.Y <= 0 && PrimaryAxis.Enabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush {Color = PrimaryAxis.Color},
                    StrokeThickness = PrimaryAxis.Thickness,
                    X1 = ToPlotArea(Min.X, AxisTags.X),
                    Y1 = ToPlotArea(0, AxisTags.Y),
                    X2 = ToPlotArea(Max.X, AxisTags.X),
                    Y2 = ToPlotArea(0, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                AxisShapes.Add(l);
            }

            if (Max.X >= 0 && Min.X <= 0 && SecondaryAxis.Enabled)
            {
                var l = new Line
                {
                    Stroke = new SolidColorBrush {Color = SecondaryAxis.Color},
                    StrokeThickness = SecondaryAxis.Thickness,
                    X1 = ToPlotArea(0, AxisTags.X),
                    Y1 = ToPlotArea(Min.Y, AxisTags.Y),
                    X2 = ToPlotArea(0, AxisTags.X),
                    Y2 = ToPlotArea(Max.Y, AxisTags.Y)
                };
                Canvas.Children.Add(l);
                AxisShapes.Add(l);
            }
        }

        public virtual void OnDataMouseEnter(object sender, MouseEventArgs e)
        {
            var b = new Border
            {
                BorderThickness = new Thickness(0),
                Background = new SolidColorBrush { Color = Color.FromRgb(30, 30, 30), Opacity = .8 },
                CornerRadius = new CornerRadius(1)
            };
            var sp = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;
            var sibilings = HoverableShapes
                .Where(s => Math.Abs(s.Value.X - senderShape.Value.X) < .001*Min.X).ToList();

            var first = sibilings.Count > 0 ? sibilings[0] : null;
            var last = sibilings.Count > 0 ? sibilings[sibilings.Count - 1] : null;
            var labels = SecondaryAxis.Labels?.ToArray();
            var vx = first?.Value.X ?? 0;

            sp.Children.Add(new TextBlock
            {
                Margin = new Thickness(10, 5, 10 ,0),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Text = labels == null
                ? (SecondaryAxis.LabelFormatter == null
                    ? vx.ToString(CultureInfo.InvariantCulture)
                    : SecondaryAxis.LabelFormatter(vx))
                : (labels.Length > vx
                    ? labels[(int)vx]
                    : ""),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Calibri"),
                FontSize = 11,
                Foreground = Brushes.White
            });

            foreach (var sibiling in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    sibiling.Target.Stroke = new SolidColorBrush {Color = sibiling.Serie.Color};
                    sibiling.Target.Fill = new SolidColorBrush {Color = PointHoverColor};
                }
                else
                {
                    sibiling.Target.Opacity = .8;
                }

                sp.Children.Add(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 10, (sibiling == last ? 5 : 0)),
                    Children =
                    {
                        new Border
                        {
                            Background = new SolidColorBrush {Color = sibiling.Serie.Color},
                            Height = 10,
                            Width = 10,
                            CornerRadius = new CornerRadius(5),
                            BorderThickness = new Thickness(0)
                        },
                        new TextBlock
                        {
                            Text = PrimaryAxis.LabelFormatter == null
                                ? sibiling.Value.Y.ToString(CultureInfo.InvariantCulture)
                                : PrimaryAxis.LabelFormatter(sibiling.Value.Y),
                            Margin = new Thickness(5, 0, 5, 0),
                            VerticalAlignment = VerticalAlignment.Center,
                            FontFamily = new FontFamily("Calibri"),
                            FontSize = 11,
                            Foreground = Brushes.White
                        }
                    }
                });
            }

            b.Child = sp;
            Canvas.Children.Add(b);

            b.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = senderShape.Value.X > (Min.X + Max.X) / 2
                ? ToPlotArea(senderShape.Value.X, AxisTags.X) - 10 - b.DesiredSize.Width
                : ToPlotArea(senderShape.Value.X, AxisTags.X) + 10;
            var y = ToPlotArea(sibilings.Select(s => s.Value.Y).DefaultIfEmpty(0).Sum()
                               /sibilings.Count, AxisTags.Y);
            y = y + b.DesiredSize.Height > ActualHeight
                ? y - (y + b.DesiredSize.Height - ActualHeight) - 5
                : y;
            Canvas.SetLeft(b, x);
            Canvas.SetTop(b, y);
            CurrentToolTip = b;
        }

        public virtual void OnDataMouseLeave(object sender, MouseEventArgs e)
        {
            var s = sender as Shape;
            if (s == null) return;

            var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
            if (shape == null) return;

            var sibilings = HoverableShapes
                .Where(x => Math.Abs(x.Value.X - shape.Value.X) < .001 * Min.X).ToList();

            foreach (var p in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    p.Target.Fill = new SolidColorBrush {Color = p.Serie.Color};
                    p.Target.Stroke = new SolidColorBrush {Color = PointHoverColor};
                }
                else
                {
                    p.Target.Opacity = 1;
                }
            }
            if (CurrentToolTip == null) return;
            Canvas.Children.Remove(CurrentToolTip);
            CurrentToolTip = null;
        }

        public void ClearAndPlot(bool animate = true)
        {
#if DEBUG
            var timer = DateTime.Now;
#endif
            HoverableShapes = new List<HoverableShape>();
            AxisShapes = new List<Shape>();
            AxisLabels = new List<TextBlock>();
            Canvas.Children.Clear();
#if DEBUG
            //this takes as much time as drawing.
            Trace.WriteLine("clearing arrays took " + (DateTime.Now-timer).TotalMilliseconds);
#endif
            Canvas.Width = ActualWidth*CurrentScale;
            Canvas.Height = ActualHeight*CurrentScale;
            PlotArea = new Rect(0, 0, ActualWidth*CurrentScale, ActualHeight*CurrentScale);
            Plot(animate);
        }

        public void ZoomIn()
        {
            CurrentScale += .1;
            ClearAndPlot(false);
            PreventGraphToBeVisible();
        }

        public void ZoomOut()
        {
            CurrentScale -= .1;
            if (CurrentScale <= 1) CurrentScale = 1;
            ClearAndPlot(false);
            PreventGraphToBeVisible();
        }

        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected double ToPlotArea(double value, AxisTags axis)
        {
            return Methods.ToPlotArea(value, axis, this);
        }
        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Point ToPlotArea(Point value)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X), ToPlotArea(value.Y, AxisTags.Y));
        }

        private void PreventGraphToBeVisible()
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

        private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
                foreach (var serie in args.OldItems.Cast<Serie>()) serie.Erase();

            var newSeries = args.NewItems?.Cast<Serie>() ?? new List<Serie>();

            if (ScaleChanged)
            {
                Scale();
                foreach (var serie in Series.Where(x => !newSeries.Contains(x)))
                {
                    serie.Erase();
                    serie.Plot(false);
                }
            }

            if (args.NewItems != null)
                foreach (var serie in newSeries)
                {
                    serie.Chart = this;
                    serie.ColorId = Series.Max(x => x.ColorId) + 1;
                    serie.Plot();
                    serie.PrimaryValues.CollectionChanged += OnDataSeriesChanged;
                }
        }

        private void OnDataSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _seriesCangedTimer.Stop();
            _seriesCangedTimer.Start();
        }

        private void UpdateModifiedDataSeries(object sender, EventArgs e)
        {
            _seriesCangedTimer.Stop();
            if (ScaleChanged) Scale();
            foreach (var serie in Series)
            {
                serie.Erase();
                serie.Plot(false);
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
            PreventGraphToBeVisible();
        }
        
    }
}