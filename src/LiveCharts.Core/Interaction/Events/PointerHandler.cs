using System;
using System.Drawing;

namespace LiveCharts.Interaction.Events
{
    /// <summary>
    /// The pointer handler.
    /// </summary>
    /// <param name="pointerLocation">The pointer location.</param>
    /// <param name="eventArgs">The event args.</param>
    public delegate void PointerHandler(PointF pointerLocation, EventArgs eventArgs);
}