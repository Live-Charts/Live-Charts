using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The bar view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel}" />
    public class BarSeriesViewProvider<TModel, TCoordinate> : ISeriesViewProvider<TModel, TCoordinate, BarViewModel>
        where TCoordinate : ICoordinate
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, ISeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, TCoordinate, BarViewModel>, TCoordinate, BarViewModel> Getter()
        {
            return new BarPointView<TModel, TCoordinate,
                Point<TModel, TCoordinate, BarViewModel>, Rectangle, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, ISeries series)
        {
        }
    }
}