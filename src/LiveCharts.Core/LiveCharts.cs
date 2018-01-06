using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Builders;

namespace LiveCharts.Core
{
    /// <summary>
    /// LiveCharts configuration class.
    /// </summary>
    public static class LiveCharts
    {
        static LiveCharts()
        {
            Options = new LiveChartsConfig();
        }

        /// <summary>
        /// Gets the constants.
        /// </summary>
        /// <value>
        /// The constants.
        /// </value>
        public static Constants Constants => new Constants();

        internal static LiveChartsConfig Options { get; }

        internal static IChartingTypeBuilder GetBuilder(Type type)
        {
            return LiveChartsConfig.GetBuilder(type);
        }

        /// <summary>
        /// Configures LiveCharts globally.
        /// </summary>
        /// <param name="options">The builder.</param>
        public static void Config(Action<LiveChartsConfig> options)
        {
            options(Options);
        }
    }
}
