using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Points;

namespace LiveCharts.Wpf.Events
{
    /// <summary>
    /// The mouse data interaction handler.
    /// </summary>
    /// <param name="chart">The chart.</param>
    /// <param name="interactedPoints">The interacted points.</param>
    /// <param name="arg">The <see cref="System.Windows.Input.MouseEventArgs" /> instance containing the event data.</param>
    public delegate void MouseDataInteractionHandler(
        IChartView chart, IChartPoint[] interactedPoints, MouseEventArgs arg);
}