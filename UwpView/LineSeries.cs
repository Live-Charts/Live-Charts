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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Components;
using LiveCharts.Uwp.Points;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The line series displays trends between points, you must add this series to a cartesian chart. 
    /// </summary>
    public class LineSeries : Series, ILineSeriesView, IFondeable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of LineSeries class
        /// </summary>
        public LineSeries() 
        {
            Model = new LineAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of LineSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
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
        protected bool IsPathInitialized { get; set; }
        internal List<LineSegmentSplitter> Splitters { get; set; }
        protected int ActiveSplitters { get; set; }
        protected int SplittersCollector { get; set; }
        #endregion

        #region Properties

        public static readonly DependencyProperty PointGeometrySizeProperty = DependencyProperty.Register(
            "PointGeometrySize", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the point geometry size, increasing this property will make the series points bigger
        /// </summary>
        public double PointGeometrySize
        {
            get { return (double) GetValue(PointGeometrySizeProperty); }
            set { SetValue(PointGeometrySizeProperty, value); }
        }

        public static readonly DependencyProperty PointForeroundProperty = DependencyProperty.Register(
            "PointForeround", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the point shape foreground.
        /// </summary>
        public Brush PointForeround
        {
            get { return (Brush) GetValue(PointForeroundProperty); }
            set { SetValue(PointForeroundProperty, value); }
        }

        public static readonly DependencyProperty LineSmoothnessProperty = DependencyProperty.Register(
            "LineSmoothness", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets line smoothness, this property goes from 0 to 1, use 0 to draw straight lines, 1 really curved lines.
        /// </summary>
        public double LineSmoothness
        {
            get { return (double) GetValue(LineSmoothnessProperty); }
            set { SetValue(LineSmoothnessProperty, value); }
        }

        #endregion

        #region Overridden Methods

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

            if (Figure != null && Values != null)
            {
                var xIni = ChartFunctions.ToDrawMargin(ActualValues.GetTracker(this).XLimit.Min, AxisOrientation.X, Model.Chart, ScalesXAt);

                if (Model.Chart.View.DisableAnimations)
                    Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
                else
                    Figure.BeginAnimation(PathFigure.StartPointProperty,
                        new PointAnimation(new Point(xIni, Model.Chart.DrawMargin.Height),
                            Model.Chart.View.AnimationsSpeed));
            }

            if (IsPathInitialized)
            {
                Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
                Path.Stroke = Stroke;
                Path.StrokeThickness = StrokeThickness;
                Path.Fill = Fill;
                Path.Visibility = Visibility;
                Path.StrokeDashArray = StrokeDashArray;
                Canvas.SetZIndex(Path, Panel.GetZIndex(this));
                return;
            }

            IsPathInitialized = true;

            Path = new Path
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Fill = Fill,
                Visibility = Visibility,
                StrokeDashArray = StrokeDashArray
            };

            Canvas.SetZIndex(Path, Panel.GetZIndex(this));
      
            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;

            Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);

            var x = ChartFunctions.ToDrawMargin(ActualValues.GetTracker(this).XLimit.Min, AxisOrientation.X, Model.Chart, ScalesXAt);
            Figure.StartPoint = new Point(x, Model.Chart.DrawMargin.Height);
        }

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var mhr = PointGeometrySize < 10 ? 10 : PointGeometrySize;

            var pbv = (HorizontalBezierPointView) view;

            if (pbv == null)
            {
                pbv = new HorizontalBezierPointView
                {
                    Segment = new BezierSegment(),
                    Container = Figure,
                    IsNew = true
                };
            }
            else
            {
                pbv.IsNew = false;
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.Shape);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HoverShape);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.DataLabel);
            }

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Width = mhr,
                    Height = mhr
                };

                Canvas.SetZIndex(pbv.HoverShape, int.MaxValue);

                var wpfChart = (Chart)Model.Chart.View;
                wpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

            if (PointGeometry != null && Math.Abs(PointGeometrySize) > 0.1 && pbv.Shape == null)
            {
                if (PointGeometry != null)
                {
                    pbv.Shape = new Path
                    {
                        Stretch = Stretch.Fill,
                        ClipToBounds = true,
                        StrokeThickness = StrokeThickness
                    };
                }

                Model.Chart.View.AddToDrawMargin(pbv.Shape);
            }

            if (pbv.Shape != null)
            {
                pbv.Shape.Fill = PointForeround;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry;
                pbv.Shape.Visibility = Visibility;
                Canvas.SetZIndex(pbv.Shape, Panel.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush) point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush) point.Fill;
            }

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Canvas.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

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

        public override void Erase(bool removeFromView = true)
        {
            ActualValues.GetPoints(this).ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Model.Chart);
            });
            if (removeFromView)
            {
                Model.Chart.View.RemoveFromDrawMargin(Path);
                Model.Chart.View.RemoveFromView(this);
            }
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
            SetCurrentValue(LineSmoothnessProperty, .7d);
            SetCurrentValue(PointGeometrySizeProperty, 8d);
            SetCurrentValue(PointForeroundProperty, Brushes.White);
            SetCurrentValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
            Splitters = new List<LineSegmentSplitter>();
        }

        #endregion
    }
}
