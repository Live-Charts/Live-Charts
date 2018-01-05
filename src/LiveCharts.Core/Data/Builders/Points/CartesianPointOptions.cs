using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The cartesian point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IChartingPointBuilder" />
    public class CartesianPointOptions<TModel> : IChartingPointBuilder
    {
        /// <summary>
        /// Gets or sets the x getter.
        /// </summary>
        /// <value>
        /// The x getter.
        /// </value>
        public Func<TModel, int, double> XGetter { get; set; }

        /// <summary>
        /// Gets or sets the y getter.
        /// </summary>
        /// <value>
        /// The y getter.
        /// </value>
        public Func<TModel, int, double> YGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index)
        {
            return new CartesianChartPoint(XGetter((TModel) instance, index), YGetter((TModel) instance, index));
        }
    }
}