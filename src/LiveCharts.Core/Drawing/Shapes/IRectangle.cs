namespace LiveCharts.Core.Drawing.Shapes
{
    /// <summary>
    /// De3fines a rectangle shape in the user interface.
    /// </summary>
    public interface IRectangle : IShape
    {
        /// <summary>
        /// Gets or sets the x radius.
        /// </summary>
        /// <value>
        /// The x radius.
        /// </value>
        double XRadius { get; set; }

        /// <summary>
        /// Gets or sets the y radius.
        /// </summary>
        /// <value>
        /// The y radius.
        /// </value>
        double YRadius { get; set; }
    }
}
