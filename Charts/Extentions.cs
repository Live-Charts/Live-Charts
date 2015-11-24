using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LiveCharts.Series;

namespace LiveCharts
{
    public static class Extentions
    {
        public static void WithChangedHandler(this ObservableCollection<Serie> serie, NotifyCollectionChangedEventHandler handler)
        {
            serie.CollectionChanged += handler;
        }
    }
}
