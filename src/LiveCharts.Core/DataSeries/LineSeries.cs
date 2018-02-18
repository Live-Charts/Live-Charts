using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The line series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class LineSeries<TModel>
        : CartesianSeries<TModel, Point2D, BezierViewModel, Point<TModel, Point2D, BezierViewModel>>, ILineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel}"/> class.
        /// </summary>
        public LineSeries()
        {
            LiveChartsSettings.Set<ILineSeries>(this);
        }

        /// <summary>
        /// Gets or sets the line smoothness, goes from 0 to 1, 0: straight lines, 1: max curved line.
        /// </summary>
        /// <value>
        /// The line smoothness.
        /// </value>
        public double LineSmoothness { get; set; }

        /// <summary>
        /// Gets or sets the point geometry.
        /// </summary>
        /// <value>
        /// The point geometry.
        /// </value>
        public Geometry PointGeometry { get; set; }

        /// <summary>
        /// Gets or sets the size of the geometry.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        public double GeometrySize { get; set; }

        /// <inheritdoc />
        public override Point DefaultPointWidth => Point.Empty;

        private ICartesianPath _path;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];
            var unitWidth = new Point(
                Math.Abs(chart.ScaleToUi(0, x) - chart.ScaleToUi(x.ActualPointWidth.X, x)),
                Math.Abs(chart.ScaleToUi(0, y) - chart.ScaleToUi(y.ActualPointWidth.Y, y)));

            Point<TModel, Point2D, BezierViewModel> previous = null;

            if (_path == null)
            {
                _path = LiveChartsSettings.Current.UiProvider.GetNewPath();
                _path.Initialize(chart.View);
            }

            double lenght = 0;
            var isFist = true;
            double i = 0, j = 0;

            foreach (var bezier in GetBeziers(unitWidth, cartesianChart, x, y))
            {
                var p = chart.ScaleToUi(bezier.Point.Coordinate, x, y);
                if (isFist)
                {
                    _path.SetStyle(bezier.ViewModel.Point1, Stroke, Fill, StrokeThickness, StrokeDashArray);
                    isFist = false;
                    i = p.X;
                }

                bezier.Point.InteractionArea = new RectangleInteractionArea
                {
                    Top = p.Y - GeometrySize * .5,
                    Left = p.X - GeometrySize * .5,
                    Height = GeometrySize,
                    Width = GeometrySize
                };

                if (bezier.Point.View == null)
                {
                    bezier.Point.View = PointViewProvider();
                }

                bezier.ViewModel.Path = _path;
                bezier.Point.ViewModel = bezier.ViewModel;
                bezier.Point.View.DrawShape(
                    bezier.Point,
                    previous);

                previous = bezier.Point;
                lenght += bezier.ViewModel.AproxLength;
                j = p.X;
            }

            _path.Close(chart.View, lenght, i, j);
        }

        private IEnumerable<BezierData> GetBeziers(Point offset, ChartModel chart, Plane x, Plane y)
        {
            Point<TModel, Point2D, BezierViewModel> pi, pn = null, pnn = null;
            Point previous, current = new Point(0,0), next = new Point(0,0), nextNext = new Point(0, 0);
            var i = 0;

            var smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);

            var e = Points.GetEnumerator();
            var isFirstPoint = true;

            double GetDistance(Point p1, Point p2)
            {
                return Math.Sqrt(
                    Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            BezierViewModel BuildModel()
            {
                if (isFirstPoint)
                {
                    previous = current;
                }

                var xc1 = (previous.X + current.X) / 2.0;
                var yc1 = (previous.Y + current.Y) / 2.0;
                var xc2 = (current.X + next.X) / 2.0;
                var yc2 = (current.Y + next.Y) / 2.0;
                var xc3 = (next.X + nextNext.X) / 2.0;
                var yc3 = (next.Y + nextNext.Y) / 2.0;

                var len1 = Math.Sqrt((current.X - previous.X) * (current.X - previous.X) +
                                     (current.Y - previous.Y) * (current.Y - previous.Y));
                var len2 = Math.Sqrt((next.X - current.X) * (next.X - current.X) +
                                     (next.Y - current.Y) * (next.Y - current.Y));
                var len3 = Math.Sqrt((nextNext.X - next.X) * (nextNext.X - next.X) +
                                     (nextNext.Y - next.Y) * (nextNext.Y - next.Y));

                var k1 = len1 / (len1 + len2);
                var k2 = len2 / (len2 + len3);

                if (double.IsNaN(k1)) k1 = 0d;
                if (double.IsNaN(k2)) k2 = 0d;

                var xm1 = xc1 + (xc2 - xc1) * k1;
                var ym1 = yc1 + (yc2 - yc1) * k1;
                var xm2 = xc2 + (xc3 - xc2) * k2;
                var ym2 = yc2 + (yc3 - yc2) * k2;

                var c1X = xm1 + (xc2 - xm1) * smoothness + current.X - xm1;
                var c1Y = ym1 + (yc2 - ym1) * smoothness + current.Y - ym1;
                var c2X = xm2 + (xc2 - xm2) * smoothness + next.X - xm2;
                var c2Y = ym2 + (yc2 - ym2) * smoothness + next.Y - ym2;

                var bezier = new BezierViewModel
                {
                    Index = i,
                    Location = current,
                    Point1 = isFirstPoint ? current : new Point(c1X, c1Y),
                    Point2 = new Point(c2X, c2Y),
                    Point3 = next,
                    Geometry = Geometry,
                    GeometrySize = GeometrySize
                };

                // based on: 
                // https://stackoverflow.com/questions/29438398/cheap-way-of-calculating-cubic-bezier-length
                // according to a previous test, this method seems fast, and accurate enough for our purposes

                var chord = GetDistance(current, next);
                var net = GetDistance(current, bezier.Point1) +
                          GetDistance(bezier.Point1, bezier.Point2) +
                          GetDistance(bezier.Point2, next);
                bezier.AproxLength = (chord + net) / 2;
                i++;

                return bezier;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            void Next(Point<TModel, Point2D, BezierViewModel> item)
            {
                pi = pn;
                pn = pnn;
                pnn = item;

                previous = current;
                current = next;
                next = nextNext;
                nextNext = chart.ScaleToUi(new Point(pnn.Coordinate.X, pnn.Coordinate.Y), x, y) + offset;
            }

            while (e.MoveNext())
            {
                Next(e.Current);

                if (pi == null)
                {
                    continue;
                }

                yield return new BezierData
                {
                    Point = pi,
                    ViewModel = BuildModel()
                };

                isFirstPoint = false;
            }

            Next(pnn);
            yield return new BezierData
            {
                Point = pi,
                ViewModel = BuildModel()
            };

            Next(pnn);
            yield return new BezierData
            {
                Point = pi,
                ViewModel = BuildModel()
            };

            e.Dispose();
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, Point2D, BezierViewModel>, Point2D, BezierViewModel> DefaultPointViewProvider()
        {
            return LiveChartsSettings.Current.UiProvider.GetNewBezierView<TModel>();
        }

        private struct BezierData
        {
            public Point<TModel, Point2D, BezierViewModel> Point { get; set; }
            public BezierViewModel ViewModel { get; set; }
        }
    }
}
