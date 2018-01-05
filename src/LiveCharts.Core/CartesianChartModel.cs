using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Point = LiveCharts.Core.Drawing.Point;
using Rectangle = LiveCharts.Core.Drawing.Rectangle;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Core
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

        /// <inheritdoc cref="ChartModel.Update"/>
        protected override void Update(bool restart)
        {
            base.Update(restart);

            var chartSpace = View.ControlSize;

            // draw legend
            if (View.Legend != null)
            {
                // ToDo:
                // we must use a smarter method to recognize if the 
                // chart legend size changed,
                // if it did not, then we could skip UpdateLayout() method.
                View.Legend.Series = View.Series;
                View.Legend.UpdateLayout();
                chartSpace -= View.Legend.ControlSize;
            }

            // see appendix/chart.spacing.png
            var drawMargin = EvaluateAxisAndGetDrawMargin();
            DrawAreaSize = new Size(
                chartSpace.Width - drawMargin.Left - drawMargin.Right,
                chartSpace.Height - drawMargin.Top - drawMargin.Bottom);

            if (DrawAreaSize.Width <= 0 || DrawAreaSize.Height <= 0)
            {
                // skip update if the chart is too small.
                // and lets delete its content...
                CollectResources();
                return;
            }

            // draw separators

            // for each dimension (for a cartesian chart X and Y)
            foreach (var axisArray in View.AxisArrayByDimension)
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
            for (var dimensionIndex = 0; dimensionIndex < View.AxisArrayByDimension.Count; dimensionIndex++)
            {
                var dimensionRanges = DataRangeMatrix[dimensionIndex];
                var planesArray = View.AxisArrayByDimension[dimensionIndex];

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