using System;
using System.Reflection.Emit;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Builders.Points;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data.Builders
{
    /// <summary>
    /// The charting type builder class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ChartingTypeBuilder<TModel> : IChartingTypeBuilder
    {
        private CartesianPointOptions<TModel> _cartesianPointOptions;
        private FinancialPointOptions<TModel> _financialPointOptions;
        private PiePointOptions<TModel> _piePointOptions;
        private PolarPointOptions<TModel> _polarPointOptions;
        private WeightedPointOptions<TModel> _weightedPointOptions;

        /// <summary>
        /// Gets the cartesian point builder.
        /// </summary>
        /// <value>
        /// The cartesian point builder.
        /// </value>
        private CartesianPointOptions<TModel> CartesianPointOptions
        {
            get
            {
                if (_cartesianPointOptions == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsCartesian)), 100);
                }
                return _cartesianPointOptions;
            }
        }

        /// <summary>
        /// Gets the financial point builder.
        /// </summary>
        /// <value>
        /// The financial point builder.
        /// </value>
        private FinancialPointOptions<TModel> FinancialPointOptions
        {
            get
            {
                if (_financialPointOptions == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsFinancial)), 100);
                }
                return _financialPointOptions;
            }
        }

        /// <summary>
        /// Gets the pie point builder.
        /// </summary>
        /// <value>
        /// The pie point builder.
        /// </value>
        private PiePointOptions<TModel> PiePointOptions
        {
            get
            {
                if (_piePointOptions == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsPie)), 100);
                }
                return _piePointOptions;
            }
        }

        /// <summary>
        /// Gets the polar point builder.
        /// </summary>
        /// <value>
        /// The polar point builder.
        /// </value>
        private PolarPointOptions<TModel> PolarPointOptions
        {
            get
            {
                if (_polarPointOptions == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsPolar)), 100);
                }
                return _polarPointOptions;
            }
        }

        /// <summary>
        /// Gets the weighted point builder.
        /// </summary>
        /// <value>
        /// The weighted point builder.
        /// </value>
        private WeightedPointOptions<TModel> WeightedPointOptions
        {
            get
            {
                 if (_weightedPointOptions == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsWeighted)), 100);
                }
                return _weightedPointOptions;
            }
        }

        /// <summary>
        /// Configures a type for cartesian series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsCartesian(Action<CartesianPointOptions<TModel>> options)
        {
            _cartesianPointOptions = _cartesianPointOptions ?? new CartesianPointOptions<TModel>();
            options(_cartesianPointOptions);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsFinancial(Action<FinancialPointOptions<TModel>> options)
        {
            _financialPointOptions = _financialPointOptions ?? new FinancialPointOptions<TModel>();
            options(_financialPointOptions);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsPie(Action<PiePointOptions<TModel>> options)
        {
            _piePointOptions = _piePointOptions ?? new PiePointOptions<TModel>();
            options(_piePointOptions);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsPolar(Action<PolarPointOptions<TModel>> options)
        {
            _polarPointOptions = _polarPointOptions ?? new PolarPointOptions<TModel>();
            options(_polarPointOptions);
            return this;
        }

        /// <summary>
        /// Uses a type for weighted series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsWeighted(Action<WeightedPointOptions<TModel>> options)
        {
            _weightedPointOptions = _weightedPointOptions ?? new WeightedPointOptions<TModel>();
            options(_weightedPointOptions);
            return this;
        }

        /// <inheritdoc cref="IChartingTypeBuilder.GetBuilder"/>
        public IChartingPointBuilder GetBuilder(ChartPointTypes pointType)
        {
            switch (pointType)
            {
                case ChartPointTypes.Cartesian:
                    return CartesianPointOptions;
                case ChartPointTypes.Financial:
                    return FinancialPointOptions;
                case ChartPointTypes.Pie:
                    return PiePointOptions;
                case ChartPointTypes.Polar:
                    return PolarPointOptions;
                case ChartPointTypes.StackedCartesian:
                    throw new NotImplementedException();
                case ChartPointTypes.Weighted:
                    return WeightedPointOptions;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pointType), pointType, null);
            }
        }

        private static string GetErrorHelp(string methodName)
        {
            return string.Format(
                "LiveCharts could not found a valid way to plot the type '{0}' at" +
                "a cartesian series, ensure '{0}' is configured to be used with " +
                "cartesian series calling Charting.For<{0}>()." + methodName + "().",
                typeof(TModel).Name);
        }
    }
}
