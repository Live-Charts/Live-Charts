using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Core.Charts
{
    /// <inheritdoc />
    public class CartesianChartModel : ChartModel
    {
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

        /// <summary>
        /// Gets the width of the unit.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns></returns>
        public double GetUnitWidth(Plane plane)
        {
            return Math.Abs(ScaleToUi(0, plane) - ScaleToUi(plane.Unit, plane));
        }

        /// <inheritdoc />
        public override double ScaleToUi(double value, Plane plane, Size? size = null)
        {
            var chartSize = size ?? DrawAreaSize;

            // based on the linear equation 
            // y = m * (x - x1) + y1
            // where m is the slope, (y2 - y1) / (x2 - x1)

            var dimension = plane.PlaneType == PlaneTypes.X ? chartSize.Width : chartSize.Height;

            double x1 = plane.ActualMaxValue, y1 = dimension;
            double x2 = plane.ActualMinValue; // y2 = 0;

            // m was simplified from => ((0 - y1) / (x2-x1))
            return (y1 / (x1 - x2)) * (value - x1) + y1;
        }

        /// <inheritdoc cref="ChartModel.Update"/>
        protected override void Update(bool restart)
        {
            base.Update(restart);

            // see appendix/chart.spacing.png
            var drawMargin = EvaluateAxisAndGetDrawMargin();
            DrawAreaSize = new Size(
                DrawAreaSize.Width - drawMargin.Left - drawMargin.Right,
                DrawAreaSize.Height - drawMargin.Top - drawMargin.Bottom);

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
                // for each axis in each dimension
                foreach (var plane in dimension)
                {
                    var axis = (Axis) plane;
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
        }

        internal Margin EvaluateAxisAndGetDrawMargin()
        {
            var requiresDrawMarginEvaluation = DrawMargin == Margin.Empty;

            int l = 0, r = 0, t = 0, b = 0;

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
                    axis.Position = axis.Position == AxisPositions.Auto
                        ? (axis.PlaneType == PlaneTypes.X
                            ? AxisPositions.Bottom
                            : AxisPositions.Left)
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
                        case AxisPositions.Top:
                            t += mi.Top + mi.Bottom;
                            l += mi.Left;
                            r += mi.Right;
                            break;
                        case AxisPositions.Left:
                            l += mi.Left + mi.Right;
                            b += mi.Bottom;
                            t += mi.Bottom;
                            break;
                        case AxisPositions.Right:
                            r += mi.Left + mi.Right;
                            b += mi.Bottom;
                            t += mi.Bottom;
                            break;
                        case AxisPositions.Bottom:
                            b += mi.Top + mi.Bottom;
                            l += mi.Left;
                            r += mi.Right;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return requiresDrawMarginEvaluation
                ? new Margin(t, r, b, l)
                : DrawMargin;
        }
    }
}