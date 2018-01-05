using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The financial point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IChartingPointBuilder" />
    public class FinancialPointOptions<TModel> : IChartingPointBuilder
    {
        /// <summary>
        /// Gets or sets the open getter.
        /// </summary>
        /// <value>
        /// The open getter.
        /// </value>
        public Func<TModel, int, double> OpenGetter { get; set; }

        /// <summary>
        /// Gets or sets the high getter.
        /// </summary>
        /// <value>
        /// The high getter.
        /// </value>
        public Func<TModel, int, double> HighGetter { get; set; }

        /// <summary>
        /// Gets or sets the low getter.
        /// </summary>
        /// <value>
        /// The low getter.
        /// </value>
        public Func<TModel, int, double> LowGetter { get; set; }

        /// <summary>
        /// Gets or sets the close getter.
        /// </summary>
        /// <value>
        /// The close getter.
        /// </value>
        public Func<TModel, int, double> CloseGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index)
        {
            return new FinancialChartPoint(
                index,
                OpenGetter((TModel) instance, index),
                HighGetter((TModel) instance, index),
                LowGetter((TModel) instance, index),
                CloseGetter((TModel) instance, index));
        }
    }
}