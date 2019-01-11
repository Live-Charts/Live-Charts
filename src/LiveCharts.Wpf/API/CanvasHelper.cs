using LiveCharts.Interaction.Controls;
using System.Windows;
using System.Windows.Controls;

namespace LiveCharts.Wpf
{
    internal static class CanvasHelper
    {
        public static void AddChild(this ICanvas canvas, UIElement child, bool clippedToDrawMargin)
        {
            var wpfCanvas = (Canvas)canvas;
            wpfCanvas.Children.Add(child);
        }

        public static void RemoveChild(this ICanvas canvas, UIElement child)
        {
            var wpfCanvas = (Canvas)canvas;
            wpfCanvas.Children.Remove(child);
        }
    }
}
