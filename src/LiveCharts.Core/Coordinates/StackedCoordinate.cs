using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// The stacked coordinate.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.ICoordinate" />
    public class StackedCoordinate : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedCoordinate"/> class.
        /// </summary>
        /// <param name="key">The index.</param>
        /// <param name="value">The value.</param>
        public StackedCoordinate(float key, float value)
        {
            _vector[0] = new[] {key};
            _vector[1] = new[] {value};
        }

        /// <inheritdoc />
        public float[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public float Key => _vector[0][0];

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value => _vector[1][0];

        /// <summary>
        /// Gets from value of the stack.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public float From { get; protected set; }

        /// <summary>
        /// Gets the end value of the stack.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public float To { get; protected set; }

        /// <inheritdoc />
        public void CompareDimensions(IDataFactoryContext context)
        {
            // stacking..
            int index;

            unchecked
            {
                index = (int) _vector[0][0];
            }

            var value = _vector[1][0];

            var stack = context.UpdateContext.Stack(index, context.SeriesScalesAt[1], value);

            From = stack.From;
            To = stack.To;

            if (context.UpdateContext.RangeByDimension == null) return;
            // store max and min limits..
            var x = context.UpdateContext.RangeByDimension[0];
            var y = context.UpdateContext.RangeByDimension[1];

            if (index > x[1]) x[1] = index; // 0: min, 1: Max
            if (index < x[0]) x[0] = index;
            if (To > y[1]) y[1] = To;
            if (To < y[0]) y[0] = To;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                dimensions[0].FormatValue(Value)
            };
        }
    }
}