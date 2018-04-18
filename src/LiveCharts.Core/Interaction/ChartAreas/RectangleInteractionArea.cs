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

using System.Drawing;

#endregion

namespace LiveCharts.Core.Interaction.ChartAreas
{
    /// <summary>
    /// The rectangle interaction area class.
    /// </summary>
    /// <seealso cref="InteractionArea" />
    public class RectangleInteractionArea : InteractionArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleInteractionArea"/> class.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public RectangleInteractionArea(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }

        /// <summary>
        /// Gets the rectangle interaction area.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public RectangleF Rectangle { get; }

        /// <inheritdoc />
        public override bool Contains(params double[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            var a = x >= Rectangle.Left && x <= Rectangle.Left + Rectangle.Width &&
                    y >= Rectangle.Top && y <= Rectangle.Top + Rectangle.Height;

            if (a)
            {
                var b = 1;
            }

            return a;
        }
    }
}