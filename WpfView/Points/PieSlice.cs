using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LiveCharts.Wpf.Points
{
    //special thanks to Colin Eberhardt for the article.
    //http://www.codeproject.com/Articles/28098/A-WPF-Pie-Chart-with-Data-Binding-Support

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Shapes.Shape" />
    public class PieSlice : Shape
    {
        #region dependency properties

        /// <summary>
        /// The radius property
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("RadiusProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The radius of this pie piece
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// The push out property
        /// </summary>
        public static readonly DependencyProperty PushOutProperty =
            DependencyProperty.Register("PushOutProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The distance to 'push' this pie piece out from the centre.
        /// </summary>
        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        /// <summary>
        /// The inner radius property
        /// </summary>
        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadiusProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// The inner radius of this pie piece
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        /// <summary>
        /// The wedge angle property
        /// </summary>
        public static readonly DependencyProperty WedgeAngleProperty =
            DependencyProperty.Register("WedgeAngleProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The wedge angle of this pie piece in degrees
        /// </summary>
        public double WedgeAngle
        {
            get { return (double)GetValue(WedgeAngleProperty); }
            set
            {
                SetValue(WedgeAngleProperty, value);
                Percentage = (value / 360.0);
            }
        }

        /// <summary>
        /// The rotation angle property
        /// </summary>
        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register("RotationAngleProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The rotation, in degrees, from the Y axis vector of this pie piece.
        /// </summary>
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        /// <summary>
        /// The centre x property
        /// </summary>
        public static readonly DependencyProperty CentreXProperty =
            DependencyProperty.Register("CentreXProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The X coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreX
        {
            get { return (double)GetValue(CentreXProperty); }
            set { SetValue(CentreXProperty, value); }
        }

        /// <summary>
        /// The centre y property
        /// </summary>
        public static readonly DependencyProperty CentreYProperty =
            DependencyProperty.Register("CentreYProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreY
        {
            get { return (double)GetValue(CentreYProperty); }
            set { SetValue(CentreYProperty, value); }
        }

        /// <summary>
        /// The percentage property
        /// </summary>
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("PercentageProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The percentage of a full pie that this piece occupies.
        /// </summary>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// The piece value property
        /// </summary>
        public static readonly DependencyProperty PieceValueProperty =
            DependencyProperty.Register("PieceValueProperty", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double PieceValue
        {
            get { return (double)GetValue(PieceValueProperty); }
            set { SetValue(PieceValueProperty, value); }
        }


        #endregion

        /// <summary>
        /// Gets a value that represents the <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        /// <summary>
        /// Draws the pie piece
        /// </summary>
        private void DrawGeometry(StreamGeometryContext context)
        {
            var innerArcStartPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle, InnerRadius);
            innerArcStartPoint.Offset(CentreX, CentreY);
            var innerArcEndPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius);
            innerArcEndPoint.Offset(CentreX, CentreY);
            var outerArcStartPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle, Radius);
            outerArcStartPoint.Offset(CentreX, CentreY);
            var outerArcEndPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius);
            outerArcEndPoint.Offset(CentreX, CentreY);
            var innerArcMidPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle * .5, InnerRadius);
            innerArcMidPoint.Offset(CentreX, CentreY);
            var outerArcMidPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle * .5, Radius);
            outerArcMidPoint.Offset(CentreX, CentreY);

            var largeArc = WedgeAngle > 180.0d;
            var requiresMidPoint = Math.Abs(WedgeAngle - 360) < .01;

            if (PushOut > 0 && !requiresMidPoint)
            {
                var offset = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, PushOut);
                innerArcStartPoint.Offset(offset.X, offset.Y);
                innerArcEndPoint.Offset(offset.X, offset.Y);
                outerArcStartPoint.Offset(offset.X, offset.Y);
                outerArcEndPoint.Offset(offset.X, offset.Y);
            }

            var outerArcSize = new Size(Radius, Radius);
            var innerArcSize = new Size(InnerRadius, InnerRadius);

            if (requiresMidPoint)
            {
                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcMidPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcMidPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
                return;
            }

            context.BeginFigure(innerArcStartPoint, true, true);
            context.LineTo(outerArcStartPoint, true, true);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
            context.LineTo(innerArcEndPoint, true, true);
            context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PieUtils
    {
        /// <summary>
        /// Converts a coordinate from the polar coordinate system to the cartesian coordinate system.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            var angleRad = (Math.PI / 180.0) * (angle - 90);

            var x = radius * Math.Cos(angleRad);
            var y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}