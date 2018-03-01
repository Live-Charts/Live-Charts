using System;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using Point = LiveCharts.Core.Coordinates.Point;

namespace LiveCharts.Core
{
    /// <summary>
    /// LiveCharts configuration class.
    /// </summary>
    public class Charting
    {
        private static readonly Dictionary<Type, object> Builders = new Dictionary<Type, object>();

        private static readonly Dictionary<Tuple<Type, Type>, object> DefaultMappers =
            new Dictionary<Tuple<Type, Type>, object>();

        /// <summary>
        /// Initializes the <see cref="Charting"/> class.
        /// </summary>
        static Charting()
        {
            Current = new Charting();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charting"/> class.
        /// </summary>
        public Charting()
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
        /// Gets the current settings.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static Charting Current { get; }

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

        #region Register types

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
        /// Maps a model to a <see cref="Point"/> and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, Point> For<TModel>(Func<TModel, int, Point> predicate)
        {
            return PlotAs(predicate);
        }

        /// <summary>
        /// Maps a model to a specified point and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TPoint">The type of the point.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, TPoint> For<TModel, TPoint>(Func<TModel, int, TPoint> predicate)
        where TPoint: ICoordinate
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

        #endregion

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
            if (DefaultMappers.TryGetValue(key, out var mapper)) return (ModelToPointMapper<TModel, TCoordinate>) mapper;
            throw new LiveChartsException(
                $"LiveCharts was not able to map from '{modelType.Name}' to " +
                $"'{coordinateType.Name}' ensure you configured properly the type you are trying to plot.", 100);
        }

        /// <summary>
        /// Builds a style for the specified selector based on the corresponding default style.
        /// </summary>
        /// <param name="builder">the builder.</param>
        /// <returns></returns>
        public Charting SetDefault<T>(Action<T> builder)
        {
            Builders[typeof(T)] = builder;
            return Current;
        }

        /// <summary>
        /// Builds the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        public static void BuildFromSettings<T>(T instance)
        {
            if (!Builders.TryGetValue(typeof(T), out var builder)) return;
            var c = (Action<T>) builder;
            c(instance);
        }

        /// <summary>
        /// Configures LiveCharts globally.
        /// </summary>
        /// <param name="options">The builder.</param>
        public static void Settings(Action<Charting> options)
        {
            options(Current);
        }
    }
}
