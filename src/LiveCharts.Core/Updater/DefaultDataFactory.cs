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

using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction;

#endregion

namespace LiveCharts.Core.Updater
{
    /// <summary>
    /// Defines the default chart point factory.
    /// </summary>
    /// <seealso cref="IDataFactory" />
    public class DefaultDataFactory : IDataFactory
    {
        /// <inheritdoc />
        public TPoint[] Fetch<TModel, TCoordinate, TViewModel, TPoint>(
            DataFactoryContext<TModel, TCoordinate, TViewModel, TPoint> context)
            where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
            where TCoordinate : ICoordinate
        {
            var results = new List<TPoint>();
            var mapper = context.Series.Mapper;
            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(context.Series.Metadata.ModelType);
            var collection = context.Collection;
            var isValueType = context.Series.Metadata.IsValueType;
            var tracker = (Dictionary<object, TPoint>) context.Series.Content[context.Chart][Series.Tracker];

            void InvalidateOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                context.Chart.Invalidate(context.Chart.View);
            }

            for (var index = 0; index < collection.Count; index++)
            {
                var instance = collection[index];

                var key = isValueType ? index : (object) instance;

                if (!tracker.TryGetValue(key, out var chartPoint))
                {
                    chartPoint = new TPoint();
                    tracker.Add(key, chartPoint);
                    if (notifiesChange)
                    {
                        var npc = (INotifyPropertyChanged) instance;

                        void DisposeByValPoint(IChartView view, object sender)
                        {
                            npc.PropertyChanged -= InvalidateOnPropertyChanged;
                            tracker.Remove(key);
                        }

                        npc.PropertyChanged += InvalidateOnPropertyChanged;
                        chartPoint.Disposed += DisposeByValPoint;
                    }
                }

                // feed our chart points ...
                chartPoint.Model = instance;
                chartPoint.Key = index;
                chartPoint.Series = context.Series;
                chartPoint.Chart = context.Chart;
                chartPoint.Coordinate = mapper.Predicate.Invoke(instance, index);

                // compare the dimensions to scale the chart.
                chartPoint.Coordinate.CompareDimensions(context);

                // evaluate model defined events
                mapper.EvaluateModelDependentActions(instance, chartPoint.View, chartPoint);

                // register our chart point at the resource collector
                context.Chart.RegisterResource(chartPoint);

                results.Add(chartPoint);
            }

            return results.ToArray();
        }
    }
}
