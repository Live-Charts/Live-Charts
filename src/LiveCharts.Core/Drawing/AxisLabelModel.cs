namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents an axis label.
    /// </summary>
    /// <seealso cref="Rectangle" />
    public struct AxisLabelViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabelViewModel"/> struct.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="offset">The offset.</param>
        /// <param name="margin">The margin.</param>
        /// <param name="content">The content.</param>
        public AxisLabelViewModel(Point location, Point offset, Margin margin, string content)
        {
            Location = location;
            Offset = offset;
            Margin = margin;
            Content = content;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the raw location of the label, to ensure label readability, we set an <see cref="Offset"/> and calculate the margin from this point, to every direction (<see cref="Margin"/>).
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the offset to the <see cref="Location"/> point, see appendix/labels.2.png (xo, yo).
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public Point Offset { get; set; }

        /// <summary>
        /// Gets or sets the margin, it represent the space taken by the label to every direction, top, left, bottom and right,see appendix/labels.2.png  [l, t, r, b].
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Margin Margin { get; set; }
    }
}