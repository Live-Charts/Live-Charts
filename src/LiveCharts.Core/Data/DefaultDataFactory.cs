using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines the default chart point factory.
    /// </summary>
    /// <seealso cref="IDataFactory" />
    public class DefaultDataFactory : IDataFactory
    {
        /// <inheritdoc />
        public IEnumerable<TPoint> Fetch<TModel, TCoordinate, TViewModel, TPoint>(
            DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint> args)
            where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
            where TCoordinate : ICoordinate
        {
            var mapper = args.Series.Mapper;
            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(args.Series.Metatada.ModelType);
            var collection = args.Collection;
            var isValueType = args.Series.Metatada.IsValueType;
            var tracker = args.Series.Tracker;

            void InvalidateOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                args.Chart.Invalidate(args.Chart.View);
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
                chartPoint.Series = args.Series;
                chartPoint.Chart = args.Chart;
                chartPoint.Coordinate = mapper.Predicate(instance, index);

                // compare the dimensions to scale the chart.
                chartPoint.Coordinate.CompareDimensions(args.Series.RangeByDimension);

                // evaluate model defined events
                mapper.EvaluateModelDependentActions(instance, chartPoint.View, chartPoint);

                // register our chart point at the resource collector
                args.Chart.RegisterResource(chartPoint);

                yield return chartPoint;
            }
        }
    }
}
