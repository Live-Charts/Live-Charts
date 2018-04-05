using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the angle in degrees.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle
        {
            get => (double) GetValue(AngleProperty);
            set
            {
                var a = value;
                if (a > 360) a = 360;
                if (a < 0) a = 0;
                SetValue(AngleProperty, a);
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
            "Rotation", typeof(double), typeof(Slice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, PropertyChangedCallback));

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
            // See docs/resources/slice.png
            // if you require to know more about the formulas in the geometry

            Height = Radius * 2 + StrokeThickness / 2;
            Width = Height;
            var center = new Point(Radius, Radius);
            const double toRadians = Math.PI / 180;
            var a = Angle;

            if (a > 359.9)
            {
                // workaround..
                // for some reason, inverting isLarge seems not enough in this case..
                // so lets just make it look like the circle is complete.
                a = 359.9;
            }

            var angle = a * toRadians;
            var cornerRadius = (Radius - InnerRadius)/2 > CornerRadius ? CornerRadius : (Radius - InnerRadius) / 2;

            var innerRoundingAngle = Math.Atan(
                cornerRadius / Math.Sqrt(
                    Math.Pow(InnerRadius + cornerRadius, 2) + Math.Pow(cornerRadius, 2)));
            var outerRoundingAngle = Math.Atan(
                cornerRadius / Math.Sqrt(
                    Math.Pow(Radius - cornerRadius, 2) + Math.Pow(cornerRadius, 2)));
            var middleRoundingAngle = Math.Atan(
                cornerRadius / Math.Sqrt(
                    Math.Pow((Radius + InnerRadius)/2, 2) + Math.Pow(cornerRadius, 2)));

            var rx = center.X + Math.Sqrt(
                         Math.Pow(Radius - cornerRadius, 2) -
                         Math.Pow(cornerRadius, 2)) * Math.Sin(angle);
            var tx = center.X + Math.Sqrt(
                         Math.Pow(Radius, 2)) * Math.Sin(angle);

            Console.WriteLine(tx - rx);

            var isLarge = angle - middleRoundingAngle * 2 >= Math.PI;

            var o1 = (InnerRadius + cornerRadius) * Math.Cos(innerRoundingAngle);
            var startPoint = new Point(center.X, center.Y + o1);
            context.BeginFigure(startPoint, true, true);

            var o2 = (Radius - cornerRadius) * Math.Cos(outerRoundingAngle);
            context.LineTo(new Point(center.X, center.Y + o2), true, true);

            var cornerSize = new Size(cornerRadius, cornerRadius);

            // corner 1
            context.ArcTo(
                new Point(
                    center.X + Radius * Math.Sin(outerRoundingAngle),
                    center.Y + Radius * Math.Cos(outerRoundingAngle)),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(new Point(
                    center.X + Radius * Math.Sin(angle - outerRoundingAngle),
                    center.Y + Radius * Math.Cos(angle - outerRoundingAngle)),
                new Size(Radius, Radius), 0, isLarge, SweepDirection.Counterclockwise, true, true);

            // corner 2
            var o3 = Math.Sqrt(Math.Pow(Radius - cornerRadius, 2) - Math.Pow(cornerRadius, 2));

            context.ArcTo(
                new Point(
                    center.X + o3 * Math.Sin(angle),
                    center.Y + o3 * Math.Cos(angle)),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            var o4 = Math.Sqrt(Math.Pow(InnerRadius + cornerRadius, 2) - Math.Pow(cornerRadius, 2));

            context.LineTo(new Point(
                center.X + o4 * Math.Sin(angle),
                center.Y + o4 * Math.Cos(angle)), true, true);

            //corner 3
            context.ArcTo(
                new Point(
                    center.X + InnerRadius * Math.Sin(angle - innerRoundingAngle),
                    center.Y + InnerRadius * Math.Cos(angle - innerRoundingAngle)),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(
                new Point(
                    center.X + InnerRadius * Math.Sin(innerRoundingAngle),
                    center.Y + InnerRadius * Math.Cos(innerRoundingAngle)),
                new Size(InnerRadius, InnerRadius), 0, isLarge, SweepDirection.Clockwise, true, true);

            // corner 4
            context.ArcTo(startPoint, cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);
        }

        private double GetLength(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var slice = (Slice) dependencyObject;
            if (slice.Rotation > 0)
            {
                slice.RenderTransform = new RotateTransform
                {
                    Angle = slice.Rotation
                };
                slice.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }
    }
}
