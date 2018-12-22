using LiveCharts.Core.Drawing.Shapes;

namespace LiveCharts.Core.Drawing
{
    public interface ISlice : IShape
    {
        double InnerRadius { get; set; }
        double Radius { get; set; }
        bool ForceAngle { get; set; } 
        double CornerRadius { get; set; }
        double PushOut { get; set; }
        float Rotation { get; set; }
        float Wedge { get; set; }
    }
}
