using System;
using LiveCharts.Core.Data;

namespace LiveCharts.Core
{
    /// <summary>
    /// LiveCharts configuration class.
    /// </summary>
    public static class LiveCharts
    {
        static LiveCharts()
        {
            Options = new ChartingConfig();
        }

        internal static ChartingConfig Options { get; }

        /// <summary>
        /// Configures LiveCharts globally.
        /// </summary>
        /// <param name="options">The builder.</param>
        public static void Config(Action<ChartingConfig> options)
        {
            options(Options);
        }
    }
}
