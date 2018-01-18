using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Represents a chart with a cartesian plane coordinate system (x,y).
    /// </summary>
    /// <seealso cref="ChartModel" />
    public class CartesianChartModel : ChartModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianChartModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
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
        public IList<Plane> XAxis => View.PlaneSets[0];

        /// <summary>
        /// Gets the y axis.
        /// </summary>
        /// <value>
        /// The y axis.
        /// </value>
        public IList<Plane> YAxis => View.PlaneSets[1];

        /// <inheritdoc />
        public override double ScaleToUi(double value, Plane plane, Size? size = null)
        {
            var chartSize = size ?? DrawAreaSize;

            // based on the linear equation 
            // y = m * (x - x1) + y1
            // where m is the slope, (y2 - y1) / (x2 - x1)

            // for now we only support cartesian planes, X, other wise we suppose it is Y.
            var dimension = plane.Type == PlaneTypes.X ? chartSize.Width : chartSize.Height;

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
                CollectResources();
                return;
            }

            View.UpdateDrawArea(new Rectangle(DrawAreaLocation, DrawAreaSize));

            // draw separators

            // for each dimension (for a cartesian chart X and Y)
            foreach (var axisArray in View.PlaneSets)
            {
                // for each axis in each dimension
                foreach (var plane in axisArray)
                {
                    var axis = (Axis) plane;
                    RegisterResource(axis);
                    axis.DrawSeparators(this);
                }
            }

            foreach (var series in View.Series.Where(x => x.IsVisible))
            {
                RegisterResource(series);
                series.UpdateView(this);
            }

            //foreach (var series in Chart.View.ActualSeries)
            //{
            //    series.OnSeriesUpdateStart();
            //    series.ActualValues.InitializeStep(series);
            //    series.Model.Update();
            //    series.ActualValues.CollectGarbage(series);
            //    series.OnSeriesUpdatedFinish();
            //    series.PlaceSpecializedElements();
            //}
        }

        internal Margin EvaluateAxisAndGetDrawMargin()
        {
            var requiresDrawMarginEvaluation = View.DrawMargin == Margin.Empty;

            int l = 0, r = 0, t = 0, b = 0;

            // for each dimension (for a cartesian chart X and Y)
            for (var dimensionIndex = 0; dimensionIndex < View.PlaneSets.Count; dimensionIndex++)
            {
                var dimensionRanges = DataRangeMatrix[dimensionIndex];
                var planesArray = View.PlaneSets[dimensionIndex];

                // for each axis in each dimension
                for (var index = 0; index < planesArray.Count; index++)
                {
                    var axis = (Axis) planesArray[index];

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
                : View.DrawMargin;
        }
    }
}