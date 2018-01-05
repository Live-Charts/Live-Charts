using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Specifies the an object is able to configure a charting type.
    /// </summary>
    public interface IChartingTypeBuilder
    {
        IChartingPointBuilder GetBuilder(ChartPointTypes pointType);
    }
}