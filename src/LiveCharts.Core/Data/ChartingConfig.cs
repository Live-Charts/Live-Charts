using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// The Charting configuration builder class.
    /// </summary>
    public class ChartingConfig
    {
        private static readonly Dictionary<string, SeriesDefault> SeriesDefaults =
            new Dictionary<string, SeriesDefault>();

        private static readonly Dictionary<Tuple<Type, Type>, object> DefaultMappers =
            new Dictionary<Tuple<Type, Type>, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingConfig"/> class.
        /// </summary>
        public ChartingConfig()
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
        public IDataFactory DataFactory { get; set; }

        /// <summary>
        /// Returns a builder for the specified series unique name.
        /// </summary>
        /// <param name="seriesKey">the series unique name.</param>
        /// <param name="builder">the builder.</param>
        /// <returns></returns>
        public ChartingConfig HasDefaults(string seriesKey, Action<SeriesDefault> builder)
        {
            if (SeriesDefaults.TryGetValue(seriesKey, out SeriesDefault defaults))
            {
                return this;
            }
            var newDefaults = new SeriesDefault(seriesKey);
            builder(newDefaults);
            SeriesDefaults[seriesKey] = newDefaults;
            return this;
        }

        public static SeriesDefault GetDefault(string seriesKey)
        {
            return !SeriesDefaults.TryGetValue(seriesKey, out SeriesDefault defaults)
                ? new SeriesDefault(seriesKey)
                : defaults;
        }

        public Func<TModel, int, TCoordinate> GetGlobalMapper<TModel, TCoordinate>()
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            var key = new Tuple<Type, Type>(modelType, coordinateType);
            if (DefaultMappers.TryGetValue(key, out object mapper)) return (Func<TModel, int, TCoordinate>) mapper;
            throw new LiveChartsException(
                $"LiveCharts was not able to map from '{modelType.Name}' to " +
                $"'{coordinateType.Name}' ensure you configured properly the type you are trying to plot.", 100);
        }

        /// <summary>
        /// Defines a plot map for a given type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">the type of the coordinate.</typeparam>
        /// <returns></returns>
        public ChartingConfig PlotAs<TModel, TCoordinate>(Func<TModel, int, TCoordinate> predicate)
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            var key = new Tuple<Type, Type>(modelType, coordinateType);
            if (DefaultMappers.ContainsKey(key))
            {
                DefaultMappers[key] = predicate;
                return this;
            }
            DefaultMappers.Add(key, predicate);
            return this;
        }

        /// <summary>
        /// Maps a model to a <see cref="Point2D"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ChartingConfig Plot2D<TModel>(Func<TModel, int, Point2D> predicate)
        {
            return PlotAs(predicate);
        }

        public ChartingConfig PlotWeighted2D<TModel>(Func<TModel, int, Weighted2DPoint> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Plots the financial.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ChartingConfig PlotFinancial<TModel>(Func<TModel, int, FinancialPoint> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Maps a model to a <see cref="PolarPoint"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ChartingConfig PlotPolar<TModel>(Func<TModel, int, PolarPoint> predicate)
        {
            return PlotAs(predicate);
        }
    }
}
