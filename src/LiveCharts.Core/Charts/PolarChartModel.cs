using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Represents a chart with a polar system.
    /// </summary>
    /// <seealso cref="ChartModel" />
    public class PolarChartModel : ChartModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        public PolarChartModel(IChartView view)
            : base(view)
        {
        }

        /// <inheritdoc />
        public override double ScaleToUi(double dataValue, Plane plane, double[] sizeVector = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override double ScaleFromUi(double pixelsValue, Plane plane, double[] sizeVector)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override void ViewOnPointerMoved(Point location, TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            throw new System.NotImplementedException();
        }
    }
}