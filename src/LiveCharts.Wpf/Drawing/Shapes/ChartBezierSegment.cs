using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Wpf.Animations;
using System.Windows;
using System.Windows.Media;

namespace LiveCharts.Wpf.Drawing
{
    public class ChartBezierSegment : IBezierSegment
    {
        private readonly BezierSegment _segment = new BezierSegment();

        public PointD Point1
        {
            get => new PointD(_segment.Point1.X, _segment.Point1.Y);
            set => _segment.Point1 = new Point(value.X, value.Y);
        }

        public PointD Point2
        {
            get => new PointD(_segment.Point2.X, _segment.Point2.Y);
            set => _segment.Point2 = new Point(value.X, value.Y);
        }

        public PointD Point3
        {
            get => new PointD(_segment.Point3.X, _segment.Point3.Y);
            set => _segment.Point3 = new Point(value.X, value.Y);
        }

        public IAnimationBuilder Animate(AnimatableArguments args) => new AnimationBuilder<BezierSegment>(_segment, args);
    }
}
