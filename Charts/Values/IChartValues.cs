using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace lvc
{
    public interface IChartValues : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        IEnumerable<Point> Points { get; }
        Point MaxChartPoint { get; }
        Point MinChartPoint { get; }
    }
}