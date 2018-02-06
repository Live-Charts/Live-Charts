using System.Linq;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.DefaultSettings
{
    /// <summary>
    /// A set of useful extensions.
    /// </summary>
    public static class LiveChartsExtensions
    {
        /// <summary>
        /// Sets the opacity.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="opacity">The opacity.</param>
        /// <returns></returns>
        public static Color SetOpacity(this Color color, double opacity)
        {
            return new Color((byte) (255 * opacity), color.R, color.G, color.B);
        }

        /// <summary>
        /// Gets the series dimensions.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        /// <returns></returns>
        public static DimensionRange[] GetSeriesDimensions(this ChartModel chart, DataSeries.BaseSeries series)
        {
            return series.ScalesAt
                .Select((scalesAtIndex, dimensionIndex) =>
                    chart.DataRangeMatrix[dimensionIndex][scalesAtIndex])
                .ToArray();
        }
    }
}
