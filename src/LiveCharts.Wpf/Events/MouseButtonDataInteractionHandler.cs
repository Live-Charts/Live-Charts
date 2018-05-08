using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Points;

namespace LiveCharts.Wpf.Events
{
    /// <summary>
    /// The mouse button data interaction handler.
    /// </summary>
    /// <param name="chart">The chart.</param>
    /// <param name="interactedPoints">The interacted points.</param>
    /// <param name="args">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
    public delegate void MouseButtonDataInteractionHandler(
        IChartView chart, IChartPoint[] interactedPoints, MouseButtonEventArgs args);
}
