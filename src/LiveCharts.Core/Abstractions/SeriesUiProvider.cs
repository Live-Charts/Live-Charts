using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.DataSeries.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// The series UI provider class.
    /// </summary>
    public interface ISeriesViewProvider<TModel, TCoordinate, TViewModel>
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Called when the series update starts.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateStarted(IChartView chart, ISeries series);

        /// <summary>
        /// Called when LiveCharts requires a new point in the UI.
        /// </summary>
        /// <returns></returns>
        IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel> Getter();

        /// <summary>the series update finishes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateFinished(IChartView chart, ISeries series);
    }
}