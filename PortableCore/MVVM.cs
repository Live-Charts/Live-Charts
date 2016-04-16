namespace LiveChartsCore
{
    public interface ISeriesModel
    {
        ISeriesView View { get; set; }
        IChartModel Chart { get; set; }
        IChartValues Values { get; set; }
        SeriesCollection SeriesCollection { get; set; }
        string Title { get; set; }
    }

    public interface ISeriesView
    {
        ISeriesModel Model { get; set; }
    }

    public interface IChartModel
    {
        IChartView View { get; set; }
        SeriesCollection Series { get; set; }

        void Update(bool restartAnimations = true);
    }

    public interface IChartView
    {
        IChartModel Model { get; }
    }
}