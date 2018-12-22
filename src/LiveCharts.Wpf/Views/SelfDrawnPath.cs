using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.Views
{
    public class SelfDrawnCartesianPath : CartesianPath
    {
        private readonly LineSegment _startSegment = new LineSegment();
        private readonly LineSegment _endSegment = new LineSegment();

        public override void SetStyle(
            PointF startPoint,
            PointF pivot,
            IBrush stroke,
            IBrush fill,
            double strokeThickness,
            int zIndex,
            float[] strokeDashArray)
        {
            if (IsNew)
            {
                StrokePathFigure.StartPoint = startPoint.AsWpf();
            }
            else
            {
                StrokePathFigure.Animate(TimeLine)
                    .Property(PathFigure.StartPointProperty, StrokePathFigure.StartPoint, startPoint.AsWpf())
                    .Begin();
            }

            StrokePath.Stroke = stroke.AsWpfBrush();
            StrokePath.Fill = null;
            StrokePath.StrokeThickness = strokeThickness;
            StrokeDashArray = strokeDashArray;
            StrokePath.StrokeDashOffset = 0;
            Panel.SetZIndex(StrokePath, zIndex);

            FillPathFigure.StartPoint = pivot.AsWpf();
            _startSegment.Point = startPoint.AsWpf();
            FillPathFigure.Segments.Remove(_startSegment);
            FillPathFigure.Segments.Insert(0, _startSegment);
            FillPath.Fill = fill.AsWpfBrush();
            FillPath.StrokeThickness = 0;
            Panel.SetZIndex(FillPath, zIndex);
        }

        public override void Close(
            IChartView view,
            PointF pivot,
            float length,
            float i,
            float j)
        {
            double l = length / StrokePath.StrokeThickness;
            double tl = l - PreviousLength;
            double remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }

            var effect = Effects.GetAnimatedDashArray(StrokeDashArray.AsEnumerable(), (float) (l + remaining));
            StrokePath.StrokeDashArray = new DoubleCollection(effect.Select(x => (double) x));
            StrokePath.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(tl + remaining, 0, view.AnimationsSpeed, FillBehavior.Stop));

            _endSegment.Point = pivot.AsWpf();

            FillPathFigure.Segments.Remove(_endSegment);
            FillPathFigure.Segments.Add(_endSegment);
            var opacityAnimation = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(TimeLine.Duration.TotalMilliseconds * 2)
            };

            opacityAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            opacityAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));
            opacityAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1)));

            if (IsNew)
            {
                FillPath.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
                IsNew = false;
            }

            StrokePath.StrokeDashOffset = 0;
            PreviousLength = l;
        }
    }
}
