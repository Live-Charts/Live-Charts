using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
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

        /// <summary>
        /// Gets the x axis.
        /// </summary>
        /// <value>
        /// The x axis.
        /// </value>
        public IList<Plane> XAxis => Dimensions[0];

        /// <summary>
        /// Gets the y axis.
        /// </summary>
        /// <value>
        /// The y axis.
        /// </value>
        public IList<Plane> YAxis => Dimensions[1];

        /// <inheritdoc />
        public override double ScaleToUi(double dataValue, Plane plane, double[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale

            var x1 = plane.ActualMaxValue + (plane.ActualPointWidth?[plane.Dimension] ?? 0);
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.ActualMinValue;
            double y2 = 0;

            if (plane.ActualReverse)
            {
                var temp1 = y1;
                var temp2 = y2;
                y1 = temp2;
                y2 = temp1;
            }

            var m = (y2 - y1) / (x2 - x1);

            return m * (dataValue - x1) + y1;
        }

        /// <inheritdoc />
        public override double ScaleFromUi(double pixelsValue, Plane plane, double[] sizeVector = null)
        {
            var chartSize = sizeVector ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale
            // then
            // x = ((y - y1) / m) + x1

            var x1 = plane.ActualMaxValue + (plane.ActualPointWidth?[plane.Dimension] ?? 0);
            var y1 = chartSize[plane.Dimension];
            var x2 = plane.ActualMinValue;
            double y2 = 0;

            var m = (y2 - y1) / (x2 - x1);

            return (pixelsValue - y1) / m + x1;
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

                View.UpdateDrawArea(new Rectangle(DrawAreaLocation, DrawAreaSize));

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
            var query = GetInteractedPoints(dimensions).ToArray();

            if (selectionMode == TooltipSelectionMode.Auto)
            {
                // ToDo: guess what the user meant here ...
            }

            ToolTip = View.DataToolTip;

            // ReSharper disable once PossibleMultipleEnumeration
            if (!query.Any())
            {
                TooltipTimoutTimer.Start();
                return;
            }

            TooltipTimoutTimer.Stop();

            View.DataToolTip.ShowAndMeasure(query, View);
            double sx = 0, sy = 0;

            foreach (var point in query)
            {
                var point2D = (Point2D) point.Coordinate;
                sx += ScaleToUi(point2D.X, XAxis[point.Series.ScalesAt[0]]);
                sy += ScaleToUi(point2D.Y, YAxis[point.Series.ScalesAt[1]]);
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

            double xt = 0, xr = 0, xb = 0, xl = 0;
            double yt = 0, yr = 0, yb = 0, yl = 0;

            // for each dimension (for a cartesian chart X and Y)
            for (var index = 0; index < Dimensions.Length; index++)
            {
                var dimension = Dimensions[index];
// for each axis in each dimension
                foreach (var plane in dimension)
                {
                    plane.Dimension = index;

                    // get the axis limits...
                    plane.ActualMinValue = double.IsNaN(plane.MinValue)
                        ? plane.DataRange.MinValue
                        : plane.MinValue;
                    plane.ActualMaxValue = double.IsNaN(plane.MaxValue)
                        ? plane.DataRange.MaxValue
                        : plane.MaxValue;

                    plane.ActualReverse = index == 1;
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