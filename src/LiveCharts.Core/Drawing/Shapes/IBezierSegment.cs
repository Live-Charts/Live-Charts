using System.Drawing;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a bezier segment.
    /// </summary>
    public interface IBezierSegment : IShape
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
        /// <summary>
        /// Gets or sets the point shape.
        /// </summary>
        ISvgPath PointShape { get; }
    }
}
