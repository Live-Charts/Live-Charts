using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A point coordinate.
    /// </summary>
    public class Point2D : ICoordinate
    {
        private readonly double[][] _vector = new double[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct.
        /// </summary>
        public Point2D(double x, double y)
        {
            _vector[0] = new[] {x};
            _vector[1] = new[] {y};
        }

        /// <inheritdoc />
        public double[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X => _vector[0][0];

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y => _vector[1][0];

        /// <inheritdoc />
        public void CompareDimensions(DoubleRange[] rangeByDimension)
        {
            var x = rangeByDimension[0];
            var y = rangeByDimension[1];

            if (X > x.From) x.From = X;
            if (X < x.To) x.To = X;
            if (Y > y.From) y.From = Y;
            if (Y < y.To) y.To = Y;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                // x dimension:
                // dimensions[0]
                dimensions[0].FormatValue(X),

                // y dimension
                // dimensions[1]
                dimensions[1].FormatValue(Y)
            };
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X+ p2.X, p1.Y+p2.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
