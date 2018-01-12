using System.Collections.Generic;
using LiveCharts.Core.Data;

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
        /// <typeparam name="TPoint">The type of the chart point.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        IEnumerable<TPoint> FetchData<TModel, TCoordinate, TViewModel, TPoint>(
            DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint> args)
            where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
            where TCoordinate : ICoordinate;
    }
}
