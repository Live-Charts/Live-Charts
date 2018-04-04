using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LiveCharts.Wpf.Shapes
{
    /// <summary>
    /// A pie slice shape.
    /// </summary>
    /// <seealso cref="System.Windows.Shapes.Shape" />
    public class PieSlice : Shape
    {
        /// <summary>
        /// The angle property
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

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
            "Radius", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

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
            "InnerRadius", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

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
        /// The smoothness property
        /// </summary>
        public static readonly DependencyProperty SmoothnessProperty = DependencyProperty.Register(
            "Smoothness", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the smoothness.
        /// </summary>
        /// <value>
        /// The smoothness.
        /// </value>
        public double Smoothness
        {
            get => (double) GetValue(SmoothnessProperty);
            set
            {
                var v = value;
                if (v > 1) v = 1;
                if (v < 0) v = 0;
                SetValue(SmoothnessProperty, v);
            }
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
            Height = Radius * 2;
            Width = Radius * 2;
            var center = new Point(Radius, Radius);
            const double toRadians = Math.PI / 180;
            var a = Angle;
            var isLarge = a > 180;
            if (a > 359.9)
            {
                // workaround..
                // for some reason, inverting isLarge seems not enough in this case..
                // so lets just make it look like the circle is complete.
                a = 359.9;
            }
            var angle = a * toRadians;
            var startPoint = new Point(center.X , center.Y + InnerRadius);
            var radiusEndPoint = new Point(
                center.X + Radius * Math.Sin(angle),
                center.Y + Radius * Math.Cos(angle));
            var innerRadiusEndPoint = new Point(
                center.X + InnerRadius * Math.Sin(angle),
                center.Y + InnerRadius * Math.Cos(angle));

            context.BeginFigure(startPoint, true, true);
            context.LineTo(new Point(center.X, center.Y + Radius), true, true);
            context.ArcTo(radiusEndPoint, new Size(Radius, Radius), 0, isLarge,
                SweepDirection.Counterclockwise, true, true);
            context.LineTo(innerRadiusEndPoint, true, true);
            context.ArcTo(startPoint, new Size(InnerRadius, InnerRadius), 0, isLarge,
                SweepDirection.Clockwise, true, true);
        }
    }
}
