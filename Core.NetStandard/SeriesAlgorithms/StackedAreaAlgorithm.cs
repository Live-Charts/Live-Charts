﻿//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using LiveCharts.Defaults;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts.SeriesAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.SeriesAlgorithm" />
    /// <seealso cref="LiveCharts.Definitions.Series.ICartesianSeries" />
    public class StackedAreaAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        private readonly IStackModelableSeriesView _stackModelable;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedAreaAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public StackedAreaAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            _stackModelable = (IStackModelableSeriesView)view;
            PreferredSelectionMode = TooltipSelectionMode.SharedXValues;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            var points = View.ActualValues.GetPoints(View).ToList();

            var segmentPosition = 0;

            var lineView = View as ILineSeriesView;
            if (lineView == null) return;

            var smoothness = lineView.LineSmoothness;
            smoothness = smoothness > 1 ? 1 : (smoothness < 0 ? 0 : smoothness);

            foreach (var segment in points.SplitEachNaN())
            {
                var p0 = segment.Count > 0
                    ? GetStackedPoint(segment[0])
                    : new CorePoint(0, 0);
                var p1 = segment.Count > 0
                    ? GetStackedPoint(segment[0])
                    : p0;
                var p2 = segment.Count > 1
                    ? GetStackedPoint(segment[1])
                    : p1;
                var p3 = segment.Count > 2
                    ? GetStackedPoint(segment[2])
                    : p2;

                var uw = new CorePoint(
                    CurrentXAxis.EvaluatesUnitWidth
                        ? ChartFunctions.GetUnitWidth(AxisOrientation.X, Chart, View.ScalesXAt) / 2
                        : 0,
                    CurrentYAxis.EvaluatesUnitWidth
                        ? ChartFunctions.GetUnitWidth(AxisOrientation.Y, Chart, View.ScalesYAt) / 2
                        : 0);

                p0 += uw;
                p1 += uw;
                p2 += uw;
                p3 += uw;

                lineView.StartSegment(segmentPosition, p1);
                segmentPosition += segmentPosition == 0 ? 1 : 2;

                ChartPoint previousDrawn = null;

                for (var index = 0; index < segment.Count; index++)
                {
                    var chartPoint = segment[index];

                    chartPoint.ChartLocation = p1;

                    chartPoint.SeriesView = View;

                    var xc1 = (p0.X + p1.X) / 2.0;
                    var yc1 = (p0.Y + p1.Y) / 2.0;
                    var xc2 = (p1.X + p2.X) / 2.0;
                    var yc2 = (p1.Y + p2.Y) / 2.0;
                    var xc3 = (p2.X + p3.X) / 2.0;
                    var yc3 = (p2.Y + p3.Y) / 2.0;

                    var len1 = Math.Sqrt((p1.X - p0.X) * (p1.X - p0.X) + (p1.Y - p0.Y) * (p1.Y - p0.Y));
                    var len2 = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
                    var len3 = Math.Sqrt((p3.X - p2.X) * (p3.X - p2.X) + (p3.Y - p2.Y) * (p3.Y - p2.Y));

                    var k1 = len1 / (len1 + len2);
                    var k2 = len2 / (len2 + len3);

                    if (double.IsNaN(k1)) k1 = 0d;
                    if (double.IsNaN(k2)) k2 = 0d;

                    var xm1 = xc1 + (xc2 - xc1) * k1;
                    var ym1 = yc1 + (yc2 - yc1) * k1;
                    var xm2 = xc2 + (xc3 - xc2) * k2;
                    var ym2 = yc2 + (yc3 - yc2) * k2;

                    var c1X = xm1 + (xc2 - xm1) * smoothness + p1.X - xm1;
                    var c1Y = ym1 + (yc2 - ym1) * smoothness + p1.Y - ym1;
                    var c2X = xm2 + (xc2 - xm2) * smoothness + p2.X - xm2;
                    var c2Y = ym2 + (yc2 - ym2) * smoothness + p2.Y - ym2;

                    chartPoint.View = View.GetPointView(chartPoint,
                        View.DataLabels
                            ? (chartPoint.Participation > 0.05 ? View.GetLabelPointFormatter()(chartPoint) : string.Empty)
                            : null);

                    var bezierView = chartPoint.View as IBezierPointView;
                    if (bezierView == null) continue;

                    bezierView.Data = index == segment.Count - 1
                        ? new BezierData(new CorePoint(p1.X, p1.Y))
                        : new BezierData
                        {
                            Point1 = index == 0 ? new CorePoint(p1.X, p1.Y) : new CorePoint(c1X, c1Y),
                            Point2 = new CorePoint(c2X, c2Y),
                            Point3 = new CorePoint(p2.X, p2.Y)
                        };

                    chartPoint.View.DrawOrMove(previousDrawn, chartPoint, segmentPosition, Chart);
                    segmentPosition++;

                    previousDrawn = chartPoint.View.IsNew
                        ? previousDrawn
                        : chartPoint;

                    p0 = new CorePoint(p1);
                    p1 = new CorePoint(p2);
                    p2 = new CorePoint(p3);
                    p3 = segment.Count > index + 3
                        ? GetStackedPoint(segment[index + 3])
                        : p2;

                    p3 += uw;
                }
                lineView.EndSegment(segmentPosition, p1);
                segmentPosition++;
            }
        }

        /// <summary>
        /// Gets the stacked point.
        /// </summary>
        /// <param name="chartPoint">The chart point.</param>
        /// <returns></returns>
        protected virtual CorePoint GetStackedPoint(ChartPoint chartPoint)
        {
            if (_stackModelable.StackMode == StackMode.Values)
                return new CorePoint(
                    ChartFunctions.ToDrawMargin(chartPoint.X, AxisOrientation.X, Chart, View.ScalesXAt),
                    ChartFunctions.ToDrawMargin(chartPoint.To, AxisOrientation.Y, Chart, View.ScalesYAt));
            return new CorePoint(
                ChartFunctions.ToDrawMargin(chartPoint.X, AxisOrientation.X, Chart, View.ScalesXAt),
                ChartFunctions.ToDrawMargin(chartPoint.StackedParticipation, AxisOrientation.Y, Chart, View.ScalesYAt));
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis);
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.SeparatorMaxRounded(axis);
        }
    }
}
