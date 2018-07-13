using System.Windows;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Draggable" />
    public class VerticalDraggable : Draggable
    {
        static VerticalDraggable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(VerticalDraggable),
                new FrameworkPropertyMetadata(typeof(VerticalDraggable)));
        }

        public VerticalDraggable()
        {
            VerticalAlignment = VerticalAlignment.Center;
        }
    }
}