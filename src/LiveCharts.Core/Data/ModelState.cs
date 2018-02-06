using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.Data
{
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
        public ModelStateHandler<TModel, TCoordinate> Handler { get; set; }
    }
}