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
        /// <param name="rangeBydimension">The series range by dimension.</param>
        void CompareDimensions(DataRange[] rangeBydimension);

        /// <summary>
        ///gets the coordinate as tooltip data.
        /// </summary>
        /// <returns></returns>
        string[] AsTooltipData(params Plane[] dimensions);
    }
}
