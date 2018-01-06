using System;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Point point, Size size)
        {
            Top = point.Y;
            Left = point.X;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(int top, int left, int width, int height)
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(double top, double left, double width, double height)
        {
            Top = (int) Math.Round(top);
            Left = (int) Math.Round(left);
            Width = (int) Math.Round(width);
            Height = (int) Math.Round(height);
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }
    }
}