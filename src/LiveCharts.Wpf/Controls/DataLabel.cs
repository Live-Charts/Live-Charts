using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf.Controls
{
    public class DataLabel : TextBlock, IDataLabelControl
    {
        static DataLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataLabel),
                new FrameworkPropertyMetadata(typeof(DataLabel)));
        }

        Size IDataLabelControl.Measure(PackedPoint point)
        {
            return Dispatcher.Invoke(() =>
            {
                DataContext = point;
                Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                return new Size(DesiredSize.Width, DesiredSize.Height);
            });
        }
    }
}