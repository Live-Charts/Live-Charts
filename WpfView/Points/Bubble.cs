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
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LiveCharts.Wpf.Points
{
    internal class Bubble : PointView
    {
        public Ellipse Ellipse { get; set; }

        public override void DrawOrMove(object previousDrawn, object current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                var r = new Random();

                var p = new LvcPoint(r.NextDouble()*chart.DrawMargin.Width,
                    r.NextDouble()*chart.DrawMargin.Height);

                Canvas.SetTop(Ellipse, p.Y);
                Canvas.SetLeft(Ellipse, p.X);

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, p.Y);
                    Canvas.SetLeft(DataLabel, p.X);
                }
            }

            if (chart.View.DisableAnimations)
            {
                Canvas.SetTop(Ellipse, Location.Y - Ellipse.Width*.5);
                Canvas.SetLeft(Ellipse, Location.X - Ellipse.Height*.5);

                if (DataLabel != null)
                {
                    var cx = CorrectXLabel(Location.X - DataLabel.Height*.5, chart);
                    var cy = CorrectYLabel(Location.Y - DataLabel.Width*.5, chart);
                    
                    Canvas.SetTop(DataLabel, cy);
                    Canvas.SetLeft(DataLabel, cx);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                var cx = CorrectXLabel(Location.X - DataLabel.Height * .5, chart);
                var cy = CorrectYLabel(Location.Y - DataLabel.Width * .5, chart);

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(cx, animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(cy, animSpeed));
            }

            Ellipse.BeginAnimation(Canvas.TopProperty, 
                new DoubleAnimation(Location.Y - Ellipse.Height*.5, animSpeed));
            Ellipse.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(Location.X - Ellipse.Width, animSpeed));
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
