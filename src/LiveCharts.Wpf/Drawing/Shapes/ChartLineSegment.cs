using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Wpf.Animations;
using System.Windows;
using System.Windows.Media;

namespace LiveCharts.Wpf.Drawing
{
    public class ChartLineSegment : ILineSegment
    {
        private readonly LineSegment _segment = new LineSegment();

        public PointD Point
        {
            get => new PointD(_segment.Point.X, _segment.Point.Y);
            set => _segment.Point = new Point(value.X, value.Y);
        }

        public IAnimationBuilder Animate(AnimatableArguments args) => new AnimationBuilder<LineSegment>(_segment, args);
    }
}
