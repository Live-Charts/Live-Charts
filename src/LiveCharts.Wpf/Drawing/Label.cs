using System.Windows.Controls;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf.Drawing
{
    public class Label : ILabel
    {
        public Label()
        {
            Shape = new System.Windows.Controls.Label();
        }

        /// <summary>
        /// Gets the platform specific shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public System.Windows.Controls.Label Shape { get; }
        object ILabel.Shape => Shape;

        public float Left
        {
            get => (float) Canvas.GetLeft(Shape);
            set => Canvas.SetLeft(Shape, value);
        }

        public float Top
        {
            get => (float) Canvas.GetTop(Shape);
            set => Canvas.SetTop(Shape, value);
        }
    }
}
