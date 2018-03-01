namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a range.
    /// </summary>
    public class RangeF
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeF"/> class.
        /// </summary>
        public RangeF()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeF"/> class.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public RangeF(float min, float max)
        {
            From = min;
            To = max;
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public float From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public float To { get; set; }
    }
}
