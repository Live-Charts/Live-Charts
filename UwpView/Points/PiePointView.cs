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
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp.Points
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
                Slice.Width = chart.DrawMargin.Width;
                Slice.Height = chart.DrawMargin.Height;

                Slice.WedgeAngle = 0;
                Slice.RotationAngle = 0;
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, chart.DrawMargin.Height / 2);
                Canvas.SetLeft(DataLabel, chart.DrawMargin.Width / 2);
            }

            if (HoverShape != null)
            {
                var hs = (PieSlice) HoverShape;

                hs.Width = chart.DrawMargin.Width;
                hs.Height = chart.DrawMargin.Height;
                hs.WedgeAngle = Wedge;
                hs.RotationAngle = Rotation;
                hs.InnerRadius = InnerRadius;
                hs.Radius = Radius;
            }

            Slice.Width = chart.DrawMargin.Width;
            Slice.Height = chart.DrawMargin.Height;

            var lh = 0d;
            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();
                lh = DataLabel.ActualHeight;
            }

            var hypo = ((PieSeries)current.SeriesView).LabelPosition == PieLabelPosition.InsideSlice
                ? (Radius + InnerRadius) * (Math.Abs(InnerRadius) < 0.01 ? .65 : .5)
                : Radius + lh;
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
                    DataLabel.UpdateLayout();

                    var lx = cp.X + chart.DrawMargin.Width / 2 - DataLabel.ActualWidth * .5;
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

                var lx = cp.X + chart.DrawMargin.Width / 2 - DataLabel.ActualWidth * .5;
                var ly = chart.DrawMargin.Height / 2 - cp.Y - DataLabel.ActualHeight * .5;

                DataLabel.CreateCanvasStoryBoardAndBegin(lx, ly, animSpeed);
            }

            Slice.BeginDoubleAnimation("(Canvas.Left)", chart.DrawMargin.Width / 2, animSpeed);
            Slice.BeginDoubleAnimation("(Canvas.Top)", chart.DrawMargin.Height / 2, animSpeed);
            Slice.BeginDoubleAnimation(nameof(PieSlice.InnerRadius), InnerRadius, animSpeed);
            Slice.BeginDoubleAnimation(nameof(PieSlice.Radius), Radius, animSpeed);
            Slice.BeginDoubleAnimation(nameof(PieSlice.WedgeAngle), Wedge, animSpeed);
            Slice.BeginDoubleAnimation(nameof(PieSlice.RotationAngle), Rotation, animSpeed);
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Slice);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            Slice.Fill = Slice.Fill.Clone();

            if (Slice?.Fill != null)
            {
                Slice.Fill.Opacity -= .15;
            }

            if (Slice != null)
            {
                var pieChart = (PieChart) point.SeriesView.Model.Chart.View;

                Slice.BeginDoubleAnimation(nameof(PieSlice.PushOut), Slice.PushOut,
                    OriginalPushOut + pieChart.HoverPushOut,
                    point.SeriesView.Model.Chart.View.AnimationsSpeed);
            }
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            if (point.Fill != null)
            {
                Slice.Fill = (Brush)point.Fill;
            }
            else
            {
                Slice.Fill = ((Series)point.SeriesView).Fill;
            }
            Slice.BeginDoubleAnimation(nameof(Slice.PushOut), OriginalPushOut,
                point.SeriesView.Model.Chart.View.AnimationsSpeed);
        }
    }
}
