namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a data label position.
    /// </summary>
    public struct DataLabelsPosition
    {
        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        public HorizontalAlingment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>
        /// The vertical alignment.
        /// </value>
        public VerticalLabelPosition VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public double Rotation { get; set; }
    }
}
