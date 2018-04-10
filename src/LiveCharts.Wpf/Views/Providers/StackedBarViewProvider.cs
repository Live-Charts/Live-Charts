using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The stacked bar view provider.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Core.Abstractions.ISeriesViewProvider{TModel, StackedCoordinate, BarViewModel}" />
    public class StackedBarViewProvider<TModel> : ISeriesViewProvider<TModel, StackedCoordinate, BarViewModel>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, ISeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, StackedCoordinate, BarViewModel>, StackedCoordinate, BarViewModel> Getter()
        {
            return new BarPointView<TModel, StackedCoordinate,
                Point<TModel, StackedCoordinate, BarViewModel>, Rectangle, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, ISeries series)
        {
        }
    }
}