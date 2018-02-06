using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// The pointer moved handler.
    /// </summary>
    /// <param name="location">The location.</param>
    /// <param name="selectionMode">The selection mode.</param>
    /// <param name="dimensions">The dimensions.</param>
    public delegate void PointerMovedHandler(Point location, TooltipSelectionMode selectionMode, params double[] dimensions);
}