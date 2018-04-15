using LiveCharts.Core.Abstractions.DataSeries;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// The series UI provider class.
    /// </summary>
    public interface ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : ISeries
    {
        /// <summary>
        /// Called when the series update starts.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateStarted(IChartView chart, TSeries series);

        /// <summary>
        /// Called when LiveCharts requires a new point in the UI.
        /// </summary>
        /// <returns></returns>
        IPointView<TModel, TCoordinate, TViewModel, TSeries> Getter();

        /// <summary>the series update finishes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateFinished(IChartView chart, TSeries series);
    }
}