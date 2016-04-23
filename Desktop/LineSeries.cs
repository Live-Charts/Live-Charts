//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveChartsCore;

namespace LiveChartsDesktop
{
    public class LineSeries : Series
    {
        private PathFigure _pathFigure;
        private bool _isInView;

        public LineSeries()
        {
            Model = new LineModel(this);
        }

        public LineSeries(SeriesConfiguration configuration) : base(configuration)
        {
            Model = new LineModel(this);
        }

        public static readonly DependencyProperty PointRadiusProperty = DependencyProperty.Register(
            "PointRadius", typeof (double), typeof (LineSeries), new PropertyMetadata(default(double)));

        public double PointRadius
        {
            get { return (double) GetValue(PointRadiusProperty); }
            set { SetValue(PointRadiusProperty, value); }
        }

        public static readonly DependencyProperty PointHoverBrushProperty = DependencyProperty.Register(
            "PointHoverBrush", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush), OnPropertyChanged()));

        public Brush PointHoverBrush
        {
            get { return (Brush) GetValue(PointHoverBrushProperty); }
            set { SetValue(PointHoverBrushProperty, value); }
        }

        public override void InitializeView()
        {
            if (_isInView) return;

            _isInView = true;

            var path = new Path();
            BindingOperations.SetBinding(path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("Stroke"), Source = this });
            BindingOperations.SetBinding(path, Shape.FillProperty,
                new Binding { Path = new PropertyPath("Fill"), Source = this });
            BindingOperations.SetBinding(path, Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath("StrokeThickness"), Source = this });
            BindingOperations.SetBinding(path, VisibilityProperty,
                new Binding { Path = new PropertyPath("Visibility"), Source = this });
            BindingOperations.SetBinding(path, Panel.ZIndexProperty,
                new Binding { Path = new PropertyPath(Panel.ZIndexProperty), Source = this });
            BindingOperations.SetBinding(path, Shape.StrokeDashArrayProperty,
                new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
            var geometry = new PathGeometry();
            _pathFigure = new PathFigure();
            geometry.Figures.Add(_pathFigure);
            path.Data = geometry;
            Model.Chart.View.AddToView(path);
        }

        public override IChartPointView InitializePointView()
        {
            var mhr = PointRadius < 5 ? 5 : PointRadius;

            Ellipse e = null;
            Rectangle hs = null;
            TextBlock tb = null;

            if (Model.Chart.View.IsHoverable)
            {
                hs = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Width = mhr,
                    Height = mhr
                };
                Panel.SetZIndex(hs, int.MaxValue);
                BindingOperations.SetBinding(hs, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
            }

            if (Math.Abs(PointRadius) > 0.1)
            {
                e = new Ellipse
                {
                    Width = PointRadius*2,
                    Height = PointRadius*2,
                    Stroke = PointHoverBrush,
                    StrokeThickness = 1
                };
                BindingOperations.SetBinding(e, Shape.FillProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(e, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(e, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
            }

            if (DataLabels)
            {
                tb = BindATextBlock(0);
                BindingOperations.SetBinding(tb, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
            }

            var bs = new BezierSegment();
            _pathFigure.Segments.Add(bs);

            return new PointBezierView
            {
                HoverShape = hs,
                Ellipse = e,
                DataLabel = tb,
                Segment = new BezierSegment(),
                IsNew = true
            };
        }
    }

    public class LineVisualPoint : VisualPoint
    {
        public PathFigure Owner { get; set; }
        public BezierSegment Segment { get; set; }

        internal BezierData Data { get; set; }
        public LineVisualPoint Previous { get; set; }
    }

    public class VisualPoint
    {
        public ChartPoint ChartPoint { get; set; }
        public Series Series { get; set; }
        public Shape Shape { get; set; }
        public Shape HoverShape { get; set; }
        public TextBlock TextBlock { get; set; }
        public bool IsNew { get; set; }
        public bool IsHighlighted { get; set; }
    }
}
