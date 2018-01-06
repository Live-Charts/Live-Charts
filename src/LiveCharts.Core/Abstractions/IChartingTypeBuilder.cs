using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Specifies the an object is able to configure a charting type.
    /// </summary>
    public interface IChartingTypeBuilder
    {
        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <param name="pointType">Type of the point.</param>
        /// <returns></returns>
        IChartingPointBuilder GetBuilder(ChartPointTypes pointType);
    }
}