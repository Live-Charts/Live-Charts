using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines the default chart point factory.
    /// </summary>
    /// <seealso cref="IDataFactory" />
    public class DefaultDataFactory : IDataFactory
    {
        /// <inheritdoc cref="IDataFactory.Fetch"/>
        public IEnumerable<ChartPoint> Fetch(DataFactoryArgs args)
        {
            var typeBuilder = args.Series.TypeBuilder ?? LiveCharts.GetBuilder(args.CollectionItemsType);
            var pointBuilder = typeBuilder.GetBuilder(args.Series.Defaults.ChartPointType);
            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(args.CollectionItemsType);
            var trackByRef = !args.CollectionItemsType.IsValueType &&
                             args.Series.TrackingMode == SeriesTrackingModes.ByReference;
            var collection = args.Collection;
            var dimensions = args.Chart.GetSeriesDimensions(args.Series);

            for (var index = 0; index < collection.Count; index++)
            {
                var instance = collection[index];

                // if INPC then attach the updater...
                if (notifiesChange)
                {
                    var npc = (INotifyPropertyChanged) instance;
                    npc.PropertyChanged += args.PropertyChangedEventHandler;
                }

                var key = trackByRef ? instance : index;

                // if the point does not exist already, we build it:
                if (!args.Series.ValuePointDictionary[args.Chart]
                    .TryGetValue(
                        key, out ChartPoint chartPoint))
                {
                    chartPoint = pointBuilder.Build(instance, index, args.Chart.UpdateId);
                }

                if (chartPoint.CompareDimensions(dimensions, args.Series.Defaults.SkipCriteria)) continue;

                // feed our chart points ...
                chartPoint.Instance = instance;
                chartPoint.Key = index;
                chartPoint.Series = args.Series;
                chartPoint.Chart = args.Chart;

                // register our chart point at the resource collector
                args.Chart.RegisterResource(chartPoint);

                yield return chartPoint;
            }
        }
    }
}
