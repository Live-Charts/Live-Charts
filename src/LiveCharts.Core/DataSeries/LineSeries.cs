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
    /// 
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
            LiveChartsSettings.Build<ILineSeries>(this);
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

            foreach (var bezier in GetBeziers(unitWidth, cartesianChart, x, y))
            {
                var p = chart.ScaleToUi(bezier.Point.Coordinate, x, y);

                bezier.Point.HoverArea = new RectangleHoverArea
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

                bezier.Point.ViewModel = bezier.ViewModel;
                bezier.Point.View.DrawShape(
                    bezier.Point,
                    previous);

                previous = bezier.Point;
            }
        }

        private IEnumerable<BezierData> GetBeziers(Point offset, CartesianChartModel chart, Plane x, Plane y)
        {
            Point<TModel, Point2D, BezierViewModel> pi, pn = null, pnn = null;
            Point previous, current = new Point(0,0), next = new Point(0,0), nextNext = new Point(0, 0);

            var smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);

            var e = Points.GetEnumerator();
            var isFirstPoint = true; 

            // ReSharper disable once ImplicitlyCapturedClosure
            BezierViewModel BuildModel()
            {
                var xc1 = (previous.X + current.X) / 2.0;
                var yc1 = (previous.Y + current.Y) / 2.0;
                var xc2 = (current.X + next.X) / 2.0;
                var yc2 = (current.Y + next.Y) / 2.0;
                var xc3 = (next.X + nextNext.X) / 2.0;
                var yc3 = (next.Y + nextNext.Y) / 2.0;

                var len1 = Math.Sqrt((current.X - previous.X) * (current.X - previous.X) + (current.Y - previous.Y) * (current.Y - previous.Y));
                var len2 = Math.Sqrt((next.X - current.X) * (next.X - current.X) + (next.Y - current.Y) * (next.Y - current.Y));
                var len3 = Math.Sqrt((nextNext.X - next.X) * (nextNext.X - next.X) + (nextNext.Y - next.Y) * (nextNext.Y - next.Y));

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

                return new BezierViewModel
                {
                    Location = new Point(current.X - .5 * GeometrySize, current.Y - .5 * GeometrySize),
                    Point1 = isFirstPoint ? current : new Point(c1X, c1Y),
                    Point2 = new Point(c2X, c2Y),
                    Point3 = new Point(next.X, next.Y),
                    Geometry = Geometry,
                    GeometrySize = GeometrySize
                };
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
            return LiveChartsSettings.Current.UiProvider.BezierViewProvider<TModel>();
        }

        private struct BezierData
        {
            public Point<TModel, Point2D, BezierViewModel> Point { get; set; }
            public BezierViewModel ViewModel { get; set; }
        }
    }
}
