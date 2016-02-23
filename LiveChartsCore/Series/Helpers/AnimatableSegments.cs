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
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.CoreComponents;

namespace LiveCharts.Helpers
{
    internal class AnimatableSegments
    {
        public bool IsNew { get; set; }
        public TrackableBezier Bezier { get; set; }
        public BezierData Data { get; set; }
        public AnimatableSegments Previous { get; set; }
        public AnimatableSegments Next { get; set; }

        public void Animate(int index, Chart chart, int pathOffset)
        {
            var s1 = new Point();
            var s2 = new Point();
            var s3 = new Point();

            if (IsNew)
            {
                Bezier.Owner.Segments.Insert(index - pathOffset, Data.AssignTo(Bezier.Segment));
                if (chart.Invert)
                {
                    s1 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P1.Y);
                    s2 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P2.Y);
                    s3 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P3.Y);
                }
                else
                {
                    s1 = new Point(Data.P1.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                    s2 = new Point(Data.P2.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                    s3 = new Point(Data.P3.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                }
            }

            var p1 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Bezier.Segment.Point3 : s1)
                : Bezier.Segment.Point1;
            var p2 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Bezier.Segment.Point3 : s2)
                : Bezier.Segment.Point2;
            var p3 = IsNew
                ? (Previous != null && !Previous.IsNew ? Previous.Bezier.Segment.Point3 : s3)
                : Bezier.Segment.Point3;

            if (chart.DisableAnimation)
            {
                Bezier.Segment.Point1 = Data.P1;
                Bezier.Segment.Point2 = Data.P2;
                Bezier.Segment.Point3 = Data.P3;
                return;
            }

            Bezier.Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(p1, Data.P1, LineSeries.AnimSpeed));
            Bezier.Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(p2, Data.P2, LineSeries.AnimSpeed));
            Bezier.Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(p3, Data.P3, LineSeries.AnimSpeed));
        }
    }
}