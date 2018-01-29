using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Size = LiveCharts.Core.Drawing.Size;

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
        public override double ScaleToUi(double dataValue, Plane plane, Size? size = null)
        {
            var chartSize = size ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale

            double x1, x2, y1, y2;

            if (plane.PlaneType == PlaneTypes.X)
            {
                x1 = plane.ActualMaxValue + plane.ActualPointWidth.X;
                y1 = chartSize.Width;
                x2 = plane.ActualMinValue;
                y2 = 0;
            }
            else
            {
                x1 = plane.ActualMaxValue + plane.ActualPointWidth.Y;
                y1 = 0;
                x2 = plane.ActualMinValue;
                y2 = chartSize.Height;
            }

            var m = (y2 - y1) / (x2 - x1);

            return m * (dataValue - x1) + y1;
        }

        /// <inheritdoc />
        public override double ScaleFromUi(double pixelsValue, Plane plane, Size? size = null)
        {
            var chartSize = size ?? DrawAreaSize;

            // based on the linear equation
            // y = m * (x - x1) + y1 
            // where x is the Series.Values scale and y the UI scale
            // then
            // x = ((y - y1) / m) + x1

            double x1, x2, y1, y2;

            if (plane.PlaneType == PlaneTypes.X)
            {
                x1 = plane.ActualMaxValue + plane.ActualPointWidth.X;
                y1 = chartSize.Width;
                x2 = plane.ActualMinValue + plane.ActualPointWidth.Y;
                y2 = 0;
            }
            else
            {
                x1 = plane.ActualMaxValue;
                y1 = 0;
                x2 = plane.ActualMinValue;
                y2 = chartSize.Height;
            }

            var m = (y2 - y1) / (x2 - x1);

            return (pixelsValue - y1) / m + x1;
        }

        /// <inheritdoc cref="ChartModel.Update"/>
        protected override void Update(bool restart)
        {
            // run the update on the view's thread
            View.InvokeOnThread(() =>
            {
                base.Update(restart);

                // see appendix/chart.spacing.png
                var drawMargin = EvaluateAxisAndGetDrawMargin();
                DrawAreaSize = new Size(
                    DrawAreaSize.Width - drawMargin.Left - drawMargin.Right,
                    DrawAreaSize.Height - drawMargin.Top - drawMargin.Bottom);
                DrawAreaLocation = new Point(
                    DrawAreaLocation.X + drawMargin.Left,
                    DrawAreaLocation.Y + drawMargin.Top);

                if (DrawAreaSize.Width <= 0 || DrawAreaSize.Height <= 0)
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
                    foreach (var plane in dimension)
                    {
                        var axis = (Axis)plane;
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
            });
        }
        /// <inheritdoc />
        protected override void ViewOnPointerMoved(Point location, TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            var selectedPoints = Series.SelectMany(series => series.SelectPointsByDimension(selectionMode, dimensions)).ToArray();

            if (selectionMode == TooltipSelectionMode.Auto)
            {
                // ToDo: guess what the user meant here ...
            }

            Console.WriteLine(selectedPoints.Length);

            ToolTip = View.DataToolTip;

            // ReSharper disable once PossibleMultipleEnumeration
            if (!selectedPoints.Any())
            {
                TooltipTimoutTimer.Start();
                return;
            }

            TooltipTimoutTimer.Stop();

            var size = View.DataToolTip.ShowAndMeasure(selectedPoints, View);
            double sx = 0, sy = 0;

            foreach (var point in selectedPoints)
            {
                var point2D = (Point2D) point.Coordinate;
                sx += ScaleToUi(point2D.X, XAxis[point.Series.ScalesAt[0]]);
                sy += ScaleToUi(point2D.Y, YAxis[point.Series.ScalesAt[1]]);
            }

            sx = sx / selectedPoints.Length;
            sy = sy / selectedPoints.Length;

            var newTooltipLocation = new Point(sx, sy);

            if (_previousTooltipLocation != newTooltipLocation)
            {
                View.DataToolTip.Move(newTooltipLocation, View);
            }

            _previousTooltipLocation = newTooltipLocation;
        }

        internal Margin EvaluateAxisAndGetDrawMargin()
        {
            var requiresDrawMarginEvaluation = DrawMargin == Margin.Empty;

            double xt = 0, xr = 0, xb = 0, xl = 0;
            double yt = 0, yr = 0, yb = 0, yl = 0;

            // for each dimension (for a cartesian chart X and Y)
            for (var dimensionIndex = 0; dimensionIndex < Dimensions.Length; dimensionIndex++)
            {
                var dimensionRanges = DataRangeMatrix[dimensionIndex];
                var dimension = Dimensions[dimensionIndex];

                // for each axis in each dimension
                for (var index = 0; index < dimension.Length; index++)
                {
                    var axis = (Axis) dimension[index];

                    axis.PlaneType = Dimensions[0].Contains(axis) ? PlaneTypes.X : PlaneTypes.Y;
                    axis.Position = axis.Position == AxisPosition.Auto
                        ? (axis.PlaneType == PlaneTypes.X
                            ? AxisPosition.Bottom
                            : AxisPosition.Left)
                        : axis.Position;

                    // get the axis limits...
                    axis.ActualMinValue = double.IsNaN(axis.MinValue)
                        ? dimensionRanges[index].Min
                        : axis.MinValue;
                    axis.ActualMaxValue = double.IsNaN(axis.MaxValue)
                        ? dimensionRanges[index].Max
                        : axis.MaxValue;

                    if (!requiresDrawMarginEvaluation) continue;

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