using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LiveCharts.Uwp.Points
{
    //special thanks to Colin Eberhardt for the article.
    //http://www.codeproject.com/Articles/28098/A-WPF-Pie-Chart-with-Data-Binding-Support
    //http://domysee.com/blogposts/Blogpost%207%20-%20Creating%20custom%20Shapes%20for%20UWP%20Apps/

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Shapes.Path" />
    [Bindable]
    public class PieSlice : Path
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice"/> class.
        /// </summary>
        public PieSlice()
        {
            RegisterPropertyChangedCallback(WidthProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(RadiusProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(PushOutProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(InnerRadiusProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(WedgeAngleProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(RotationAngleProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(XOffsetProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(YOffsetProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(PieceValueProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(HeightProperty, RenderAffectingPropertyChanged);
            RegisterPropertyChangedCallback(WidthProperty, RenderAffectingPropertyChanged);
        }

        #region dependency properties

        /// <summary>
        /// The radius property
        /// </summary>
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

        /// <summary>
        /// The push out property
        /// </summary>
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

        /// <summary>
        /// The inner radius property
        /// </summary>
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

        /// <summary>
        /// The wedge angle property
        /// </summary>
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

        /// <summary>
        /// The rotation angle property
        /// </summary>
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

        /// <summary>
        /// The x offset property
        /// </summary>
        public static readonly DependencyProperty XOffsetProperty =
            DependencyProperty.Register(nameof(XOffset), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The X coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double XOffset
        {
            get { return (double)GetValue(XOffsetProperty); }
            set { SetValue(XOffsetProperty, value); }
        }

        /// <summary>
        /// The y offset property
        /// </summary>
        public static readonly DependencyProperty YOffsetProperty =
            DependencyProperty.Register(nameof(YOffset), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double YOffset
        {
            get { return (double)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        /// <summary>
        /// The percentage property
        /// </summary>
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(PieSlice),
                              new PropertyMetadata(0.0));
        //new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The percentage of a full pie that this piece occupies.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// The piece value property
        /// </summary>
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

        private static void RenderAffectingPropertyChanged(DependencyObject obj, DependencyProperty dp)
        {
            (obj as PieSlice)?.SetRenderData();
        }

        /// <summary>
        /// Draws the pie piece
        /// </summary>
        private void SetRenderData()
        {
            var innerArcStartPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle, InnerRadius, this);
            innerArcStartPoint.X += XOffset;
            innerArcStartPoint.Y += YOffset;
            var innerArcEndPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius, this);
            innerArcEndPoint.X += XOffset;
            innerArcEndPoint.Y += YOffset;
            var outerArcStartPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle, Radius, this);
            outerArcStartPoint.X += XOffset;
            outerArcStartPoint.Y += YOffset;
            var outerArcEndPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius, this);
            outerArcEndPoint.X += XOffset;
            outerArcEndPoint.Y += YOffset;
            var innerArcMidPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle * .5, InnerRadius, this);
            innerArcMidPoint.X += XOffset;
            innerArcMidPoint.Y += YOffset;
            var outerArcMidPoint = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle * .5, Radius, this);
            outerArcMidPoint.X += XOffset;
            outerArcMidPoint.Y += YOffset;

            var largeArc = WedgeAngle > 180.0d;
            var requiresMidPoint = Math.Abs(WedgeAngle - 360) < .01;

            if (PushOut > 0 && !requiresMidPoint)
            {
                var offset = PieUtils.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, PushOut, this);
                offset.X -= Width*.5;
                offset.Y -= Height*.5;
                innerArcStartPoint.X += offset.X;
                innerArcStartPoint.Y += offset.Y;
                innerArcEndPoint.X += offset.X;
                innerArcEndPoint.Y += offset.Y;
                outerArcStartPoint.X += offset.X;
                outerArcStartPoint.Y += offset.Y;
                outerArcEndPoint.X += offset.X;
                outerArcEndPoint.Y += offset.Y;
            }

            var outerArcSize = new Size(Radius, Radius);
            var innerArcSize = new Size(InnerRadius, InnerRadius);

            var pathFigure = new PathFigure { IsClosed = true, IsFilled = true, StartPoint = innerArcStartPoint };
            
            if (requiresMidPoint)
            {
                pathFigure.Segments.Add(new LineSegment { Point = outerArcStartPoint });
                pathFigure.Segments.Add(new ArcSegment { Point = outerArcMidPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new ArcSegment { Point = outerArcEndPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new LineSegment { Point = innerArcEndPoint });
                pathFigure.Segments.Add(new ArcSegment { Point = innerArcMidPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Counterclockwise });
                pathFigure.Segments.Add(new ArcSegment { Point = innerArcStartPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = false, SweepDirection = SweepDirection.Counterclockwise });
            }
            else
            {
                pathFigure.Segments.Add(new LineSegment { Point = outerArcStartPoint });
                pathFigure.Segments.Add(new ArcSegment { Point = outerArcEndPoint, Size = outerArcSize, RotationAngle = 0, IsLargeArc = largeArc, SweepDirection = SweepDirection.Clockwise });
                pathFigure.Segments.Add(new LineSegment { Point = innerArcEndPoint });
                pathFigure.Segments.Add(new ArcSegment { Point = innerArcStartPoint, Size = innerArcSize, RotationAngle = 0, IsLargeArc = largeArc, SweepDirection = SweepDirection.Counterclockwise });
            }

            Data = new PathGeometry { Figures = new PathFigureCollection() { pathFigure }, FillRule = FillRule.EvenOdd };
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
        /// <param name="container"></param>
        /// <returns></returns>
        public static Point ComputeCartesianCoordinate(double angle, double radius, Path container)
        {
            // convert to radians
            var angleRad = (Math.PI / 180.0) * (angle - 90);

            var x = radius * Math.Cos(angleRad);
            var y = radius * Math.Sin(angleRad);

            return new Point(x + container.Width*.5, y + container.Height*.5);
        }
    }
}