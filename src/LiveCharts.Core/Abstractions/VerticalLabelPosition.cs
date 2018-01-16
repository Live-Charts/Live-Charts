namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Vertical label positioning.
    /// </summary>
    public enum VerticalLabelPosition
    {
        /// <summary>
        /// Places the label at the center.
        /// </summary>
        Centered,
        /// <summary>
        /// Places the label at the top of the point.
        /// </summary>
        Top,
        /// <summary>
        /// Places the label at the bottom of the point.
        /// </summary>
        Bottom,
        /// <summary>
        /// Places the label between the center of the point, and the bottom limit of the series.
        /// </summary>
        Between
    }
}