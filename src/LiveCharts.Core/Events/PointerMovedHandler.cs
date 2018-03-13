using System.Drawing;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// The pointer moved handler.
    /// </summary>
    /// <param name="location">The location.</param>
    /// <param name="selectionMode">The selection mode.</param>
    /// <param name="dimensions">The dimensions.</param>
    public delegate void PointerMovedHandler(PointF location, TooltipSelectionMode selectionMode, params double[] dimensions);
}