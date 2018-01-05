namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a pie chart point.
    /// </summary>
    /// <seealso cref="ChartPoint" />
    public class PieChartPoint : ChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartPoint"/> class.
        /// </summary>
        /// <param name="index">the index.</param>
        /// <param name="value">The value.</param>
        public PieChartPoint(int index, double value)
        {
            Index = index;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; internal set; }

        /// <summary>
        /// Gets the participation.
        /// </summary>
        /// <value>
        /// The participation.
        /// </value>
        public double Participation { get; internal set; }

        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            return false;
        }
    }
}