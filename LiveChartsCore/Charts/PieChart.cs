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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.CoreComponents;
using LiveCharts.Shapes;
using LiveCharts.Tooltip;
using LiveCharts.Viewers;

namespace LiveCharts
{
    public class PieChart : Chart
    {
        public PieChart()
        {
            SetValue(AxisXProperty, new Axis());
            SetValue(AxisYProperty,
                new Axis {FontWeight = FontWeights.Bold, FontSize = 11, FontFamily = new FontFamily("Calibri")});
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            DrawPadding = 20;
            AnimatesNewPoints = true;
            SupportsMultipleSeries = false;
        }

        #region Dependency Properties

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof (double), typeof (PieChart), new PropertyMetadata(0d));

        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets angle in degrees that indicates pie rotation, form 0 to 360, default is 0
        /// </summary>
        public double PieRotation { get; set; }

        /// <summary>
        /// Gets the total sum of the values in the chart.
        /// </summary>
        public double[] PieTotalSums { get; private set; }

        /// <summary>
        /// Gets or sets the distance between pie and shortest chart dimnsion.
        /// </summary>
        public double DrawPadding { get; set; }

        internal new bool HasValidSeriesAndValues
        {
            get { return Series.Any(x => x.Values != null && x.Values.Count > 0); }
        }
        #endregion

        #region Overriden Methods

        protected override void Scale()
        {
            var w = MockedArea != null ? MockedArea.Value.Width : ActualWidth;
            var h = MockedArea != null ? MockedArea.Value.Height : ActualHeight;

            Canvas.Width = w;
            Canvas.Height = h;
            PlotArea = new Rect(0, 0, w, h);
            RequiresScale = true;

            if (!HasValidSeriesAndValues) return;
            base.Scale();
            DrawAxes();
            //rest of the series are ignored by now, we only plot the firt one
            var hasInvalidSeries = Series.Cast<PieSeries>().Any(x => x == null);
            if (hasInvalidSeries)
                return;
            
            PieTotalSums = GetPieSum();
        }

        protected override void DrawAxes()
        {
            foreach (var l in Shapes) Canvas.Children.Remove(l);
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
                    var top = new Point(ActualWidth * .5 - legend.DesiredSize.Width * .5, 0);
                    PlotArea.Y += top.Y + legend.DesiredSize.Height;
                    PlotArea.Height -= legend.DesiredSize.Height;
                    Canvas.SetTop(legend, top.Y);
                    Canvas.SetLeft(legend, top.X);
                    break;
                case LegendLocation.Bottom:
                    var bot = new Point(ActualWidth * .5 - legend.DesiredSize.Width * .5, ActualHeight - legend.DesiredSize.Height);
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
                    Canvas.SetTop(legend, Canvas.ActualHeight * .5 - legend.DesiredSize.Height * .5);
                    Canvas.SetLeft(legend, ActualWidth - legend.DesiredSize.Width);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void LoadLegend(ChartLegend legend)
        {
            var series = Series.FirstOrDefault() as PieSeries;
            if (series == null) return;

            var l = new List<SeriesStandin>();
            var f = GetFormatter(AxisX);

            for (var index = 0; index < series.Values.Count; index++)
            {
                l.Add(new SeriesStandin
                {
                    Fill = series.Fill,
                    Stroke = series.Stroke,
                    Title = f(index)
                });
            }

            legend.Series = l;

            legend.Orientation = LegendLocation == LegendLocation.Bottom || LegendLocation == LegendLocation.Top
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        internal override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataTooltip == null) return;

            DataTooltip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = ShapesMapper.FirstOrDefault(s => Equals(s.HoverShape, sender));
            if (senderShape == null) return;
            var pieSlice = senderShape.HoverShape as PieSlice;
            if (pieSlice == null) return;

            var labels = AxisX.Labels != null ? AxisX.Labels.ToArray() : null;

            senderShape.Shape.Opacity = .8;
            var vx = senderShape.ChartPoint.X;

            var indexedToolTip = DataTooltip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                indexedToolTip.Header = labels == null
                        ? (AxisX.LabelFormatter == null
                            ? vx.ToString(CultureInfo.InvariantCulture)
                            : AxisX.LabelFormatter(vx))
                        : (labels.Length > vx
                            ? labels[(int)vx]
                            : "");
                indexedToolTip.Data = new[]
                {
                    new IndexedTooltipData
                    {
                        Index = (int) vx,
                        Stroke = senderShape.HoverShape.Stroke,
                        Fill = senderShape.HoverShape.Fill,
                        Series = senderShape.Series,
                        Point = senderShape.ChartPoint,
                        Value = AxisY.LabelFormatter == null
                            ? senderShape.ChartPoint.Y.ToString(CultureInfo.InvariantCulture)
                            : AxisY.LabelFormatter(senderShape.ChartPoint.Y)
                    }
                };
            }

            var alpha = pieSlice.RotationAngle + pieSlice.WedgeAngle*.5 + 180;
            var alphaRad = alpha*Math.PI/180;
            var sliceMidAngle = alpha - 180;

            DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var y = Canvas.ActualHeight*.5 + (sliceMidAngle > 90 && sliceMidAngle < 270 ? -1 : 0)* DataTooltip.DesiredSize.Height - Math.Cos(alphaRad)*15;
            var x = Canvas.ActualWidth*.5 + (sliceMidAngle > 0 && sliceMidAngle < 180 ? -1 : 0) * DataTooltip.DesiredSize.Width + Math.Sin(alphaRad)*15;

            var p = new Point(x, y);

            DataTooltip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X,
                Duration = TimeSpan.FromMilliseconds(200)
            });
            DataTooltip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y,
                Duration = TimeSpan.FromMilliseconds(200)
            });
            
            pieSlice.Opacity = .8;

            var anim = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);
        }

        internal override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            base.DataMouseLeave(sender, e);
            var senderShape = ShapesMapper.FirstOrDefault(s => Equals(s.HoverShape, sender));
            var pieSlice = senderShape != null ? senderShape.HoverShape as PieSlice : null;
            if (pieSlice == null) return;
            pieSlice.Opacity = 1;

            var anim = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);
        }

        #endregion

        #region Private Methods

        private double[] GetPieSum()
        {
            var l = new List<double>();

            var fSeries = Series.First();

            var pts = Series.Select(x => x.Values.Points.ToList()).ToList();

            for (var i = 0; i < fSeries.Values.Count; i++)
            {
                l.Add(0);
                foreach (var series in pts)
                {
                    if (series.Count - 1 >= i)
                        l[i] += series[i].Y;
                }
            }

            return l.ToArray();
        }

        #endregion
    }
}