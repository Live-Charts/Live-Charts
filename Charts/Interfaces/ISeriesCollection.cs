using System;
using System.Windows;
using lvc.Charts;

namespace lvc
{
    public interface ISeriesCollection
    {
        Point MaxChartPoint { get; }
        Point MinChartPoint { get; }
        Chart Chart { get; set; }
    }
}