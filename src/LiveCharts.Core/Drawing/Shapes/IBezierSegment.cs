namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a bezier segment.
    /// </summary>
    public interface IBezierSegment : IPathSegment
    {
        /// <summary>
        /// Gets or sets the Point 1 vertex.
        /// </summary>
        PointD Point1 { get; set; }
        /// <summary>
        /// Gets or sets the Point 1 vertex.
        /// </summary>
        PointD Point2 { get; set; }
        /// <summary>
        /// Gets or sets the Point 1 vertex.
        /// </summary>
        PointD Point3 { get; set; }
    }
}
