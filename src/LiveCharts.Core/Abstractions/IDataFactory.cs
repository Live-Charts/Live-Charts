using System.Collections.Generic;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart point factory.
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="args">The options.</param>
        /// <returns></returns>
        IEnumerable<ChartPoint> Fetch(DataFactoryArgs args);
    }
}
