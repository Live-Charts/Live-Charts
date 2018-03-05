namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a size.
    /// </summary>
    public struct Size
    {
        private readonly bool _isEmpty;

        private Size(bool isEmpty)
        {
            _isEmpty = isEmpty;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Size(float width, float height)
        {
            _isEmpty = false;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// The empty size.
        /// </summary>
        public static Size Empty = new Size(true);

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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Width}, {Height}";
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Size c1, Size c2)
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
        public static bool operator !=(Size c1, Size c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator -(Size c1, Size c2)
        {
            return new Size(c1.Width - c2.Width, c1.Height - c2.Height);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator +(Size c1, Size c2)
        {
            return new Size(c1.Width + c2.Width, c1.Height + c2.Height);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator -(Size c1, Margin c2)
        {
            return new Size(c1.Width - c2.Left - c2.Right, c1.Height - c2.Top - c2.Bottom);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator +(Size c1, Margin c2)
        {
            return new Size(c1.Width + c2.Left + c2.Right, c1.Height + c2.Top + c2.Bottom);
        }

        /// <summary>
        /// Compares the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Size other)
        {
            return _isEmpty == other._isEmpty && Width == other.Width && Height == other.Height;
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
            return obj is Size && Equals((Size)obj);
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
                var hashCode = _isEmpty.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }
    }
}