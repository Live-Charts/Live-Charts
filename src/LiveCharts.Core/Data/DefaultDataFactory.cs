using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DefaultSettings;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines the default chart point factory.
    /// </summary>
    /// <seealso cref="IDataFactory" />
    public class DefaultDataFactory : IDataFactory
    {
        /// <inheritdoc />
        public IEnumerable<TPoint> FetchData<TModel, TCoordinate, TViewModel, TPoint>(
            DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint> args) 
            where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
            where TCoordinate : ICoordinate
        {
            var modelType = typeof(TModel);
            var mapper = args.Series.Mapper;
            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(modelType);
            var observable = typeof(IObservablePoint<TModel, TCoordinate, TViewModel, TPoint>).IsAssignableFrom(modelType);
            var collection = args.Collection;
            var dimensions = args.Chart.GetSeriesDimensions(args.Series);
            var pcHandler = BuildPCHandlerFor(args.Chart);

            for (var index = 0; index < collection.Count; index++)
            {
                var instance = collection[index];

                TPoint chartPoint;

                if (observable)
                {
                    var iocp = (IObservablePoint<TModel, TCoordinate, TViewModel, TPoint>) instance;
                    if (iocp.ChartPoint == null)
                    {
                        chartPoint = new TPoint();
                        iocp.ChartPoint = chartPoint;
                        // if INPC then invalidate the chart on property change.
                        if (notifiesChange)
                        {
                            var npc = (INotifyPropertyChanged)instance;
                            npc.PropertyChanged += pcHandler;
                            chartPoint.Disposed += (view, point) => { npc.PropertyChanged -= pcHandler; };
                        }
                    }
                    else
                    {
                        chartPoint = iocp.ChartPoint;
                    }
                }
                else
                {
                    if (index < args.Series.ByValTracker.Count)
                    {
                        chartPoint = args.Series.ByValTracker[index];
                    }
                    else
                    {
                        chartPoint = new TPoint();
                        args.Series.ByValTracker.Add(chartPoint);
                        // if INPC then invalidate the chart on property change.
                        if (notifiesChange)
                        {
                            var npc = (INotifyPropertyChanged)instance;
                            npc.PropertyChanged += pcHandler;
                            chartPoint.Disposed += (view, point) => { npc.PropertyChanged -= pcHandler; };
                        }
                    }
                }

                // feed our chart points ...
                chartPoint.Model = instance;
                chartPoint.Key = index;
                chartPoint.Series = args.Series;
                chartPoint.Chart = args.Chart;
                chartPoint.Coordinate = mapper.Predicate(instance, index);

                // compare the dimensions to scale the chart.
                if (chartPoint.Coordinate.CompareDimensions(dimensions, SeriesSkipCriteria.None)) continue;

                // evaluate model defined events
                mapper.EvaluateModelDependentActions(instance, chartPoint.View, chartPoint);

                // register our chart point at the resource collector
                args.Chart.RegisterResource(chartPoint);

                yield return chartPoint;
            }
        }

        private PropertyChangedEventHandler BuildPCHandlerFor(ChartModel chart)
        {
            return (sender, args) => { chart.Invalidate(); };
        }
    }
}
