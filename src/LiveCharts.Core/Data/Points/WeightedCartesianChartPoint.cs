using System;
using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a weighted point in a cartesian plane.
    /// </summary>
    /// <seealso cref="ChartPoint{TModel,TCoordinate, TViewModel}" />
    public class WeightedCartesianChartPoint<TModel, TViewModel> : ChartPoint<TModel, Weighted2DPoint, TViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedCartesianChartPoint{TModel, TViewModel}"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="creationStamp">The creation stamp.</param>
        public WeightedCartesianChartPoint(double x, double y, double weight, object creationStamp)
            : base(creationStamp)
        {
            Coordinate = new Weighted2DPoint(x, y, weight);
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; internal set; }

        /// <inheritdoc cref="ChartPoint{T, U, V}.CompareDimensions" />
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var xDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];
            var wDimensionRange = ranges[2];

            switch (skipCriteria)
            {
                case SeriesSkipCriteria.IgnoreXOverflow:
                    if (Coordinate.X < xDimensionRange.From || Coordinate.X > xDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreYOverflow:
                    if (Coordinate.Y < yDimensionRange.From || Coordinate.Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreOverflow:
                    if (Coordinate.X < xDimensionRange.From || Coordinate.X > xDimensionRange.To ||
                        Coordinate.Y < yDimensionRange.From || Coordinate.Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skipCriteria), skipCriteria, null);
            }

            if (Coordinate.X > xDimensionRange.Max) xDimensionRange.Max = Coordinate.X;
            if (Coordinate.X < xDimensionRange.Min) xDimensionRange.Min = Coordinate.X;
            if (Coordinate.Y > yDimensionRange.Max) yDimensionRange.Max = Coordinate.Y;
            if (Coordinate.Y < yDimensionRange.Min) yDimensionRange.Min = Coordinate.Y;
            if (Weight > wDimensionRange.Max) wDimensionRange.Max = Weight;
            if (Weight < wDimensionRange.Min) wDimensionRange.Min = Weight;

            return false;
        }
    }
}