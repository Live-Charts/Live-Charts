namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ChartPoint" />
    public class StackedCartesianChartPoint : ChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedCartesianChartPoint"/> class.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dimensionRange">The range.</param>
        public StackedCartesianChartPoint(int index, DimensionRange dimensionRange)
        {
            DimensionRange = dimensionRange;
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        public DimensionRange DimensionRange { get; internal set; }

        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}