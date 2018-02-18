using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A stacked coordinate.
    /// </summary>
    public struct StackedPoint : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedPoint"/> struct.
        /// </summary>
        public StackedPoint(double participation, double value, int index)
        {
            Participation = participation;
            Value = value;
            Index = index;
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; }

        /// <summary>
        /// Gets or sets the participation.
        /// </summary>
        /// <value>
        /// The participation.
        /// </value>
        public double Participation { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public double TotalStacked => Value / Participation;

        /// <inheritdoc />
        public void CompareDimensions(DataRange[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            if (Index > x.MaxValue) x.MaxValue = Index;
            if (Index < x.MinValue) x.MinValue = Index;
            if (Value > y.MaxValue) y.MaxValue = Value;
            if (Value < y.MinValue) y.MinValue = Value;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            throw new NotImplementedException();
            //return new[]
            //{
            //    // x dimension:
            //    // dimensions[0]
            //    new[] {dimensions[0].FormatValue(X)}, // first line in the tooltip.

            //    // y dimension
            //    // dimensions[1]
            //    new[] {dimensions[1].FormatValue(Y)} // first line in the tooltip
            //};
        }
    }
}