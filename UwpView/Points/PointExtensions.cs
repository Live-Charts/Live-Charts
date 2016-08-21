using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
