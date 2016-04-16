using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace LiveChartsCore
{
    public class SeriesCollection : ObservableCollection<IChartSeries>
    {
        public SeriesCollection()
        {
            Configuration = new SeriesConfiguration<double>().X((v, i) => i).Y(v => v);
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (var series in args.NewItems.Cast<Series>())
                        series.Collection = this;
            };
        }

        public SeriesCollection(SeriesConfiguration configuration)
        {
            Configuration = configuration;
        }

        public SeriesConfiguration Configuration { get; set; }

        /// <summary>
        /// Setup a configuration for this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public SeriesCollection Setup<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                base.CollectionChanged += value;

            }
            remove { base.CollectionChanged -= value; }
        }
    }
}