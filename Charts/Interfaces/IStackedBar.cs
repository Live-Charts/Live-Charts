using System.Collections.Generic;

namespace LiveCharts
{
    public interface IStackedBar
    {
        double MaxColumnWidth { get; set; }
        Dictionary<int, StackedBarHelper> IndexTotals { get; }
    }
}
