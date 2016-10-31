using Windows.Foundation;

namespace LiveCharts.Uwp.Points
{
    public static class PointExtensions
    {
        public static void Offset(this Point point, double x, double y)
        {
            point.X += x;
            point.Y += y;
        }
    }
}
