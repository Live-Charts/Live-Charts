using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveChartsCore;

namespace LiveChartsDesktop
{
    public class PointBezierView : PointView
    {
        public BezierSegment Segment { get; set; }
        public Ellipse Ellipse { get; set; }
        internal BezierData Data { get; set; }

        public override void UpdateView(object previous, object current, IChartModel chart)
        {
            var s1 = new Point();
            var s2 = new Point();
            var s3 = new Point();

            var previosPbv = previous as PointBezierView;

            if (IsNew)
            {
                //Owner.Segments.Insert(index - pathOffset, Data.AssignTo(Segment));
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

            if (chart.DisableAnimatons)
            {
                Segment.Point1 = Data.Point1;
                Segment.Point2 = Data.Point2;
                Segment.Point3 = Data.Point3;
                return;
            }

            Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(p1, Data.Point1, speed));
            Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(p2, Data.Point2, speed));
            Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(p3, Data.Point3, speed));
        }
    }

    internal class BezierData
    {
        public BezierData()
        {
        }

        public BezierData(Point point)
        {
            Point1 = point;
            Point2 = point;
            Point3 = point;
        }

        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Point Point3 { get; set; }
        public Point StartPoint { get; set; }
    }

    public class PointView : IChartPointView
    {
        public Shape HoverShape { get; set; }
        public TextBlock DataLabel { get; set; }
        public bool IsNew { get; set; }

        public virtual void UpdateView(object previous, object current, IChartModel chart)
        {
            throw new NotImplementedException();
        }
    }
}
