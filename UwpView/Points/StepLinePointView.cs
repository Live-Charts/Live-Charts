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

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp.Points
{
    internal class StepLinePointView : PointView, IStepPointView
    {
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public Line VerticalLine { get; set; }
        public Line HorizontalLine { get; set; }
        public Path Shape { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                VerticalLine.X1 = current.ChartLocation.X;
                VerticalLine.X2 = current.ChartLocation.X;
                VerticalLine.Y1 = chart.DrawMargin.Height;
                VerticalLine.Y2 = chart.DrawMargin.Height;

                HorizontalLine.X1 = current.ChartLocation.X - DeltaX;
                HorizontalLine.X2 = current.ChartLocation.X;
                HorizontalLine.Y1 = chart.DrawMargin.Height;
                HorizontalLine.Y2 = chart.DrawMargin.Height;

                if (Shape != null)
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width/2);
                    Canvas.SetTop(Shape, chart.DrawMargin.Height);
                }
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, chart.DrawMargin.Height);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X);
            }

            if (HoverShape != null)
            {
                HoverShape.Width = Shape != null ? (Shape.Width > 5 ? Shape.Width : 5) : 5;
                HoverShape.Height = Shape != null ? (Shape.Height > 5 ? Shape.Height : 5) : 5;
                Canvas.SetLeft(HoverShape, current.ChartLocation.X - HoverShape.Width / 2);
                Canvas.SetTop(HoverShape, current.ChartLocation.Y - HoverShape.Height / 2);
            }

            if (chart.View.DisableAnimations)
            {
                VerticalLine.X1 = current.ChartLocation.X;
                VerticalLine.X2 = current.ChartLocation.X;
                VerticalLine.Y1 = current.ChartLocation.Y;
                VerticalLine.Y2 = current.ChartLocation.Y - DeltaY;

                HorizontalLine.X1 = current.ChartLocation.X - DeltaX;
                HorizontalLine.X2 = current.ChartLocation.X;
                HorizontalLine.Y1 = current.ChartLocation.Y - DeltaY;
                HorizontalLine.Y2 = current.ChartLocation.Y - DeltaY;

                if (Shape != null)
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width/2);
                    Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height/2);
                }

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();
                    var xl = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth * .5, chart);
                    var yl = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight * .5, chart);
                    Canvas.SetLeft(DataLabel, xl);
                    Canvas.SetTop(DataLabel, yl);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            VerticalLine.BeginDoubleAnimation(nameof(Line.X1), current.ChartLocation.X, animSpeed);
            VerticalLine.BeginDoubleAnimation(nameof(Line.X2), current.ChartLocation.X, animSpeed);
            VerticalLine.BeginDoubleAnimation(nameof(Line.Y1), current.ChartLocation.Y, animSpeed);
            VerticalLine.BeginDoubleAnimation(nameof(Line.Y2), current.ChartLocation.Y - DeltaY, animSpeed);

            HorizontalLine.BeginDoubleAnimation(nameof(Line.X1), current.ChartLocation.X - DeltaX, animSpeed);
            HorizontalLine.BeginDoubleAnimation(nameof(Line.X2), current.ChartLocation.X, animSpeed);
            HorizontalLine.BeginDoubleAnimation(nameof(Line.Y1), current.ChartLocation.Y - DeltaY, animSpeed);
            HorizontalLine.BeginDoubleAnimation(nameof(Line.Y2), current.ChartLocation.Y - DeltaY, animSpeed);

            Shape?.CreateCanvasStoryBoardAndBegin(current.ChartLocation.X - Shape.Width/2,
                current.ChartLocation.Y - Shape.Height/2, animSpeed);

            if (DataLabel == null) return;

            {
                DataLabel.UpdateLayout();
                var xl = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth * .5, chart);
                var yl = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight * .5, chart);
                Canvas.SetLeft(DataLabel, xl);
                Canvas.SetTop(DataLabel, yl);
            }
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Shape);
            chart.View.RemoveFromDrawMargin(DataLabel);
            chart.View.RemoveFromDrawMargin(VerticalLine);
            chart.View.RemoveFromDrawMargin(HorizontalLine);
        }

        public override void OnHover(ChartPoint point)
        {
            var lineSeries = (StepLineSeries) point.SeriesView;
            if (Shape != null) Shape.Fill = Shape.Stroke;
            lineSeries.StrokeThickness = lineSeries.StrokeThickness + 1;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            var lineSeries = (StepLineSeries) point.SeriesView;
            if (Shape != null)
                Shape.Fill = point.Fill == null
                    ? lineSeries.PointForeround
                    : (Brush) point.Fill;
            lineSeries.StrokeThickness = lineSeries.StrokeThickness - 1;
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + DataLabel.ActualWidth * .5 < -0.1) return -DataLabel.ActualWidth;

            if (desiredPosition + DataLabel.ActualWidth > chart.DrawMargin.Width)
                desiredPosition -= desiredPosition + DataLabel.ActualWidth - chart.DrawMargin.Width + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            desiredPosition -= (Shape?.ActualHeight * .5 ?? 0) + DataLabel.ActualHeight * .5 + 2;

            if (desiredPosition + DataLabel.ActualHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
    }
}
