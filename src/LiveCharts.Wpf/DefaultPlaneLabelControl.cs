using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// a default label for a point.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.TextBlock" />
    public class DefaultPlaneLabelControl : TextBlock, IPlaneLabelControl
    {
        Size IPlaneLabelControl.Measure(string label)
        {
            Text = label;
            return Dispatcher.Invoke(() =>
            {
                Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                return new Size(DesiredSize.Width, DesiredSize.Height);
            });
        }
    }
}
