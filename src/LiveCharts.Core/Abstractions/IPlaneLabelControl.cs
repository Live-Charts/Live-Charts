using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a control that is able to measure it's size.
    /// </summary>
    public interface IPlaneLabelControl
    {
        Size Measure(string label);
    }
}