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
using LiveCharts.CrossNet;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    public class LineSeries : Series, ILineSeriesView
    {
        #region Contructors

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

        #endregion

        #region Private Properties
        private PathFigure Figure { get; set; }
        private LineSegment RightSegment { get; set; }
        private LineSegment BottomSegment { get; set; }
        private LineSegment LeftSegment { get; set; }
        private Path Path { get; set; }
        private bool IsInView { get; set; }
        #endregion

#region Properties

        public static readonly DependencyProperty PointDiameterProperty = DependencyProperty.Register(
            "PointDiameter", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CalChartUpdater()));

        public double PointDiameter
        {
            get { return (double) GetValue(PointDiameterProperty); }
            set { SetValue(PointDiameterProperty, value); }
        }

        public static readonly DependencyProperty PointForeroundProperty = DependencyProperty.Register(
            "PointForeround", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush), CalChartUpdater()));

        public Brush PointForeround
        {
            get { return (Brush) GetValue(PointForeroundProperty); }
            set { SetValue(PointForeroundProperty, value); }
        }

        public static readonly DependencyProperty LineSmoothnessProperty = DependencyProperty.Register(
            "LineSmoothness", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CalChartUpdater()));

        public double LineSmoothness
        {
            get { return (double) GetValue(LineSmoothnessProperty); }
            set { SetValue(LineSmoothnessProperty, value); }
        }
        #endregion

#region Overriden Methods

        public override void OnSeriesUpdateStart()
        {
            if (IsInView) return;

            IsInView = true;

            Path = new Path();
            BindingOperations.SetBinding(Path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("Stroke"), Source = this });
            BindingOperations.SetBinding(Path, Shape.FillProperty,
                new Binding { Path = new PropertyPath("Fill"), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath("StrokeThickness"), Source = this });
            BindingOperations.SetBinding(Path, VisibilityProperty,
                new Binding { Path = new PropertyPath("Visibility"), Source = this });
            BindingOperations.SetBinding(Path, Panel.ZIndexProperty,
                new Binding { Path = new PropertyPath(Panel.ZIndexProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeDashArrayProperty,
                new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;
            Model.Chart.View.AddToDrawMargin(Path);

            var xIni = ChartFunctions.ToDrawMargin(Values.MinChartPoint.X, AxisTags.X, Model.Chart, ScalesXAt);
            var xEnd = ChartFunctions.ToDrawMargin(Values.MaxChartPoint.X, AxisTags.X, Model.Chart, ScalesXAt);

            Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
            RightSegment.Point = new Point(xEnd, Model.Chart.DrawMargin.Height);
            BottomSegment.Point = new Point(xIni, Model.Chart.DrawMargin.Height);
            LeftSegment.Point = new Point(xIni, Model.Chart.DrawMargin.Height);

            var wpfChart = Model.Chart.View as Chart;
            if (wpfChart == null) return;

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(Chart.GetDefaultColor(wpfChart.Series.IndexOf(this))));
            if (Fill == null)
                SetValue(FillProperty,
                    new SolidColorBrush(Chart.GetDefaultColor(wpfChart.Series.IndexOf(this))) {Opacity = 0.35});
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
                    Container = Figure,
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

        public override void OnSeriesUpdatedFinish()
        {
            Figure.Segments.Remove(RightSegment);
            Figure.Segments.Remove(BottomSegment);
            Figure.Segments.Remove(LeftSegment);

            Figure.Segments.Add(RightSegment);
            Figure.Segments.Add(BottomSegment);
            Figure.Segments.Insert(0, LeftSegment);

            var xIni = ChartFunctions.ToDrawMargin(Values.MinChartPoint.X, AxisTags.X, Model.Chart, ScalesXAt);
            var xEnd = ChartFunctions.ToDrawMargin(Values.MaxChartPoint.X, AxisTags.X, Model.Chart, ScalesXAt);

            if (Model.Chart.View.DisableAnimations)
            {
                Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
                RightSegment.Point = new Point(xEnd, Model.Chart.DrawMargin.Height);
                BottomSegment.Point = new Point(xIni, Model.Chart.DrawMargin.Height);
                LeftSegment.Point =
                    ChartFunctions.ToDrawMargin(Values.Points.FirstOrDefault(), ScalesXAt, ScalesYAt, Model.Chart)
                        .AsPoint();
            }
            else
            {
                var ansp = Model.Chart.View.AnimationsSpeed;

                Figure.BeginAnimation(PathFigure.StartPointProperty,
                    new PointAnimation(new Point(xIni, Model.Chart.DrawMargin.Height), ansp));

                RightSegment.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(xEnd, Model.Chart.DrawMargin.Height), ansp));

                BottomSegment.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(xIni, Model.Chart.DrawMargin.Height), ansp));

                LeftSegment.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(
                        ChartFunctions.ToDrawMargin(Values.Points.FirstOrDefault(), ScalesXAt, ScalesYAt, Model.Chart)
                            .AsPoint(), ansp));
            }
        }

        public override void Erase()
        {
            Values.Points.ForEach(p => p.View.RemoveFromView(Model.Chart));
            Model.Chart.View.RemoveFromDrawMargin(Path);
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetValue(LineSmoothnessProperty, .7d);
            SetValue(PointDiameterProperty, 8d);
            SetValue(PointForeroundProperty, Brushes.White);
            SetValue(StrokeThicknessProperty, 2d);
            RightSegment = new LineSegment(new Point(), false);
            LeftSegment = new LineSegment(new Point(), false);
            BottomSegment = new LineSegment(new Point(), false);
        }

        #endregion
    }
}
