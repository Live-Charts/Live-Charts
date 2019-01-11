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

using LiveCharts.Animations;
using LiveCharts.Animations.Ease;
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction;
using LiveCharts.Interaction.Areas;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;
using System;
using System.Collections.Generic;

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// The line series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class LineSeries<TModel>
        : CartesianStrokeSeries<TModel, PointCoordinate, IBezierShape>, ILineSeries
    {
        private double _lineSmoothness;
        private double _geometrySize;
        private double _pivot;
        private double _previousLenght;
        private const string PathKey = "path";

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel}"/> class.
        /// </summary>
        public LineSeries()
        {
            Geometry = Geometry.Circle;
            GeometrySize = 12f;
            DefaultFillOpacity = .8;
            LineSmoothness = .8f;
            DataLabelFormatter = coordinate => Format.AsMetricNumber(coordinate.Y);
            TooltipFormatter = DataLabelFormatter;
            Global.Settings.BuildFromTheme<ILineSeries>(this);
            Global.Settings.BuildFromTheme<ISeries<PointCoordinate>>(this);
        }

        /// <inheritdoc />
        public double LineSmoothness
        {
            get => _lineSmoothness;
            set
            {
                _lineSmoothness = value;
                OnPropertyChanged(nameof(LineSmoothness));
            }
        }

        /// <inheritdoc />
        public double GeometrySize
        {
            get => _geometrySize;
            set
            {
                _geometrySize = value;
                OnPropertyChanged(nameof(GeometrySize));
            }
        }

        /// <inheritdoc />
        public double Pivot
        {
            get => _pivot;
            set
            {
                _pivot = value;
                OnPropertyChanged(nameof(Pivot));
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(ILineSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 0f, 0f };

        /// <inheritdoc />
        public override float PointMargin => (float)GeometrySize;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel)chart;

            var x = chart.Dimensions[0][ScalesAt[0]];
            var y = chart.Dimensions[1][ScalesAt[1]];

            BezierViewModel? previous = null;
            var animation = AnimatableArguments.BuildFrom(chart.View, this);
            int k = 0;

            Content[chart].TryGetValue(PathKey, out var pathObject);
            var path = (IPath)pathObject;

            if (path == null)
            {
                path = UIFactory.GetNewCartesianPath(chart);
                path.FlushToCanvas(chart.View.Canvas, true);
                Content[chart][PathKey] = path;
            }

            double length = 0;
            bool isFist = true;
            float i = 0, j = 0;

            foreach (var currentBezier in GetBeziers(cartesianChart, x, y))
            {
                float[] p = new[]
                {
                    chart.ScaleToUi(currentBezier.ChartPoint.Coordinate.X, x),
                    chart.ScaleToUi(currentBezier.ChartPoint.Coordinate.Y, y)
                };

                if (chart.InvertXy)
                {
                    currentBezier.Invert();
                }

                if (isFist)
                {
                    path.StartPoint = currentBezier.ViewModel.Point1; // animate to start point
                    var startSegment = UIFactory.GetNewLineSegment(chart);
                    startSegment.Point = chart.InvertXy
                        ? new PointD(chart.ScaleToUi(Pivot, y), currentBezier.ViewModel.Point1.Y)
                        : new PointD(currentBezier.ViewModel.Point1.X, chart.ScaleToUi(Pivot, y));
                    path.InsertSegment(startSegment, 0);
                    path.StrokeThickness = StrokeThickness;
                    path.ZIndex = ZIndex;
                    path.StrokeDashArray = StrokeDashArray;
                    isFist = false;
                    i = p[0];
                }

                if (DelayRule != DelayRules.None)
                {
                    animation.SetDelay(DelayRule, currentBezier.ViewModel.Index / (double)PointsCount);
                }

                currentBezier.ChartPoint.InteractionArea = new RectangleInteractionArea(
                    new RectangleD(
                        currentBezier.ViewModel.Location.X,
                        currentBezier.ViewModel.Location.Y,
                        GeometrySize,
                        GeometrySize));

                DrawPointShape(
                    currentBezier.ChartPoint, path, animation, currentBezier.ViewModel);

                if (DataLabels)
                {
                    DrawPointLabel(currentBezier.ChartPoint);
                }

                Mapper.EvaluateModelDependentActions(
                    currentBezier.ChartPoint.Model, currentBezier.ChartPoint.Shape, currentBezier.ChartPoint);

                previous = currentBezier.ViewModel;
                length += currentBezier.ViewModel.AproxLength;
                j = p[0];
                k++;
            }

            if (previous == null)
            {
                previous = new BezierViewModel
                {
                    Point1 = path.StartPoint,
                    Point2 = path.StartPoint,
                    Point3 = path.StartPoint
                };
            }

            var endSegment = UIFactory.GetNewLineSegment(chart);
            endSegment.Point = chart.InvertXy
                ? new PointD(chart.ScaleToUi(Pivot, y), previous.Point1.Y)
                : new PointD(previous.Point1.X, chart.ScaleToUi(Pivot, y));
            path.InsertSegment(endSegment, k + 1);
            path.Stroke = Stroke;
            path.Fill = Fill;
            path.Paint();

            double l = length / StrokeThickness;
            double tl = l - _previousLenght;
            double remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }
            var effect = Effects.GetAnimatedDashArray(StrokeDashArray, l + remaining);
            path.StrokeDashArray = effect;
            path.Animate(animation)
                .Property(nameof(path.StrokeDashOffset), 0f, (float)(tl + remaining));
            // cartesianPath.Close(chart.View, closePivot, (float)length, i, j);
            _previousLenght = length;
        }

        private IEnumerable<BezierData> GetBeziers(ChartModel chart, Plane x, Plane y)
        {
            ChartPoint<TModel, PointCoordinate, IBezierShape>? pi, pn = null, pnn = null;
            PointD previous, current = new PointD(0, 0), next = new PointD(0, 0), nextNext = new PointD(0, 0);
            int i = 0;

            double smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);
            float r = (float)(GeometrySize * .5);

            var e = GetPoints(chart.View).GetEnumerator();
            bool isFirstPoint = true;

            // ReSharper disable once ImplicitlyCapturedClosure
            BezierViewModel BuildModel()
            {
                if (isFirstPoint)
                {
                    previous = current;
                }

                double xc1 = (previous.X + current.X) / 2d;
                double yc1 = (previous.Y + current.Y) / 2d;
                double xc2 = (current.X + next.X) / 2d;
                double yc2 = (current.Y + next.Y) / 2d;
                double xc3 = (next.X + nextNext.X) / 2d;
                double yc3 = (next.Y + nextNext.Y) / 2d;

                double len1 = GetDistance(current, previous);
                double len2 = GetDistance(current, next);
                double len3 = GetDistance(next, nextNext);

                double k1 = len1 / (len1 + len2);
                double k2 = len2 / (len2 + len3);

                if (double.IsNaN(k1))
                {
                    k1 = 0d;
                }

                if (double.IsNaN(k2))
                {
                    k2 = 0d;
                }

                double xm1 = xc1 + (xc2 - xc1) * k1;
                double ym1 = yc1 + (yc2 - yc1) * k1;
                double xm2 = xc2 + (xc3 - xc2) * k2;
                double ym2 = yc2 + (yc3 - yc2) * k2;

                double c1X = xm1 + (xc2 - xm1) * smoothness + current.X - xm1;
                double c1Y = ym1 + (yc2 - ym1) * smoothness + current.Y - ym1;
                double c2X = xm2 + (xc2 - xm2) * smoothness + next.X - xm2;
                double c2Y = ym2 + (yc2 - ym2) * smoothness + next.Y - ym2;

                var bezier = new BezierViewModel
                {
                    Index = i + 1,
                    Location = new PointD(current.X - r, current.Y - r),
                    Point1 = isFirstPoint ? current : new PointD((float)c1X, (float)c1Y),
                    Point2 = new PointD((float)c2X, (float)c2Y),
                    Point3 = next,
                    Geometry = Geometry,
                    GeometrySize = (float)GeometrySize
                };

                // based on: 
                // https://stackoverflow.com/questions/29438398/cheap-way-of-calculating-cubic-bezier-length
                // according to a previous test, this method seems fast, and accurate enough for our purposes

                double chord = GetDistance(current, next);
                double net = GetDistance(current, bezier.Point1) + GetDistance(bezier.Point1, bezier.Point2) + GetDistance(bezier.Point2, next);
                bezier.AproxLength = (chord + net) / 2;
                i++;

                return bezier;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            void Next(ChartPoint<TModel, PointCoordinate, IBezierShape>? item)
            {
                pi = pn;
                pn = pnn;
                pnn = item;

                previous = current;
                current = next;
                next = nextNext;
                nextNext = pnn == null ? new PointD() : new PointD(chart.ScaleToUi(pnn.Coordinate.X, x), chart.ScaleToUi(pnn.Coordinate.Y, y));
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
                    ChartPoint = pi,
                    ViewModel = BuildModel()
                };

                isFirstPoint = false;
            }

            Next(pnn);
            yield return new BezierData
            {
                ChartPoint = pi,
                ViewModel = BuildModel()
            };

            Next(pnn);
            yield return new BezierData
            {
                ChartPoint = pi,
                ViewModel = BuildModel()
            };

            e.Dispose();
        }

        private double GetDistance(PointD p1, PointD p2)
        {
            return Math.Sqrt(
                Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private void DrawPointShape(
            ChartPoint<TModel, PointCoordinate, IBezierShape> current,
            IPath path,
            AnimatableArguments animationArgs,
            BezierViewModel vm)
        {
            bool isNew = current.Shape == null;

            if (current.Shape == null)
            {
                current.Shape = UIFactory.GetNewBezierShape(current.Chart.Model);
                current.Shape.FlushToCanvas(current.Chart.Canvas, true);
                current.Shape.Left = vm.Location.X;
                current.Shape.Top = vm.Location.Y;
                current.Shape.Width = 0;
                current.Shape.Height = 0;

                current.Shape.Segment.Point1 = vm.Point1;
                current.Shape.Segment.Point2 = vm.Point2;
                current.Shape.Segment.Point3 = vm.Point3;
                path.InsertSegment(current.Shape.Segment, vm.Index);

                var geometryAnimation = new AnimatableArguments(
                    TimeSpan.FromMilliseconds(animationArgs.Duration.TotalMilliseconds * 2),
                    new DelayedFunction(animationArgs.EasingFunction, .5));

                float r = vm.GeometrySize * .5f;

                current.Shape.Animate(geometryAnimation)
                    .Property(nameof(IShape.Left), vm.Location.X + r, vm.Location.X, 0.5)
                    .Property(nameof(IShape.Top), vm.Location.Y + r, vm.Location.Y, 0.5)
                    .Property(nameof(IShape.Width), 0, vm.GeometrySize, 0.5)
                    .Property(nameof(IShape.Height), 0, vm.GeometrySize, 0.5)
                    .Begin();
            }

            current.Shape.StrokeThickness = StrokeThickness;
            current.Shape.Fill = Fill;
            current.Shape.Stroke = Stroke;
            current.Shape.Paint();

            if (!isNew)
            {
                current.Shape.Animate(animationArgs)
                    .Property(nameof(IShape.Left), current.Shape.Left, (float)vm.Location.X)
                    .Property(nameof(IShape.Top), current.Shape.Top, (float)vm.Location.Y)
                    .Begin();
            }

            current.Shape.Segment.Animate(animationArgs)
                .Property(nameof(IBezierSegment.Point1), current.Shape.Segment.Point1, vm.Point1)
                .Property(nameof(IBezierSegment.Point2), current.Shape.Segment.Point2, vm.Point2)
                .Property(nameof(IBezierSegment.Point3), current.Shape.Segment.Point3, vm.Point3)
                .Begin();
        }

        private struct BezierData
        {
            public ChartPoint<TModel, PointCoordinate, IBezierShape> ChartPoint { get; set; }
            public BezierViewModel ViewModel { get; set; }

            public void Invert()
            {
                ViewModel.Location = new PointD(ViewModel.Location.Y, ViewModel.Location.X);
                ViewModel.Point1 = new PointD(ViewModel.Point1.Y, ViewModel.Point1.X);
                ViewModel.Point2 = new PointD(ViewModel.Point2.Y, ViewModel.Point2.X);
                ViewModel.Point3 = new PointD(ViewModel.Point3.Y, ViewModel.Point3.X);
            }
        }

        /// <inheritdoc />
        protected override void OnDisposing(IChartView view, bool force)
        {
            Dictionary<string, object> viewContent = Content[view.Model];

            viewContent.TryGetValue(PathKey, out var path);
            if (path == null)
            {
                return;
            }

            var cartesianPath = path as IPath;
            // cartesianPath?.Dispose(view);
            viewContent.Remove(PathKey);
            base.OnDisposing(view, force);
        }
    }
}
