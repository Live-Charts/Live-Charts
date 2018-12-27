#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
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
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Core.Interaction.Points;

#endregion

namespace LiveCharts.Core.Interaction.Series
{
    /// <summary>
    /// Defines a model to chart point mapping.
    /// </summary>
    public class ModelToCoordinateMapper<TModel, TCoordinate>
        where TCoordinate : ICoordinate
    {
        private List<ModelState<TModel, TCoordinate>> _modelDependentActions = new List<ModelState<TModel, TCoordinate>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelToCoordinateMapper{TModel,TCoordinate}"/> class.
        /// </summary>
        /// <param name="pointPredicate">The point predicate.</param>
        public ModelToCoordinateMapper(
            Func<TModel, int, TCoordinate> pointPredicate)
        {
            PointPredicate = pointPredicate;
        }

        /// <summary>
        /// Gets an empty mapper.
        /// </summary>
        public static ModelToCoordinateMapper<TModel, TCoordinate> Default =>
            new ModelToCoordinateMapper<TModel, TCoordinate>((x, y) =>
                throw new LiveChartsException(100, typeof(TModel).Name, typeof(TCoordinate).Name));

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public Func<TModel, int, TCoordinate> PointPredicate { get; }

        /// <summary>
        /// Whens this instance.
        /// </summary>
        /// <returns></returns>
        public ModelToCoordinateMapper<TModel, TCoordinate> When(
            Func<TModel, bool> trigger,
            ModelStateHandler<TModel, TCoordinate> handler)
        {
            _modelDependentActions.Add(new ModelState<TModel, TCoordinate>(trigger, handler));
            return this;
        }

        /// <summary>
        /// Evaluates models dependent actions.
        /// </summary>
        internal void EvaluateModelDependentActions<TPointShape>(
            TModel model,
            object? visual,
            ChartPoint<TModel, TCoordinate, TPointShape> chartPoint)
            where TPointShape : class, IShape
        {
            if (_modelDependentActions == null) return;

            foreach (ModelState<TModel, TCoordinate> mda in _modelDependentActions)
            {
                if (mda.Trigger(model))
                {
                    mda.Handler(
                        model,
                        new ModelStateEventArgs<TModel, TCoordinate>(
                            visual,
                            chartPoint));
                }
            }
        }
    }
}