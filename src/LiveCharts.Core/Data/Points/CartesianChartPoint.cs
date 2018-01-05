using System;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a point in a cartesian plane.
    /// </summary>
    /// <seealso cref="ChartPoint" />
    public class CartesianChartPoint : ChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianChartPoint"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public CartesianChartPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the x coordinate value.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; internal set; }

        /// <summary>
        /// Gets the y coordinate value.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; internal set; }

        /// <inheritdoc cref="ChartPoint.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var xDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];

            switch (skipCriteria)
            {
                case SeriesSkipCriteria.IgnoreXOverflow:
                    if (X < xDimensionRange.From || X > xDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreYOverflow:
                    if (Y < yDimensionRange.From || Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreOverflow:
                    if (X < xDimensionRange.From || X > xDimensionRange.To || Y < yDimensionRange.From || Y > yDimensionRange.To) return true;
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