using System;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines a model state action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ModelStateEventArgs{TModel, TCoordinate}"/> instance containing the event data.</param>
    public delegate void ModelStateAction<TModel, TCoordinate>(
        TModel sender,
        ModelStateEventArgs<TModel, TCoordinate> args)
        where TCoordinate : ICoordinate;

    /// <summary>
    /// the model state class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The coordinate type.</typeparam>
    public class ModelState<TModel, TCoordinate>
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public Func<TModel, bool> Trigger { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ModelStateAction<TModel, TCoordinate> Action { get; set; }
    }
}