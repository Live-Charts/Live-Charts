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
using System.Drawing;

#endregion

namespace LiveCharts.Drawing
{
    /// <summary>
    /// Represents an axis label.
    /// </summary>
    /// <seealso cref="Rectangle" />
    internal struct AxisSectionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisSectionViewModel"/> struct.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="offset">The offset.</param>
        /// <param name="margin">The margin.</param>
        /// <param name="content">The content.</param>
        /// <param name="size">The size.</param>
        /// <param name="actuaLabelsRotation">The plane actual labels rotation.</param>
        /// <param name="position">The axis position</param>
        public AxisSectionViewModel(
            PointF pointer,
            PointF offset,
            Padding margin,
            object content,
            SizeF size,
            double actuaLabelsRotation,
            AxisPosition position)
        {
            Pointer = pointer;
            Offset = offset;
            if (actuaLabelsRotation > 90 || position == AxisPosition.Top)
            {
                offset = new PointF(offset.X, -offset.Y);
            }
            UiPosition = Perform.Sum(pointer, offset);
            Margin = margin;
            Content = content;
            Size = size;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the pointer, its the point where the separator is drawn in the chart, see the × in the diagram.
        /// </summary>
        /// <value>
        /// The pointer.
        /// </value>
        public PointF Pointer { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public PointF Offset { get; set; }

        /// <summary>
        /// Gets the actual location (Location + Offset)
        /// </summary>
        /// <value>
        /// The actual location.
        /// </value>
        public PointF UiPosition { get; }

        /// <summary>
        /// Gets the size of the label.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public SizeF Size { get; }

        /// <summary>
        /// Gets or sets the margin, it represent the space taken by the label to every direction, top, left, bottom and right,see appendix/labels.2.png  [l, t, r, b].
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Padding Margin { get; set; }
    }
}