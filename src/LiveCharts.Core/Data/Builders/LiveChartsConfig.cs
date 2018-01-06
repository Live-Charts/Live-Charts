using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Data.Builders
{
    /// <summary>
    /// The Charting configuration builder class.
    /// </summary>
    public class LiveChartsConfig
    {
        internal static readonly Dictionary<Type, IChartingTypeBuilder> Builders = 
            new Dictionary<Type, IChartingTypeBuilder>();
        private static readonly Dictionary<string, SeriesMetadata> SeriesConfig =
            new Dictionary<string, SeriesMetadata>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsConfig"/> class.
        /// </summary>
        public LiveChartsConfig()
        {
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
        /// Returns a builder for the specified series type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public SeriesMetadata HasSeriesDefault(string type)
        {
            if (SeriesConfig.TryGetValue(type, out SeriesMetadata builder))
            {
                builder.Type = type;
                return builder;
            }
            builder = new SeriesMetadata();
            SeriesConfig[type] = builder;
            builder.Type = type;
            return builder;
        }

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
