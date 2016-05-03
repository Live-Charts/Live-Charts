//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using LiveCharts.Wpf.Points;
using Size = System.Windows.Size;

namespace LiveCharts.Wpf.Components
{
    public abstract class Chart : UserControl, IChartView
    {
        protected ChartCore ChartCoreModel;
       
        protected Chart()
        {
            Canvas = new Canvas();
            Content = Canvas;

            DrawMargin = new Canvas {ClipToBounds = true};
            Canvas.Children.Add(DrawMargin);

            SetValue(MinHeightProperty, 125d);
            SetValue(MinWidthProperty, 125d);

            SetValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(500));

            SetValue(ChartLegendProperty, new ChartLegend());

            CursorX = new ChartCursor(this, AxisTags.X);
            CursorY = new ChartCursor(this, AxisTags.Y);

            SetValue(AxisXProperty, new List<Axis>());
            SetValue(AxisYProperty, new List<Axis>());

            if (RandomizeStartingColor) 
                CurrentColorIndex = Randomizer.Next(0, Colors.Count - 1);

            ResizeTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            ResizeTimer.Tick += (sender, args) =>
            {
#if DEBUG
                Debug.WriteLine("ChartResized");
#endif
                Model.ChartControlSize = new LvcSize(ActualWidth, ActualHeight);

                Model.DrawMargin.Left = 0;
                Model.DrawMargin.Top = 0;
                Model.DrawMargin.Width = ActualWidth;
                Model.DrawMargin.Height = ActualHeight;

                Model.Updater.Run();
                ResizeTimer.Stop();
            };
            SizeChanged += (sender, args) =>
            {
                ResizeTimer.Stop();
                ResizeTimer.Start();
            };

            Loaded += (sender, args) =>
            {
                IsControlLoaded = true;

                Model.ChartControlSize = new LvcSize(ActualWidth, ActualHeight);

                Model.DrawMargin.Left = 0;
                Model.DrawMargin.Top = 0;
                Model.DrawMargin.Width = ActualWidth;
                Model.DrawMargin.Height = ActualHeight;
            };
        }

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

        
        protected Canvas Canvas { get; set; }
        internal Canvas DrawMargin { get; set; }

        private static Random Randomizer { get; set; }
        public static bool RandomizeStartingColor { get; set; }

        public static List<Color> Colors { get; set; }
        public int CurrentColorIndex { get; set; }

        public DispatcherTimer ResizeTimer { get; set; }

        #region Dependency Properties

        public static readonly DependencyProperty AxisYProperty = DependencyProperty.Register(
            "AxisY", typeof (List<Axis>), typeof (Chart),
            new PropertyMetadata(null, UpdateChart()));
        /// <summary>
        /// Gets or sets vertical axis
        /// </summary>
        public List<Axis> AxisY
        {
            get { return (List<Axis>)GetValue(AxisYProperty); }
            set { SetValue(AxisYProperty, value); }
        }

        public static readonly DependencyProperty AxisXProperty = DependencyProperty.Register(
            "AxisX", typeof (List<Axis>), typeof (Chart),
            new PropertyMetadata(null, UpdateChart()));
        /// <summary>
        /// Gets or sets horizontal axis
        /// </summary>
        public List<Axis> AxisX
        {
            get { return (List<Axis>)GetValue(AxisXProperty); }
            set { SetValue(AxisXProperty, value); }
        }

        public static readonly DependencyProperty ChartLegendProperty = DependencyProperty.Register(
            "ChartLegend", typeof (ChartLegend), typeof (Chart), 
            new PropertyMetadata(default(ChartLegend), UpdateChart()));
        /// <summary>
        /// Gets or sets the control to use as chart legend fot this chart.
        /// </summary>
        public ChartLegend ChartLegend
        {
            get { return (ChartLegend) GetValue(ChartLegendProperty); }
            set { SetValue(ChartLegendProperty, value); }
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof (ZoomingOptions), typeof (Chart),
            new PropertyMetadata(default(ZoomingOptions)));
        /// <summary>
        /// Gets or sets chart zoom behavior
        /// </summary>
        public ZoomingOptions Zoom
        {
            get { return (ZoomingOptions)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public static readonly DependencyProperty LegendLocationProperty = DependencyProperty.Register(
            "LegendLocation", typeof (LegendLocation), typeof (Chart),
            new PropertyMetadata(LegendLocation.None, UpdateChart()));
        /// <summary>
        /// Gets or sets where legend is located
        /// </summary>
        public LegendLocation LegendLocation
        {
            get { return (LegendLocation)GetValue(LegendLocationProperty); }
            set { SetValue(LegendLocationProperty, value); }
        }

        public static readonly DependencyProperty CursorXProperty = DependencyProperty.Register(
            "CursorX", typeof (ChartCursor), typeof (Chart), 
            new PropertyMetadata(default(ChartCursor), UpdateChart()));
        /// <summary>
        /// Gets or the current chart cursor
        /// </summary>
        public ChartCursor CursorX
        {
            get { return (ChartCursor) GetValue(CursorXProperty); }
            set { SetValue(CursorXProperty, value); }
        }

        public static readonly DependencyProperty CursorYProperty = DependencyProperty.Register(
            "CursorY", typeof (ChartCursor), typeof (Chart), new PropertyMetadata(default(ChartCursor)));
        /// <summary>
        /// Gets or sets the current chart cursor
        /// </summary>
        public ChartCursor CursorY
        {
            get { return (ChartCursor) GetValue(CursorYProperty); }
            set { SetValue(CursorYProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof (SeriesCollection), typeof (Chart),
            new PropertyMetadata(default(SeriesCollection), UpdateChart()));
        /// <summary>
        /// Gets or sets chart series collection to plot.
        /// </summary>
        public SeriesCollection Series
        {
            get { return (SeriesCollection) GetValue(SeriesProperty); }
            set
            {
#if DEBUG
                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    if (value == null || value.Count == 0)
                    {
                        var r = new Random();
                        SetValue(SeriesProperty,
                            new SeriesCollection(new SeriesConfiguration<double>().Y(val => val))
                            {
                                new LineSeries
                                {
                                    Values =
                                        new ChartValues<double>
                                        {
                                            r.NextDouble()*10,
                                            r.NextDouble()*10,
                                            r.NextDouble()*10,
                                            r.NextDouble()*10,
                                            r.NextDouble()*10
                                        }
                                }
                            });
                        return;
                    }
                    return;
                }
#endif
                SetValue(SeriesProperty, value);
            }
        }

        public static readonly DependencyProperty DataTooltipProperty = DependencyProperty.Register(
            "DataTooltip", typeof (ChartTooltip), typeof (Chart),
            new PropertyMetadata(default(ChartTooltip)));
        /// <summary>
        /// Gets or sets the current control to use as the chart tooltip
        /// </summary>
        public ChartTooltip DataTooltip
        {
            get { return (ChartTooltip) GetValue(DataTooltipProperty); }
            set { SetValue(DataTooltipProperty, value); }
        }

        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            "TooltipTimeout", typeof (TimeSpan), typeof (Chart),
            new PropertyMetadata(default(TimeSpan)));
        /// <summary>
        /// Gets or sets the time a tooltip takes to hide when the user leaves the data point.
        /// </summary>
        [TypeConverter(typeof(TimespanMillisecondsConverter))]
        public TimeSpan TooltipTimeout
        {
            get { return (TimeSpan) GetValue(TooltipTimeoutProperty); }
            set { SetValue(TooltipTimeoutProperty, value); }
        }

        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan), typeof (Chart), 
            new PropertyMetadata(default(TimeSpan), UpdateChart(true)));
        /// <summary>
        /// Gets or sets the default animation speed for this chart, you can override this speed for each element (series and axes)
        /// </summary>
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof (bool), typeof (Chart),
            new PropertyMetadata(default(bool), UpdateChart(true)));
        /// <summary>
        /// Gets or sets if the chart is animated or not.
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }
        #endregion

        public ChartCore Model
        {
            get { return ChartCoreModel; }
        }

        public event Action<object, ChartPoint> DataClick;

        public bool HasTooltip
        {
            get { return DataTooltip != null; }
        }

        public bool HasDataClickEventAttached
        {
            get { return DataClick != null; }
        }

        public bool IsControlLoaded { get; private set; }

        public static Color GetDefaultColor(int index)
        {
            return Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))];
        }

        public void SetDrawMarginTop(double value)
        {
            Canvas.SetTop(DrawMargin, value);
        }

        public void SetDrawMarginLeft(double value)
        {
            Canvas.SetLeft(DrawMargin, value);
        }

        public void SetDrawMarginHeight(double value)
        {
            DrawMargin.Height = value;
        }

        public void SetDrawMarginWidth(double value)
        {
            DrawMargin.Width = value;
        }

        public void Erase()
        {
            throw new NotImplementedException();
            //foreach (var yi in AxisY) yi.Reset();
            //foreach (var xi in AxisX) xi.Reset();
            //DrawMargin.Children.Clear();
            //Canvas.Children.Clear();
            //Shapes.Clear();
            //ShapesMapper.Clear();
        }

        public void AddToView(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            Canvas.Children.Add(wpfElement);
        }

        public void AddToDrawMargin(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            DrawMargin.Children.Add(wpfElement);
        }

        public void RemoveFromView(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            Canvas.Children.Remove(wpfElement);
        }

        public void RemoveFromDrawMargin(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            DrawMargin.Children.Remove(wpfElement);
        }

        public void ShowTooltip(ChartPoint sender, IEnumerable<ChartPoint> sibilings, LvcPoint at)
        {
            if (DataTooltip == null) return;

            if (DataTooltip.Parent == null)
            {
                AddToView(DataTooltip);
                Canvas.SetLeft(DataTooltip, 0d);
                Canvas.SetTop(DataTooltip, 0d);
                Panel.SetZIndex(DataTooltip, int.MaxValue);
            }

            DataTooltip.Visibility = Visibility.Visible;

            var ts = TimeSpan.FromMilliseconds(300);
            DataTooltip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(at.X, ts));
            DataTooltip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(at.Y, ts));
        }

        public void HideTooltop()
        {
            if (DataTooltip != null)
                DataTooltip.Visibility = Visibility.Hidden;
        }

        public void ShowLegend(LvcPoint at)
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

        public void HideLegend()
        {
            if (ChartLegend != null)
                ChartLegend.Visibility = Visibility.Hidden;
        }

        public LvcSize LoadLegend()
        {
            if (ChartLegend == null || LegendLocation == LegendLocation.None)
                return new LvcSize();

            if (ChartLegend.Parent == null)
                Canvas.Children.Add(ChartLegend);

            ChartLegend.Series = Series.Cast<Series>().Select(x => new SeriesViewModel
            {
                Fill = x.Fill,
                Stroke = x.Stroke,
                Title = x.Title
            });
            ChartLegend.Orientation = LegendLocation == LegendLocation.Bottom || LegendLocation == LegendLocation.Top
                ? Orientation.Horizontal
                : Orientation.Vertical;

            ChartLegend.UpdateLayout();
            ChartLegend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return new LvcSize(ChartLegend.DesiredSize.Width,
                ChartLegend.DesiredSize.Height);
        }

        public TimeSpan? GetZoomingSpeed()
        {
            return DisableAnimations ? null : (TimeSpan?) AnimationsSpeed;
        }

        public List<AxisCore> MapXAxes(ChartCore chart)
        {
            if (AxisX.Count == 0) AxisX.Add(DefaultAxes.CleanAxis);
            return AxisX.Select(x => x.AsCoreElement(Model)).ToList();
        }

        public List<AxisCore> MapYAxes(ChartCore chart)
        {
            if (AxisY.Count == 0) AxisY.Add(DefaultAxes.DefaultAxis);
            return AxisY.Select(x => x.AsCoreElement(Model)).ToList();
        }

        #region Event Handlers
        internal void DataMouseDown(object sender, MouseEventArgs e)
        {
            var result = Series.SelectMany(x => x.Values.Points).FirstOrDefault(x =>
            {
                var pointView = x.View as PointView;
                return pointView != null && Equals(pointView.HoverShape, sender);
            });
            if (DataClick != null) DataClick.Invoke(sender, result);
        }
        #endregion

        #region Property Changed
        protected static PropertyChangedCallback UpdateChart(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfChart = o as Chart;
                if (wpfChart == null) return;
                if (wpfChart.Model != null) wpfChart.Model.Updater.Run(animate);
            };
        }
        #endregion

        #region Obsoletes, this properties will dissapear in future versions
        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof(bool), typeof(Chart), new PropertyMetadata(true));
        [Obsolete("This property is obsolete, if you need to disable tooltips, set the Chart.DataTooltip to null")]
        public bool Hoverable
        {
            get { return (bool)GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }
        #endregion


#if DEBUG
        public void CountElements()
        {
            //Trying to avoid memory leaks
            Debug.WriteLine("Canvas Elements -> " + Canvas.Children.Count);
            Debug.WriteLine("Draw Margin Elements ->" + DrawMargin.Children.Count);
        }
#endif
    }
}
