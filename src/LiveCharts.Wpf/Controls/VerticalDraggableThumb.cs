using System.Windows;

namespace LiveCharts.Wpf.Controls
{
    public class VerticalDraggableThumb : Draggable
    {
        static VerticalDraggableThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(VerticalDraggableThumb),
                new FrameworkPropertyMetadata(typeof(VerticalDraggableThumb)));
        }
    }
}