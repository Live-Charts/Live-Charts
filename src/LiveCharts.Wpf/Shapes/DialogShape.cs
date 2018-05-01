using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;

namespace LiveCharts.Wpf.Shapes
{
    /// <summary>
    /// A bubble shape.
    /// </summary>
    /// <seealso cref="System.Windows.Shapes.Shape" />
    public class DialogShape : Shape, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(double), typeof(DialogShape),
            new FrameworkPropertyMetadata(3d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CornerRadius
        {
            get => (double) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty WedgeProperty = DependencyProperty.Register(
            nameof(Wedge), typeof(double), typeof(DialogShape),
            new FrameworkPropertyMetadata(75d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Wedge
        {
            get => (double) GetValue(WedgeProperty);
            set => SetValue(WedgeProperty, value);
        }

        public static readonly DependencyProperty WedgeHypotenuseProperty = DependencyProperty.Register(
            nameof(WedgeHypotenuse), typeof(double), typeof(DialogShape),
            new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double WedgeHypotenuse
        {
            get => (double) GetValue(WedgeHypotenuseProperty);
            set => SetValue(WedgeHypotenuseProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            nameof(Position), typeof(ToolTipPosition), typeof(DialogShape),
            new FrameworkPropertyMetadata(ToolTipPosition.Right, FrameworkPropertyMetadataOptions.AffectsRender));

        private Rect _rectangle;

        public ToolTipPosition Position
        {
            get => (ToolTipPosition) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public Rect Rectangle
        {
            get => _rectangle;
            private set
            {
                _rectangle = value;
                OnPropertyChanged();
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (double.IsNaN(Height) || double.IsNaN(Width))
                {
                    return new RectangleGeometry();
                }

                var hyp = WedgeHypotenuse;
                var alpha = Wedge * Math.PI / 180d;
                var tx = Math.Sin(alpha / 2d) * hyp;
                var ty = Math.Cos(alpha / 2d) * hyp;
                var t = tx < ty ? ty : tx;

                if (Width < t)
                {
                    Width = t;
                }

                if (Height < t)
                {
                    Height = t;
                }

                switch (Position)
                {
                    case ToolTipPosition.Top:
                        return PlaceTop();
                    case ToolTipPosition.Left:
                        return PlaceLeft();
                    case ToolTipPosition.Right:
                        return PlaceRight();
                    case ToolTipPosition.Bottom:
                        return PlaceBottom();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Geometry PlaceBottom()
        {
            var combined = new GeometryGroup();

            var hyp = WedgeHypotenuse;
            var alpha = Wedge * Math.PI / 180d;
            var tx = Math.Sin(alpha / 2d) * hyp;
            var ty = Math.Cos(alpha / 2d) * hyp;

            var rectangle =
                new RectangleGeometry(
                    Rectangle = new Rect(0, ty, Width, Height - ty),
                    CornerRadius,
                    CornerRadius);

            var triangle = new StreamGeometry();

            using (var context = triangle.Open())
            {
                var x = Width / 2;
                context.BeginFigure(new Point(x, 0), true, true);
                context.LineTo(new Point(x + tx, ty), true, true);
                context.LineTo(new Point(x - tx, ty), true, true);
                context.Close();
            }

            combined.Children.Add(rectangle);
            combined.Children.Add(triangle);

            combined.Freeze();

            return combined;
        }

        private Geometry PlaceTop()
        {
            var combined = new GeometryGroup();

            var hyp = WedgeHypotenuse;
            var alpha = Wedge * Math.PI / 180d;
            var tx = Math.Sin(alpha / 2d) * hyp;
            var ty = Math.Cos(alpha / 2d) * hyp;

            var rectangle =
                new RectangleGeometry(
                    Rectangle = new Rect(0, 0, Width, Height - ty),
                    CornerRadius,
                    CornerRadius);

            var triangle = new StreamGeometry();

            using (var context = triangle.Open())
            {
                var x = Width / 2;
                var yo = Height - ty;
                context.BeginFigure(new Point(x - tx, yo), true, true);
                context.LineTo(new Point(x + tx, yo), true, true);
                context.LineTo(new Point(x, yo + ty), true, true);
                context.Close();
            }

            combined.Children.Add(rectangle);
            combined.Children.Add(triangle);

            combined.Freeze();

            return combined;
        }

        private Geometry PlaceRight()
        {
            var combined = new GeometryGroup();

            var hyp = WedgeHypotenuse;
            var alpha = Wedge * Math.PI / 180d;
            var tx = Math.Sin(alpha / 2d) * hyp;
            var ty = Math.Cos(alpha / 2d) * hyp;

            var rectangle =
                new RectangleGeometry(
                    Rectangle = new Rect(ty, 0, Width - ty, Height),
                    CornerRadius,
                    CornerRadius);

            var triangle = new StreamGeometry();

            using (var context = triangle.Open())
            {
                var yo = Height / 2;
                context.BeginFigure(new Point(0, yo), true, true);
                context.LineTo(new Point(ty, yo - tx), true, true);
                context.LineTo(new Point(ty, yo + ty), true, true);
                context.Close();
            }

            combined.Children.Add(rectangle);
            combined.Children.Add(triangle);

            combined.Freeze();

            return combined;
        }

        private Geometry PlaceLeft()
        {
            var combined = new GeometryGroup();

            var hyp = WedgeHypotenuse;
            var alpha = Wedge * Math.PI / 180d;
            var tx = Math.Sin(alpha / 2d) * hyp;
            var ty = Math.Cos(alpha / 2d) * hyp;

            var rectangle =
                new RectangleGeometry(
                    Rectangle = new Rect(0, 0, Width - ty, Height),
                    CornerRadius,
                    CornerRadius);

            var triangle = new StreamGeometry();

            using (var context = triangle.Open())
            {
                var xo = Width - ty;
                var yo = Height / 2;
                context.BeginFigure(new Point(xo, yo - tx), true, true);
                context.LineTo(new Point(xo + ty, yo), true, true);
                context.LineTo(new Point(xo, yo + ty), true, true);
                context.Close();
            }

            combined.Children.Add(rectangle);
            combined.Children.Add(triangle);

            combined.Freeze();

            return combined;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
