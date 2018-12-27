namespace LiveCharts.Animations
{
    /// <summary>
    /// Represents an animation frame, where <see cref="Time"/> is the percentage of 
    /// the total time in the animation and <see cref="Value"/> is the value the 
    /// animation should have at the respective time.
    /// </summary>
    public struct KeyFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyFrame"/> struct.
        /// </summary>
        /// <param name="time">The time elapsed nin percentage.</param>
        /// <param name="value">The value elapsed in percentage.</param>
        public KeyFrame(float time, float value)
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
        public float Time { get; set; }

        /// <summary>
        /// Gets or sets the value in percentage.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value { get; set; }
    }
}