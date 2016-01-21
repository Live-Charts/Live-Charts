using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using LiveCharts.Charts;

namespace LiveCharts
{
    public interface IChartValues : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        IEnumerable<ChartPoint> Points { get; }
        Point MaxChartPoint { get; }
        Point MinChartPoint { get; }
        Chart Chart { get; set; }
        IChartSeries Series { get; set; }
        void Evaluate();
        bool RequiresEvaluation { get; set; }
    }
}