using LiveCharts.Dimensions;

namespace LiveCharts.Interaction.Events
{
    /// <summary>
    /// The plane range changed event.
    /// </summary>
    /// <param name="plane">The plane.</param>
    /// <param name="previousMin">The previous min limit of the axis.</param>
    /// /// <param name="previousMax">The previous max limit of the axis.</param>
    public delegate void RangeChangedEventHandler(Plane plane, double previousMin, double previousMax);
}