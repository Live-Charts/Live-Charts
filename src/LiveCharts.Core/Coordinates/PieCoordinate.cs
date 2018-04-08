using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.ICoordinate" />
    public class PieCoordinate : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="PieCoordinate"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public PieCoordinate(float index, float value)
        {
            _vector[0] = new[] {index};
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

        /// <inheritdoc />
        public void CompareDimensions(float[][] rangeByDimension, Dictionary<object, float[]> stacker)
        {
            if (!stacker.ContainsKey(_vector[0][0]))
            {
                stacker.Add(_vector[0][0], new []{0f,0f});
            }

            stacker[_vector[0][0]][0] += _vector[1][0];
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