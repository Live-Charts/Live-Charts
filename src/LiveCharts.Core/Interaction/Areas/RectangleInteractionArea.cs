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
using System.Drawing;
using LiveCharts.Drawing;
using LiveCharts.Interaction.Controls;

#endregion

namespace LiveCharts.Interaction.Areas
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
        public RectangleInteractionArea(RectangleD rectangle)
        {
            Rectangle = rectangle;
        }

        /// <summary>
        /// Gets the rectangle interaction area.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public RectangleD Rectangle { get; }

        /// <summary>
        /// Gets an empty intance.
        /// </summary>
        public static RectangleInteractionArea Empty => new RectangleInteractionArea(new RectangleD(new PointD(0, 0), new SizeD(0, 0)));

        /// <inheritdoc />
        public override bool Contains(PointF pointerLocation, ToolTipSelectionMode selectionMode)
        {
            float x = pointerLocation.X;
            float y = pointerLocation.Y;

            bool sharesX = x >= Rectangle.Left && x <= Rectangle.Left + Rectangle.Width;
            bool sharesY = y >= Rectangle.Top && y <= Rectangle.Top + Rectangle.Height;

            switch (selectionMode)
            {
                case ToolTipSelectionMode.SharedXy:
                    return sharesX && sharesY;
                case ToolTipSelectionMode.SharedX:
                    return sharesX;
                case ToolTipSelectionMode.SharedY:
                    return sharesY;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
            }
        }

        /// <inheritdoc />
        public override float DistanceTo(PointF pointerLocation, ToolTipSelectionMode selectionMode)
        {
            var x = pointerLocation.X;
            var y = pointerLocation.Y;

            switch (selectionMode)
            {
                case ToolTipSelectionMode.SharedXy:
                    return (float)Math.Sqrt(Math.Pow(x - Rectangle.Left, 2) + Math.Pow(y - Rectangle.Top, 2));
                case ToolTipSelectionMode.SharedX:
                    return Math.Abs(x - (float)Rectangle.Left);
                case ToolTipSelectionMode.SharedY:
                    return Math.Abs(y - (float)Rectangle.Top);
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
            }
        }
    }
}