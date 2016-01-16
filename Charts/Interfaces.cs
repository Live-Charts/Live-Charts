using System.Collections.Generic;

namespace lvc
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
        Dictionary<int, StackedBarHelper> IndexTotals { get; }
    }
}
