namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a UI object that its position in the screen is based on the <see cref="Top"/> and <see cref="Left"/> coordinates.
    /// </summary>
    public interface IPlaceable
    {
        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        double Top { get; set; }
    }
}