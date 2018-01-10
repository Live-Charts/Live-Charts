using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferenceChartPoint<TModel, TCoordinate, TViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new()
    {
        TChartPoint ChartPoint { get; set; }
    }

    /// <summary>
    /// Defines the default chart point factory.
    /// </summary>
    /// <seealso cref="IDataFactory" />
    public class DefaultDataFactory : IDataFactory
    {
        public IEnumerable<TChartPoint> FetchData<TModel, TCoordinate, TViewModel, TChartPoint>(
            DataFactoryArgs<TModel, TCoordinate, TViewModel, TChartPoint> args) 
            where TChartPoint: ChartPoint<TModel, TCoordinate, TViewModel>, new()
        {
            var modelType = typeof(TModel);
            // var pointBuilder = args.Series.PointBuilder ?? ChartingTypeBuilder<TModel>.GetPointBuilder<TChartPoint>();
            var mapper = args.Series.Mapper;
            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(modelType);
            var trackByRef = typeof(IReferenceChartPoint<TModel, TCoordinate, TViewModel, TChartPoint>).IsAssignableFrom(modelType);
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

                TChartPoint chartPoint;

                if (trackByRef)
                {
                    var ircp = (IReferenceChartPoint<TModel, TCoordinate, TViewModel, TChartPoint>) instance;
                    if (ircp.ChartPoint == null)
                    {
                        chartPoint = new TChartPoint();
                        ircp.ChartPoint = chartPoint;
                    }
                    else
                    {
                        chartPoint = ircp.ChartPoint;
                    }
                }
                else
                {
                    if (args.Series.ValueTracker.Count < index)
                    {
                        chartPoint = args.Series.ValueTracker[index];
                    }
                    else
                    {
                        chartPoint = new TChartPoint();
                        args.Series.ValueTracker.Add(chartPoint);
                    }
                }

                // ChartingConfig.GetDefault(key);
                // if (chartPoint.CompareDimensions(dimensions, args.Series.Defaults.SkipCriteria)) continue;

                // feed our chart points ...
                chartPoint.Model = instance;
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
