using System.Collections.Generic;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// A handler related with user interaction with a chart point.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="points">The points the user is interacting with.</param>
    public delegate void DataInteractionHandler(object sender, IEnumerable<PackedPoint> points);
}