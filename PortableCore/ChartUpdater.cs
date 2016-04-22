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
            foreach (var series in Chart.Series)
            {
                series.Model.Update();
            }
#if DEBUG
            Debug.WriteLine("<<Chart UI Updated>>");
#endif
        }

        public void Cancel()
        {
#if DEBUG
            Debug.WriteLine("Chart update cancelation requested!");
#endif
        }
    }
}
