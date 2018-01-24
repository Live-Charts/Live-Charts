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

        public override double ScaleToUi(double dataValue, Plane plane, Size? size = null)
        {
            throw new System.NotImplementedException();
        }

        public override double ScaleFromUi(double pixelsValue, Plane plane, Size? size = null)
        {
            throw new System.NotImplementedException();
        }
    }
}