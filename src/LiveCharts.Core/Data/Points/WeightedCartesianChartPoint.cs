using System;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a weighted point in a cartesian plane.
    /// </summary>
    /// <seealso cref="CartesianChartPoint" />
    public class WeightedCartesianChartPoint : CartesianChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedCartesianChartPoint"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="creationStamp">the update stamp.</param>
        public WeightedCartesianChartPoint(double x, double y, double weight, object creationStamp)
            : base(x, y, creationStamp)
        {
            Weight = weight;
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; internal set; }

        /// <inheritdoc cref="CartesianChartPoint.CompareDimensions" />
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var xDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];
            var wDimensionRange = ranges[2];

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
            if (Weight > wDimensionRange.Max) wDimensionRange.Max = Weight;
            if (Weight < wDimensionRange.Min) wDimensionRange.Min = Weight;

            return false;
        }
    }
}