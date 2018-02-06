using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// Defines a model state action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ModelStateEventArgs{TModel, TCoordinate}"/> instance containing the event data.</param>
    public delegate void ModelStateHandler<TModel, TCoordinate>(
        TModel sender,
        ModelStateEventArgs<TModel, TCoordinate> args)
        where TCoordinate : ICoordinate;
}