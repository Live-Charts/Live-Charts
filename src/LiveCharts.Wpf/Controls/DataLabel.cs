using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;

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

        SizeF IDataLabelControl.Measure(PackedPoint point)
        {
            DataContext = point;
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new SizeF((float) DesiredSize.Width, (float) DesiredSize.Height);
        }
    }
}