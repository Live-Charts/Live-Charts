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

namespace LiveCharts
{
    public class PieChart : Chart
    {
        public PieChart()
        {
            AxisY = new Axis {FontWeight = FontWeights.Bold, FontSize = 11, FontFamily = new FontFamily("Calibri")};
            AxisX = new Axis();
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            InnerRadius = 0;
            SlicePadding = 5;
            DrawPadding = 20;
            Background = Brushes.White;
            AnimatesNewPoints = true;
        }

        #region Dependency Properties

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof (double), typeof (PieChart));

        /// <summary>
        /// Gets or sets chart inner radius, set this property to transform a pie chart into a doughnut!
        /// </summary>
        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty SlicePaddingProperty = DependencyProperty.Register(
            "SlicePadding", typeof (double), typeof (Chart));

        /// <summary>
        /// Gets or sets padding between slices.
        /// </summary>
        public double SlicePadding
        {
            get { return (double) GetValue(SlicePaddingProperty); }
            set { SetValue(SlicePaddingProperty, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the total sum of the values in the chart.
        /// </summary>
        public double PieTotalSum { get; private set; }

        /// <summary>
        /// Gets or sets the distance between pie and shortest chart dimnsion.
        /// </summary>
        public double DrawPadding { get; set; }

        #endregion

        #region Overriden Methods

        protected override void Scale()
        {
            base.Scale();
            DrawAxes();
            //rest of the series are ignored by now, we only plot the firt one
            var serie = Series.FirstOrDefault();
            var pieSerie = serie as PieSeries;
            if (pieSerie == null) return;
            PieTotalSum = GetPieSum();
        }

        protected override void DrawAxes()
        {
            foreach (var l in Shapes) Canvas.Children.Remove(l);
        }

        public override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataToolTip == null) return;

            DataToolTip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;
            var pieSlice = senderShape.Shape as PieSlice;
            if (pieSlice == null) return;

            var labels = AxisX.Labels != null ? AxisX.Labels.ToArray() : null;

            senderShape.Target.Opacity = .8;
            var vx = senderShape.Value.X;

            var indexedToolTip = DataToolTip as IndexedTooltip;
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
                        Stroke = senderShape.Shape.Stroke,
                        Fill = senderShape.Shape.Fill,
                        Series = senderShape.Series,
                        Point = senderShape.Value,
                        Value = AxisY.LabelFormatter == null
                            ? senderShape.Value.Y.ToString(CultureInfo.InvariantCulture)
                            : AxisY.LabelFormatter(senderShape.Value.Y)
                    }
                };
            }

            var alpha = pieSlice.RotationAngle + pieSlice.WedgeAngle*.5 + 180;
            var alphaRad = alpha*Math.PI/180;
            var sliceMidAngle = alpha - 180;

            DataToolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var y = Canvas.ActualHeight*.5 + (sliceMidAngle > 90 && sliceMidAngle < 270 ? -1 : 0)*DataToolTip.DesiredSize.Height - Math.Cos(alphaRad)*15;
            var x = Canvas.ActualWidth*.5 + (sliceMidAngle > 0 && sliceMidAngle < 180 ? -1 : 0) * DataToolTip.DesiredSize.Width + Math.Sin(alphaRad)*15;

            var p = new Point(x, y);

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
            
            pieSlice.Opacity = .8;

            var anim = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);
        }

        public override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            base.DataMouseLeave(sender, e);
            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            var pieSlice = senderShape != null ? senderShape.Shape as PieSlice : null;
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

        private double GetPieSum()
        {
            var serie = Series.FirstOrDefault();
            var pieSerie = serie as PieSeries;
            return pieSerie != null ? pieSerie.Values.Points.Select(pt => pt.Y).DefaultIfEmpty(0).Sum() : 0;
        }

        #endregion
    }
}