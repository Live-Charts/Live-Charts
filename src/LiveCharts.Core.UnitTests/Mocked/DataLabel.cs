using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class DataLabel : IDataLabelControl
    {
        public Size Measure(PackedPoint point)
        {
            return new Size(10, 30);
        }
    }
}