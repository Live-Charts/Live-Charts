using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction.Controls;

namespace LiveCharts.Wpf
{
    public abstract class ChartShape : Shape, IShape
    {
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(ChartRectangle), new UIPropertyMetadata(0d));

        public int ZIndex
        {
            get => Panel.GetZIndex(this);
            set => Panel.SetZIndex(this, value);
        }

        public double Left
        {
            get => Canvas.GetLeft(this);
            set => Canvas.SetLeft(this, value);
        }

        public double Top
        {
            get => Canvas.GetTop(this);
            set => Canvas.SetTop(this, value);
        }

        IEnumerable<double> IDashable.StrokeDashArray
        {
            get => StrokeDashArray.AsEnumerable();
            set => StrokeDashArray = new DoubleCollection(value);
        }

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }
        Drawing.Brushes.Brush IShape.Fill { get; set; }

        Drawing.Brushes.Brush IShape.Stroke { get; set; }

        public virtual IAnimationBuilder Animate(AnimatableArguments args) => new AnimationBuilder<Shape>(this, args);

        protected virtual Transform GetTransform()
        {
            TransformGroup transform = null;

            var requiresTransform = Math.Abs(Rotation) > 0.01 || RenderTransform != null;

            if (requiresTransform)
            {
                transform = new TransformGroup();
                transform.Children.Add(RenderTransform);
                transform.Children.Add(new RotateTransform(Rotation));
            }

            return transform;
        }

        void ICanvasElement.FlushToCanvas(ICanvas canvas, bool clippedToDrawMargin) => canvas.AddChild(this, clippedToDrawMargin);

        void ICanvasElement.RemoveFromCanvas(ICanvas canvas) => canvas.RemoveChild(this);

        void ICanvasElement.Paint()
        {
            var iShape = (IShape) this;
            Fill = iShape.Fill.AsWpfBrush();
            Stroke = iShape.Stroke.AsWpfBrush();
        }
    }
}
