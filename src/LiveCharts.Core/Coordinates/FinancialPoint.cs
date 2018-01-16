using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// Financial coordinate.
    /// </summary>
    public struct FinancialPoint : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPoint"/> struct.
        /// </summary>
        /// <param name="index">the index.</param>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        public FinancialPoint(int index, double open, double high, double low, double close)
        {
            Index = index;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets the open.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        public double Open { get; }

        /// <summary>
        /// Gets the high.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public double High { get; }

        /// <summary>
        /// Gets the low.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public double Low { get; }

        /// <summary>
        /// Gets the close.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        public double Close { get; }

        /// <inheritdoc cref="ICoordinate.CompareDimensions"/>
        public bool CompareDimensions(DimensionRange[] dimensionRanges, SeriesSkipCriteria skipCriteria)
        {
            var indexDimensionRange = dimensionRanges[0];
            var yDimensionRange = dimensionRanges[1];

            if (Index > indexDimensionRange.Max) indexDimensionRange.Max = Index;
            if (Index < indexDimensionRange.Min) indexDimensionRange.Min = Index;
            if (High > yDimensionRange.Max) yDimensionRange.Max = High;
            if (Low < yDimensionRange.Min) yDimensionRange.Min = Low;

            return false;
        }

        /// <inheritdoc />
        public string[][] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                // x dimension:
                // dimensions[0]
                new[] {dimensions[0].FormatValue(Index)},           // first line in the tooltip.

                // y dimension
                // dimensions[1]
                new[]
                {
                    $"Open: {dimensions[1].FormatValue(Open)}",     // first line in the tooltip
                    $"High: {dimensions[1].FormatValue(High)}",     // second line
                    $"Low: {dimensions[1].FormatValue(Low)}",       // third line
                    $"Close: {dimensions[1].FormatValue(Close)}"    // fourth line
                }
            };
        }
    }
}