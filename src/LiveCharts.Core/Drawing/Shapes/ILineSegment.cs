namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a line segment.
    /// </summary>
    public interface ILineSegment: IPathSegment
    {
        /// <summary>
        /// Gets or sets the reference point.
        /// </summary>
        PointD Point { get; set; }
    }
}
