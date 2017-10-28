//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The line series displays trends between points, you must add this series to a cartesian chart. 
    /// </summary>
    public class LineSeries : Series, ILineSeriesView, IFondeable, IAreaPoint
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
        /// <summary>
        /// Gets or sets the figure.
        /// </summary>
        /// <value>
        /// The figure.
        /// </value>
        protected PathFigure Figure { get; set; }
        internal Path Path { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is path initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is path initialized; otherwise, <c>false</c>.
        /// </value>
        protected bool IsPathInitialized { get; set; }
        internal List<LineSegmentSplitter> Splitters { get; set; }
        /// <summary>
        /// Gets or sets the active splitters.
        /// </summary>
        /// <value>
        /// The active splitters.
        /// </value>
        protected int ActiveSplitters { get; set; }
        /// <summary>
        /// Gets or sets the splitters collector.
        /// </summary>
        /// <value>
        /// The splitters collector.
        /// </value>
        protected int SplittersCollector { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        protected bool IsNew { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// The point geometry size property
        /// </summary>
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

        /// <summary>
        /// The point foreground property
        /// </summary>
        public static readonly DependencyProperty PointForegroundProperty = DependencyProperty.Register(
            "PointForeground", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the point shape foreground.
        /// </summary>
        public Brush PointForeground
        {
            get { return (Brush) GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); }
        }

        /// <summary>
        /// The line smoothness property
        /// </summary>
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

        /// <summary>
        /// The area limit property
        /// </summary>
        public static readonly DependencyProperty AreaLimitProperty = DependencyProperty.Register(
            "AreaLimit", typeof(double), typeof(LineSeries), new PropertyMetadata(double.NaN));
        /// <summary>
        /// Gets or sets the limit where the fill area changes orientation
        /// </summary>
        public double AreaLimit
        {
            get { return (double) GetValue(AreaLimitProperty); }
            set { SetValue(AreaLimitProperty, value); }
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// This method runs when the update starts
        /// </summary>
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

            if (IsPathInitialized)
            {
                Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
                Path.Stroke = Stroke;
                Path.StrokeThickness = StrokeThickness;
                Path.Fill = Fill;
                Path.Visibility = Visibility;
                Path.StrokeDashArray = StrokeDashArray;
                Panel.SetZIndex(Path, Panel.GetZIndex(this));
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

            Panel.SetZIndex(Path, Panel.GetZIndex(this));
      
            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;
            
            Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
        }

        /// <summary>
        /// Gets the view of a given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var mhr = PointGeometrySize < 10 ? 10 : PointGeometrySize;

            var pbv = (HorizontalBezierPointView) point.View;

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

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);

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
                        StrokeThickness = StrokeThickness
                    };
                }

                Model.Chart.View.AddToDrawMargin(pbv.Shape);
            }

            if (pbv.Shape != null)
            {
                pbv.Shape.Fill = PointForeground;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry;
                pbv.Shape.Visibility = Visibility;
                Panel.SetZIndex(pbv.Shape, Panel.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush) point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush) point.Fill;
            }

            if (DataLabels)
            {
                pbv.DataLabel = UpdateLabelContent(new DataLabelViewModel
                {
                    FormattedText = label,
                    Point = point
                }, pbv.DataLabel);
            }

            if (!DataLabels && pbv.DataLabel != null)
            {
                Model.Chart.View.RemoveFromDrawMargin(pbv.DataLabel);
                pbv.DataLabel = null;
            }

            return pbv;
        }

        /// <summary>
        /// This method runs when the update finishes
        /// </summary>
        public override void OnSeriesUpdatedFinish()
        {
            base.OnSeriesUpdatedFinish();

            foreach (var inactive in Splitters
                .Where(s => s.SplitterCollectorIndex < SplittersCollector).ToList())
            {
                Figure.Segments.Remove(inactive.Left);
                Figure.Segments.Remove(inactive.Bottom);
                Figure.Segments.Remove(inactive.Right);
                Splitters.Remove(inactive);
            }
        }

        /// <summary>
        /// Erases series
        /// </summary>
        /// <param name="removeFromView"></param>
        public override void Erase(bool removeFromView = true)
        {
            ActualValues.GetPoints(this).ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Model.Chart);
            });
            if (Path != null) Path.Visibility = Visibility.Hidden;
            if (removeFromView)
            {
                Model.Chart.View.RemoveFromDrawMargin(Path);
                Model.Chart.View.RemoveFromView(this);
            }
        }

        #endregion

        #region Public Methods         
        /// <summary>
        /// Gets the point diameter.
        /// </summary>
        /// <returns></returns>
        public double GetPointDiameter()
        {
            return (PointGeometry == null ? 0 : PointGeometrySize)/2;
        }

        /// <summary>
        /// Starts the segment.
        /// </summary>
        /// <param name="atIndex">At index.</param>
        /// <param name="location">The location.</param>
        public virtual void StartSegment(int atIndex, CorePoint location)
        {
            if (Splitters.Count <= ActiveSplitters)
                Splitters.Add(new LineSegmentSplitter {IsNew = true});

            var splitter = Splitters[ActiveSplitters];
            splitter.SplitterCollectorIndex = SplittersCollector;

            ActiveSplitters++;
            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            var areaLimit = ChartFunctions.ToDrawMargin(double.IsNaN(AreaLimit)
                ? Model.Chart.AxisY[ScalesYAt].FirstSeparator
                : AreaLimit, AxisOrientation.Y, Model.Chart, ScalesYAt);

            if (Values != null && atIndex == 0)
            {
                if (Model.Chart.View.DisableAnimations || IsNew)
                    Figure.StartPoint = new Point(location.X, areaLimit);
                else
                    Figure.BeginAnimation(PathFigure.StartPointProperty,
                        new PointAnimation(new Point(location.X, areaLimit), animSpeed));

                IsNew = false;
            }

            if (atIndex != 0)
            {
                Figure.Segments.Remove(splitter.Bottom);

                if (splitter.IsNew)
                {
                    splitter.Bottom.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                    splitter.Left.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
                }

                if (noAnim)
                    splitter.Bottom.Point = new Point(location.X, areaLimit);
                else
                    splitter.Bottom.BeginAnimation(LineSegment.PointProperty,
                        new PointAnimation(new Point(location.X, areaLimit), animSpeed));
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

        /// <summary>
        /// Ends the segment.
        /// </summary>
        /// <param name="atIndex">At index.</param>
        /// <param name="location">The location.</param>
        public virtual void EndSegment(int atIndex, CorePoint location)
        {
            var splitter = Splitters[ActiveSplitters-1];

            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            var areaLimit = ChartFunctions.ToDrawMargin(double.IsNaN(AreaLimit)
                 ? Model.Chart.AxisY[ScalesYAt].FirstSeparator
                 : AreaLimit, AxisOrientation.Y, Model.Chart, ScalesYAt);

            var uw = Model.Chart.AxisX[ScalesXAt].EvaluatesUnitWidth
                ? ChartFunctions.GetUnitWidth(AxisOrientation.X, Model.Chart, ScalesXAt)/2
                : 0;
            location.X -= uw;

            if (splitter.IsNew)
            {
                splitter.Right.Point = new Point(location.X, Model.Chart.DrawMargin.Height);
            }

            Figure.Segments.Remove(splitter.Right);
            if (noAnim)
                splitter.Right.Point = new Point(location.X, areaLimit);
            else
                splitter.Right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, areaLimit), animSpeed));
            Figure.Segments.Insert(atIndex, splitter.Right);

            splitter.IsNew = false;
        }
        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetCurrentValue(LineSmoothnessProperty, .7d);
            SetCurrentValue(PointGeometrySizeProperty, 8d);
            SetCurrentValue(PointForegroundProperty, Brushes.White);
            SetCurrentValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
            Splitters = new List<LineSegmentSplitter>();

            IsNew = true;
        }

        #endregion
    }
}
