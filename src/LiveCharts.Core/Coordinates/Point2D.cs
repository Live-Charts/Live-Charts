using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A point coordinate.
    /// </summary>
    public struct Point2D : ICoordinate
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
    }
}
