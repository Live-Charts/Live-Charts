#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using LiveCharts.Dimensions;
using LiveCharts.Updating;

#endregion

namespace LiveCharts.Coordinates
{
    /// <summary>
    /// The stacked coordinate.
    /// </summary>
    /// <seealso cref="ICoordinate" />
    public class StackedPointCoordinate : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedPointCoordinate"/> class.
        /// </summary>
        /// <param name="key">The index.</param>
        /// <param name="value">The value.</param>
        public StackedPointCoordinate(float key, float value)
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

        /// <summary>
        /// Gets the total stack.
        /// </summary>
        /// <value>
        /// The total stack.
        /// </value>
        public float TotalStack { get; protected internal set; }

        /// <inheritdoc />
        public void Compare(IDataFactoryContext context)
        {
            // stacking..
            int index;

            unchecked
            {
                index = (int) _vector[0][0];
            }

            float value = _vector[1][0];

            var stack = context.UpdateContext.Stack(index, context.SeriesScalesAt[1], value);

            From = stack.From;
            To = stack.To;

            if (context.UpdateContext.Ranges == null) return;
            // store max and min limits..
            float[] x = context.UpdateContext.Ranges[0][context.SeriesScalesAt[0]];
            float[] y = context.UpdateContext.Ranges[1][context.SeriesScalesAt[0]];

            if (index > x[1]) x[1] = index; // 0: min, 1: Max
            if (index < x[0]) x[0] = index;
            if (To > y[1]) y[1] = To;
            if (To < y[0]) y[0] = To;
        }
    }
}