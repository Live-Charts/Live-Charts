using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The pie point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IChartingPointBuilder" />
    public class PiePointBuilder<TModel> : IChartingPointBuilder
    {
        /// <summary>
        /// Gets or sets the value getter.
        /// </summary>
        /// <value>
        /// The value getter.
        /// </value>
        public Func<TModel, int, double> ValueGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index, object createdAtUpdate)
        {
            return new PieChartPoint(
                index, 
                ValueGetter((TModel) instance, index), 
                createdAtUpdate);
        }
    }
}