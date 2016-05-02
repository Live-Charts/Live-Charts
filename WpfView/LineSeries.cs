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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    public class LineSeries : Series, ILineSeriesView
    {
        private PathFigure _pathFigure;
        private readonly LineSegment _right = new LineSegment(new Point(), false);
        private readonly LineSegment _bottom = new LineSegment(new Point(), false);
        private readonly LineSegment _left = new LineSegment(new Point(), false);

        private bool _isInView;

        public LineSeries()
        {
            Model = new LineAlgorithm(this);
            InitializeDefuaults();
        }

        public LineSeries(SeriesConfiguration configuration)
        {
            Model = new LineAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        public static readonly DependencyProperty PointDiameterProperty = DependencyProperty.Register(
            "PointDiameter", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), UpdateChart()));

        public double PointDiameter
        {
            get { return (double) GetValue(PointDiameterProperty); }
            set { SetValue(PointDiameterProperty, value); }
        }

        public static readonly DependencyProperty PointForeroundProperty = DependencyProperty.Register(
            "PointForeround", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush), UpdateChart()));

        public Brush PointForeround
        {
            get { return (Brush) GetValue(PointForeroundProperty); }
            set { SetValue(PointForeroundProperty, value); }
        }

        public static readonly DependencyProperty LineSmoothnessProperty = DependencyProperty.Register(
            "LineSmoothness", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), UpdateChart()));

        public double LineSmoothness
        {
            get { return (double) GetValue(LineSmoothnessProperty); }
            set { SetValue(LineSmoothnessProperty, value); }
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
            Model.Chart.View.AddToDrawMargin(path);

            var wpfChart = Model.Chart.View as Chart;
            if (wpfChart == null) return;

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(Chart.GetDefaultColor(wpfChart.Series.IndexOf(this))));
            if (Fill == null)
                SetValue(FillProperty,
                    new SolidColorBrush(Chart.GetDefaultColor(wpfChart.Series.IndexOf(this))) { Opacity = 0.35 });

            _pathFigure.StartPoint = new Point(0, Model.Chart.DrawMargin.Height);
        }

        public override IChartPointView RenderPoint(IChartPointView view)
        {
            var mhr = PointDiameter < 5 ? 5 : PointDiameter;

            var pbv = (view as HorizontalBezierView);

            if (pbv == null)
            {
                pbv = new HorizontalBezierView
                {
                    Segment = new BezierSegment(),
                    Container = _pathFigure,
                    IsNew = true
                };
            }
            else
            {
                pbv.IsNew = false;
            }

            if ((Model.Chart.View.HasTooltip || Model.Chart.View.HasDataClickEventAttached) && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Width = mhr,
                    Height = mhr
                };

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);
                BindingOperations.SetBinding(pbv.HoverShape, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                if (Model.Chart.View.HasDataClickEventAttached)
                {
                    var wpfChart = Model.Chart.View as Chart;
                    if (wpfChart == null) return null;
                    pbv.HoverShape.MouseDown += wpfChart.DataMouseDown;
                }

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (Math.Abs(PointDiameter) > 0.1 && pbv.Ellipse == null)
            {
                pbv.Ellipse = new Ellipse();

                BindingOperations.SetBinding(pbv.Ellipse, Shape.FillProperty,
                    new Binding {Path = new PropertyPath(PointForeroundProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
                BindingOperations.SetBinding(pbv.Ellipse, WidthProperty,
                    new Binding {Path = new PropertyPath(PointDiameterProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, HeightProperty,
                    new Binding {Path = new PropertyPath(PointDiameterProperty), Source = this});

                BindingOperations.SetBinding(pbv.Ellipse, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                Panel.SetZIndex(pbv.Ellipse, int.MaxValue - 2);

                Model.Chart.View.AddToDrawMargin(pbv.Ellipse);
            }

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Panel.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            return pbv;
        }

        public override void RemovePointView(object view)
        {
            var bezierView = view as HorizontalBezierView;
            if (bezierView == null) return;
            Model.Chart.View.RemoveFromView(bezierView.HoverShape);
            Model.Chart.View.RemoveFromView(bezierView.Ellipse);
            Model.Chart.View.RemoveFromView(bezierView.DataLabel);
            bezierView.Container.Segments.Remove(bezierView.Segment);
        }

        public override void CloseView()
        {
            _pathFigure.Segments.Remove(_right);
            _pathFigure.Segments.Remove(_bottom);
            _pathFigure.Segments.Remove(_left);

            _pathFigure.Segments.Add(_right);
            _pathFigure.Segments.Add(_bottom);
            _pathFigure.Segments.Insert(0, _left);

            var fp = Values.Points.FirstOrDefault() ?? new ChartPoint();
            var lp = Values.Points.Reverse().FirstOrDefault() ?? new ChartPoint();

            if (Model.Chart.View.DisableAnimations)
            {
                _pathFigure.StartPoint = new Point(fp.ChartLocation.X, Model.Chart.DrawMargin.Height);
                _right.Point = new Point(lp.ChartLocation.X, Model.Chart.DrawMargin.Height);
                _bottom.Point = new Point(fp.ChartLocation.X, Model.Chart.DrawMargin.Height);   
                _left.Point = ChartFunctions.ToDrawMargin(fp, ScalesXAt, ScalesYAt, Model.Chart).AsPoint();
            }
            else
            {
                _pathFigure.BeginAnimation(PathFigure.StartPointProperty,
                    new PointAnimation(new Point(fp.ChartLocation.X, Model.Chart.DrawMargin.Height), Model.Chart.View.AnimationsSpeed));

                _right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(lp.ChartLocation.X, Model.Chart.DrawMargin.Height),
                        Model.Chart.View.AnimationsSpeed));

                _bottom.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(fp.ChartLocation.X, Model.Chart.DrawMargin.Height), Model.Chart.View.AnimationsSpeed));

                _left.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(ChartFunctions.ToDrawMargin(fp, ScalesXAt, ScalesYAt, Model.Chart).AsPoint(),
                        Model.Chart.View.AnimationsSpeed));
            }
        }

        private void InitializeDefuaults()
        {
            SetValue(LineSmoothnessProperty, .7d);
            SetValue(PointDiameterProperty, 8d);
            SetValue(PointForeroundProperty, Brushes.White);
            SetValue(StrokeThicknessProperty, 2d);
        }
    }
}
