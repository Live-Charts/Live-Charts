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
        internal readonly PathHelper _strokePath = new PathHelper();
        internal readonly PathHelper _fillPath = new PathHelper();
        private Brush _fill;
        private Brush _stroke;

        public int ZIndex
        {
            get => Panel.GetZIndex(_strokePath.Path);
            set
            {
                Panel.SetZIndex(_strokePath.Path, value);
                Panel.SetZIndex(_fillPath.Path, value);
            }
        }

        public double Opacity
        {
            get => _strokePath.Path.Opacity;
            set
            {
                _strokePath.Path.Opacity = value;
                _fillPath.Path.Opacity = value;
            }
        }

        public PointD StartPoint
        {
            get => new PointD(_strokePath.Figure.StartPoint.X, _strokePath.Figure.StartPoint.Y);
            set
            {
                _strokePath.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
                _fillPath.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
            }
        }

        public double StrokeThickness
        {
            get => _strokePath.Path.StrokeThickness;
            set => _strokePath.Path.StrokeThickness = value;
        }

        public IEnumerable<double> StrokeDashArray
        {
            get => _strokePath.Path.StrokeDashArray.Select(x => x);
            set => _strokePath.Path.StrokeDashArray = new System.Windows.Media.DoubleCollection(value);
        }

        public double StrokeDashOffset
        {
            get => _strokePath.Path.StrokeDashOffset;
            set => _strokePath.Path.StrokeDashOffset = value;
        }

        public Brush Fill
        {
            get => _fill; set
            {
                if (_fill != null) _fill.Target = null;
                _fill = value;
                _fillPath.Path.Fill = _fill.AsWpfBrush();
                _fill.Target = _fillPath.Path.Fill;
            }
        }

        public Brush Stroke
        {
            get => _stroke;
            set
            {
                if (_stroke != null) _stroke.Target = null;
                _stroke = value;
                _strokePath.Path.Stroke = _stroke.AsWpfBrush();
                _stroke.Target = _strokePath.Path.Stroke;
            }
        }

        public IAnimationBuilder Animate(AnimatableArguments arguments) => new PathAnimationBuilder(this, arguments);

        public void InsertSegment(IPathSegment segment, int index)
        {
            var chartSegment = (ChartSegment)segment;
            _strokePath.Figure.Segments.Insert(index, chartSegment.PathSegment);
        }

        public void RemoveSegment(IPathSegment segment)
        {
            var chartSegment = (ChartSegment)segment;
            _strokePath.Figure.Segments.Remove(chartSegment.PathSegment);
        }

        void ICanvasElement.FlushToCanvas(ICanvas canvas, bool clippedToDrawMargin)
        {
            canvas.AddChild(_strokePath.Path, true);
            canvas.AddChild(_fillPath.Path, true);
        }

        void ICanvasElement.RemoveFromCanvas(ICanvas canvas)
        {
            canvas.RemoveChild(_strokePath.Path);
            canvas.RemoveChild(_fillPath.Path);
        }
    }
}
