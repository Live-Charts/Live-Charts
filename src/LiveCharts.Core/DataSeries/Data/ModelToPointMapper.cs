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
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Events;

#endregion

namespace LiveCharts.Core.DataSeries.Data
{
    /// <summary>
    /// Defines a model to chart point mapping.
    /// </summary>
    public class ModelToPointMapper<TModel, TCoordinate>
        where TCoordinate : ICoordinate
    {
        private List<ModelState<TModel, TCoordinate>> _modelDependentActions;
        private readonly Dictionary<UiActions, List<ModelStateHandler<TModel, TCoordinate>>> _userUiActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelToPointMapper{TModel,TCoordinate}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public ModelToPointMapper(Func<TModel, int, TCoordinate> predicate)
        {
            Predicate = predicate;
            _userUiActions = new Dictionary<UiActions, List<ModelStateHandler<TModel, TCoordinate>>>();
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public Func<TModel, int, TCoordinate> Predicate { get; }

        /// <summary>
        /// Gets the point predicate.
        /// </summary>
        /// <value>
        /// The point predicate.
        /// </value>
        public Func<TModel, string> PointPredicate { get; private set; }

        /// <summary>
        /// Sets a delegate to map a type to a series data label, if null, then the series will set it for you, default is null.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, TCoordinate> LabeledAs(Func<TModel, string> predicate)
        {
            PointPredicate = predicate;
            return this;
        }

        /// <summary>
        /// Whens this instance.
        /// </summary>
        /// <returns></returns>
        public ModelToPointMapper<TModel, TCoordinate> When(
            Func<TModel, bool> trigger, ModelStateHandler<TModel, TCoordinate> handler)
        {
            if (_modelDependentActions == null)
            {
                _modelDependentActions = new List<ModelState<TModel, TCoordinate>>();
            }
            _modelDependentActions.Add(new ModelState<TModel, TCoordinate>
            {
               Trigger = trigger,
               Handler = handler
            });
            return this;
        }

        /// <summary>
        /// Whens the specified trigger.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="handler">The action.</param>
        /// <returns></returns>
        public ModelToPointMapper<TModel, TCoordinate> When(
            UiActions trigger, ModelStateHandler<TModel, TCoordinate> handler)
        {
            if (!_userUiActions.ContainsKey(trigger))
            {
                _userUiActions.Add(trigger, new List<ModelStateHandler<TModel, TCoordinate>>());
            }
            _userUiActions[trigger].Add(handler);
            return this;
        }

        /// <summary>
        /// Runs the UI action.
        /// </summary>
        public void RunUiAction<TViewModel>(
            UiActions action, 
            TModel sender, 
            object visual,
            Point<TModel, TCoordinate, TViewModel> point)
        {
            var actionsCollection = _userUiActions[action];

            if (actionsCollection == null) return;

            foreach (var userAction in actionsCollection)
            {
                userAction(
                    sender, 
                    new ModelStateEventArgs<TModel, TCoordinate>(
                        visual, 
                        point.Pack()));
            }
        }

        /// <summary>
        /// Evaluates models dependent actions.
        /// </summary>
        public void EvaluateModelDependentActions<TViewModel>(
            TModel model, 
            object visual, 
            Point<TModel, TCoordinate, TViewModel> point)
        {
            if (_modelDependentActions == null) return;

            foreach (var mda in _modelDependentActions)
            {
                if (mda.Trigger(model))
                {
                    mda.Handler(
                        model,
                        new ModelStateEventArgs<TModel, TCoordinate>(
                            visual,
                            point.Pack()));
                }
            }
        }
    }
}