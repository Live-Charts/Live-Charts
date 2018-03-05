using System.CodeDom;
using System.Collections.Generic;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a rectangle.
    /// </summary>
    public struct Rectangle
    {
        private readonly bool _isEmpty;

        /// <summary>
        /// The empty instance.
        /// </summary>
        public static Rectangle Empty = new Rectangle(true);

        private Rectangle(bool isEmpty)
        {
            _isEmpty = isEmpty;
            Top = 0f;
            Left = 0f;
            Width = 0f;
            Height = 0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Point point, Size size)
        {
            _isEmpty = false;
            Top = point.Y;
            Left = point.X;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(float left, float top, float width, float height)
        {
            _isEmpty = false;
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="size">The size.</param>
        public Rectangle(IReadOnlyList<float> point, IReadOnlyList<float> size)
        {
            _isEmpty = false;
            Top = point[1];
            Left = point[0];
            Width = size[0];
            Height = size[1];
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public float Top { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public float Left { get; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float Width { get; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public float Height { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
            {
                return false;
            }

            var rectangle = (Rectangle)obj;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            return _isEmpty == rectangle._isEmpty &&
                   
                   Top == rectangle.Top &&
                   Left == rectangle.Left &&
                   Width == rectangle.Width &&
                   Height == rectangle.Height;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _isEmpty.GetHashCode();
                hashCode = (hashCode * 397) ^ Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Rectangle r1, Rectangle r2)
        {
            return Equals(r1, r2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Rectangle r1, Rectangle r2)
        {
            return !(r1 == r2);
        }
    }
}