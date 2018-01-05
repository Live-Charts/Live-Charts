namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a financial chart point.
    /// </summary>
    /// <seealso cref="ChartPoint" />
    public class FinancialChartPoint : ChartPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialChartPoint"/> class.
        /// </summary>
        /// <param name="key">the key.</param>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        public FinancialChartPoint(int key, double open, double high, double low, double close)
        {
            Index = key;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// Gets the key of the point, a key is used internally as a unique identifier in
        /// in a <see cref="N:LiveCharts.Core.Series" />
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the open.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        public double Open { get; internal set; }

        /// <summary>
        /// Gets the high value.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public double High { get; internal set; }

        /// <summary>
        /// Gets the low value.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public double Low { get; internal set; }

        /// <summary>
        /// Gets the close value.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        public double Close { get; internal set; }

        /// <inheritdoc cref="ChartPoint.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var indexDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];

            if (Index > indexDimensionRange.Max) indexDimensionRange.Max = Index;
            if (Index < indexDimensionRange.Min) indexDimensionRange.Min = Index;
            if (High > yDimensionRange.Max) yDimensionRange.Max = High;
            if (Low < yDimensionRange.Min) yDimensionRange.Min = Low;

            return false;
        }
    }
}
