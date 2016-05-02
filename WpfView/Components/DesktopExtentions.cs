using System.Windows;

namespace LiveCharts.Wpf.Components
{
    internal static class DesktopExtentions
    {
        public static Point AsPoint(this LvcPoint point)
        {
            return new Point(point.X, point.Y);
        }
    }
}