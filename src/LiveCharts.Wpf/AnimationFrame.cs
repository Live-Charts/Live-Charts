namespace LiveCharts.Wpf
{
    /// <summary>
    /// A helper class to build an animation using key frames.
    /// </summary>
    public struct Frame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame" /> struct.
        /// </summary>
        /// <param name="proportion">The time span proportion.</param>
        /// <param name="value">The value.</param>
        public Frame(double proportion, double value)
        {
            Value = value;
            Proportion = proportion;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the time span.
        /// </summary>
        /// <value>
        /// The time span.
        /// </value>
        public double Proportion { get; set; }
    }
}