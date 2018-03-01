using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// Financial coordinate.
    /// </summary>
    public class FinancialPoint : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPoint"/> struct.
        /// </summary>
        /// <param name="index">the index.</param>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        public FinancialPoint(int index, float open, float high, float low, float close)
        {
            _vector[0] = new[] {(float) index};
            _vector[1] = new[] {low, high};
            Open = open;
            Close = close;
        }

        /// <inheritdoc />
        public float[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index => (int) _vector[0][0];

        /// <summary>
        /// Gets the open.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        public float Open { get; }

        /// <summary>
        /// Gets the high.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public float High => _vector[1][1];

        /// <summary>
        /// Gets the low.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public float Low => _vector[1][0];

        /// <summary>
        /// Gets the close.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        public float Close { get; }

        /// <inheritdoc />
        public void CompareDimensions(RangeF[] rangeByDimension)
        {
            var x = rangeByDimension[0];
            var y = rangeByDimension[1];

            if (Index > x.From) x.From = Index;
            if (Index < x.To) x.To = Index;
            if (High > y.From) y.From = High;
            if (Low < y.To) y.To = Low;
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