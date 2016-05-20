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
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Points
{
    internal class BubblePointView : PointView, IBubblePointView
    {
        public Ellipse Ellipse { get; set; }
        public double Diameter { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
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

            if (HoverShape != null)
            {
                HoverShape.Width = Diameter;
                HoverShape.Height = Diameter;
                Canvas.SetLeft(HoverShape, current.ChartLocation.X - Diameter / 2);
                Canvas.SetTop(HoverShape, current.ChartLocation.Y - Diameter / 2);
            }

            if (chart.View.DisableAnimations)
            {
                Ellipse.Width = Diameter;
                Ellipse.Height = Diameter;

                Canvas.SetTop(Ellipse, current.ChartLocation.Y - Ellipse.Width*.5);
                Canvas.SetLeft(Ellipse, current.ChartLocation.X - Ellipse.Height*.5);

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualHeight*.5, chart);
                    var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight*.5, chart);
                    
                    Canvas.SetTop(DataLabel, cy);
                    Canvas.SetLeft(DataLabel, cx);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth*.5, chart);
                var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight*.5, chart);

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(cx, animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(cy, animSpeed));
            }

            Ellipse.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(Diameter, animSpeed));
            Ellipse.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(Diameter, animSpeed));

            Ellipse.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(current.ChartLocation.Y - Diameter*.5, animSpeed));
            Ellipse.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(current.ChartLocation.X - Diameter*.5, animSpeed));
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
                desiredPosition -= desiredPosition + DataLabel.ActualWidth - chart.DrawMargin.Width;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + DataLabel.ActualHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.DrawMargin.Height;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        public override void OnHover(ChartPoint point)
        {
            var copy = Ellipse.Fill.Clone();
            copy.Opacity -= .15;
            Ellipse.Fill = copy;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            BindingOperations.SetBinding(Ellipse, Shape.FillProperty,
                new Binding
                {
                    Path = new PropertyPath(Series.Series.FillProperty),
                    Source = ((Series.Series) point.SeriesView)
                });
        }
    }
}
