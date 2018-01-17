using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core
{
    /// <summary>
    /// LiveCharts configuration class.
    /// </summary>
    public class LiveChartsSettings
    {
        private static readonly Dictionary<string, SeriesDefault> SeriesDefaults =
            new Dictionary<string, SeriesDefault>();

        private static readonly Dictionary<Tuple<Type, Type>, object> DefaultMappers =
            new Dictionary<Tuple<Type, Type>, object>();

        static LiveChartsSettings()
        {
            Current = new LiveChartsSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsSettings"/> class.
        /// </summary>
        public LiveChartsSettings()
        {
            Colors = new List<Color>();
        }

        /// <summary>
        /// Gets the current settings.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static LiveChartsSettings Current { get; }

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
        public LiveChartsSettings Series(string seriesKey, Action<SeriesDefault> builder)
        {
            if (SeriesDefaults.ContainsKey(seriesKey))
            {
                return Current;
            }
            var newDefaults = new SeriesDefault(seriesKey);
            builder(newDefaults);
            SeriesDefaults[seriesKey] = newDefaults;
            return Current;
        }

        /// <summary>
        /// Defines a plot map for a given type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">the type of the coordinate.</typeparam>
        /// <returns></returns>
        public ModelToPointMapper<TModel, TCoordinate> PlotAs<TModel, TCoordinate>(
            Func<TModel, int, TCoordinate> predicate)
            where TCoordinate : ICoordinate
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            var key = new Tuple<Type, Type>(modelType, coordinateType);
            // ReSharper disable once InconsistentNaming
            var m2p = new ModelToPointMapper<TModel, TCoordinate>(predicate);
            if (DefaultMappers.ContainsKey(key))
            {
                DefaultMappers[key] = m2p;
                return m2p;
            }
            DefaultMappers.Add(key, m2p);
            return m2p;
        }

        /// <summary>
        /// Maps a model to a <see cref="Point2D"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, Point2D> Has2DPlotFor<TModel>(Func<TModel, int, Point2D> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Maps a model to a <see cref="Weighted2DPoint"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, Weighted2DPoint> PlotWeighted2D<TModel>(
            Func<TModel, int, Weighted2DPoint> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Plots the financial.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, FinancialPoint> PlotFinancial<TModel>(
            Func<TModel, int, FinancialPoint> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Maps a model to a <see cref="PolarPoint"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, PolarPoint> PlotPolar<TModel>(Func<TModel, int, PolarPoint> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Gets the default mapper for the pair TModel, TCoordinate.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <returns></returns>
        public static ModelToPointMapper<TModel, TCoordinate> GetCurrentMapperFor<TModel, TCoordinate>()
            where TCoordinate : ICoordinate
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            var key = new Tuple<Type, Type>(modelType, coordinateType);
            if (DefaultMappers.TryGetValue(key, out object mapper)) return (ModelToPointMapper<TModel, TCoordinate>) mapper;
            throw new LiveChartsException(
                $"LiveCharts was not able to map from '{modelType.Name}' to " +
                $"'{coordinateType.Name}' ensure you configured properly the type you are trying to plot.", 100);
        }

        /// <summary>
        /// Gets the default for a given series.
        /// </summary>
        /// <param name="seriesKey">The series key.</param>
        /// <returns></returns>
        public static SeriesDefault GetSeriesDefault(string seriesKey)
        {
            return !SeriesDefaults.TryGetValue(seriesKey, out SeriesDefault defaults)
                ? new SeriesDefault(seriesKey)
                : defaults;
        }

        /// <summary>
        /// Configures LiveCharts globally.
        /// </summary>
        /// <param name="settings">The builder.</param>
        public static void Define(Action<LiveChartsSettings> settings)
        {
            settings(Current);
        }
    }
}
