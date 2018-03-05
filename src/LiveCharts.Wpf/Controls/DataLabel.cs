using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf.Controls
{
    public class DataLabel : TextBlock, IDataLabelControl
    {
        static DataLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DataLabel),
                new FrameworkPropertyMetadata(typeof(DataLabel)));
        }

        Size IDataLabelControl.Measure(PackedPoint point)
        {
            DataContext = point;
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size((float) DesiredSize.Width, (float) DesiredSize.Height);
        }
    }
}