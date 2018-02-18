using System;
using LiveCharts.Core.Abstractions;
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
        public int Index { get; }

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

        /// <inheritdoc />
        public void CompareDimensions(DataRange[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            if (Index > x.MaxValue) x.MaxValue = Index;
            if (Index < x.MinValue) x.MinValue = Index;
            if (High > y.MaxValue) y.MaxValue = High;
            if (Low < y.MinValue) y.MinValue = Low;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            var n = Environment.NewLine;
            return new[]
            {
                // x dimension:
                // dimensions[0]
                dimensions[0].FormatValue(Index),

                // y dimension
                // dimensions[1]
                $"Open: {dimensions[1].FormatValue(Open)}{n}High: {dimensions[1].FormatValue(High)}{n}Low: {dimensions[1].FormatValue(Low)}{n}Close: {dimensions[1].FormatValue(Close)}"
            };
        }
    }
}