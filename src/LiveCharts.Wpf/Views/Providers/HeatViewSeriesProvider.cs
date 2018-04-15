using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The heat view series provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Core.Abstractions.ISeriesViewProvider{TModel, WeightedCoordinate, HeatViewModel, IHeatSeries}" />
    public class HeatViewSeriesProvider<TModel>
        : ISeriesViewProvider<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, IHeatSeries series)
        { 
        }

        /// <inheritdoc />
        public IPointView<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> Getter()
        {
            return new HeatPointView<TModel, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, IHeatSeries series)
        {
        }
    }
}
