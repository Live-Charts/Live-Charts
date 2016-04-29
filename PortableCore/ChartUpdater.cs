using System.Collections;
using System.Diagnostics;
using System.Linq;

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
            if (!Chart.View.IsControlLoaded || Chart.Series == null) return;

            foreach (var series in Chart.Series)
            {
                InitializeSeriesParams(series);
                series.InitializeView();
                series.Model.Values.GetLimits();
            }

            Chart.PrepareAxes();

            foreach (var series in Chart.Series)
            {
                series.Model.Values.InitializeGarbageCollector();
                series.Model.Update();
                series.CloseView();
                series.Model.Values.CollectGarbage();
            }
#if DEBUG
            Debug.WriteLine("<<Chart UI Updated>>");
#endif
        }

        private void InitializeSeriesParams(ISeriesView seriesView)
        {
            seriesView.Model.Chart = Chart;
            seriesView.Model.Values.Series = seriesView.Model;
            seriesView.Model.SeriesCollection = Chart.Series;
        }

        public void Cancel()
        {
#if DEBUG
            Debug.WriteLine("Chart update cancelation requested!");
#endif
        }
    }
}