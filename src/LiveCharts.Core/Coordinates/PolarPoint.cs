using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A polar coordinate.
    /// </summary>
    public struct PolarPoint : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarPoint"/> struct.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        public PolarPoint(double radius, double angle)
        {
            Angle = angle;
            Radius = radius;
        }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle { get; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius { get; }

        /// <inheritdoc cref="CompareDimensions"/>
        public bool CompareDimensions(DimensionRange[] dimensionRanges, SeriesSkipCriteria skipCriteria)
        {
            var radiusDimensionRange = dimensionRanges[0];

            if (Radius > radiusDimensionRange.Max) radiusDimensionRange.Max = Radius;
            if (Radius < radiusDimensionRange.Min) radiusDimensionRange.Min = Radius;

            return false;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                // x dimension:
                // dimensions[0]
                dimensions[0].FormatValue(Angle),

                // y dimension
                // dimensions[1]
                dimensions[1].FormatValue(Radius)
            };
        }
    }
}