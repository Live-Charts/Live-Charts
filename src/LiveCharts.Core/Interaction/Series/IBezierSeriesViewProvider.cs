using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Controls;

namespace LiveCharts.Core.Interaction.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="LiveCharts.Core.Interaction.Series.IBezierSeriesViewProvider{TModel, TCoordinate, TViewModel, TSeries}" />
    public interface IBezierSeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries>
        : ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : ISeries
    {
        /// <summary>
        /// Gets a new path instance.
        /// </summary>
        /// <returns></returns>
        ICartesianPath GetNewPath();
    }
}