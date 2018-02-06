using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class AxisSeparator : ICartesianAxisSeparator
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {
            Disposed?.Invoke(view, this);
        }

        public IPlaneLabelControl Label { get; }
        public void Move(CartesianAxisSeparatorArgs args)
        {
            
        }
    }
}