using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The weighted point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class WeightedPointBuilder<TModel> : IChartingPointBuilder
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

        /// <summary>
        /// Gets or sets the weight getter.
        /// </summary>
        /// <value>
        /// The weight getter.
        /// </value>
        public Func<TModel, int, double> WeightGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index, object createdAtUpdate)
        {
            return new WeightedCartesianChartPoint(
                XGetter((TModel) instance, index),
                YGetter((TModel) instance, index),
                WeightGetter((TModel) instance, index),
                createdAtUpdate);
        }
    }
}