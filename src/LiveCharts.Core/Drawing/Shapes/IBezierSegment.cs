using System.Drawing;

namespace LiveCharts.Core.Drawing.Shapes
{
    public interface IBezierSegment : IShape
    {
        PointD Point1 { get; set; }
        PointD Point2 { get; set; }
        PointD Point3 { get; set; }
        ISvgPath PointShape { get; }
    }
}
