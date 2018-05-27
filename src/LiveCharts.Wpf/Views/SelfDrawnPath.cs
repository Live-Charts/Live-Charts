using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Wpf.Animations;
using Brush = LiveCharts.Core.Drawing.Brush;

namespace LiveCharts.Wpf.Views
{
    public class SelfDrawnCartesianPath : CartesianPath
    {
        private readonly LineSegment _startSegment = new LineSegment();
        private readonly LineSegment _endSegment = new LineSegment();

        public override void SetStyle(
            PointF startPoint,
            PointF pivot,
            Brush stroke,
            Brush fill,
            double strokeThickness,
            int zIndex,
            IEnumerable<double> strokeDashArray)
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

            StrokePath.Stroke = stroke.AsWpf();
            StrokePath.Fill = null;
            StrokePath.StrokeThickness = strokeThickness;
            StrokeDashArray = strokeDashArray;
            StrokePath.StrokeDashOffset = 0;
            Panel.SetZIndex(StrokePath, zIndex);

            FillPathFigure.StartPoint = pivot.AsWpf();
            _startSegment.Point = startPoint.AsWpf();
            FillPathFigure.Segments.Remove(_startSegment);
            FillPathFigure.Segments.Insert(0, _startSegment);
            FillPath.Fill = fill.AsWpf();
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
