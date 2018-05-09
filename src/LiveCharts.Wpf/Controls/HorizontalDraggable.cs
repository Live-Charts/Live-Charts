using System.Windows;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Draggable" />
    public class HorizontalDraggable : Draggable
    {
        static HorizontalDraggable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HorizontalDraggable),
                new FrameworkPropertyMetadata(typeof(HorizontalDraggable)));
        }

        public HorizontalDraggable()
        {
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}