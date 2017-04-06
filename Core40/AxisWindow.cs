using System.Collections.Generic;
using System.Linq;
using LiveCharts.Helpers;

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

        public virtual bool TryGetSeparatorIndices(IEnumerable<double> indices, out IEnumerable<double> separators)
        {
            separators = new List<double>();
            ((List<double>)separators).AddRange(indices.Where(IsSeparator));
            return true;
        }
    }
}