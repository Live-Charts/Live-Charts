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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;

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
        private double _lineSmoothness;
        private double _geometrySize;
        private double _pivot;
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
            Charting.BuildFromTheme<ILineSeries>(this);
            Charting.BuildFromTheme<ISeries<PointCoordinate>>(this);
        }

        /// <inheritdoc />
        public double LineSmoothness
        {
            get => _lineSmoothness;
            set
            {
                _lineSmoothness = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double GeometrySize
        {
            get => _geometrySize;
            set
            {
                _geometrySize = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double Pivot
        {
            get => _pivot;
            set
            {
                _pivot = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(ILineSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {0f, 0f};

        /// <inheritdoc />
        public override float PointMargin => (float) GeometrySize;

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries>
            DefaultViewProvider => _provider ?? (_provider = Charting.Settings.UiProvider.BezierViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel) chart;

            IBezierSeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries> bezierViewProvider =
                (IBezierSeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries>) ViewProvider;

            var x = chart.Dimensions[0][ScalesAt[0]];
            var y = chart.Dimensions[1][ScalesAt[1]];

            ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries> previous = null;
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };
            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int itl = 0;

            Content[chart].TryGetValue(PathKey, out var path);
            var cartesianPath = (ICartesianPath) path;

            if (cartesianPath == null)
            {
                cartesianPath = bezierViewProvider.GetNewPath();
                cartesianPath.Initialize(chart.View, timeLine);
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

                if (chart.InvertXy) currentBezier.Invert();

                if (isFist)
                {
                    var pivot = chart.InvertXy
                        ? new PointF(chart.ScaleToUi(Pivot, y), currentBezier.ViewModel.Point1.Y)
                        : new PointF(currentBezier.ViewModel.Point1.X, chart.ScaleToUi(Pivot, y));
                    cartesianPath.SetStyle(
                        currentBezier.ViewModel.Point1, pivot, Stroke, Fill, StrokeThickness, ZIndex, StrokeDashArray);
                    isFist = false;
                    i = p[0];
                }

                if (currentBezier.ChartPoint.View == null)
                {
                    currentBezier.ChartPoint.View = ViewProvider.GetNewPoint();
                }

                if (DelayRule != DelayRules.None)
                {
                    timeLine = AnimationExtensions.Delay(
                        // ReSharper disable once PossibleMultipleEnumeration
                        originalDuration, originalAnimationLine, itl / (float) PointsCount, DelayRule);
                }

                currentBezier.ViewModel.Path = cartesianPath;
                currentBezier.ChartPoint.ViewModel = currentBezier.ViewModel;
                currentBezier.ChartPoint.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        currentBezier.ViewModel.Location.X,
                        currentBezier.ViewModel.Location.Y,
                        (float) GeometrySize,
                        (float) GeometrySize));
                currentBezier.ChartPoint.View.DrawShape(currentBezier.ChartPoint, previous, timeLine);
                if (DataLabels)
                    currentBezier.ChartPoint.View.DrawLabel(
                        currentBezier.ChartPoint, DataLabelsPosition, LabelsStyle, timeLine);
                Mapper.EvaluateModelDependentActions(
                    currentBezier.ChartPoint.Model, currentBezier.ChartPoint.View.VisualElement, currentBezier.ChartPoint);

                previous = currentBezier.ChartPoint;
                length += currentBezier.ViewModel.AproxLength;
                j = p[0];
                itl++;
            }

            if (previous == null)
            {
                previous = new ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries>
                {
                    ViewModel = new BezierViewModel
                    {
                        Point1 = new PointF()
                    }
                };
            }
            var closePivot = chart.InvertXy
                ? new PointF(chart.ScaleToUi(Pivot, y), previous.ViewModel.Point1.Y)
                : new PointF(previous.ViewModel.Point1.X, chart.ScaleToUi(Pivot, y));
            cartesianPath.Close(chart.View, closePivot, (float) length, i, j);
        }

        private IEnumerable<BezierData> GetBeziers(ChartModel chart, Plane x, Plane y)
        {
            ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries> pi, pn = null, pnn = null;
            PointF previous, current = new PointF(0,0), next = new  PointF(0,0), nextNext = new PointF(0, 0);
            int i = 0;

            double smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);
            float r = (float) (GeometrySize * .5);

            IEnumerator<ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries>> e =
                GetPoints(chart.View).GetEnumerator();
            bool isFirstPoint = true;

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

                double xc1 = (previous.X + current.X) / 2d;
                double yc1 = (previous.Y + current.Y) / 2d;
                double xc2 = (current.X + next.X) / 2d;
                double yc2 = (current.Y + next.Y) / 2d;
                double xc3 = (next.X + nextNext.X) / 2d;
                double yc3 = (next.Y + nextNext.Y) / 2d;

                double len1 = Math.Sqrt((current.X - previous.X) * (current.X - previous.X) +
                                     (current.Y - previous.Y) * (current.Y - previous.Y));
                double len2 = Math.Sqrt((next.X - current.X) * (next.X - current.X) +
                                     (next.Y - current.Y) * (next.Y - current.Y));
                double len3 = Math.Sqrt((nextNext.X - next.X) * (nextNext.X - next.X) +
                                     (nextNext.Y - next.Y) * (nextNext.Y - next.Y));

                double k1 = len1 / (len1 + len2);
                double k2 = len2 / (len2 + len3);

                if (double.IsNaN(k1)) k1 = 0d;
                if (double.IsNaN(k2)) k2 = 0d;

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
                    Location = new PointF(current.X - r, current.Y - r),
                    Point1 = isFirstPoint ? current : new PointF((float) c1X, (float) c1Y),
                    Point2 = new PointF((float) c2X, (float) c2Y),
                    Point3 = next,
                    Geometry = Geometry,
                    GeometrySize = (float) GeometrySize
                };

                // based on: 
                // https://stackoverflow.com/questions/29438398/cheap-way-of-calculating-cubic-bezier-length
                // according to a previous test, this method seems fast, and accurate enough for our purposes

                double chord = GetDistance(current, next);
                double net = GetDistance(current, bezier.Point1) +
                          GetDistance(bezier.Point1, bezier.Point2) +
                          GetDistance(bezier.Point2, next);
                bezier.AproxLength = (chord + net) / 2;
                i++;

                return bezier;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            void Next(ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries> item)
            {
                pi = pn;
                pn = pnn;
                pnn = item;

                previous = current;
                current = next;
                next = nextNext;
                nextNext = new PointF(chart.ScaleToUi(pnn.Coordinate.X, x), chart.ScaleToUi(pnn.Coordinate.Y, y));

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

        private struct BezierData
        {
            public ChartPoint<TModel, PointCoordinate, BezierViewModel, ILineSeries> ChartPoint { get; set; }
            public BezierViewModel ViewModel { get; set; }

            public void Invert()
            {
                ViewModel.Location = new PointF(ViewModel.Location.Y, ViewModel.Location.X);
                ViewModel.Point1 = new PointF(ViewModel.Point1.Y, ViewModel.Point1.X);
                ViewModel.Point2 = new PointF(ViewModel.Point2.Y, ViewModel.Point2.X);
                ViewModel.Point3 = new PointF(ViewModel.Point3.Y, ViewModel.Point3.X);
            }
        }

        /// <inheritdoc />
        protected override void OnDisposing(IChartView view, bool force)
        {
            Dictionary<string, object> viewContent = Content[view.Model];

            viewContent.TryGetValue(PathKey, out var path);
            if (path == null) return;
            var cartesianPath = path as ICartesianPath;
            cartesianPath?.Dispose(view);
            viewContent.Remove(PathKey);
            base.OnDisposing(view, force);
        }
    }
}
