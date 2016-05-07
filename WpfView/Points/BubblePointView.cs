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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Points
{
    internal class BubblePointView : PointView
    {
        public Ellipse Ellipse { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart, ISeriesView series)
        {
            if (IsNew)
            {
                Canvas.SetTop(Ellipse, current.ChartLocation.Y);
                Canvas.SetLeft(Ellipse, current.ChartLocation.X);

                Ellipse.Width = 0;
                Ellipse.Height = 0;

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, current.ChartLocation.Y);
                    Canvas.SetLeft(DataLabel, current.ChartLocation.X);
                }
            }

            var p1 = new LvcPoint();
            var p2 = new LvcPoint();

            var bubbleSeries = (IBubbleSeries) series;

            p1.X = chart.Value3Limit.Max;
            p1.Y = bubbleSeries.MaxBubbleDiameter;

            p2.X = chart.Value3Limit.Min;
            p2.Y = bubbleSeries.MinBubbleDiameter;

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);

            var diameter = m*(current.Weight - p1.X) + p1.Y;

            if (chart.View.DisableAnimations)
            {
                Ellipse.Width = diameter;
                Ellipse.Height = diameter;

                Canvas.SetTop(Ellipse, current.ChartLocation.Y - Ellipse.Width*.5);
                Canvas.SetLeft(Ellipse, current.ChartLocation.X - Ellipse.Height*.5);

                if (DataLabel != null)
                {
                    var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.Height*.5, chart);
                    var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.Width*.5, chart);
                    
                    Canvas.SetTop(DataLabel, cy);
                    Canvas.SetLeft(DataLabel, cx);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.Height*.5, chart);
                var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.Width*.5, chart);

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(cx, animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(cy, animSpeed));
            }

            Ellipse.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(diameter, animSpeed));
            Ellipse.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(diameter, animSpeed));

            Ellipse.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(current.ChartLocation.Y - diameter * .5, animSpeed));
            Ellipse.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(current.ChartLocation.X - diameter * .5, animSpeed));
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Ellipse);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + DataLabel.ActualWidth > chart.DrawMargin.Width)
                desiredPosition -= desiredPosition + DataLabel.ActualWidth - chart.DrawMargin.Width + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            desiredPosition -= Ellipse.ActualHeight * .5 + DataLabel.ActualHeight * .5 + 2;

            if (desiredPosition + DataLabel.ActualHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
    }
}
