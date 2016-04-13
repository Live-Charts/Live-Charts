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
using LiveCharts.CoreComponents;

namespace LiveCharts.Cache
{
    internal class LineVisualPoint : VisualPoint
    {
        public LineVisualPoint(PathFigure ownerFigure)
        {
            Owner = ownerFigure;
        }

        public PathFigure Owner { get; set; }
        public BezierSegment Segment { get; set; }
        
        public BezierData Data { get; set; }
        public LineVisualPoint Previous { get; set; }

        public void Animate(int index, Chart chart, int pathOffset, TimeSpan speed)
        {
            var s1 = new Point();
            var s2 = new Point();
            var s3 = new Point();

            if (IsNew)
            {
                Owner.Segments.Insert(index - pathOffset, Data.AssignTo(Segment));
                if (chart.Invert)
                {
                    //var x = chart.ToDrawMargin(chart.Min.X, AxisTags.X);
                    var x = Canvas.GetLeft(chart.DrawMargin);
                    s1 = new Point(x, Data.Point1.Y);
                    s2 = new Point(x, Data.Point2.Y);
                    s3 = new Point(x, Data.Point3.Y);
                }
                else
                {
                    //var y = chart.ToDrawMargin(chart.Min.Y, AxisTags.Y);
                    var y = Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Height;
                    s1 = new Point(Data.Point1.X, y);
                    s2 = new Point(Data.Point2.X, y);
                    s3 = new Point(Data.Point3.X, y);
                }
            }

            var p1 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s1)
                : Segment.Point1;
            var p2 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s2)
                : Segment.Point2;
            var p3 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s3)
                : Segment.Point3;

            if (chart.DisableAnimations)
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

    internal class VisualPoint
    {
        public ChartPoint ChartPoint { get; set; }
        public Series Series { get; set; }
        public Shape Shape { get; set; }
        public Shape HoverShape { get; set; }
        public TextBlock TextBlock { get; set; }
        public bool IsNew { get; set; }
        public bool IsHighlighted { get; set; }
    }
}