namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Defines an ARGB color.
    /// </summary>
    public struct Color
    {
        private readonly bool _isEmpty;

        private Color(bool isEmpty)
        {
            _isEmpty = isEmpty;
            A = 0;
            B = 0;
            G = 0;
            R = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="a">alpha.</param>
        /// <param name="r">red.</param>
        /// <param name="g">green.</param>
        /// <param name="b">blue.</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            _isEmpty = false;
            A = a;
            B = b;
            G = g;
            R = r;
        }

        /// <summary>
        /// An empty color.
        /// </summary>
        public static Color Empty = new Color(true);

        /// <summary>
        /// Gets or sets alpha component.
        /// </summary>
        /// <value>
        /// a.
        /// </value>
        public byte A { get; }

        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        /// <value>
        /// The r.
        /// </value>
        public byte R { get; }

        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        /// <value>
        /// The g.
        /// </value>
        public byte G { get; }
        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        /// <value>
        /// The b.
        /// </value>
        public byte B { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Color c1, Color c2)
        {
            return Equals(c1, c2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Color c1, Color c2)
        {
            return !Equals(c1, c2);
        }

        /// <summary>
        /// Compares the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Color other)
        {
            return _isEmpty == other._isEmpty && A == other.A && R == other.R && G == other.G && B == other.B;
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
            return obj is Color && Equals((Color)obj);
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
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }
    }
}