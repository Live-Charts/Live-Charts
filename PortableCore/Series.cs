
using System.Linq;

namespace LiveChartsCore
{
    public class Series : ISeriesModel
    {
        public Series()
        {
            Intialize();
        }

        public Series(SeriesConfiguration configuration)
        {
            Intialize();
        }

        public ISeriesView View { get; set; }
        public IChartModel Chart { get; set; }
        public IChartValues Values { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string Title { get; set; }

        private void Intialize()
        {
            Chart = new Chart();
        }
    }

    public class Chart : IChartModel
    {
        public IChartView View { get; set; }
        public SeriesCollection Series { get; set; }

        public void Update(bool restartAnimations = true)
        {
            foreach (var point in Series
                .SelectMany(series => series.Values.Points))
                point.UpdateView();
        }
    }

    public class DataStep
    {
        public bool IsRunning { get; set; }
        public bool RestartsAnimations { get; set; }

        public void Reset()
        {
            IsRunning = false;
            RestartsAnimations = false;
        }
    }
}
