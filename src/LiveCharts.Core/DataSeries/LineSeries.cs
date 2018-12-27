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
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
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
        : CartesianStrokeSeries<TModel, PointCoordinate, IBezierSegment, IBrush>, ILineSeries
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
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };

            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int itl = 0;

            Content[chart].TryGetValue(PathKey, out var path);
            var cartesianPath = (ICartesianPath)path;

            if (cartesianPath == null)
            {
                cartesianPath = UIFactory.GetNewCartesianPath(chart);
                chart.View.Canvas.AddChild(cartesianPath, true);
                Content[chart][PathKey] = cartesianPath;
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
                    cartesianPath.StartPoint = currentBezier.ViewModel.Point1;
                    cartesianPath.StartPivot = chart.InvertXy
                        ? new PointD(chart.ScaleToUi(Pivot, y), currentBezier.ViewModel.Point1.Y)
                        : new PointD(currentBezier.ViewModel.Point1.X, chart.ScaleToUi(Pivot, y));
                    cartesianPath.StrokeThickness = StrokeThickness;
                    cartesianPath.ZIndex = ZIndex;
                    cartesianPath.StrokeDashArray = StrokeDashArray;
                    isFist = false;
                    i = p[0];
                }

                if (DelayRule != DelayRules.None)
                {
                    timeLine = AnimationExtensions.Delay(
                        // ReSharper disable once PossibleMultipleEnumeration
                        originalDuration, originalAnimationLine, itl / (float)PointsCount, DelayRule);
                }

                currentBezier.ChartPoint.InteractionArea = new RectangleInteractionArea(
                    new RectangleD(
                        currentBezier.ViewModel.Location.X,
                        currentBezier.ViewModel.Location.Y,
                        GeometrySize,
                        GeometrySize));

                DrawPointShape(
                    currentBezier.ChartPoint, cartesianPath, timeLine, currentBezier.ViewModel);

                if (DataLabels)
                {
                    DrawPointLabel(currentBezier.ChartPoint);
                }

                Mapper.EvaluateModelDependentActions(
                    currentBezier.ChartPoint.Model, currentBezier.ChartPoint.Shape, currentBezier.ChartPoint);

                previous = currentBezier.ViewModel;
                length += currentBezier.ViewModel.AproxLength;
                j = p[0];
                itl++;
            }

            if (previous == null)
            {
                previous = new BezierViewModel
                {
                    Point1 = cartesianPath.StartPivot,
                    Point2 = cartesianPath.StartPivot,
                    Point3 = cartesianPath.StartPivot
                };
            }

            cartesianPath.EndPivot = chart.InvertXy
                ? new PointD(chart.ScaleToUi(Pivot, y), previous.Point1.Y)
                : new PointD(previous.Point1.X, chart.ScaleToUi(Pivot, y));
            cartesianPath.Paint(Stroke, Fill);

            double l = length / StrokeThickness;
            double tl = l - _previousLenght;
            double remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }
            var effect = Effects.GetAnimatedDashArray(StrokeDashArray, l + remaining);
            cartesianPath.StrokeDashArray = effect;
            cartesianPath.Animate(timeLine)
                .Property(nameof(cartesianPath.StrokeDashOffset), 0f, (float)(tl + remaining));
            // cartesianPath.Close(chart.View, closePivot, (float)length, i, j);
            _previousLenght = length;
        }

        private IEnumerable<BezierData> GetBeziers(ChartModel chart, Plane x, Plane y)
        {
            ChartPoint<TModel, PointCoordinate, IBezierSegment>? pi, pn = null, pnn = null;
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
                    Index = i,
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
            void Next(ChartPoint<TModel, PointCoordinate, IBezierSegment>? item)
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
            ChartPoint<TModel, PointCoordinate, IBezierSegment> current,
            ICartesianPath path,
            TimeLine timeline,
            BezierViewModel vm)
        {
            bool isNew = current.Shape == null;

            if (current.Shape == null)
            {
                current.Shape = UIFactory.GetNewBezierSegment(current.Chart.Model);
                current.Chart.Canvas.AddChild(current.Shape, true);
                current.Shape.PointShape.Left = (float)vm.Location.X;
                current.Shape.PointShape.Top = (float)vm.Location.Y;
                current.Shape.PointShape.Width = 0;
                current.Shape.PointShape.Height = 0;

                current.Shape.Point1 = vm.Point1;
                current.Shape.Point2 = vm.Point2;
                current.Shape.Point3 = vm.Point3;
                path.InsertSegment(current.Shape, vm.Index);

                var geometryAnimation = new TimeLine
                {
                    AnimationLine = timeline.AnimationLine,
                    Duration = TimeSpan.FromMilliseconds(timeline.Duration.TotalMilliseconds * 2)
                };

                float r = vm.GeometrySize * .5f;

                current.Shape.PointShape.Animate(geometryAnimation)
                    .Property(nameof(IBezierSegment.Left), vm.Location.X + r, vm.Location.X, 0.5)
                    .Property(nameof(IBezierSegment.Top), vm.Location.Y + r, vm.Location.Y, 0.5)
                    .Property(nameof(IBezierSegment.Width), 0, vm.GeometrySize, 0.5)
                    .Property(nameof(IBezierSegment.Height), 0, vm.GeometrySize, 0.5)
                    .Begin();
            }

            current.Shape.PointShape.StrokeThickness = StrokeThickness;
            current.Shape.PointShape.Paint(Stroke, Fill);

            if (!isNew)
            {
                current.Shape.PointShape.Animate(timeline)
                    .Property(nameof(IBezierSegment.Left), current.Shape.Left, (float)vm.Location.X)
                    .Property(nameof(IBezierSegment.Top), current.Shape.Top, (float)vm.Location.Y)
                    .Begin();
            }

            current.Shape.Animate(timeline)
                .Property(nameof(IBezierSegment.Point1), current.Shape.Point1, vm.Point1)
                .Property(nameof(IBezierSegment.Point2), current.Shape.Point2, vm.Point2)
                .Property(nameof(IBezierSegment.Point3), current.Shape.Point3, vm.Point3)
                .Begin();
        }

        private struct BezierData
        {
            public ChartPoint<TModel, PointCoordinate, IBezierSegment> ChartPoint { get; set; }
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

            var cartesianPath = path as ICartesianPath;
            // cartesianPath?.Dispose(view);
            viewContent.Remove(PathKey);
            base.OnDisposing(view, force);
        }
    }
}
