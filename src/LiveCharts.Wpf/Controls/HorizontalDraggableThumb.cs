using System.Windows;

namespace LiveCharts.Wpf.Controls
{
    public class HorizontalDraggableThumb : Draggable
    {
        static HorizontalDraggableThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HorizontalDraggableThumb),
                new FrameworkPropertyMetadata(typeof(HorizontalDraggableThumb)));
        }
    }
}