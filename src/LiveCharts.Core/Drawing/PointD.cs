namespace LiveCharts.Drawing
{
    /// <summary>
    /// A Point with double precision.
    /// </summary>
    public struct PointD
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; set; }

        /// <inheritdoc></inheritdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is PointD))
            {
                return false;
            }

            var d = (PointD)obj;
            return X == d.X &&
                   Y == d.Y;
        }

        /// <inheritdoc></inheritdoc>
        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}