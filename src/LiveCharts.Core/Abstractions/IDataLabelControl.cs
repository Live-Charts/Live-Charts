using System.Drawing;
using LiveCharts.Core.Data;

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
        SizeF Measure(PackedPoint point);
    }
}