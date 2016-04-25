
using System;
using System.Linq;

namespace LiveChartsCore
{
    public class LineAlgorithm : Series
    {
        public LineAlgorithm(ISeriesView view) : base(view)
        {
        }

        public LineAlgorithm(ISeriesView view, SeriesConfiguration configuration) : base(view, configuration)
        {
        }

        public double LineSmoothness { get; set; }

        public override void Update()
        {
            ChartPoint previous = null;

            var points = Values.Points.ToList();

            var p0 = points.Count > 0
                ? ChartFunctions.ToDrawMargin(points[0], ScalesXAt, ScalesYAt, Chart)
                : new LvcPoint(0, 0);
            var p1 = points.Count > 1
                ? ChartFunctions.ToDrawMargin(points[1], ScalesXAt, ScalesYAt, Chart)
                : p0;
            var p2 = points.Count > 2
                ? ChartFunctions.ToDrawMargin(points[2], ScalesXAt, ScalesYAt, Chart)
                : p1;
            var p3 = points.Count > 3
                ? ChartFunctions.ToDrawMargin(points[3], ScalesXAt, ScalesYAt, Chart)
                : p2;

            var smoothness = LineSmoothness;
            smoothness = smoothness > 1 ? 1 : (smoothness < 0 ? 0 : smoothness);

            for (var index = 0; index < points.Count ; index++)
            {
                var chartPoint = points[index];

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

                var xm1 = xc1 + (xc2 - xc1)*k1;
                var ym1 = yc1 + (yc2 - yc1)*k1;
                var xm2 = xc2 + (xc3 - xc2)*k2;
                var ym2 = yc2 + (yc3 - yc2)*k2;

                var c1X = xm1 + (xc2 - xm1)*smoothness + p1.X - xm1;
                var c1Y = ym1 + (yc2 - ym1)*smoothness + p1.Y - ym1;
                var c2X = xm2 + (xc2 - xm2)*smoothness + p2.X - xm2;
                var c2Y = ym2 + (yc2 - ym2)*smoothness + p2.Y - ym2;

                if (chartPoint.View == null)
                    chartPoint.View = View.InitializePointView();

                var bezierView = chartPoint.View as IBezierData;
                if (bezierView == null) continue;

                bezierView.Data = new BezierData
                {
                    Point1 = index == 0 ? new LvcPoint(p1.X, p1.Y) : new LvcPoint(c1X, c1Y),
                    Point2 = index == points.Count ? new LvcPoint(p2.X, p2.Y) : new LvcPoint(c2X, c2Y),
                    Point3 = new LvcPoint(p2.X, p2.Y)
                };

                chartPoint.View.Update(previous, chartPoint, Chart);

                previous = chartPoint;
            }
        }


        public BezierData CalculateBezier()
        {
            return new BezierData();
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

    public interface IBezierData
    {
        BezierData Data { get; set; }
    }
}
