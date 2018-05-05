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
using System.Drawing;
using System.Linq;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.Charts
{
    /// <inheritdoc />
    public class CartesianChartModel : ChartModel
    {
        /// <inheritdoc />
        public CartesianChartModel(IChartView view)
            : base(view)
        {
            Charting.BuildFromTheme((ICartesianChartView) view);
        }

        /// <inheritdoc />
        protected override void CopyDataFromView()
        {
            base.CopyDataFromView();
            InvertXy = ((ICartesianChartView) View).InvertAxes;
        }

        /// <inheritdoc />
        protected override int DimensionsCount => 3;

        /// <inheritdoc />
        public override float ScaleToUi(double dataValue, Plane plane, float[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale

            var x1 = plane.InternalMaxValue;
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.InternalMinValue;
            var y2 = 0f;

            if (plane.ActualReverse)
            {
                var temp1 = y1;
                var temp2 = y2;
                y1 = temp2;
                y2 = temp1;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (x2 == x1) return y1;

            var m = (y2 - y1) / (x2 - x1);

            return (float) (m * (dataValue - x1) + y1);
        }

        /// <inheritdoc />
        public override double ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale
            // then
            // x = ((y - y1) / m) + x1

            var x1 = plane.InternalMaxValue;
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.InternalMinValue;
            var y2 = 0f;

            if (plane.ActualReverse)
            {
                var temp1 = y1;
                var temp2 = y2;
                y1 = temp2;
                y2 = temp1;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (x2 == x1) return x1;

            var m = (y2 - y1) / (x2 - x1);

            return (pixelsValue - y1) / m + x1;
        }

        /// <summary>
        /// Performs the lineal equation with 2 given points.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public float LinealScale(PointF p1, PointF p2, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (p2.X == p1.X) return p1.Y;
            
            var m = (p2.Y - p1.Y) / (p2.X - p1.X);

            return m * (value - p1.X) + p1.Y;
        }

        /// <summary>
        /// Zooms a unit in, where a unit is calculated automatically based on the ZoomingSpeed property.
        /// </summary>
        /// <param name="pivot">The pivot, the point in the screen where the zoom was requested.</param>
        public void ZoomIn(PointF pivot)
        {
            if (!IsViewInitialized) return;

            var cartesianView = (ICartesianChartView) View;
            View.DataToolTip.Hide(View);

            var speed = cartesianView.ZoomingSpeed < 0.1
                ? 0.1
                : (cartesianView.ZoomingSpeed > 0.95
                    ? 0.95
                    : cartesianView.ZoomingSpeed);

            if (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy)
            {
                for (var index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = (Axis) cartesianView.Dimensions[0][index];

                    var px = ScaleFromUi(pivot.X, xPlane);

                    var max = double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue;
                    var min = double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue;
                    var l = max - min;

                    var rMin = (px - min) / l;
                    var rMax = 1 - rMin;

                    var unit = l * speed;
                    if (unit < xPlane.MinRange) return;

                    var minR = px - unit * rMin;
                    var maxR = px + unit * rMax;

                    xPlane.SetRange(minR, maxR);
                }
            }

            if (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy)
            {
                for (var index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = (Axis) cartesianView.Dimensions[1][index];

                    var py = ScaleFromUi(pivot.Y, yPlane);

                    var max = double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue;
                    var min = double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue;
                    var l = max - min;
                    var rMin = (py - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * speed;
                    if (target < yPlane.MinRange) return;

                    var minR = py - target * rMin;
                    var maxR = py + target * rMax;
                    yPlane.SetRange(minR, maxR);
                }
            }
        }

        /// <summary>
        /// Zooms a unit out, where a unit is calculated automatically based on the ZoomingSpeed property.
        /// </summary>
        /// <param name="pivot">The pivot, the point in the UI where the zoom was requested.</param>
        public void ZoomOut(PointF pivot)
        {
            var cartesianView = (ICartesianChartView)View;

            if (!IsViewInitialized) return;
            if (cartesianView.Zooming == Zooming.None) return;

            View.DataToolTip.Hide(View);

            var speed = cartesianView.ZoomingSpeed < 0.1
                ? 0.1
                : (cartesianView.ZoomingSpeed > 0.95
                    ? 0.95
                    : cartesianView.ZoomingSpeed); 

            if (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy)
            {
                for (var index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = (Axis) Dimensions[0][index];

                    var px = ScaleFromUi(pivot.X, xPlane);

                    var max = double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue;
                    var min = double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue;
                    var l = max - min;
                    var rMin = (px - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * (1 / speed);
                    if (target > xPlane.MaxRange) return;

                    var minR = px - target * rMin;
                    var maxR = px + target * rMax;
                    xPlane.SetRange(minR, maxR);
                }
            }

            if (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy)
            {
                for (var index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = (Axis) Dimensions[1][index];

                    var py = ScaleFromUi(pivot.Y, yPlane);

                    var max = double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue;
                    var min = double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue;
                    var l = max - min;
                    var rMin = (py - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * (1 / speed);
                    if (target > yPlane.MaxRange) return;
                    var minR = py - target * rMin;
                    var maxR = py + target * rMax;
                    yPlane.SetRange(minR, maxR);
                }
            }
        }

        /// <summary>
        /// Drags the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void Drag(PointF delta)
        {
            var cartesianView = (ICartesianChartView) View;

            if ((cartesianView.Panning == Panning.Unset &&
                 cartesianView.Zooming == Zooming.None) ||
                cartesianView.Panning == Panning.None)
            {
                return;
            }

            var px = cartesianView.Panning == Panning.Unset &&
                     (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy);

            px = px || cartesianView.Panning == Panning.X || cartesianView.Panning == Panning.Xy;

            if (px)
            {
                for (var index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = Dimensions[0][index];

                    var dx = ScaleFromUi(delta.X, xPlane) - ScaleFromUi(0f, xPlane);

                    xPlane.SetRange(
                        (double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue) + dx,
                        (double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue) + dx);
                }
            }

            var py = cartesianView.Panning == Panning.Unset &&
                     (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy);

            py = py || cartesianView.Panning == Panning.Y || cartesianView.Panning == Panning.Xy;

            if (py)
            {
                for (var index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = Dimensions[1][index];

                    var dy = ScaleFromUi(delta.Y, yPlane) - ScaleFromUi(0f, yPlane);

                    yPlane.SetRange(
                        (double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue) + dy,
                        (double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue) + dy);
                }
            }
        }

        /// <inheritdoc cref="ChartModel.Update"/>
        protected override void Update(bool restart, UpdateContext context)
        {
            OnUpdateStarted();

            base.Update(restart, context);

            // see docs/resources/chart.spacing.png
            var drawMargin = EvaluateAxisAndGetDrawMargin(context, this);

            DrawAreaSize = new[]
            {
                DrawAreaSize[0] - drawMargin.Left - drawMargin.Right,
                DrawAreaSize[1] - drawMargin.Top - drawMargin.Bottom
            };

            DrawAreaLocation = new[]
            {
                DrawAreaLocation[0] + drawMargin.Left,
                DrawAreaLocation[1] + drawMargin.Top
            };

            if (DrawAreaSize[0] <= 0 || DrawAreaSize[1] <= 0)
            {
                // skip update if the chart is too small.
                // and lets delete its content...
                CollectResources(true);
                return;
            }

            View.Content.DrawArea = new RectangleF(
                new PointF(DrawAreaLocation[0], DrawAreaLocation[1]),
                new SizeF(DrawAreaSize[0], DrawAreaSize[1]));

            // draw separators
            // for each dimension (for a cartesian chart X and Y)
            foreach (var dimension in Dimensions)
            {
                // for each plane in each dimension, in this case CartesianLinearAxis, for convention named Axis
                foreach (var plane in dimension)
                {
                    RegisterINotifyPropertyChanged(plane);
                    if (!(plane is Axis axis)) continue;
                    var labelsStyle = new LabelStyle
                    {
                        Font = plane.LabelsFont,
                        Foreground = plane.LabelsForeground,
                        LabelsRotation = plane.LabelsRotation,
                        Padding = plane.LabelsPadding
                    };
                    axis.DrawSeparators(this, labelsStyle);
                    axis.DrawSections(this, labelsStyle);
                }
            }

            foreach (var series in Series.Where(x => x.IsVisible))
            {
                if (!(series is ICartesianSeries))
                {
                    throw new LiveChartsException(
                        $"{series.ThemeKey.Name} is not supported at a {nameof(ICartesianChartView)}", 110);
                }

                series.UpdateStarted(View);
                series.UpdateView(this, context);
                series.UpdateFinished(View);
            }

            CollectResources();
            OnUpdateFinished();
        }

        /// <inheritdoc />
        protected override void OnPreparingSeries(UpdateContext context, ISeries series)
        {
            context.PointLength = Perform.MaxEach2D(context.PointLength, series.DefaultPointWidth);
            context.PointMargin = series.PointMargin > context.PointMargin ? series.PointMargin : context.PointMargin;
        }

        /// <inheritdoc />
        protected override PointF GetToolTipLocationAndFireHovering(
            PackedPoint[] points)
        {
            float x = 0f, y = 0f;

            var xDirection = 1;
            if (View.DataToolTip.Position == ToolTipPosition.Left) xDirection = -1;
            if (View.DataToolTip.Position == ToolTipPosition.Top || View.DataToolTip.Position == ToolTipPosition.Bottom) xDirection = 0;

            var yDirection = 1;
            if (View.DataToolTip.Position == ToolTipPosition.Top) yDirection = -1;
            if (View.DataToolTip.Position == ToolTipPosition.Right || View.DataToolTip.Position == ToolTipPosition.Left) yDirection = 0;

            int xi = 0, yi = 1;
            if (InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            foreach (var point in points)
            {
                var coordinate = point.Coordinate;
                var cartesianSeries = (ICartesianSeries) point.Series;

                var xCorr = cartesianSeries.PointMargin * .5f * xDirection;
                var yCorr = cartesianSeries.PointMargin * .5f * yDirection;

                x += ScaleToUi(coordinate[xi][0], Dimensions[xi][cartesianSeries.ScalesAt[0]]) + xCorr;
                y += ScaleToUi(coordinate[yi][0], Dimensions[yi][cartesianSeries.ScalesAt[1]]) + yCorr;

                if (View.Hoverable)
                {
                    cartesianSeries.OnPointHighlight(point, View);
                }
            }

            x = x / points.Length;
            y = y / points.Length;

            return new PointF(x, y);
        }

        internal Margin EvaluateAxisAndGetDrawMargin(UpdateContext context, ChartModel chart)
        {
            var requiresDrawMarginEvaluation = DrawMargin == Margin.Empty;

            float xt = 0f, xr = 0f, xb = 0f, xl = 0f;
            float yt = 0f, yr = 0f, yb = 0f, yl = 0f;

            // for each dimension (for a cartesian chart X and Y)
            for (var dimensionIndex = 0; dimensionIndex < Dimensions.Length; dimensionIndex++)
            {
                // for each plane in each dimension
                var dimension = Dimensions[dimensionIndex];

                for (var planeIndex = 0; planeIndex < dimension.Length; planeIndex++)
                {
                    var plane = dimension[planeIndex];
                    plane.Dimension = dimensionIndex;

                    if (InvertXy)
                    {
                        switch (dimensionIndex)
                        {
                            case 0:
                                plane.Dimension = 1;
                                break;
                            case 1:
                                plane.Dimension = 0;
                                break;
                            case 2:
                                plane.Dimension = 2;
                                break;
                            default:
                                throw new LiveChartsException(
                                    "A cartesian chart is not able to handle more than 3 dimensions (X, Y, Weight).",
                                    130);
                        }
                    }

                    // set the axis limits, use the user defined value if not double.Nan, otherwise use the value calculated by LVC

                    var uiPointMargin = 0d;

                    plane.ActualPointMargin = double.IsNaN(plane.PointMargin)
                        ? context.PointMargin
                        : plane.PointMargin;

                    plane.ActualReverse = plane.Dimension == 1;
                    if (plane.Reverse) plane.ActualReverse = !plane.ActualReverse;

                    plane.ActualPointLength = plane.PointLength ?? context.PointLength;
                    if (InvertXy)
                    {
                        plane.ActualPointLength = new[] { plane.ActualPointLength[1], plane.ActualPointLength[0] };
                    }

                    if (dimensionIndex < 2 && (double.IsNaN(plane.MinValue) || double.IsNaN(plane.MaxValue)))
                    {
                        plane.InternalMinValue = context.Ranges[dimensionIndex][planeIndex][0];
                        plane.InternalMaxValue = context.Ranges[dimensionIndex][planeIndex][1];
                        uiPointMargin = Math.Abs(
                            ScaleFromUi((float) plane.ActualPointMargin, plane) - ScaleFromUi(0f, plane));
                    }

                    var length = plane.ActualPointLength.Length > plane.Dimension
                        ? plane.ActualPointLength[plane.Dimension]
                        : 0;

                    plane.ActualMinValue = double.IsNaN(plane.MinValue)
                        ? context.Ranges[dimensionIndex][planeIndex][0] - uiPointMargin
                        : plane.MinValue;
                    plane.ActualMaxValue = double.IsNaN(plane.MaxValue)
                        ? context.Ranges[dimensionIndex][planeIndex][1] + uiPointMargin
                        : plane.MaxValue;

                    plane.InternalMinValue = plane.ActualMinValue - 0.5f * length;
                    plane.InternalMaxValue = plane.ActualMaxValue + 0.5F * length;

                    if (!(plane is Axis axis)) continue;

                    axis.ActualPosition = axis.Position == AxisPosition.Auto
                        ? (plane.Dimension == 0
                            ? AxisPosition.Bottom
                            : AxisPosition.Left)
                        : axis.Position;

                    if (!requiresDrawMarginEvaluation) continue;

                    // we stack the axis required margin

                    var mi = axis.CalculateAxisMargin(chart, axis);

                    foreach (var sharedAx in axis.SharedAxes)
                    {
                        foreach (var dependentChart in sharedAx.DependentCharts)
                        {
                            var mj = axis.CalculateAxisMargin(dependentChart.Key, sharedAx);

                            mi = new Margin(
                                mj.Top > mi.Top ? mj.Top : mi.Top,
                                mj.Right > mi.Top ? mj.Right : mi.Right,
                                mj.Bottom > mi.Bottom ? mj.Bottom : mi.Bottom,
                                mj.Left > mi.Left ? mj.Left : mi.Left);
                        }
                    }

                    switch (axis.ActualPosition)
                    {
                        case AxisPosition.Top:
                            plane.ByStackMargin = new Margin(yt, 0, 0, 0);
                            yt += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Bottom:
                            plane.ByStackMargin = new Margin(0, 0, yb, 0);
                            yb += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Left:
                            plane.ByStackMargin = new Margin(0, 0, 0, xl);
                            xl += mi.Left + mi.Right;
                            xb += mi.Bottom;
                            xt += mi.Top;
                            break;
                        case AxisPosition.Right:
                            plane.ByStackMargin = new Margin(0, xr, 0, 0);
                            xr += mi.Left + mi.Right;
                            xb += mi.Bottom;
                            xt += mi.Top;
                            break;
                        case AxisPosition.Auto:
                            // code should never reach here.
                            // previously set by the library...
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return requiresDrawMarginEvaluation
                ? new Margin(
                    xt > yt ? xt : yt,
                    xr > yr ? xr : yr,
                    xb > yb ? xb : yb,
                    xl > yl ? xl : yl)
                : DrawMargin;
        }
    }
}
