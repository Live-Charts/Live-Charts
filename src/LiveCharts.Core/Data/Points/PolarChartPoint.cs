using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a PolarChartPoint.
    /// </summary>
    public class PolarChartPoint<TModel, TViewModel> : ChartPoint<TModel, PolarPoint, TViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarChartPoint{TModel, TViewModel}"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="creationStamp">The creation stamp.</param>
        public PolarChartPoint(double radius, double angle, object creationStamp)
            : base(creationStamp)
        {
            Coordinate = new PolarPoint(radius, angle);
        }

        /// <inheritdoc cref="ChartPoint{TModel, PolarCoordinate, TViewModel}.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var radiusDimensionRange = ranges[0];

            if (Coordinate.Radius > radiusDimensionRange.Max) radiusDimensionRange.Max = Coordinate.Radius;
            if (Coordinate.Radius < radiusDimensionRange.Min) radiusDimensionRange.Min = Coordinate.Radius;

            return false;
        }
    }
}