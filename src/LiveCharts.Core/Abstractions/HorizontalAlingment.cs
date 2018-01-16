namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Horizontal label positioning.
    /// </summary>
    public enum HorizontalAlingment
    {
        /// <summary>
        /// Places the label at the center.
        /// </summary>
        Centered,
        /// <summary>
        /// Places the label at the top of the point.
        /// </summary>
        Left,
        /// <summary>
        /// Places the label at the bottom of the point.
        /// </summary>
        Right,
        /// <summary>
        /// Places the label between the center of the point, and the bottom limit of the series.
        /// </summary>
        Between
    }
}