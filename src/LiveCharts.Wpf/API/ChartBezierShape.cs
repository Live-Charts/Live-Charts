using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Wpf
{
    public class ChartBezierShape : ChartSvgPath, IBezierShape
    {
        private readonly ChartBezierSegment _segment = new ChartBezierSegment();

        public IBezierSegment Segment => _segment;
    }
}
