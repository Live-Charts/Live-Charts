using LiveCharts.Drawing.Shapes;
using System.Windows;
using System.Windows.Media;
using Geometry = System.Windows.Media.Geometry;

namespace LiveCharts.Wpf.Drawing
{
    public class ChartRectangle : ChartShape, IRectangle
    {
        public static readonly DependencyProperty XRadiusProperty =
            DependencyProperty.Register("XRadius", typeof(double), typeof(ChartRectangle), new UIPropertyMetadata(0d));

        public static readonly DependencyProperty YRadiusProperty =
            DependencyProperty.Register("YRadius", typeof(double), typeof(ChartRectangle), new UIPropertyMetadata(0d));

        public double XRadius
        {
            get => (double)GetValue(XRadiusProperty);
            set => SetValue(XRadiusProperty, value);
        }

        public double YRadius
        {
            get => (double)GetValue(YRadiusProperty);
            set => SetValue(YRadiusProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new RectangleGeometry(
                    new Rect(Left, Top, Width, Height),
                    XRadius,
                    YRadius,
                    GetTransform());

                geometry.Freeze();
                return geometry;
            }
        }
    }
}
