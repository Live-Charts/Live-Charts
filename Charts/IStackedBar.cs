using System.Collections.Generic;

namespace lvc
{
    public interface IStackedBar
    {
        double MaxColumnWidth { get; set; }
        Dictionary<int, StackedBarHelper> IndexTotals { get; }
    }
}
