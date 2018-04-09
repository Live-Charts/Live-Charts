using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
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
        public float Index => _vector[0][0];

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
        public void CompareDimensions(float[][] rangeByDimension, Dictionary<int, float[]> stacker)
        {
            int index;

            unchecked
            {
                index = (int) _vector[0][0];
            }

            var value = _vector[1][0];

            if (!stacker.ContainsKey(index))
            {
                stacker.Add(index, new[] {0f, 0f});
            }

            From = stacker[index][0];
            To = From + value;
            stacker[index][0] = To;
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