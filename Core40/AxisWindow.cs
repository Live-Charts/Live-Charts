namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AxisWindow : IAxisWindow
    {
        public abstract double MinimumSeparatorWidth { get; }

        public abstract bool IsHeader(double x);

        public abstract bool IsSeparator(double x);

        public abstract string FormatAxisLabel(double x);
    }
}