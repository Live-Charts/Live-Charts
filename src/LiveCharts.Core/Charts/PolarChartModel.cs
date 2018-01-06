using LiveCharts.Core.Abstractions;

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
    }
}