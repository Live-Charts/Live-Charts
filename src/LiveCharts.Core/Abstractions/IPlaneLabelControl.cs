using System.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a control that is able to measure it's size.
    /// </summary>
    public interface IPlaneLabelControl
    {
        /// <summary>
        /// Measures the specified label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        SizeF Measure(string label);
    }
}