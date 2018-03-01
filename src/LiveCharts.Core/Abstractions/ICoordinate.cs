using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// defines a live charts coordinate.
    /// </summary>
    public interface ICoordinate
    {
        /// <summary>
        /// Gets or sets the <see cref="float"/> with the specified dimension.
        /// </summary>
        /// <value>
        /// The <see cref="float"/> value.
        /// </value>
        /// <param name="dimension">The dimension.</param>
        /// <returns></returns>
        float[] this[int dimension] { get; }

        /// <summary>
        /// Compares the dimensions.
        /// </summary>
        /// <param name="rangeByDimension">The series range by dimension.</param>
        void CompareDimensions(RangeF[] rangeByDimension);

        /// <summary>
        ///gets the coordinate as tooltip data.
        /// </summary>
        /// <returns></returns>
        string[] AsTooltipData(params Plane[] dimensions);
    }
}
