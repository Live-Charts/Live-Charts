namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines a ranged value.
    /// </summary>
    public class DimensionRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionRange"/> class.
        /// </summary>
        public DimensionRange()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionRange"/> struct.
        /// </summary>
        public DimensionRange(double from, double to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public double From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public double To { get; set; }

        /// <summary>
        /// Gets or sets from value.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public double Min { get; set; }

        /// <summary>
        /// Gets or sets to value.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public double Max { get; set; }
    }
}