using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace lvc
{
    public interface IChartsValues : IList, INotifyCollectionChanged
    {
        IEnumerable<Point> Points { get; }
        Point Max { get; }
        Point Min { get; }
    }
}