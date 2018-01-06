namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a PolarChartPoint.
    /// </summary>
    public class PolarChartPoint : ChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarChartPoint"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="creationStamp">the update stamp.</param>
        public PolarChartPoint(double radius, double angle, object creationStamp)
            : base(creationStamp)
        {
            Radius = radius;
            Angle = angle;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius { get; internal set; }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle { get; internal set; }

        /// <inheritdoc cref="ChartPoint.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var radiusDimensionRange = ranges[0];

            if (Radius > radiusDimensionRange.Max) radiusDimensionRange.Max = Radius;
            if (Radius < radiusDimensionRange.Min) radiusDimensionRange.Min = Radius;

            return false;
        }
    }
}