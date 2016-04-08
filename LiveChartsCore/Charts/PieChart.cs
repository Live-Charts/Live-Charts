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
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            DefaultFillOpacity = .85;
            DrawPadding = 20;
            AnimatesNewPoints = true;
            SupportsMultipleSeries = false;
        }

        #region Dependency Properties

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof (double), typeof (PieChart), new PropertyMetadata(0d));

        private double _pieRotation;

        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets angle in degrees that indicates pie rotation, default is 0
        /// </summary>
        public double PieRotation
        {
            get { return _pieRotation; }
            set
            {
                _pieRotation = value;
            }
        }

        /// <summary>
        /// Gets the total sum of the values in the chart.
        /// </summary>
        public List<PieResult> PieTotalSums { get; private set; }

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

        protected override void PrepareAxes()
        {
            var w = MockedArea != null ? MockedArea.Value.Width : ActualWidth;
            var h = MockedArea != null ? MockedArea.Value.Height : ActualHeight;

            RequiresScale = true;

            if (!HasValidSeriesAndValues) return;
            base.PrepareAxes();
            CalculateComponentsAndMargin();
            //rest of the series are ignored by now, we only plot the firt one
            var hasInvalidSeries = Series.Cast<PieSeries>().Any(x => x == null);
            if (hasInvalidSeries)
                return;
            
            PieTotalSums = GetPieSum();
        }

        protected override void CalculateComponentsAndMargin()
        {
            foreach (var l in Shapes) Canvas.Children.Remove(l);

            LoadLegend();

            if (LegendLocation != LegendLocation.None)
            {
                Legend.UpdateLayout();
                Legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            switch (LegendLocation)
            {
                case LegendLocation.None:
                    Legend.Visibility = Visibility.Hidden;
                    break;
                case LegendLocation.Top:
                    var top = new Point(ActualWidth * .5 - Legend.DesiredSize.Width * .5, 0);
                    Canvas.SetTop(DrawMargin, Canvas.GetTop(DrawMargin) + top.Y + Legend.DesiredSize.Height);
                    DrawMargin.Height -= Legend.DesiredSize.Height;
                    Canvas.SetTop(Legend, top.Y);
                    Canvas.SetLeft(Legend, top.X);
                    break;
                case LegendLocation.Bottom:
                    var bot = new Point(ActualWidth * .5 - Legend.DesiredSize.Width * .5, ActualHeight - Legend.DesiredSize.Height);
                    DrawMargin.Height -= Legend.DesiredSize.Height;
                    Canvas.SetTop(Legend, Canvas.ActualHeight - Legend.DesiredSize.Height);
                    Canvas.SetLeft(Legend, bot.X);
                    break;
                case LegendLocation.Left:
                    Canvas.SetLeft(DrawMargin, Canvas.GetLeft(DrawMargin) + Legend.DesiredSize.Width);
                    DrawMargin.Width -= Legend.DesiredSize.Width;
                    Canvas.SetTop(Legend, Canvas.ActualHeight * .5 - Legend.DesiredSize.Height * .5);
                    Canvas.SetLeft(Legend, 0);
                    break;
                case LegendLocation.Right:
                    DrawMargin.Width -= Legend.DesiredSize.Width;
                    Canvas.SetTop(Legend, Canvas.ActualHeight * .5 - Legend.DesiredSize.Height * .5);
                    Canvas.SetLeft(Legend, ActualWidth - Legend.DesiredSize.Width);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            var xi = senderShape.Series.ScalesXAt;
            var yi = senderShape.Series.ScalesYAt;

            var labels = AxisX[xi].Labels != null ? AxisX[xi].Labels.ToArray() : null;

            senderShape.Shape.Opacity = .8;
            var vx = senderShape.ChartPoint.X;

            var indexedToolTip = DataTooltip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                indexedToolTip.Header = null;
                    //labels == null
                    //    ? (AxisX.LabelFormatter == null
                    //        ? vx.ToString(CultureInfo.InvariantCulture)
                    //        : AxisX.LabelFormatter(vx))
                    //    : (labels.Length > vx
                    //        ? labels[(int)vx]
                    //        : "");
                indexedToolTip.Data = new[]
                {
                    new IndexedTooltipData
                    {
                        Index = (int) vx,
                        Stroke = senderShape.HoverShape.Stroke,
                        Fill = senderShape.HoverShape.Fill,
                        Series = senderShape.Series,
                        Point = senderShape.ChartPoint,
                        Value = AxisY[yi].LabelFormatter == null
                            ? senderShape.ChartPoint.Y.ToString(CultureInfo.InvariantCulture)
                            : AxisY[yi].LabelFormatter(senderShape.ChartPoint.Y)
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
        
        private List<PieResult> GetPieSum()
        {
            var l = new List<PieResult>();

            var fSeries = Series.First();

            var pts = Series.Select(x => x.Values.Points.ToList()).ToList();

            for (var i = 0; i < fSeries.Values.Count; i++)
            {
                l.Add(new PieResult());
                foreach (var series in pts)
                {
                    if (series.Count - 1 >= i)
                    {
                        var thisVal = series[i].Y;
                        l[i].Stack.Add(thisVal);
                        l[i].TotalSum += thisVal;
                    }
                        
                }
            }

            foreach (var pieResult in l)
            {
                //Only when it is exactly zero.
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (pieResult.TotalSum == 0) pieResult.TotalSum = double.MaxValue;
                for (var i = 0; i < pieResult.Stack.Count; i++)
                {
                    pieResult.Participation.Add(pieResult.Stack[i]/pieResult.TotalSum);
                    pieResult.Rotation.Add(i > 0
                        ? pieResult.Rotation[i - 1] + pieResult.Participation[i - 1]
                        : PieRotation/360d);
                }
            }

            return l;
        }

        #endregion
    }

    public class PieResult
    {
        public PieResult()
        {
            TotalSum = 0;
            Rotation = new List<double>();
            Stack = new List<double>();
            Participation = new List<double>();
        }
        public double TotalSum { get; set; }
        public List<double> Stack { get; set; }
        public List<double> Participation { get; set; }
        public List<double> Rotation { get; set; }
    }
}