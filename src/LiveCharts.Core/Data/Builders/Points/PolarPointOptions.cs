using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The polar point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IChartingPointBuilder" />
    public class PolarPointOptions<TModel> : IChartingPointBuilder
    {
        /// <summary>
        /// Gets or sets the radius getter.
        /// </summary>
        /// <value>
        /// The radius getter.
        /// </value>
        public Func<TModel, int, double> RadiusGetter { get; set; }

        /// <summary>
        /// Gets or sets the angle getter.
        /// </summary>
        /// <value>
        /// The angle getter.
        /// </value>
        public Func<TModel, int, double> AngleGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index)
        {
            return new PolarChartPoint(
                RadiusGetter((TModel) instance, index),
                AngleGetter((TModel) instance, index));
        }
    }
}