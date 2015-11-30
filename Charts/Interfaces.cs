using System.Collections.Generic;

namespace LiveCharts
{
    public interface IBar
    {
        double MaxColumnWidth { get; set; }
    }
    public interface ILine
    {
        LineChartLineType LineType { get; set; }
    }
    public interface IStackedBar
    {
        double MaxColumnWidth { get; set; }
        Dictionary<int, StackedBarHelper> IndexTotals { get; set; }
    }
}
