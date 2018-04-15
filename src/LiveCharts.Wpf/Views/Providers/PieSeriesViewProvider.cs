using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The pie view provider.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Core.Abstractions.ISeriesViewProvider{TModel, PieCoordinate, PieViewModel, TSeries}" />
    public class PieSeriesViewProvider<TModel>
        : ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, IPieSeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> Getter()
        {
            return new PiePointView<TModel, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, IPieSeries series)
        {
        }
    }
}