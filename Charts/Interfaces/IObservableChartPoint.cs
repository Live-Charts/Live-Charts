namespace LiveCharts
{
    public interface IObservableChartPoint
    {
        SeriesCollection Collection { get; set; }
        void UpdateChart();
    }
}
