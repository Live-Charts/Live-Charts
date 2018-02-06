using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class PlaneLabel : IPlaneLabelControl
    {
        public Size Measure(string label)
        {
            return new Size(10, 30);
        }
    }
}