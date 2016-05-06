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
using System.Linq;
using LiveCharts.Helpers;

namespace LiveCharts.SeriesAlgorithms
{
    public class LineAlgorithm : SeriesCore, ICartesianSeries
    {
        public LineAlgorithm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.Stretch;
            YAxisMode = AxisLimitsMode.HalfSparator;
        }

        public AxisLimitsMode XAxisMode { get; set; }
        public AxisLimitsMode YAxisMode { get; set; }

        public override void Update()
        {
            var points = View.Values.Points.ToList();
            var fx = CurentXAxis.GetFormatter();
            var fy = CurrentYAxis.GetFormatter();
            var segmentPosition = 0;

            var lineView = View as ILineSeriesView;
            if (lineView == null) return;

            var smoothness = lineView.LineSmoothness;
            smoothness = smoothness > 1 ? 1 : (smoothness < 0 ? 0 : smoothness);

            foreach (var segment in points.SplitEachNaN())
            {
                var p0 = segment.Count > 0
                    ? ChartFunctions.ToDrawMargin(segment[0], View.ScalesXAt, View.ScalesYAt, Chart)
                    : new LvcPoint(0, 0);
                var p1 = segment.Count > 0
                    ? ChartFunctions.ToDrawMargin(segment[0], View.ScalesXAt, View.ScalesYAt, Chart)
                    : p0;
                var p2 = segment.Count > 1
                    ? ChartFunctions.ToDrawMargin(segment[1], View.ScalesXAt, View.ScalesYAt, Chart)
                    : p1;
                var p3 = segment.Count > 2
                    ? ChartFunctions.ToDrawMargin(segment[2], View.ScalesXAt, View.ScalesYAt, Chart)
                    : p2;

                lineView.StartSegment(segmentPosition, p1);
                segmentPosition += segmentPosition == 0 ? 1 : 2;

                ChartPoint previousDrawn = null;

                for (var index = 0; index < segment.Count; index++)
                {
                    var chartPoint = segment[index];

                    chartPoint.ChartLocation = p1;

                    var xc1 = (p0.X + p1.X)/2.0;
                    var yc1 = (p0.Y + p1.Y)/2.0;
                    var xc2 = (p1.X + p2.X)/2.0;
                    var yc2 = (p1.Y + p2.Y)/2.0;
                    var xc3 = (p2.X + p3.X)/2.0;
                    var yc3 = (p2.Y + p3.Y)/2.0;

                    var len1 = Math.Sqrt((p1.X - p0.X)*(p1.X - p0.X) + (p1.Y - p0.Y)*(p1.Y - p0.Y));
                    var len2 = Math.Sqrt((p2.X - p1.X)*(p2.X - p1.X) + (p2.Y - p1.Y)*(p2.Y - p1.Y));
                    var len3 = Math.Sqrt((p3.X - p2.X)*(p3.X - p2.X) + (p3.Y - p2.Y)*(p3.Y - p2.Y));

                    var k1 = len1/(len1 + len2);
                    var k2 = len2/(len2 + len3);

                    if (double.IsNaN(k1)) k1 = 0d;
                    if (double.IsNaN(k2)) k2 = 0d;

                    var xm1 = xc1 + (xc2 - xc1)*k1;
                    var ym1 = yc1 + (yc2 - yc1)*k1;
                    var xm2 = xc2 + (xc3 - xc2)*k2;
                    var ym2 = yc2 + (yc3 - yc2)*k2;

                    var c1X = xm1 + (xc2 - xm1)*smoothness + p1.X - xm1;
                    var c1Y = ym1 + (yc2 - ym1)*smoothness + p1.Y - ym1;
                    var c2X = xm2 + (xc2 - xm2)*smoothness + p2.X - xm2;
                    var c2Y = ym2 + (yc2 - ym2)*smoothness + p2.Y - ym2;

                    chartPoint.View = View.GetView(chartPoint.View,
                        View.DataLabels ? fx(chartPoint.X) + ", " + fy(chartPoint.Y) : null);
                    chartPoint.View.Location = p1;

                    var bezierView = chartPoint.View as IBezierData;
                    if (bezierView == null) continue;

                    bezierView.Data = index == segment.Count - 1
                        ? new BezierData(new LvcPoint(p1.X, p1.Y))
                        : new BezierData
                        {
                            Point1 = index == 0 ? new LvcPoint(p1.X, p1.Y) : new LvcPoint(c1X, c1Y),
                            Point2 = new LvcPoint(c2X, c2Y),
                            Point3 = new LvcPoint(p2.X, p2.Y)
                        };

                    chartPoint.View.DrawOrMove(previousDrawn, chartPoint, segmentPosition, Chart);
                    segmentPosition++;

                    previousDrawn = chartPoint.View.IsNew
                        ? previousDrawn
                        : chartPoint;

                    p0 = new LvcPoint(p1);
                    p1 = new LvcPoint(p2);
                    p2 = new LvcPoint(p3);
                    p3 = segment.Count > index + 3
                        ? ChartFunctions.ToDrawMargin(segment[index + 3], View.ScalesXAt, View.ScalesYAt, Chart)
                        : p2;
                }
                lineView.EndSegment(segmentPosition, p1);
                segmentPosition++;
            }
        }
    }

    public class BezierData
    {
        public BezierData()
        {
        }

        public BezierData(LvcPoint point)
        {
            Point1 = point;
            Point2 = point;
            Point3 = point;
        }

        public LvcPoint Point1 { get; set; }
        public LvcPoint Point2 { get; set; }
        public LvcPoint Point3 { get; set; }
        public LvcPoint StartPoint { get; set; }
    }
}
