using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;

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

        SizeF IPlaneLabelControl.Measure(string label)
        {
            Content = label;
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new SizeF((float) DesiredSize.Width, (float) DesiredSize.Height);
        }
    }
}
