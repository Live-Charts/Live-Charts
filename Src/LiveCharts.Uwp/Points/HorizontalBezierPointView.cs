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

using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp.Points
{
    internal class HorizontalBezierPointView : PointView, IBezierPointView
    {
        public BezierSegment Segment { get; set; }
        public Path Shape { get; set; }
        public PathFigure Container { get; set; }
        public BezierData Data { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            var previosPbv = previousDrawn == null
                ? null
                : (HorizontalBezierPointView) previousDrawn.View;

            var y = chart.DrawMargin.Top + chart.DrawMargin.Height;

            Container.Segments.Remove(Segment);
            Container.Segments.Insert(index, Segment);

            if (IsNew)
            {
                if (previosPbv != null && !previosPbv.IsNew)
                {
                    Segment.Point1 = previosPbv.Segment.Point3;
                    Segment.Point2 = previosPbv.Segment.Point3;
                    Segment.Point3 = previosPbv.Segment.Point3;

                    if (DataLabel != null)
                    {
                        Canvas.SetTop(DataLabel, Canvas.GetTop(previosPbv.DataLabel));
                        Canvas.SetLeft(DataLabel, Canvas.GetLeft(previosPbv.DataLabel));
                    }

                    if (Shape != null)
                    {
                        Canvas.SetTop(Shape, Canvas.GetTop(previosPbv.Shape));
                        Canvas.SetLeft(Shape, Canvas.GetLeft(previosPbv.Shape));
                    }
                }
                else
                {
                    Segment.Point1 = new Point(Data.Point1.X, y);
                    Segment.Point2 = new Point(Data.Point2.X, y);
                    Segment.Point3 = new Point(Data.Point3.X, y);

                    if (DataLabel != null)
                    {
                        Canvas.SetTop(DataLabel, y);
                        Canvas.SetLeft(DataLabel, current.ChartLocation.X - DataLabel.ActualWidth * .5);
                    }

                    if (Shape != null)
                    {
                        Canvas.SetTop(Shape, y);
                        Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width*.5);
                    }
                }
            }
            else if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, y);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X - DataLabel.ActualWidth*.5);
            }

            #region No Animated

                if (chart.View.DisableAnimations)
            {
                Segment.Point1 = Data.Point1.AsPoint();
                Segment.Point2 = Data.Point2.AsPoint();
                Segment.Point3 = Data.Point3.AsPoint();

                if (HoverShape != null)
                {
                    Canvas.SetLeft(HoverShape, current.ChartLocation.X - HoverShape.Width*.5);
                    Canvas.SetTop(HoverShape, current.ChartLocation.Y - HoverShape.Height*.5);
                }

                if (Shape != null)
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width*.5);
                    Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height*.5);
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

            #endregion

            var animSpeed = chart.View.AnimationsSpeed;

            Segment.BeginPointAnimation(nameof(BezierSegment.Point1), Data.Point1.AsPoint(), animSpeed);
            Segment.BeginPointAnimation(nameof(BezierSegment.Point2), Data.Point2.AsPoint(), animSpeed);
            Segment.BeginPointAnimation(nameof(BezierSegment.Point3), Data.Point3.AsPoint(), animSpeed);

            if (Shape != null)
            {
                if (double.IsNaN(Canvas.GetLeft(Shape)))
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width * .5);
                    Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height * .5);
                }
                else
                {
                    Shape.CreateCanvasStoryBoardAndBegin(current.ChartLocation.X - Shape.Width*.5,
                        current.ChartLocation.Y - Shape.Height*.5, chart.View.AnimationsSpeed);
                }
            }

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                var xl = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth*.5, chart);
                var yl = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight*.5, chart);

                DataLabel.CreateCanvasStoryBoardAndBegin(xl, yl, chart.View.AnimationsSpeed);
            }

            if (HoverShape != null)
            {
                Canvas.SetLeft(HoverShape, current.ChartLocation.X - HoverShape.Width*.5);
                Canvas.SetTop(HoverShape, current.ChartLocation.Y - HoverShape.Height*.5);
            }
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Shape);
            chart.View.RemoveFromDrawMargin(DataLabel);
            Container.Segments.Remove(Segment);
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
            desiredPosition -= (Shape == null ? 0 : Shape.ActualHeight*.5) + DataLabel.ActualHeight*.5 + 2;

            if (desiredPosition + DataLabel.ActualHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        public override void OnHover(ChartPoint point)
        {
            var lineSeries = (LineSeries)point.SeriesView;
            if (Shape != null) Shape.Fill = Shape.Stroke;
            lineSeries.Path.StrokeThickness = lineSeries.StrokeThickness + 1;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            var lineSeries = (LineSeries) point.SeriesView;
            if (Shape != null)
                Shape.Fill = point.Fill == null
                    ? lineSeries.PointForeround
                    : (Brush) point.Fill;
            lineSeries.Path.StrokeThickness = lineSeries.StrokeThickness;
        }
    }
}
