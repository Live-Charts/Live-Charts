using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LiveChartsCore
{
    public interface IChartValues : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets series points to draw.
        /// </summary>
        IEnumerable<ChartPoint> Points { get; }
        /// <summary>
        /// Gets max X and Y values
        /// </summary>
        Point MaxChartPoint { get; }
        /// <summary>
        /// Gets min X and Y values
        /// </summary>
        Point MinChartPoint { get; }
        /// <summary>
        /// Gets or sets series that owns the values
        /// </summary>
        IChartSeries Series { get; set; }
        /// <summary>
        /// Forces values to calculate max, min and index data.
        /// </summary>
        void GetLimits();
    }
}