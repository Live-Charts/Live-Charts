using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using System.Windows;
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    public class ChartLineSegment : ChartSegment, ILineSegment
    {
        private readonly LineSegment _segment = new LineSegment();

        public PointD Point
        {
            get => new PointD(_segment.Point.X, _segment.Point.Y);
            set => _segment.Point = new Point(value.X, value.Y);
        }

        public override PathSegment PathSegment => _segment;

        public override IAnimationBuilder Animate(AnimatableArguments args) => new AnimationBuilder<LineSegment>(_segment, args);
    }
}
