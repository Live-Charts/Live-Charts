using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders.Points
{
    /// <summary>
    /// The weighted point builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class WeightedPointOptions<TModel> : IChartingPointBuilder
    {
        public Func<TModel, int, double> XGetter { get; set; }
        public Func<TModel, int, double> YGetter { get; set; }
        public Func<TModel, int, double> WeightGetter { get; set; }

        /// <inheritdoc cref="IChartingPointBuilder.Build"/>
        ChartPoint IChartingPointBuilder.Build(object instance, int index)
        {
            return new WeightedCartesianChartPoint(
                XGetter((TModel) instance, index),
                YGetter((TModel) instance, index),
                WeightGetter((TModel) instance, index));
        }
    }
}