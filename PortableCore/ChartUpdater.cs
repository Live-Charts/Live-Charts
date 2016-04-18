using System.Diagnostics;

namespace LiveChartsCore
{
    public class ChartUpdater : IChartUpdater
    {
        public ChartUpdater(IChartModel chart)
        {
            Chart = chart;
        }

        public IChartModel Chart { get; private set; }

        public void Run(bool restartsAnimations = false)
        {
            Chart.Update(restartsAnimations);
        }

        public void Cancel()
        {
#if DEBUG
            Debug.WriteLine("Chart update cancelation requested!");
#endif
        }
    }
}
