using System;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data.Builders
{
    /// <summary>
    /// The Charting configuration builder class.
    /// </summary>
    public class LiveChartsConfig
    {
        internal static readonly Dictionary<Type, IChartingTypeBuilder> Builders = 
            new Dictionary<Type, IChartingTypeBuilder>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsConfig"/> class.
        /// </summary>
        public LiveChartsConfig()
        {
            PointFactory = new DefaultDataFactory();
            DefaultLineSeriesFillOpacity = .35d;
            DefaultColumnSeriesFillOpacity = .8d;
            Colors = new List<Color>();
        }

        /// <summary>
        /// Gets the default colors.
        /// </summary>
        /// <value>
        /// The colors.
        /// </value>
        internal List<Color> Colors { get; }

        /// <summary>
        /// Gets the drawing provider.
        /// </summary>
        /// <value>
        /// The drawing provider.
        /// </value>
        public IUiProvider UiProvider { get; set; }

        /// <summary>
        /// Gets or sets the chart point factory.
        /// </summary>
        /// <value>
        /// The chart point factory.
        /// </value>
        public IDataFactory PointFactory { get; set; }

        /// <summary>
        /// Gets or sets the default line series fill opacity.
        /// </summary>
        /// <value>
        /// The default line series fill opacity.
        /// </value>
        public double DefaultLineSeriesFillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the default column series fill opacity.
        /// </summary>
        /// <value>
        /// The default column series fill opacity.
        /// </value>
        public double DefaultColumnSeriesFillOpacity { get; set; }

        /// <summary>
        /// Uses the type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        internal ChartingTypeBuilder<TModel> ConfigureType<TModel>()
        {
            Builders.TryGetValue(typeof(TModel), out IChartingTypeBuilder builder);
            if (builder != null) return (ChartingTypeBuilder<TModel>) builder;
            builder = new ChartingTypeBuilder<TModel>();
            Builders.Add(typeof(TModel), builder);
            return (ChartingTypeBuilder<TModel>)builder;
        }

        /// <summary>
        /// Gets the builder for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="LiveChartsException"></exception>
        internal static IChartingTypeBuilder GetBuilder(Type type)
        {
            Builders.TryGetValue(type, out IChartingTypeBuilder builder);
            if (builder == null)
            {
                throw new LiveChartsException(
                    string.Format(
                        "LiveCharts was not able to find the type '{0}'." +
                        "Ensure it is properly configured calling options.PlotType<{0}>().",
                        type.Name), 100);
            }
            return builder;
        }
    }
}
