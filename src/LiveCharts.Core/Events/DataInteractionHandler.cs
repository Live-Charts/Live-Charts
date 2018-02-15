using LiveCharts.Core.Data;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// A handler related with user interaction with a chart point.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="point">The point.</param>
    public delegate void DataInteractionHandler(object sender, PackedPoint point);
}