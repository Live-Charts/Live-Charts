using System.Collections.Generic;
using System.Windows.Controls;
using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Wpf.Drawing
{
    public class ChartPath : IPath
    {
        private readonly PaintablePath _stroke = new PaintablePath();
        private readonly PaintablePath _fill = new PaintablePath();
        private int _zIndex;
        private double _opacity;
        private PointD _startPoint = new PointD();

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                Panel.SetZIndex(_stroke.Path, value);
                Panel.SetZIndex(_fill.Path, value);
            }
        }

        public double Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                _stroke.Path.Opacity = value;
                _fill.Path.Opacity = value;
            }
        }

        public PointD StartPoint
        {
            get => _startPoint;
            set
            {
                _startPoint = value;
                _stroke.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
                _fill.Figure.StartPoint = new System.Windows.Point(value.X, value.Y);
            }
        }

        public double StrokeThickness { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public IEnumerable<double> StrokeDashArray { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public double StrokeDashOffset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public IAnimationBuilder Animate(Transition timeline)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IPaintable> GetPaintables()
        {
            yield return _fill;
            yield return _stroke;
        }

        public void InsertSegment(ISegment segment, int index)
        {
            throw new System.NotImplementedException();
        }

        public void Paint(IBrush stroke, IBrush fill)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveSegment(ISegment segment)
        {
            throw new System.NotImplementedException();
        }
    }
}
