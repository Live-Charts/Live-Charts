using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.Views;

namespace LiveCharts.Wpf.Providers
{
    /// <summary>
    /// The scatter view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel}" />
    public class ScatterSeriesViewProvider<TModel> : ISeriesViewProvider<TModel, WeightedPoint, ScatterViewModel>
    {
        public void OnUpdateStarted(IChartView chart, ISeries series)
        {
        }

        public IPointView<TModel, Point<TModel, WeightedPoint, ScatterViewModel>, WeightedPoint, ScatterViewModel> GetNewPointView()
        {
            return new ScatterPointView<TModel, Point<TModel, WeightedPoint, ScatterViewModel>, DataLabel>();
        }

        public void OnUpdateFinished(IChartView chart, ISeries series)
        {
        }
    }
}
