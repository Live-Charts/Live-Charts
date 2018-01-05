using System;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a margin.
    /// </summary>
    public struct Margin
    {
        /// <summary>
        /// The empty
        /// </summary>
        public static Margin Empty = new Margin(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="topRightBottomAndLeftMargin">The top right bottom and left margin.</param>
        public Margin(int topRightBottomAndLeftMargin)
        {
            Top = topRightBottomAndLeftMargin;
            Right = topRightBottomAndLeftMargin;
            Bottom = topRightBottomAndLeftMargin;
            Left = topRightBottomAndLeftMargin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="xMargin">The x margin.</param>
        /// <param name="yMargin">The y margin.</param>
        public Margin(int xMargin, int yMargin)
        {
            Top = yMargin;
            Right = xMargin;
            Bottom = yMargin;
            Left = xMargin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="left">The left.</param>
        public Margin(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="left">The left.</param>
        public Margin(double top, double right, double bottom, double left)
        {
            Top = (int) Math.Round(top);
            Right = (int) Math.Round(right);
            Bottom = (int) Math.Round(bottom);
            Left = (int) Math.Round(left);
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int Top { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public int Left { get; }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public int Right { get; }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        public int Bottom { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Margin c1, Margin c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Margin c1, Margin c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Margin operator +(Margin c1, Margin c2)
        {
            return new Margin(c1.Top + c2.Top, c1.Right + c2.Right, c1.Bottom + c2.Bottom, c1.Left + c2.Left);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Margin operator -(Margin c1, Margin c2)
        {
            return new Margin(c1.Top - c2.Top, c1.Right - c2.Right, c1.Bottom - c2.Bottom, c1.Left - c2.Left);
        }

        /// <summary>
        /// Compares equality the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Margin other)
        {
            return Top == other.Top && Left == other.Left && Right == other.Right && Bottom == other.Bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Margin && Equals((Margin)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Top;
                hashCode = (hashCode * 397) ^ Left;
                hashCode = (hashCode * 397) ^ Right;
                hashCode = (hashCode * 397) ^ Bottom;
                return hashCode;
            }
        }
    }
}