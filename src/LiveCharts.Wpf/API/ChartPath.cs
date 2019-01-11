using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction.Controls;

namespace LiveCharts.Wpf
{
    public class ChartPath : IPath
    {
        internal readonly PathHelper _stroke = new PathHelper();
        internal readonly PathHelper _fill = new PathHelper();

        public int ZIndex
        {
            get => Panel.GetZIndex(_stroke.Path);
            set
            {
                Panel.SetZIndex(_stroke.Path, value);
                Panel.SetZIndex(_fill.Path, value);
            }
        }

        public double Opacity
        {
            get => _stroke.Path.Opacity;
            set
            {
                _stroke.Path.Opacity = value;
                _fill.Path.Opacity = value;
            }
        }

        public PointD StartPoint
        {
            get => new PointD(_stroke.Figure.StartPoint.X, _stroke.Figure.StartPoint.Y);
            set
            {
                _stroke.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
                _fill.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
            }
        }

        public double StrokeThickness
        {
            get => _stroke.Path.StrokeThickness;
            set => _stroke.Path.StrokeThickness = value;
        }

        public IEnumerable<double> StrokeDashArray
        {
            get => _stroke.Path.StrokeDashArray.Select(x => x);
            set => _stroke.Path.StrokeDashArray = new System.Windows.Media.DoubleCollection(value);
        }

        public double StrokeDashOffset
        {
            get => _stroke.Path.StrokeDashOffset;
            set => _stroke.Path.StrokeDashOffset = value;
        }

        public IAnimationBuilder Animate(AnimatableArguments arguments) => new PathAnimationBuilder(this, arguments);

        public void InsertSegment(IPathSegment segment, int index)
        {
            var chartSegment = (ChartSegment)segment;
            _stroke.Figure.Segments.Insert(index, chartSegment.PathSegment);
        }

        public void RemoveSegment(IPathSegment segment)
        {
            var chartSegment = (ChartSegment)segment;
            _stroke.Figure.Segments.Remove(chartSegment.PathSegment);
        }

        public void Paint(IBrush stroke, IBrush fill)
        {
            _stroke.Path.Fill = fill.AsWpfBrush();
            _fill.Path.Stroke = stroke.AsWpfBrush();
        }

        void ICanvasElement.FlushToCanvas(ICanvas canvas, bool clippedToDrawMargin)
        {
            canvas.AddChild(_stroke.Path, true);
            canvas.AddChild(_fill.Path, true);
        }

        void ICanvasElement.RemoveFromCanvas(ICanvas canvas)
        {
            canvas.RemoveChild(_stroke.Path);
            canvas.RemoveChild(_fill.Path);
        }
    }
}
