using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// a default label for a point.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.TextBlock" />
    public class AxisLabel : Label, IPlaneLabelControl
    {
        static AxisLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AxisLabel),
                new FrameworkPropertyMetadata(typeof(AxisLabel)));
        }

        Size IPlaneLabelControl.Measure(string label)
        {
            Content = label;
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size((float) DesiredSize.Width, (float) DesiredSize.Height);
        }
    }
}
