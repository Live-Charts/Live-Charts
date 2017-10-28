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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;

namespace LiveCharts.Wpf.Points
{
    internal class PiePointView : PointView, IPieSlicePointView
    {
        public double Rotation { get; set; }
        public double InnerRadius { get; set; }
        public double Radius { get; set; }
        public double Wedge { get; set; }
        public PieSlice Slice { get; set; }
        public double OriginalPushOut { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                Canvas.SetTop(Slice, chart.DrawMargin.Height/2);
                Canvas.SetLeft(Slice, chart.DrawMargin.Width/2);

                Slice.WedgeAngle = 0;
                Slice.RotationAngle = 0;
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, chart.DrawMargin.Height/2);
                Canvas.SetLeft(DataLabel, chart.DrawMargin.Width/2);
            }

            if (HoverShape != null)
            {
                var hs = (PieSlice) HoverShape;

                Canvas.SetTop(hs, chart.DrawMargin.Height/2);
                Canvas.SetLeft(hs, chart.DrawMargin.Width/2);
                hs.WedgeAngle = Wedge;
                hs.RotationAngle = Rotation;
                hs.InnerRadius = InnerRadius;
                hs.Radius = Radius;
            }

            var lh = 0d;
            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();
                lh = DataLabel.ActualHeight;
            }

            var hypo = ((PieSeries) current.SeriesView).LabelPosition == PieLabelPosition.InsideSlice
                ? (Radius + InnerRadius)*(Math.Abs(InnerRadius) < 0.01 ? .65 : .5)
                : Radius+lh;
            var gamma = current.Participation*360/2 + Rotation;
            var cp = new Point(hypo * Math.Sin(gamma * (Math.PI / 180)), hypo * Math.Cos(gamma * (Math.PI / 180)));

            if (chart.View.DisableAnimations)
            {
                Slice.InnerRadius = InnerRadius;
                Slice.Radius = Radius;
                Slice.WedgeAngle = Wedge;
                Slice.RotationAngle = Rotation;
                Canvas.SetTop(Slice, chart.DrawMargin.Height / 2);
                Canvas.SetLeft(Slice, chart.DrawMargin.Width / 2);

                if (DataLabel != null)
                {
                    var lx = cp.X + chart.DrawMargin.Width/2 - DataLabel.ActualWidth * .5;
                    var ly = chart.DrawMargin.Height/2 - cp.Y - DataLabel.ActualHeight*.5;

                    Canvas.SetLeft(DataLabel, lx);
                    Canvas.SetTop(DataLabel, ly);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                var lx = cp.X + chart.DrawMargin.Width/2 - DataLabel.ActualWidth * .5;
                var ly = chart.DrawMargin.Height/2 - cp.Y - DataLabel.ActualHeight * .5;

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(lx, animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(ly, animSpeed));
            }

            Slice.BeginAnimation(Canvas.LeftProperty, 
                new DoubleAnimation(chart.DrawMargin.Width / 2, animSpeed));
            Slice.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(chart.DrawMargin.Height/2, animSpeed));
            Slice.BeginAnimation(PieSlice.InnerRadiusProperty, new DoubleAnimation(InnerRadius, animSpeed));
            Slice.BeginAnimation(PieSlice.RadiusProperty, new DoubleAnimation(Radius, animSpeed));
            Slice.BeginAnimation(PieSlice.WedgeAngleProperty, new DoubleAnimation(Wedge, animSpeed));
            Slice.BeginAnimation(PieSlice.RotationAngleProperty, new DoubleAnimation(Rotation, animSpeed));
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Slice);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            var copy = Slice.Fill.Clone();
            copy.Opacity -= .15;
            Slice.Fill = copy;

            var pieChart = (PieChart) point.SeriesView.Model.Chart.View;

            Slice.BeginAnimation(PieSlice.PushOutProperty,
                new DoubleAnimation(Slice.PushOut, OriginalPushOut + pieChart.HoverPushOut,
                    point.SeriesView.Model.Chart.View.AnimationsSpeed));
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            if (point.Fill != null)
            {
                Slice.Fill = (Brush)point.Fill;
            }
            else
            {
                Slice.Fill = ((Series) point.SeriesView).Fill;
            }
            Slice.BeginAnimation(PieSlice.PushOutProperty,
                new DoubleAnimation(OriginalPushOut, point.SeriesView.Model.Chart.View.AnimationsSpeed));
        }
    }
}
