using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The bezier view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel, ILineSeries}" />
    public class BezierSeriesViewProvider<TModel> 
        : ISeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, ILineSeries series)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, PointCoordinate, BezierViewModel, ILineSeries> Getter()
        {
            return new BezierPointView<TModel, DataLabel>();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, ILineSeries series)
        {
        }
    }
}