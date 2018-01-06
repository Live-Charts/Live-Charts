namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Defines an ARGB color.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="a">alpha.</param>
        /// <param name="r">red.</param>
        /// <param name="g">green.</param>
        /// <param name="b">blue.</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            B = b;
            G = g;
            R = r;
        }

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
    }
}