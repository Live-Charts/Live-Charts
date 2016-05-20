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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Chart;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

// ReSharper disable once CheckNamespace
namespace LiveCharts.Wpf
{
    public class LineSeries : Series.Series, ILineSeriesView
    {
        #region Contructors

        public LineSeries() 
        {
            Model = new LineAlgorithm(this);
            InitializeDefuaults();
        }

        public LineSeries(object configuration)
        {
            Model = new LineAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties
        protected PathFigure Figure { get; set; }
        protected Path Path { get; set; }
        protected bool IsInView { get; set; }
        internal List<LineSegmentSplitter> Splitters { get; set; }
        protected int ActiveSplitters { get; set; }
        protected int SplittersCollector { get; set; }
        #endregion

        #region Properties

        public static readonly DependencyProperty PointDiameterProperty = DependencyProperty.Register(
            "PointDiameter", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));

        public double PointDiameter
        {
            get { return (double) GetValue(PointDiameterProperty); }
            set { SetValue(PointDiameterProperty, value); }
        }

        public static readonly DependencyProperty PointForeroundProperty = DependencyProperty.Register(
            "PointForeround", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush)));

        public Brush PointForeround
        {
            get { return (Brush) GetValue(PointForeroundProperty); }
            set { SetValue(PointForeroundProperty, value); }
        }

        public static readonly DependencyProperty LineSmoothnessProperty = DependencyProperty.Register(
            "LineSmoothness", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));

        public double LineSmoothness
        {
            get { return (double) GetValue(LineSmoothnessProperty); }
            set { SetValue(LineSmoothnessProperty, value); }
        }

        #endregion

        #region Overriden Methods

        public override void OnSeriesUpdateStart()
        {
            ActiveSplitters = 0;

            if (SplittersCollector == int.MaxValue - 1)
            {
                //just in case!
                Splitters.ForEach(s => s.SplitterCollectorIndex = 0);
                SplittersCollector = 0;
            }

            SplittersCollector++;

            if (Figure != null)
            {
                var xIni = ChartFunctions.ToDrawMargin(Values.Limit1.Min, AxisTags.X, Model.Chart, ScalesXAt);

                if (Model.Chart.View.DisableAnimations)
                    Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
                else
                    Figure.BeginAnimation(PathFigure.StartPointProperty,
                        new PointAnimation(new Point(xIni, Model.Chart.DrawMargin.Height),
                            Model.Chart.View.AnimationsSpeed));
            }

            if (IsInView) return;

            IsInView = true;

            Path = new Path();
            BindingOperations.SetBinding(Path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(StrokeProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.FillProperty,
                new Binding { Path = new PropertyPath(FillProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
            BindingOperations.SetBinding(Path, VisibilityProperty,
                new Binding { Path = new PropertyPath(VisibilityProperty), Source = this });
            BindingOperations.SetBinding(Path, Panel.ZIndexProperty,
                new Binding { Path = new PropertyPath(Panel.ZIndexProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeDashArrayProperty,
                new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;
            Model.Chart.View.AddToDrawMargin(Path);

            var x = ChartFunctions.ToDrawMargin(Values.Limit1.Min, AxisTags.X, Model.Chart, ScalesXAt);
            Figure.StartPoint = new Point(x, Model.Chart.DrawMargin.Height);

            base.OnSeriesUpdateStart();
        }

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var mhr = PointDiameter < 10 ? 10 : PointDiameter;

            var pbv = (view as HBezierPointView);

            if (pbv == null)
            {
                pbv = new HBezierPointView
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

                var wpfChart = Model.Chart.View as Chart;
                if (wpfChart == null) return null;
                wpfChart.AttachEventsTo(pbv.HoverShape);

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

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

            return pbv;
        }

        public override void OnSeriesUpdatedFinish()
        {
            foreach (var inactive in Splitters
                .Where(s => s.SplitterCollectorIndex < SplittersCollector).ToList())
            {
                Figure.Segments.Remove(inactive.Left);
                Figure.Segments.Remove(inactive.Bottom);
                Figure.Segments.Remove(inactive.Right);
                Splitters.Remove(inactive);
            }
        }

        public override void Erase()
        {
            Values.Points.ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Model.Chart);
            });
            Model.Chart.View.RemoveFromDrawMargin(Path);
            Model.Chart.View.RemoveFromView(this);
        }

        #endregion

        #region Public Methods 

        public virtual void StartSegment(int atIndex, CorePoint location)
        {
            if (Splitters.Count <= ActiveSplitters)
                Splitters.Add(new LineSegmentSplitter {IsNew = true});

            var splitter = Splitters[ActiveSplitters];
            splitter.SplitterCollectorIndex = SplittersCollector;

            ActiveSplitters++;
            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            if (atIndex != 0)
            {
                Figure.Segments.Remove(splitter.Bottom);

                if (splitter.IsNew)
                {
                    splitter.Bottom.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                    splitter.Left.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                }

                if (noAnim)
                    splitter.Bottom.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                else
                    splitter.Bottom.BeginAnimation(LineSegment.PointProperty,
                        new PointAnimation(new Point(location.X, Model.Chart.DrawMargin.Height), animSpeed));
                Figure.Segments.Insert(atIndex, splitter.Bottom);

                Figure.Segments.Remove(splitter.Left);
                if (noAnim)
                    splitter.Left.Point = location.AsPoint();
                else
                    splitter.Left.BeginAnimation(LineSegment.PointProperty,
                        new PointAnimation(location.AsPoint(), animSpeed));
                Figure.Segments.Insert(atIndex + 1, splitter.Left);

                return;
            }

            if (splitter.IsNew)
            {
                splitter.Bottom.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                splitter.Left.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
            }

            Figure.Segments.Remove(splitter.Left);
            if (Model.Chart.View.DisableAnimations)
                splitter.Left.Point = location.AsPoint();
            else
                splitter.Left.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(location.AsPoint(), animSpeed));
            Figure.Segments.Insert(atIndex, splitter.Left);
        }

        public virtual void EndSegment(int atIndex, CorePoint location)
        {
            var splitter = Splitters[ActiveSplitters-1];

            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            if (splitter.IsNew)
            {
                splitter.Right.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
            }

            Figure.Segments.Remove(splitter.Right);
            if (noAnim)
                splitter.Right.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
            else
                splitter.Right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, Model.Chart.DrawMargin.Height), animSpeed));
            Figure.Segments.Insert(atIndex, splitter.Right);

            splitter.IsNew = false;
        }
        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetValue(LineSmoothnessProperty, .7d);
            SetValue(PointDiameterProperty, 8d);
            SetValue(PointForeroundProperty, Brushes.White);
            SetValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
            Splitters = new List<LineSegmentSplitter>();
        }

        #endregion
    }
}
