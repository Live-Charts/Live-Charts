using System;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Charts
{
    /// <inheritdoc />
    public class CartesianChartModel : ChartModel
    {
        private Point _previousTooltipLocation = Point.Empty;
        
        /// <inheritdoc />
        public CartesianChartModel(IChartView view)
            : base(view)
        {

        }

        /// <inheritdoc />
        protected override void CopyDataFromView()
        {
            base.CopyDataFromView();
            InvertXy = ((ICartesianChartView) View).InvertAxis;
        }

        /// <inheritdoc />
        public override float ScaleToUi(float dataValue, Plane plane, float[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale

            var x1 = plane.ActualMaxValue + (plane.ActualPointWidth?[plane.Dimension] ?? 0f);
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.ActualMinValue;
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

            return m * (dataValue - x1) + y1;
        }

        /// <inheritdoc />
        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale
            // then
            // x = ((y - y1) / m) + x1

            var x1 = plane.ActualMaxValue + (plane.ActualPointWidth?[plane.Dimension] ?? 0f);
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.ActualMinValue;
            var y2 = 0f;

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
        public float LinealScale(Point p1, Point p2, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (p2.X == p1.X) return p1.Y;
            
            var m = (p2.Y - p1.Y) / (p2.X - p1.X);

            return m * (value - p1.X) + p1.Y;
        }

        /// <inheritdoc cref="ChartModel.Update"/>
        protected override void Update(bool restart)
        {
            // run the update on the view's thread
            View.InvokeOnUiThread(() =>
            {
                OnUpdateStarted();

                base.Update(restart);

                // see appendix/chart.spacing.png
                var drawMargin = EvaluateAxisAndGetDrawMargin();

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

                View.Content.DrawArea = new Rectangle(DrawAreaLocation, DrawAreaSize);

                // draw separators
                // for each dimension (for a cartesian chart X and Y)
                foreach (var dimension in Dimensions)
                {
                    // for each plane in each dimension, in this case CartesianLinearAxis, for convention named Axis
                    foreach (var plane in dimension.OfType<Axis>())
                    {
                        var axis = plane;
                        RegisterResource(axis);
                        axis.DrawSeparators(this);
                    }
                }

                foreach (var series in Series.Where(x => x.IsVisible))
                {
                    RegisterResource(series);
                    series.UpdateView(this);
                }

                CollectResources();
                OnUpdateFinished();
            });
        }

        /// <inheritdoc />
        protected override void ViewOnPointerMoved(Point location, TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            if (Series == null) return;
            var query = GetInteractedPoints(dimensions).ToArray();

            if (selectionMode == TooltipSelectionMode.Auto)
            {
                // ToDo: guess what the user meant here ...
            }

            ToolTip = View.DataToolTip;

            // ReSharper disable once PossibleMultipleEnumeration
            if (!query.Any())
            {
                ToolTipTimeoutTimer.Start();
                return;
            }

            ToolTipTimeoutTimer.Stop();

            View.DataToolTip.ShowAndMeasure(query, View);
            float sx = 0f, sy = 0f;

            foreach (var point in query)
            {
                var coordinate = point.Coordinate;
                sx += ScaleToUi(coordinate[0][0], Dimensions[0][point.Series.ScalesAt[0]]);
                sy += ScaleToUi(coordinate[1][0], Dimensions[1][point.Series.ScalesAt[1]]);
            }

            sx = sx / query.Length;
            sy = sy / query.Length;

            var newTooltipLocation = new Point(sx, sy);

            if (_previousTooltipLocation != newTooltipLocation)
            {
                View.DataToolTip.Move(newTooltipLocation, View);
            }

            OnDataPointerEnter(query);
            var leftPoints = PreviousHoveredPoints?.ToArray()
                .Where(x => !x.InteractionArea.Contains(dimensions));
            // ReSharper disable once PossibleMultipleEnumeration
            if (leftPoints != null && leftPoints.Any())
            {
                // ReSharper disable once PossibleMultipleEnumeration
                OnDataPointerLeave(leftPoints);
            }
            PreviousHoveredPoints = query;

            _previousTooltipLocation = newTooltipLocation;
        }

        internal Margin EvaluateAxisAndGetDrawMargin()
        {
            var requiresDrawMarginEvaluation = DrawMargin == Margin.Empty;

            float xt = 0f, xr = 0f, xb = 0f, xl = 0f;
            float yt = 0f, yr = 0f, yb = 0f, yl = 0f;

            // for each dimension (for a cartesian chart X and Y)
            for (var index = 0; index < Dimensions.Length; index++)
            {
                var dimension = Dimensions[index];
// for each axis in each dimension

                foreach (var plane in dimension)
                {
                    plane.Dimension = index;

                    if (InvertXy)
                    {
                        switch (index)
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

                    // get the axis limits...
                    plane.ActualMinValue = double.IsNaN(plane.MinValue)
                        ? plane.DataRange.To
                        : plane.MinValue;
                    plane.ActualMaxValue = double.IsNaN(plane.MaxValue)
                        ? plane.DataRange.From
                        : plane.MaxValue;

                    plane.ActualReverse = plane.Dimension == 1;
                    if (plane.Reverse) plane.ActualReverse = !plane.ActualReverse;

                    if (!requiresDrawMarginEvaluation) continue;
                    if (!(plane is Axis axis)) continue;

                    axis.Position = axis.Position == AxisPosition.Auto
                        ? (plane.Dimension == 0
                            ? AxisPosition.Bottom
                            : AxisPosition.Left)
                        : axis.Position;

                    // we stack the axis required margin
                    var mi = axis.CalculateAxisMargin(this);
                    switch (axis.Position)
                    {
                        case AxisPosition.Top:
                            yt += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Bottom:
                            yb += mi.Top + mi.Bottom;
                            yl += mi.Left;
                            yr += mi.Right;
                            break;
                        case AxisPosition.Left:
                            xl += mi.Left + mi.Right;
                            xb += mi.Bottom;
                            xt += mi.Top;
                            break;
                        case AxisPosition.Right:
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