using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf.Views
{
    public class SelfDrawnPath : CartesianPath
    {
        public override void Close(IChartView view, float length, float i, float j)
        {
            var l = length / StrokePath.StrokeThickness;
            var tl = l - PreviousLength;
            var remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }

            StrokePath.StrokeDashArray = new DoubleCollection(
                Effects.GetAnimatedDashArray(StrokeDashArray, (float) (l + remaining)));
            StrokePath.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(tl + remaining, 0, view.AnimationsSpeed, FillBehavior.Stop));

            StrokePath.StrokeDashOffset = 0;
            PreviousLength = l;
        }
    }
}
