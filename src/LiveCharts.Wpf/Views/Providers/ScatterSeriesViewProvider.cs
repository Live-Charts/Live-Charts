using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The scatter view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel, TSeries}" />
    public class GeometryPointSeriesViewProvider<TModel, TCoordinate, TSeries>
        : ISeriesViewProvider<TModel, TCoordinate, GeometryPointViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : IStrokeSeries, ICartesianSeries
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, TSeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel,  TCoordinate, GeometryPointViewModel, TSeries> Getter()
        {
            return new GeometryPointView<TModel, TCoordinate, TSeries, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, TSeries series)
        {
        }
    }
}
