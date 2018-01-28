using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a control that can be used as a data label.
    /// </summary>
    public interface IDataLabelControl
    {
        /// <summary>
        /// Measures the control with the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        Size Measure(PackedPoint point);
    }
}