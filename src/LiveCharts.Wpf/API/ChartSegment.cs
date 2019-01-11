using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    public abstract class ChartSegment : IPathSegment
    {
        public abstract PathSegment PathSegment { get; }

        public abstract IAnimationBuilder Animate(AnimatableArguments args);
    }
}
