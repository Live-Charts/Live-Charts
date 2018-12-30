using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Animations;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.Drawing
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

        public IAnimationBuilder Animate(Transition timeline)
        {
            return new AnimationBuilder(this, timeline.Time, timeline.KeyFrames, true);
        }

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

        IEnumerable<IPaintable> IUIContent.GetPaintables()
        {
            yield return this;
        }

        void IPaintable.Paint(IBrush stroke, IBrush fill)
        {
            Fill = stroke.AsWpfBrush();
            Fill = stroke.AsWpfBrush();
        }
    }
}
