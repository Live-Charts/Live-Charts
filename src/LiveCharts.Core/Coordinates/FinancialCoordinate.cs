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

using System;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// Financial coordinate.
    /// </summary>
    public class FinancialCoordinate : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialCoordinate"/> struct.
        /// </summary>
        /// <param name="index">the index.</param>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        public FinancialCoordinate(int index, float open, float high, float low, float close)
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
        public void Compare(IDataFactoryContext context)
        {
            var x = context.UpdateContext.Ranges[0][context.SeriesScalesAt[0]];
            var y = context.UpdateContext.Ranges[1][context.SeriesScalesAt[0]];

            // 0: min, 1: max
            if (Index > x[1]) x[1] = Index;
            if (Index < x[0]) x[0] = Index;
            if (High > y[1]) y[1] = High;
            if (Low < y[0]) y[0] = Low;
        }
    }
}