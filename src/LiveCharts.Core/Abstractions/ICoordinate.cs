using System;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// defines a live charts coordinate.
    /// </summary>
    public interface ICoordinate
    {
        /// <summary>
        /// Compares the dimensions.
        /// </summary>
        /// <param name="dimensionRanges">The chart dimension ranges.</param>
        /// <param name="skipCriteria">The skip criteria.</param>
        /// <returns></returns>
        bool CompareDimensions(DimensionRange[] dimensionRanges, SeriesSkipCriteria skipCriteria);
    }
}
