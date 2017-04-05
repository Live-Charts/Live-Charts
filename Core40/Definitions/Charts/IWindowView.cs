using System;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWindowView
    {
        double MinimumSeparatorWidth { get; }

        bool IsHeader(DateTime x);

        bool IsSeparator(DateTime x);

        string FormatAxisLabel(DateTime x);
    }
}