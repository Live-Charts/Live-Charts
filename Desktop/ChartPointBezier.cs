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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveChartsCore;

namespace LiveChartsDesktop
{
    public class PointBezierView : PointView, IBezierData
    {
        public BezierSegment Segment { get; set; }
        public Ellipse Ellipse { get; set; }
        public PathFigure Container { get; set; }
        public BezierData Data { get; set; }

        public override void Locate(object previous, object current, int index, IChartModel chart)
        {
            var s1 = new Point();
            var s2 = new Point();
            var s3 = new Point();

            var previosPbv = previous as PointBezierView;

            if (IsNew)
            {
                Container.Segments.Insert(index, Segment);
                if (chart.Invert)
                {
                    var x = chart.DrawMargin.Left;
                    s1 = new Point(x, Data.Point1.Y);
                    s2 = new Point(x, Data.Point2.Y);
                    s3 = new Point(x, Data.Point3.Y);
                }
                else
                {
                    var y = chart.DrawMargin.Left + chart.DrawMargin.Width;
                    s1 = new Point(Data.Point1.X, y);
                    s2 = new Point(Data.Point2.X, y);
                    s3 = new Point(Data.Point3.X, y);
                }
            }

            var p1 = IsNew
                ? (previosPbv != null && !previosPbv.IsNew ? previosPbv.Segment.Point3 : s1)
                : Segment.Point1;
            var p2 = IsNew
                ? (previosPbv != null && !previosPbv.IsNew ? previosPbv.Segment.Point3 : s2)
                : Segment.Point2;
            var p3 = IsNew
                ? (previosPbv != null && !previosPbv.IsNew ? previosPbv.Segment.Point3 : s3)
                : Segment.Point3;

            if (chart.View.DisableAnimations)
            {
                var l = Data.Point1.AsPoint();

                Segment.Point1 = l;
                Segment.Point2 = Data.Point2.AsPoint();
                Segment.Point3 = Data.Point1.AsPoint();

                if (HoverShape != null)
                {
                    Canvas.SetLeft(Ellipse, l.X - Ellipse.Width * .5);
                    Canvas.SetTop(Ellipse, l.Y - Ellipse.Height * .5);
                }

                if (Ellipse != null)
                {
                    Canvas.SetLeft(Ellipse, l.X - Ellipse.Width*.5);
                    Canvas.SetTop(Ellipse, l.Y - Ellipse.Height*.5);
                }

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();
                    Canvas.SetLeft(DataLabel, l.X - DataLabel.ActualWidth*.5);
                    Canvas.SetTop(DataLabel, l.Y - DataLabel.ActualWidth*.5);
                }
                return;
            }

            Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(p1, Data.Point1.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(p2, Data.Point2.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(p3, Data.Point3.AsPoint(), chart.View.AnimationsSpeed));

            if (Ellipse != null)
            {
                var l = Data.Point3.AsPoint();

                Ellipse.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(p3.X, l.X - Ellipse.Width*.5, chart.View.AnimationsSpeed));
                Ellipse.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(p3.Y, l.Y - Ellipse.Height*.5, chart.View.AnimationsSpeed));
            }

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();
                var l = Data.Point3.AsPoint();

                DataLabel.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(p3.X, l.X - DataLabel.ActualWidth*.5, chart.View.AnimationsSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(p3.X, l.Y - DataLabel.ActualWidth*.5, chart.View.AnimationsSpeed));
            }
        }
    }

    public class PointView : IChartPointView
    {
        public Shape HoverShape { get; set; }
        public TextBlock DataLabel { get; set; }
        public bool IsNew { get; set; }

        public virtual void Locate(object previous, object current, int index, IChartModel chart)
        {
            throw new NotImplementedException();
        }
    }

    internal static class DesktopExtentions
    {
        public static Point AsPoint(this LvcPoint point)
        {
            return new Point(point.X, point.Y);
        }
    }

}
