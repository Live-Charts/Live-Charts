namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a shape that contains a bezier segment.
    /// </summary>
    public interface IBezierShape : ISvgPath
    {
        /// <summary>
        /// Gets or sets the bezier segment.
        /// </summary>
        IBezierSegment Segment { get; }
    }
}
