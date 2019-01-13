using LiveCharts.Animations;
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    public static class Initialize
    {
        public static void UI()
        {
            UIFactory.WhenDrawingBezierShapes += context => new ChartBezierShape();
            UIFactory.WhenDrawingChartContents += context => new ChartCanvas(context);
            UIFactory.WhenDrawingLabels += context => new ChartLabel();
            UIFactory.WhenDrawingLineSegments += context => new ChartLineSegment();
            UIFactory.WhenDrawingPaths += context => new ChartPath();
            UIFactory.WhenDrawingRectangles += context => new ChartRectangle();
            UIFactory.WhenDrawingSlices += context => new ChartSlice();
            UIFactory.WhenDrawingSvgPaths += context => new ChartSvgPath();
            UIFactory.WhenAnimatingSolidColorBrushes += WhenAnimatingSolidColorBrushes;
        }

        private static IAnimationBuilder WhenAnimatingSolidColorBrushes(object brush, AnimatableArguments args)
        {
            if (!(brush is SolidColorBrush solidColorBrush)) throw new LiveChartsException(149);
            return new AnimationBuilder<SolidColorBrush>(solidColorBrush, args);
        }
    }
}
