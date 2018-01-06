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
        private CartesianPointBuilder<TModel> _cartesianPointBuilder;
        private FinancialPointBuilder<TModel> _financialPointBuilder;
        private PiePointBuilder<TModel> _piePointBuilder;
        private PolarPointBuilder<TModel> _polarPointBuilder;
        private WeightedPointBuilder<TModel> _builder;

        /// <summary>
        /// Gets the cartesian point builder.
        /// </summary>
        /// <value>
        /// The cartesian point builder.
        /// </value>
        private CartesianPointBuilder<TModel> CartesianPointBuilder
        {
            get
            {
                if (_cartesianPointBuilder == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsCartesian)), 100);
                }
                return _cartesianPointBuilder;
            }
        }

        /// <summary>
        /// Gets the financial point builder.
        /// </summary>
        /// <value>
        /// The financial point builder.
        /// </value>
        private FinancialPointBuilder<TModel> FinancialPointBuilder
        {
            get
            {
                if (_financialPointBuilder == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsFinancial)), 100);
                }
                return _financialPointBuilder;
            }
        }

        /// <summary>
        /// Gets the pie point builder.
        /// </summary>
        /// <value>
        /// The pie point builder.
        /// </value>
        private PiePointBuilder<TModel> PiePointBuilder
        {
            get
            {
                if (_piePointBuilder == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsPie)), 100);
                }
                return _piePointBuilder;
            }
        }

        /// <summary>
        /// Gets the polar point builder.
        /// </summary>
        /// <value>
        /// The polar point builder.
        /// </value>
        private PolarPointBuilder<TModel> PolarPointBuilder
        {
            get
            {
                if (_polarPointBuilder == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsPolar)), 100);
                }
                return _polarPointBuilder;
            }
        }

        /// <summary>
        /// Gets the weighted point builder.
        /// </summary>
        /// <value>
        /// The weighted point builder.
        /// </value>
        private WeightedPointBuilder<TModel> Builder
        {
            get
            {
                 if (_builder == null)
                {
                    throw new LiveChartsException(GetErrorHelp(nameof(AsWeighted)), 100);
                }
                return _builder;
            }
        }

        /// <summary>
        /// Configures a type for cartesian series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsCartesian(Action<CartesianPointBuilder<TModel>> options)
        {
            _cartesianPointBuilder = _cartesianPointBuilder ?? new CartesianPointBuilder<TModel>();
            options(_cartesianPointBuilder);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsFinancial(Action<FinancialPointBuilder<TModel>> options)
        {
            _financialPointBuilder = _financialPointBuilder ?? new FinancialPointBuilder<TModel>();
            options(_financialPointBuilder);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsPie(Action<PiePointBuilder<TModel>> options)
        {
            _piePointBuilder = _piePointBuilder ?? new PiePointBuilder<TModel>();
            options(_piePointBuilder);
            return this;
        }

        /// <summary>
        /// Uses a type for financial series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsPolar(Action<PolarPointBuilder<TModel>> options)
        {
            _polarPointBuilder = _polarPointBuilder ?? new PolarPointBuilder<TModel>();
            options(_polarPointBuilder);
            return this;
        }

        /// <summary>
        /// Uses a type for weighted series.
        /// </summary>
        /// <param name="options">The builder.</param>
        /// <returns></returns>
        public ChartingTypeBuilder<TModel> AsWeighted(Action<WeightedPointBuilder<TModel>> options)
        {
            _builder = _builder ?? new WeightedPointBuilder<TModel>();
            options(_builder);
            return this;
        }

        /// <inheritdoc cref="IChartingTypeBuilder.GetBuilder"/>
        public IChartingPointBuilder GetBuilder(ChartPointTypes pointType)
        {
            switch (pointType)
            {
                case ChartPointTypes.Cartesian:
                    return CartesianPointBuilder;
                case ChartPointTypes.Financial:
                    return FinancialPointBuilder;
                case ChartPointTypes.Pie:
                    return PiePointBuilder;
                case ChartPointTypes.Polar:
                    return PolarPointBuilder;
                case ChartPointTypes.StackedCartesian:
                    throw new NotImplementedException();
                case ChartPointTypes.Weighted:
                    return Builder;
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
