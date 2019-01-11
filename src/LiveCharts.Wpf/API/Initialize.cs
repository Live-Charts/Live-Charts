using LiveCharts.Animations;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Wpf
{
    public static class Initialize
    {
        public static void UI()
        {
            UIFactory.WhenDrawingBezierShapes += context => new ChartBezierShape();
            UIFactory.WhenDrawingChartContents += context => new ChartCanvas(context);
            UIFactory.WhenDrawingHeatShapes += context => new ChartHeatShape();
            UIFactory.WhenDrawingLabels += context => new ChartLabel();
            UIFactory.WhenDrawingLineSegments += context => new ChartLineSegment();
            UIFactory.WhenDrawingPaths += context => new ChartPath();
            UIFactory.WhenDrawingRectangles += context => new ChartRectangle();
            UIFactory.WhenDrawingSlices += context => new ChartSlice();
            UIFactory.WhenDrawingSvgPaths += context => new ChartSvgPath();
            UIFactory.WhenAnimatingSolidBrushes += (shape, brush, args) => brush.Animate(shape, args);
        }
    }
}
