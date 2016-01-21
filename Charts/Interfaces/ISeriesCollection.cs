using System;
using System.Windows;
using LiveCharts.Charts;

namespace LiveCharts
{
    public interface ISeriesCollection
    {
        Point MaxChartPoint { get; }
        Point MinChartPoint { get; }
        Chart Chart { get; set; }
    }
}