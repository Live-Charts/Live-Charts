namespace LiveCharts.Core.Drawing.Shapes
{
    public interface IRectangle : IShape
    {
        /// <summary>
        /// Gets or sets the x radius.
        /// </summary>
        /// <value>
        /// The x radius.
        /// </value>
        float XRadius { get; set; }

        /// <summary>
        /// Gets or sets the y radius.
        /// </summary>
        /// <value>
        /// The y radius.
        /// </value>
        float YRadius { get; set; }
    }
}
