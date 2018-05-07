namespace LiveCharts.Core.Drawing
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
    }
}