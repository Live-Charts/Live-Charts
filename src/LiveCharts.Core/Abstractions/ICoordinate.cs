using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

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

        /// <summary>
        ///gets the coordinate as tooltip data.
        /// </summary>
        /// <returns></returns>
        string[] AsTooltipData(params Plane[] dimensions);
    }
}
