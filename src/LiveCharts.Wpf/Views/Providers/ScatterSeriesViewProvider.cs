using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The scatter view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel}" />
    public class ScatterSeriesViewProvider<TModel> : ISeriesViewProvider<TModel, WeightedCoordinate, ScatterViewModel>
    {
        public void OnUpdateStarted(IChartView chart, ISeries series)
        {
        }

        public IPointView<TModel, Point<TModel, WeightedCoordinate, ScatterViewModel>, WeightedCoordinate, ScatterViewModel> Getter()
        {
            return new ScatterPointView<TModel, Point<TModel, WeightedCoordinate, ScatterViewModel>, DataLabel>();
        }

        public void OnUpdateFinished(IChartView chart, ISeries series)
        {
        }
    }
}
