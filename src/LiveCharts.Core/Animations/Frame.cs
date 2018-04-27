namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// Represents an animation frame, where <see cref="Time"/> is the percentage of 
    /// the total time in the animation and <see cref="Value"/> is the value the 
    /// animation should have at the respective time.
    /// </summary>
    public struct Frame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> struct.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="value">The value.</param>
        public Frame(double time, double value)
        {
            Time = time;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the elapsed time in percentage.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public double Time { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }
    }
}