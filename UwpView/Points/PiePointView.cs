﻿//The MIT License(MIT)

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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
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
                Canvas.SetTop(Slice, chart.DrawMargin.Height/2);
                Canvas.SetLeft(Slice, chart.DrawMargin.Width/2);

                Slice.WedgeAngle = 0;
                Slice.RotationAngle = 0;

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, 0d);
                    Canvas.SetLeft(DataLabel, 0d);
                }
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

            Canvas.SetTop(Slice, chart.DrawMargin.Height / 2);
            Canvas.SetLeft(Slice, chart.DrawMargin.Width / 2);
            Slice.InnerRadius = InnerRadius;
            Slice.Radius = Radius;

            var hypo = (Slice.Radius + Slice.InnerRadius)/2;
            var gamma = current.Participation*360/2 + Rotation;
            var cp = new Point(hypo * Math.Sin(gamma * (Math.PI / 180)), hypo * Math.Cos(gamma * (Math.PI / 180)));

            if (chart.View.DisableAnimations)
            {
                Slice.WedgeAngle = Wedge;
                Slice.RotationAngle = Rotation;

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    var lx = cp.X + chart.DrawMargin.Width / 2 - DataLabel.ActualWidth * .5;
                    var ly = chart.DrawMargin.Height/2 - cp.Y - DataLabel.ActualHeight*.5;

                    Canvas.SetTop(DataLabel, lx);
                    Canvas.SetLeft(DataLabel, ly);
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

            var wedge = AnimationHelper.CreateDouble(Wedge, animSpeed, nameof(PieSlice.WedgeAngle));
            var rotation = AnimationHelper.CreateDouble(Rotation, animSpeed, nameof(PieSlice.RotationAngle));
            AnimationHelper.CreateStoryBoardAndBegin(Slice, wedge, rotation);
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Slice);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            var copy = Slice.Fill;//.Clone();
            copy.Opacity -= .15;
            Slice.Fill = copy;

            var pieChart = (PieChart) point.SeriesView.Model.Chart.View;

            Slice.BeginDoubleAnimation(nameof(Slice.PushOut), Slice.PushOut,
                OriginalPushOut + pieChart.HoverPushOut,
                point.SeriesView.Model.Chart.View.AnimationsSpeed);
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            BindingOperations.SetBinding(Slice, Shape.FillProperty,
                new Binding
                {
                    Path = new PropertyPath("Series.Fill"),
                    Source = ((Series) point.SeriesView)
                });

            Slice.BeginDoubleAnimation(nameof(PieSlice.PushOut), OriginalPushOut,
                point.SeriesView.Model.Chart.View.AnimationsSpeed);
        }
    }
}
