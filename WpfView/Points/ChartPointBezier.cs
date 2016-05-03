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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Wpf.Components;

namespace LiveCharts.Wpf.Points
{
    public class HorizontalBezierView : PointView, IBezierData
    {
        public BezierSegment Segment { get; set; }
        public Ellipse Ellipse { get; set; }
        public PathFigure Container { get; set; }
        public BezierData Data { get; set; }

        public override void DrawOrMove(object previous, object current, int index, ChartCore chart)
        {
            var previosPbv = previous as HorizontalBezierView;

            var y = chart.DrawMargin.Left + chart.DrawMargin.Width;

            Container.Segments.Remove(Segment);
            Container.Segments.Insert(index, Segment);

            if (IsNew)
            {
                Segment.Point1 = new Point(Data.Point1.X, y);
                Segment.Point2 = new Point(Data.Point2.X, y);
                Segment.Point3 = new Point(Data.Point3.X, y);

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, y);
                    Canvas.SetLeft(DataLabel, Location.X - DataLabel.ActualWidth*.5);
                }

                if (Ellipse != null)
                {
                    Canvas.SetTop(Ellipse, y);
                    Canvas.SetLeft(Ellipse, Location.X - Ellipse.Width*.5);
                }
            }
            else
            {
                if (previosPbv != null && !previosPbv.IsNew)
                {
                    Segment.Point1 = previosPbv.Segment.Point3;
                    Segment.Point2 = previosPbv.Segment.Point3;
                    Segment.Point3 = previosPbv.Segment.Point3;

                    if (DataLabel != null)
                    {
                        Canvas.SetTop(DataLabel, y);
                        Canvas.SetLeft(DataLabel, Location.X - DataLabel.ActualWidth * .5);
                    }

                    if (Ellipse != null)
                    {
                        Canvas.SetTop(Ellipse, y);
                        Canvas.SetLeft(Ellipse, Location.X - Ellipse.Width * .5);
                    }
                }
            }

            #region No-Animated
            if (chart.View.DisableAnimations)
            {
                Segment.Point1 = Data.Point1.AsPoint();
                Segment.Point2 = Data.Point2.AsPoint();
                Segment.Point3 = Data.Point3.AsPoint();

                if (HoverShape != null)
                {
                    Canvas.SetLeft(HoverShape, Location.X - HoverShape.Width*.5);
                    Canvas.SetTop(HoverShape, Location.Y - HoverShape.Height*.5);
                }

                if (Ellipse != null)
                {
                    Canvas.SetLeft(Ellipse, Location.X - Ellipse.Width*.5);
                    Canvas.SetTop(Ellipse, Location.Y - Ellipse.Height*.5);
                }

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();
                    Canvas.SetLeft(DataLabel, Location.X - DataLabel.ActualWidth*.5);
                    Canvas.SetTop(DataLabel, Location.Y - DataLabel.ActualWidth*.5);
                }
                return;
            }
            #endregion

            Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(Data.Point1.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(Data.Point2.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(Data.Point3.AsPoint(), chart.View.AnimationsSpeed));

            if (Ellipse != null)
            {
                Ellipse.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Location.X - Ellipse.Width*.5, chart.View.AnimationsSpeed));
                Ellipse.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(Location.Y - Ellipse.Height*.5, chart.View.AnimationsSpeed));
            }

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();
                
                DataLabel.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Location.X - DataLabel.ActualWidth*.5, chart.View.AnimationsSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(Location.Y - DataLabel.ActualWidth*.5, chart.View.AnimationsSpeed));
            }

            if (HoverShape != null)
            {
                Canvas.SetLeft(HoverShape, Location.X - HoverShape.Width*.5);
                Canvas.SetTop(HoverShape, Location.Y - HoverShape.Height*.5);
            }
        }
    }
}
