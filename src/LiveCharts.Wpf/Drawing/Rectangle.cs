using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Drawing;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.Drawing
{
    public class Rectangle: IRectangle
    {
        public Rectangle()
        {
            Shape = new System.Windows.Shapes.Rectangle();
        }

        public System.Windows.Shapes.Rectangle Shape { get; }
        object IShape.Shape => Shape;

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

        public float Width
        {
            get => (float) Shape.Width;
            set => Shape.Width = value;
        }

        public float Height
        {
            get => (float) Shape.Height;
            set => Shape.Height = value;
        }

        public IBrush Fill { get; set; }

        public IBrush Stroke { get; set; }

        public float StrokeThickness
        {
            get => (float) Shape.StrokeThickness;
            set => Shape.StrokeThickness = value;
        }

        public int ZIndex
        {
            get => Panel.GetZIndex(Shape);
            set => Panel.SetZIndex(Shape, value);
        }

        public float Opacity
        {
            get => (float) Shape.Opacity;
            set => Shape.Opacity = value;
        }

        public float[] StrokeDashArray
        {
            get => Shape.StrokeDashArray.Select(x => (float) x).ToArray();
            set => Shape.StrokeDashArray = new DoubleCollection(value.Select(x => (double) x));
        }

        public float XRadius
        {
            get => (float) Shape.RadiusX;
            set => Shape.RadiusX = value;
        }

        public float YRadius
        {
            get => (float) Shape.RadiusY;
            set => Shape.RadiusY = value;
        }

        public IAnimationBuilder Animate(TimeLine timeline)
        {
            return new AnimationBuilder(Shape, timeline.Duration, timeline.AnimationLine, true);
        }

        public void Paint(IBrush stroke, IBrush fill)
        {
            Shape.Fill = stroke.AsWpfBrush();
            Shape.Stroke = stroke.AsWpfBrush();
        }
    }
}
