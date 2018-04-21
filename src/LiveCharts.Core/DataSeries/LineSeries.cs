#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The line series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class LineSeries<TModel>
        : CartesianStrokeSeries<TModel, PointCoordinate, BezierViewModel, ILineSeries>, ILineSeries
    {
        private ISeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries> _provider;
        private const string Path = "path";

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel}"/> class.
        /// </summary>
        public LineSeries()
        {
            Geometry = Geometry.Circle;
            GeometrySize = 12f;
            LineSmoothness = .8f;
            Charting.BuildFromSettings<ILineSeries>(this);
        }

        /// <inheritdoc />
        public double LineSmoothness { get; set; }

        /// <inheritdoc />
        public double GeometrySize { get; set; }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(ILineSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {0f, 0f};

        /// <inheritdoc />
        public override float[] PointMargin => new[] {(float) GeometrySize, (float) GeometrySize};

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries>
            DefaultViewProvider => _provider ?? (_provider = Charting.Current.UiProvider.BezierViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel) chart;

            var x = chart.Dimensions[0][ScalesAt[0]];
            var y = chart.Dimensions[1][ScalesAt[1]];

            var uw = chart.Get2DUiUnitWidth(x, y);

            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> previous = null;

            Content[chart].TryGetValue(Path, out var path);
            var cartesianPath = (ICartesianPath) path;

            if (cartesianPath == null)
            {
                cartesianPath = Charting.Current.UiProvider.GetNewPath();
                cartesianPath.Initialize(chart.View);
                Content[chart][Path] = cartesianPath;
            }

            double length = 0;
            var isFist = true;
            float i = 0, j = 0;

            foreach (var bezier in GetBeziers(new PointF(uw[0]*.5f, uw[1]*.5f), cartesianChart, x, y))
            {
                var p = new[]
                {
                    chart.ScaleToUi(bezier.Point.Coordinate.X, x),
                    chart.ScaleToUi(bezier.Point.Coordinate.Y, y)
                };

                if (chart.InvertXy) bezier.Invert();

                if (isFist)
                {
                    cartesianPath.SetStyle(bezier.ViewModel.Point1, Stroke, Fill, StrokeThickness, StrokeDashArray);
                    isFist = false;
                    i = p[0];
                }

                bezier.Point.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        p[0] - (float) GeometrySize * .5f,
                        p[1] - (float) GeometrySize * .5f,
                        (float) GeometrySize,
                        (float) GeometrySize));

                if (bezier.Point.View == null)
                {
                    bezier.Point.View = ViewProvider.GetNewPoint();
                }

                bezier.ViewModel.Path = cartesianPath;
                bezier.Point.ViewModel = bezier.ViewModel;
                bezier.Point.View.DrawShape(bezier.Point, previous);
                if (DataLabels) bezier.Point.View.DrawLabel(bezier.Point, DataLabelsPosition, LabelsStyle);

                previous = bezier.Point;
                length += bezier.ViewModel.AproxLength;
                j = p[0];
            }

            cartesianPath.Close(chart.View, (float) length, i, j);
        }

        private IEnumerable<BezierData> GetBeziers(PointF offset, ChartModel chart, Plane x, Plane y)
        {
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> pi, pn = null, pnn = null;
            PointF previous, current = new PointF(0,0), next = new  PointF(0,0), nextNext = new PointF(0, 0);
            var i = 0;

            var smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);

            var e = Points.GetEnumerator();
            var isFirstPoint = true;

            double GetDistance(PointF p1, PointF p2)
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

                var xc1 = (previous.X + current.X) / 2d;
                var yc1 = (previous.Y + current.Y) / 2d;
                var xc2 = (current.X + next.X) / 2d;
                var yc2 = (current.Y + next.Y) / 2d;
                var xc3 = (next.X + nextNext.X) / 2d;
                var yc3 = (next.Y + nextNext.Y) / 2d;

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
                    Point1 = isFirstPoint ? current : new PointF((float) c1X, (float) c1Y),
                    Point2 = new PointF((float) c2X, (float) c2Y),
                    Point3 = next,
                    Geometry = Geometry,
                    GeometrySize = (float) GeometrySize
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
            void Next(Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> item)
            {
                pi = pn;
                pn = pnn;
                pnn = item;

                previous = current;
                current = next;
                next = nextNext;
                nextNext =
                    Perform.Sum(
                        new PointF(chart.ScaleToUi(pnn.Coordinate.X, x), chart.ScaleToUi(pnn.Coordinate.Y, y)),
                        offset);
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

        private struct BezierData
        {
            public Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> Point { get; set; }
            public BezierViewModel ViewModel { get; set; }

            public void Invert()
            {
                ViewModel.Location = new PointF(ViewModel.Location.Y, ViewModel.Location.X);
                ViewModel.Point1 = new PointF(ViewModel.Point1.Y, ViewModel.Point1.X);
                ViewModel.Point2 = new PointF(ViewModel.Point2.Y, ViewModel.Point2.X);
                ViewModel.Point3 = new PointF(ViewModel.Point3.Y, ViewModel.Point3.X);
            }
        }
    }
}
