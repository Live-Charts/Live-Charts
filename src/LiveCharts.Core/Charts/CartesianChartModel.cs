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
using System.Linq;
using LiveCharts.Coordinates;
using LiveCharts.DataSeries;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Interaction;
using LiveCharts.Interaction.Controls;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;

#endregion

namespace LiveCharts.Charts
{
    /// <inheritdoc />
    public class CartesianChartModel : ChartModel
    {
        private bool _isDragging;
        private PointF _previous;

        /// <inheritdoc />
        public CartesianChartModel(IChartView view)
            : base(view)
        {
            Global.Settings.BuildFromTheme((ICartesianChartView)view);
        }

        /// <inheritdoc />
        protected override void CopyDataFromView()
        {
            base.CopyDataFromView();
            InvertXy = ((ICartesianChartView)View).InvertAxes;
        }

        /// <inheritdoc />
        protected override int DimensionsCount => 3;

        /// <inheritdoc />
        public override float ScaleToUi(double dataValue, Plane plane, float[]? sizeVector = null)
        {
            float[] chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale

            double x1 = plane.InternalMaxValue;
            float y1 = chartSize[plane.Dimension];
            double x2 = plane.InternalMinValue;
            float y2 = 0f;

            if (plane.ActualReverse)
            {
                float temp1 = y1;
                float temp2 = y2;
                y1 = temp2;
                y2 = temp1;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (x2 == x1) return y1;

            double m = (y2 - y1) / (x2 - x1);

            return (float)(m * (dataValue - x1) + y1);
        }

        /// <inheritdoc />
        public override double ScaleFromUi(float pixelsValue, Plane plane, float[]? sizeVector = null)
        {
            float[] chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale
            // then
            // x = ((y - y1) / m) + x1

            double x1 = plane.InternalMaxValue;
            float y1 = chartSize[plane.Dimension];
            double x2 = plane.InternalMinValue;
            float y2 = 0f;

            if (plane.ActualReverse)
            {
                float temp1 = y1;
                float temp2 = y2;
                y1 = temp2;
                y2 = temp1;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (x2 == x1) return x1;

            double m = (y2 - y1) / (x2 - x1);

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

            float m = (p2.Y - p1.Y) / (p2.X - p1.X);

            return m * (value - p1.X) + p1.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="delta"></param>
        public void Zoom(PointF pivot, int delta)
        {
            if (((ICartesianChartView)View).Zooming == Zooming.None) return;

            var cartesianModel = (CartesianChartModel)View.Model;

            if (delta > 0)
            {
                cartesianModel.ZoomIn(new PointF(pivot.X, pivot.Y));
            }
            else
            {
                cartesianModel.ZoomOut(new PointF(pivot.X, pivot.Y));
            }
        }

        /// <summary>
        /// Zooms a unit in, where a unit is calculated automatically based on the ZoomingSpeed property.
        /// </summary>
        /// <param name="pivot">The pivot, the point in the screen where the zoom was requested.</param>
        public void ZoomIn(PointF pivot)
        {
            if (!IsViewInitialized) return;

            var cartesianView = (ICartesianChartView)View;
            View.DataToolTip.Hide(View);

            double speed = cartesianView.ZoomingSpeed < 0.1
                ? 0.1
                : (cartesianView.ZoomingSpeed > 0.95
                    ? 0.95
                    : cartesianView.ZoomingSpeed);

            if (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy)
            {
                for (int index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = (Axis)cartesianView.Dimensions[0][index];

                    double px = ScaleFromUi(pivot.X, xPlane);

                    double max = double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue;
                    double min = double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue;
                    double l = max - min;

                    double rMin = (px - min) / l;
                    double rMax = 1 - rMin;

                    double unit = l * speed;
                    if (unit < xPlane.MinRange) return;

                    double minR = px - unit * rMin;
                    double maxR = px + unit * rMax;

                    xPlane.SetRange(minR, maxR);
                }
            }

            if (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy)
            {
                for (int index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = (Axis)cartesianView.Dimensions[1][index];

                    double py = ScaleFromUi(pivot.Y, yPlane);

                    double max = double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue;
                    double min = double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue;
                    double l = max - min;
                    double rMin = (py - min) / l;
                    double rMax = 1 - rMin;

                    double target = l * speed;
                    if (target < yPlane.MinRange) return;

                    double minR = py - target * rMin;
                    double maxR = py + target * rMax;
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

            double speed = cartesianView.ZoomingSpeed < 0.1
                ? 0.1
                : (cartesianView.ZoomingSpeed > 0.95
                    ? 0.95
                    : cartesianView.ZoomingSpeed);

            if (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy)
            {
                for (int index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = (Axis)Dimensions[0][index];

                    double px = ScaleFromUi(pivot.X, xPlane);

                    double max = double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue;
                    double min = double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue;
                    double l = max - min;
                    double rMin = (px - min) / l;
                    double rMax = 1 - rMin;

                    double target = l * (1 / speed);
                    if (target > xPlane.MaxRange) return;

                    double minR = px - target * rMin;
                    double maxR = px + target * rMax;
                    xPlane.SetRange(minR, maxR);
                }
            }

            if (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy)
            {
                for (int index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = (Axis)Dimensions[1][index];

                    double py = ScaleFromUi(pivot.Y, yPlane);

                    double max = double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue;
                    double min = double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue;
                    double l = max - min;
                    double rMin = (py - min) / l;
                    double rMax = 1 - rMin;

                    double target = l * (1 / speed);
                    if (target > yPlane.MaxRange) return;
                    double minR = py - target * rMin;
                    double maxR = py + target * rMax;
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
            if (Math.Abs(delta.X) < 0.1 && Math.Abs(delta.Y) < 0.1)
            {
                return;
            }

            var cartesianView = (ICartesianChartView)View;

            if (cartesianView.Panning == Panning.Unset &&
                cartesianView.Zooming == Zooming.None ||
                cartesianView.Panning == Panning.None)
            {
                return;
            }

            bool px = cartesianView.Panning == Panning.Unset &&
                     (cartesianView.Zooming == Zooming.X || cartesianView.Zooming == Zooming.Xy);

            px = px || cartesianView.Panning == Panning.X || cartesianView.Panning == Panning.Xy;

            if (px)
            {
                for (int index = 0; index < Dimensions[0].Length; index++)
                {
                    var xPlane = Dimensions[0][index];

                    double dx = ScaleFromUi(delta.X, xPlane) - ScaleFromUi(0f, xPlane);

                    xPlane.SetRange(
                        (double.IsNaN(xPlane.MinValue) ? xPlane.InternalMinValue : xPlane.MinValue) + dx,
                        (double.IsNaN(xPlane.MaxValue) ? xPlane.InternalMaxValue : xPlane.MaxValue) + dx);
                }
            }

            bool py = cartesianView.Panning == Panning.Unset &&
                     (cartesianView.Zooming == Zooming.Y || cartesianView.Zooming == Zooming.Xy);

            py = py || cartesianView.Panning == Panning.Y || cartesianView.Panning == Panning.Xy;

            if (py)
            {
                for (int index = 0; index < Dimensions[1].Length; index++)
                {
                    var yPlane = Dimensions[1][index];

                    double dy = ScaleFromUi(delta.Y, yPlane) - ScaleFromUi(0f, yPlane);

                    yPlane.SetRange(
                        (double.IsNaN(yPlane.MinValue) ? yPlane.InternalMinValue : yPlane.MinValue) + dy,
                        (double.IsNaN(yPlane.MaxValue) ? yPlane.InternalMaxValue : yPlane.MaxValue) + dy);
                }
            }
        }

        /// <summary>
        /// Called when the pointer goes down.
        /// </summary>
        /// <param name="pointerLocation"></param>
        /// <param name="args"></param>
        protected override void OnViewPointerDown(PointF pointerLocation, EventArgs args)
        {
            base.OnViewPointerDown(pointerLocation, args);

            var cartesianView = (ICartesianChartView)View;
            if (cartesianView.Panning == Panning.None) return;
            _isDragging = true;
            cartesianView.CapturePointer();
        }

        /// <summary>
        /// Called when the pointer goes up.
        /// </summary>
        protected override void OnViewPointerUp()
        {
            base.OnViewPointerUp();

            var cartesianView = (ICartesianChartView)View;
            if (cartesianView.Panning == Panning.None) return;

            _isDragging = false;
            cartesianView.ReleasePointerCapture();
        }

        /// <summary>
        /// Called when the ppinter moves.
        /// </summary>
        /// <param name="pointerLocation"></param>
        /// <param name="args"></param>
        protected override void OnViewPointerMoved(PointF pointerLocation, EventArgs args)
        {
            base.OnViewPointerMoved(pointerLocation, args);

            if (!_isDragging) return;

            Drag(new PointF(
                    _previous.X - pointerLocation.X,
                    _previous.Y - pointerLocation.Y));
            _previous = pointerLocation;
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

            View.Canvas.DrawArea = new RectangleF(
                new PointF(drawMargin.Left, drawMargin.Top),
                new SizeF(DrawAreaSize[0], DrawAreaSize[1]));

            // draw separators
            // for each dimension (for a cartesian chart X and Y)
            foreach (Plane[] dimension in Dimensions)
            {
                // for each plane in each dimension, in this case CartesianLinearAxis, for convention named Axis
                foreach (var plane in dimension)
                {
                    RegisterINotifyPropertyChanged(plane);
                    if (!(plane is Axis axis)) continue;
                    axis.DrawSeparators(this);
                    axis.DrawSections(this);
                }
            }

            foreach (var series in Series.Where(x => x.IsVisible))
            {
                if (!(series is ICartesianSeries))
                {
                    throw new LiveChartsException(102, series.ThemeKey.Name, nameof(CartesianChartModel));
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
        protected override PointF GetTooltipLocation(
            IChartPoint[] points)
        {
            float x = 0f, y = 0f;

            int xDirection = 1;
            if (View.DataToolTip.Position == ToolTipPosition.Left) xDirection = -1;
            if (View.DataToolTip.Position == ToolTipPosition.Top || View.DataToolTip.Position == ToolTipPosition.Bottom) xDirection = 0;

            int yDirection = 1;
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
                var cartesianSeries = (ICartesianSeries)point.Series;

                float xCorr = cartesianSeries.PointMargin * .5f * xDirection;
                float yCorr = cartesianSeries.PointMargin * .5f * yDirection;

                if (coordinate is StackedPointCoordinate stacked)
                {
                    float xc = stacked.Key;
                    float yc = stacked.TotalStack;
                    if (InvertXy)
                    {
                        y += ScaleToUi(xc, Dimensions[0][cartesianSeries.ScalesAt[0]]);
                        x += ScaleToUi(yc, Dimensions[1][cartesianSeries.ScalesAt[1]]);
                    }
                    else
                    {
                        x += ScaleToUi(xc, Dimensions[0][cartesianSeries.ScalesAt[0]]);
                        y += ScaleToUi(yc, Dimensions[1][cartesianSeries.ScalesAt[1]]);
                    }
                }
                else
                {
                    // TODO: As fas as I can see this is a bug in C#8
                    //       currently this was written in the beta version of C#8
                    //       I get a warning saying something about a possible null reference (coordinate var)
                    //       I really don't how coordinate could be null at this point.
                    if (coordinate == null)
                    {
                        throw new Exception("Is this a compiler bug???");
                    }
                    x += ScaleToUi(coordinate[xi][0], Dimensions[xi][cartesianSeries.ScalesAt[0]]) + xCorr;
                    y += ScaleToUi(coordinate[yi][0], Dimensions[yi][cartesianSeries.ScalesAt[1]]) + yCorr;
                }
            }

            x = x / points.Length;
            y = y / points.Length;

            return new PointF(x, y);
        }

        internal Padding EvaluateAxisAndGetDrawMargin(UpdateContext context, ChartModel chart)
        {
            bool requiresDrawMarginEvaluation = DrawMargin == Padding.Empty;

            float xt = 0f, xr = 0f, xb = 0f, xl = 0f;
            float yt = 0f, yr = 0f, yb = 0f, yl = 0f;

            // for each dimension (for a cartesian chart X and Y)
            for (int dimensionIndex = 0; dimensionIndex < Dimensions.Length; dimensionIndex++)
            {
                // for each plane in each dimension
                Plane[] dimension = Dimensions[dimensionIndex];

                for (int planeIndex = 0; planeIndex < dimension.Length; planeIndex++)
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
                                throw new LiveChartsException(103, null);
                        }
                    }

                    // set the axis limits, use the user defined value if not double.Nan, otherwise use the value calculated by LVC

                    double uiPointMargin = 0d;

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
                            ScaleFromUi((float)plane.ActualPointMargin, plane) - ScaleFromUi(0f, plane));
                    }

                    float length = plane.ActualPointLength.Length > plane.Dimension
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
                        foreach (KeyValuePair<ChartModel, Dictionary<double, PlaneSeparator>> dependentChart in sharedAx.DependentCharts)
                        {
                            var mj = axis.CalculateAxisMargin(dependentChart.Key, sharedAx);

                            mi = new Padding(
                                mj.Top > mi.Top ? mj.Top : mi.Top,
                                mj.Right > mi.Top ? mj.Right : mi.Right,
                                mj.Bottom > mi.Bottom ? mj.Bottom : mi.Bottom,
                                mj.Left > mi.Left ? mj.Left : mi.Left);
                        }
                    }

                    switch (axis.ActualPosition)
                    {
                        case AxisPosition.Top:
                            plane.ByStackMargin = new Padding(yt, 0, 0, 0);
                            yt += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Bottom:
                            plane.ByStackMargin = new Padding(0, 0, yb, 0);
                            yb += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Left:
                            plane.ByStackMargin = new Padding(0, 0, 0, xl);
                            xl += mi.Left + mi.Right;
                            xb += mi.Bottom;
                            xt += mi.Top;
                            break;
                        case AxisPosition.Right:
                            plane.ByStackMargin = new Padding(0, xr, 0, 0);
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
                ? new Padding(
                    xt > yt ? xt : yt,
                    xr > yr ? xr : yr,
                    xb > yb ? xb : yb,
                    xl > yl ? xl : yl)
                : DrawMargin;
        }
    }
}
