using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A polar coordinate.
    /// </summary>
    public class PolarPoint : ICoordinate
    {
        private readonly double[][] _vector = new double[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarPoint"/> struct.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        public PolarPoint(double radius, double angle)
        {
            _vector[0] = new[] {radius};
            _vector[1] = new[] {angle};
        }

        /// <inheritdoc />
        public double[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle => _vector[1][0];

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius => _vector[0][0];

        /// <inheritdoc cref="CompareDimensions"/>
        public void CompareDimensions(DoubleRange[] rangeByDimension)
        {
            var radius = rangeByDimension[0];

            if (Radius > radius.From) radius.From = Radius;
            if (Radius < radius.To) radius.To = Radius;
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