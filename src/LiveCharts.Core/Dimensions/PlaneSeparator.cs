using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Events;

namespace LiveCharts.Core.Dimensions
{
    internal class PlaneSeparator : IResource
    {
        public IPlaneSeparatorView View { get; set; }

        public event DisposingResourceHandler Disposed;

        public object UpdateId { get; set; }

        public void Dispose(IChartView view)
        {
            View?.Dispose(view);
            View = null;
        }
    }
}