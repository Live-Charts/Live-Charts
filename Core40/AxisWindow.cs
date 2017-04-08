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

        public virtual bool TryGetSeparatorIndices(IEnumerable<double> indices, int maximumSeparatorCount, out IEnumerable<double> separators)
        {
            var separatorList = new List<double>();

            foreach (var index in indices)
            {
                if (separatorList.Count > maximumSeparatorCount)
                {
                    // We have exceeded the maxmimum separators.
                    separators = Enumerable.Empty<double>();
                    return false;
                }

                if (!IsSeparator(index)) continue;

                // Add this separator
                separatorList.Add(index);
            }

            // Return the separators
            separators = separatorList;
            return true;
        }
    }
}