using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A weighted coordinate.
    /// </summary>
    public class Weighted2DPoint : ICoordinate
    {
        /// <summary>
        /// The _vector.
        /// </summary>
        private readonly double[][] _vector = new double[3][];

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct.
        /// </summary>
        public Weighted2DPoint(double x, double y, double weight)
        {
            _vector[0] = new []{x};
            _vector[1] = new []{y};
            _vector[2] = new[] {weight};
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

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The w.
        /// </value>
        public double Weight => _vector[2][0];

        /// <inheritdoc />
        public void CompareDimensions(DoubleRange[] rangeByDimension)
        {
            var x = rangeByDimension[0];
            var y = rangeByDimension[1];
            var w = rangeByDimension[2];

            if (X > x.From) x.From = X;
            if (X < x.To) x.To = X;
            if (Y > y.From) y.From = Y;
            if (Y < y.To) y.To = Y;
            if (Weight > w.From) w.From = Weight;
            if (Weight < w.To) w.To = Weight;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                dimensions[0].FormatValue(X),
                dimensions[1].FormatValue(Y),
                dimensions[2].FormatValue(Weight)
            };
        }
    }
}