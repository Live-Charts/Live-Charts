namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a range.
    /// </summary>
    public class DoubleRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class.
        /// </summary>
        public DoubleRange()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public DoubleRange(double min, double max)
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
        public double From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public double To { get; set; }
    }
}
