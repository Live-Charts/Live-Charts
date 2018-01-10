using System.Collections.Generic;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart point factory.
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Fetches the data.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TChartPoint">The type of the chart point.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        IEnumerable<TChartPoint> FetchData<TModel, TCoordinate, TViewModel, TChartPoint>(
            DataFactoryArgs<TModel, TCoordinate, TViewModel, TChartPoint> args)
            where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new();
    }
}
