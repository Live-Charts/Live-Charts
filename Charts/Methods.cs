using System.Windows;
using Charts.Charts;

namespace Charts
{
    /// <summary>
    /// Dont repeat yourself!
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisTags axis, Chart chart)
        {
            //y = m (x - x1) + y1
            var p1 = axis == AxisTags.X
                ? new Point(chart.Max.X, chart.PlotArea.Width + chart.PlotArea.X)
                : new Point(chart.Max.Y, chart.PlotArea.Y);
            var p2 = axis == AxisTags.X
                ? new Point(chart.Min.X, chart.PlotArea.X)
                : new Point(chart.Min.Y, chart.PlotArea.Y + chart.PlotArea.Height);
            var m = (p2.Y - p1.Y) / (p2.X - p1.X);
            return m * (value - p1.X) + p1.Y;
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static Point ToPlotArea(Point value, Chart chart)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X, chart), ToPlotArea(value.Y, AxisTags.Y, chart));
        }
    }
}