#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodr�guez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;

#endregion

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
            Settings = new Charting();
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
        public static Charting Settings { get; }

        /// <summary>
        /// Gets or sets the chart point factory.
        /// </summary>
        /// <value>
        /// The chart point factory.
        /// </value>
        public IDataFactory DataFactory { get; set; }

        /// <summary>
        /// Gets or sets the UI provider.
        /// </summary>
        /// <value>
        /// The UI provider.
        /// </value>
        public IUiProvider UiProvider { get; set; }

        #region Register types

        /// <summary>
        /// Defines a plot map for a given type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">the type of the coordinate.</typeparam>
        /// <returns></returns>
        private ModelToCoordinateMapper<TModel, TCoordinate> Learn<TModel, TCoordinate>(
            Func<TModel, int, TCoordinate> pointPredicate)
            where TCoordinate : ICoordinate
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            Tuple<Type, Type> key = new Tuple<Type, Type>(modelType, coordinateType);
            ModelToCoordinateMapper<TModel, TCoordinate> modelToPoint =
                new ModelToCoordinateMapper<TModel, TCoordinate>(pointPredicate);

            if (DefaultMappers.ContainsKey(key))
            {
                DefaultMappers[key] = modelToPoint;
                return modelToPoint;
            }

            DefaultMappers.Add(key, modelToPoint);
            return modelToPoint;
        }

        /// <summary>
        /// Maps a model to a specified point and saves the mapper globally.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TPoint">The type of the point.</typeparam>
        /// <param name="predicate">The point predicate.</param>
        /// <returns></returns>
        public ModelToCoordinateMapper<TModel, TPoint> LearnType<TModel, TPoint>(
            Func<TModel, int, TPoint> predicate)
            where TPoint: ICoordinate
        {
            return Learn(predicate);
        }

        #endregion

        /// <summary>
        /// Gets the default mapper for the pair TModel, TCoordinate.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <returns></returns>
        public static ModelToCoordinateMapper<TModel, TCoordinate> GetCurrentMapperFor<TModel, TCoordinate>()
            where TCoordinate : ICoordinate
        {
            var modelType = typeof(TModel);
            var coordinateType = typeof(TCoordinate);
            Tuple<Type, Type> key = new Tuple<Type, Type>(modelType, coordinateType);
            if (DefaultMappers.TryGetValue(key, out var mapper)) return (ModelToCoordinateMapper<TModel, TCoordinate>) mapper;
            throw new LiveChartsException(100, modelType.Name, coordinateType.Name);
        }

        /// <summary>
        /// Builds a style for the specified selector based on the corresponding default style.
        /// </summary>
        /// <param name="builder">the builder.</param>
        /// <returns></returns>
        public Charting SetDefault<T>(Action<T> builder)
        {
            Builders[typeof(T)] = builder;
            return Settings;
        }

        /// <summary>
        /// Builds the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        public static void BuildFromTheme<T>(T instance)
        {
            if (!Builders.TryGetValue(typeof(T), out var builder)) return;
            Action<T> builderForT = (Action<T>) builder;
            builderForT(instance);
        }

        /// <summary>
        /// Configures LiveCharts globally.
        /// </summary>
        /// <param name="options">The builder.</param>
        public static void Configure(Action<Charting> options)
        {
            options(Settings);
        }
    }
}
