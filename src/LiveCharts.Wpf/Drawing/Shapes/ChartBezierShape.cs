using LiveCharts.Drawing.Shapes;
using System.Windows;

namespace LiveCharts.Wpf.Drawing
{
    public class ChartBezierShape : ChartShape, IBezierShape
    {
        private readonly ChartBezierSegment _segment = new ChartBezierSegment();

        public static readonly DependencyProperty SvgProperty =
            DependencyProperty.Register("Svg", typeof(string), typeof(ChartBezierShape), new UIPropertyMetadata(null));

        public IBezierSegment Segment => _segment;

        public string Svg
        {
            get { return (string)GetValue(SvgProperty); }
            set { SetValue(SvgProperty, value); }
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                var geometry = System.Windows.Media.Geometry.Parse(Svg);
                geometry.Freeze();
                return geometry;
            }
        }
    }
}
