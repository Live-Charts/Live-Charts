using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A point coordinate.
    /// </summary>
    public class Point2D : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct.
        /// </summary>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; }

        /// <summary>
        /// Compares the dimensions.
        /// </summary>
        /// <param name="dimensionRanges">The dimension ranges.</param>
        /// <param name="skipCriteria">The skip criteria.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CompareDimensions(DimensionRange[] dimensionRanges, SeriesSkipCriteria skipCriteria)
        {
            var xDimensionRange = dimensionRanges[0];
            var yDimensionRange = dimensionRanges[1];

            switch (skipCriteria)
            {
                case SeriesSkipCriteria.IgnoreXOverflow:
                    if (X < xDimensionRange.From || X > xDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreYOverflow:
                    if (Y < yDimensionRange.From || Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreOverflow:
                    if (X < xDimensionRange.From || X > xDimensionRange.To ||
                        Y < yDimensionRange.From || Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skipCriteria), skipCriteria, null);
            }

            if (X > xDimensionRange.Max) xDimensionRange.Max = X;
            if (X < xDimensionRange.Min) xDimensionRange.Min = X;
            if (Y > yDimensionRange.Max) yDimensionRange.Max = Y;
            if (Y < yDimensionRange.Min) yDimensionRange.Min = Y;
            return false;
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
