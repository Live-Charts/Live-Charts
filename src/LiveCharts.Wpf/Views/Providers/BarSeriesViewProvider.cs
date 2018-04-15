using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The bar view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel, IBarSeries}" />
    public class BarSeriesViewProvider<TModel, TCoordinate, TSeries> 
        : ISeriesViewProvider<TModel, TCoordinate, BarViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : ICartesianSeries, IStrokeSeries
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, TSeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, TCoordinate, BarViewModel, TSeries> Getter()
        {
            return new BarPointView<TModel, TCoordinate, TSeries, Rectangle, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, TSeries series)
        {
        }
    }
}