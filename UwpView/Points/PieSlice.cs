using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LiveCharts.Uwp.Points
{
    //special thanks to Colin Eberhardt for the article.
    //http://www.codeproject.com/Articles/28098/A-WPF-Pie-Chart-with-Data-Binding-Support
    //http://domysee.com/blogposts/Blogpost%207%20-%20Creating%20custom%20Shapes%20for%20UWP%20Apps/

    public class PieSlice : Path
    {
        public PieSlice()
        {
            this.RegisterPropertyChangedCallback(PieSlice.WidthProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.RadiusProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.PushOutProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.InnerRadiusProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.WedgeAngleProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.RotationAngleProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.CentreXProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.CentreYProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
            this.RegisterPropertyChangedCallback(PieSlice.PieceValueProperty, new DependencyPropertyChangedCallback(RenderAffectingPropertyChanged));
        }

        #region dependency properties

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The radius of this pie piece
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty PushOutProperty =
            DependencyProperty.Register(nameof(PushOut), typeof(double), typeof(PieSlice),
                new PropertyMetadata(0.0));
            //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The distance to 'push' this pie piece out from the centre.
        /// </summary>
        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register(nameof(InnerRadius), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// The inner radius of this pie piece
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WedgeAngleProperty =
            DependencyProperty.Register(nameof(WedgeAngle), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register(nameof(RotationAngle), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The rotation, in degrees, from the Y axis vector of this pie piece.
        /// </summary>
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        public static readonly DependencyProperty CentreXProperty =
            DependencyProperty.Register(nameof(CentreX), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The X coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreX
        {
            get { return (double)GetValue(CentreXProperty); }
            set { SetValue(CentreXProperty, value); }
        }

        public static readonly DependencyProperty CentreYProperty =
            DependencyProperty.Register(nameof(CentreY), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreY
        {
            get { return (double)GetValue(CentreYProperty); }
            set { SetValue(CentreYProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The percentage of a full pie that this piece occupies.
        /// </summary>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PieceValueProperty =
            DependencyProperty.Register(nameof(PieceValue), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //    new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double PieceValue
        {
            get { return (double)GetValue(PieceValueProperty); }
            set { SetValue(PieceValueProperty, value); }
        }

        #endregion

        private void RenderAffectingPropertyChanged(DependencyObject obj, DependencyProperty dp)
        {
            (obj as PieSlice)?.SetRenderData();
        }

        /// <summary>
        /// Draws the pie piece
        /// </summary>
        private void SetRenderData()
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

            var pathFigure = new PathFigure() { IsClosed = true, IsFilled = true, StartPoint = innerArcStartPoint };
            
            if (requiresMidPoint)
            {
                pathFigure.Segments.Add(new LineSegment() { Point = outerArcStartPoint });
                pathFigure.Segments.Add(new ArcSegment() { Point = outerArcMidPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new ArcSegment() { Point = outerArcEndPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new LineSegment() { Point = innerArcEndPoint });
                pathFigure.Segments.Add(new ArcSegment() { Point = innerArcMidPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Counterclockwise });
                pathFigure.Segments.Add(new ArcSegment() { Point = innerArcStartPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Counterclockwise });
            }
            else
            {
                pathFigure.Segments.Add(new LineSegment() { Point = outerArcStartPoint });
                pathFigure.Segments.Add(new ArcSegment() { Point = outerArcEndPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = largeArc, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new LineSegment() { Point = innerArcEndPoint });
                pathFigure.Segments.Add(new ArcSegment() { Point = innerArcStartPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = largeArc, SweepDirection = SweepDirection.Counterclockwise });
            }

            this.Data = new PathGeometry() { Figures = new PathFigureCollection() { pathFigure }, FillRule = FillRule.EvenOdd };
        }
    }

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