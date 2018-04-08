using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace LiveCharts.Wpf.Shapes
{
    /// <summary>
    /// A pie slice shape.
    /// </summary>
    /// <seealso cref="Shape" />
    public class Slice : Shape
    {
        /// <summary>
        /// The angle property
        /// </summary>
        public static readonly DependencyProperty OpeningAngleProperty = DependencyProperty.Register(
            "OpeningAngle", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the angle in degrees.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double OpeningAngle
        {
            get => (double) GetValue(OpeningAngleProperty);
            set
            {
                var a = value;
                if (a > 360) a = 360;
                if (a < 0) a = 0;
                SetValue(OpeningAngleProperty, a);
            }
        }

        /// <summary>
        /// The radius property
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius
        {
            get => (double) GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// The inner radius property
        /// </summary>
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        public double InnerRadius
        {
            get => (double) GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        /// <summary>
        /// The rotation property
        /// </summary>
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            "Rotation", typeof(double), typeof(Slice), 
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, 
                SetTransformOnPropertyChanged));

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public double Rotation
        {
            get => (double) GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public double CornerRadius
        {
            get => (double) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// The push out property
        /// </summary>
        public static readonly DependencyProperty PushOutProperty = DependencyProperty.Register(
            "PushOut", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(
                0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the push out.
        /// </summary>
        /// <value>
        /// The push out.
        /// </value>
        public double PushOut
        {
            get => (double) GetValue(PushOutProperty);
            set => SetValue(PushOutProperty, value);
        }

        /// <summary>
        /// The force angle property
        /// </summary>
        public static readonly DependencyProperty ForceAngleProperty = DependencyProperty.Register(
            "ForceAngle", typeof(bool), typeof(Slice), new PropertyMetadata(true));

        /// <summary>
        /// The possible push out property
        /// </summary>
        public static readonly DependencyProperty PossiblePushOutProperty = DependencyProperty.Register(
            "PossiblePushOut", typeof(double), typeof(Slice), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the possible push out.
        /// </summary>
        /// <value>
        /// The possible push out.
        /// </value>
        public double PossiblePushOut
        {
            get => (double) GetValue(PossiblePushOutProperty);
            set => SetValue(PossiblePushOutProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [force angle].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force angle]; otherwise, <c>false</c>.
        /// </value>
        public bool ForceAngle
        {
            get => (bool) GetValue(ForceAngleProperty);
            set => SetValue(ForceAngleProperty, value);
        }

        /// <summary>
        /// Gets a value that represents the <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            var center = new PointF((float) Height/2, (float) Width/2);

            var model = Core.Drawing.SliceModel.Build(
                OpeningAngle, Radius, InnerRadius, CornerRadius, center, ForceAngle, PushOut);

            context.BeginFigure(model.Points[0].AsWpf(), true, true);
            context.LineTo(model.Points[1].AsWpf(), true, true);

            var cornerSize = new Size(model.CornerRadius, model.CornerRadius);

            // corner 1
            context.ArcTo(model.Points[2].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(model.Points[3].AsWpf(), new Size(Radius, Radius), 0,
                model.IsRadiusLargeArc, SweepDirection.Counterclockwise, true, true);

            // corner 2
            context.ArcTo(model.Points[4].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.LineTo(model.Points[5].AsWpf(), true, true);

            //corner 3
            context.ArcTo(model.Points[6].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(model.Points[7].AsWpf(), new Size(InnerRadius, InnerRadius), 0, 
                model.IsInnerRadiusLargeArc, SweepDirection.Clockwise, true, true);

            // corner 4
            context.ArcTo(model.Points[0].AsWpf(), cornerSize, 0,
                false, SweepDirection.Counterclockwise, true, true);
        }

        private static void SetTransformOnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var slice = (Slice) dependencyObject;
            if (!(slice.Rotation > 0)) return;

            slice.RenderTransformOrigin = new Point(0.5, 0.5);
            slice.RenderTransform = new RotateTransform(-slice.Rotation);
        }
    }
}
