using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The scatter view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel}" />
    public class GeometryPointSeriesViewProvider<TModel, TCoordinate>
        : ISeriesViewProvider<TModel, TCoordinate, GeometryPointViewModel>
        where TCoordinate : ICoordinate
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, ISeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, TCoordinate, GeometryPointViewModel>, TCoordinate,
            GeometryPointViewModel> Getter()
        {
            return new GeometryPointView<TModel, TCoordinate,
                Point<TModel, TCoordinate, GeometryPointViewModel>, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, ISeries series)
        {
        }
    }
}
