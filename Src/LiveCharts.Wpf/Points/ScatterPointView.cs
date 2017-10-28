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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;

namespace LiveCharts.Wpf.Points
{
    internal class ScatterPointView : PointView, IScatterPointView
    {
        public Shape Shape { get; set; }
        public double Diameter { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                Canvas.SetTop(Shape, current.ChartLocation.Y);
                Canvas.SetLeft(Shape, current.ChartLocation.X);

                Shape.Width = 0;
                Shape.Height = 0;
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, current.ChartLocation.Y);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X);
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
                Shape.Width = Diameter;
                Shape.Height = Diameter;

                Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height*.5);
                Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width*.5);

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth*.5, chart);
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

            Shape.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(Diameter, animSpeed));
            Shape.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(Diameter, animSpeed));

            Shape.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(current.ChartLocation.Y - Diameter*.5, animSpeed));
            Shape.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(current.ChartLocation.X - Diameter*.5, animSpeed));
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Shape);
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
            var copy = Shape.Fill.Clone();
            copy.Opacity -= .15;
            Shape.Fill = copy;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            if (Shape == null) return;

            if (point.Fill != null)
            {
                Shape.Fill = (Brush) point.Fill;
            }
            else
            {
                Shape.Fill = ((Series) point.SeriesView).Fill;
            }
        }
    }
}
