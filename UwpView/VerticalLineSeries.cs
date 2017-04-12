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
using Windows.Foundation;
using Windows.Perception.Spatial;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The vertical line series is useful to compare trends, this is the inverted version of the LineSeries, this series must be added in a cartesian chart.
    /// </summary>
    public class VerticalLineSeries : LineSeries
    {
        #region Constructors
        /// <summary>
        /// Initializes an new instance of VerticalLineSeries class
        /// </summary>
        public VerticalLineSeries()
        {
            Model = new VerticalLineAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes an new instance of VerticalLineSeries class, with a given mapper
        /// </summary>
        public VerticalLineSeries(object configuration)
        {
            Model = new VerticalLineAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
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
                Canvas.SetZIndex(Path, Canvas.GetZIndex(this));
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

            Canvas.SetZIndex(Path, Canvas.GetZIndex(this));

            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;

            Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
        }

        /// <summary>
        /// Gets the point view.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var mhr = PointGeometrySize < 10 ? 10 : PointGeometrySize;

            var pbv = (VerticalBezierPointView)point.View;

            if (pbv == null)
            {
                pbv = new VerticalBezierPointView
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
                    Fill = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    StrokeThickness = 0,
                    Width = mhr,
                    Height = mhr
                };

                Canvas.SetZIndex(pbv.HoverShape, short.MaxValue);

                var uwpfChart = (Chart)Model.Chart.View;
                uwpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

            if (Math.Abs(PointGeometrySize) > 0.1 && pbv.Shape == null)
            {
                pbv.Shape = new Path
                {
                    Stretch = Stretch.Fill,
                    StrokeThickness = StrokeThickness
                };

                Model.Chart.View.AddToDrawMargin(pbv.Shape);
            }

            if (pbv.Shape != null)
            {
                pbv.Shape.Fill = PointForeround;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry.Parse();
                pbv.Shape.Visibility = Visibility;
                Canvas.SetZIndex(pbv.Shape, Canvas.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush)point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush)point.Fill;
            }

            if (DataLabels)
            {
                pbv.DataLabel = UpdateLabelContent(new DataLabelViewModel
                {
                    FormattedText = label,
                    Instance = point.Instance
                }, pbv.DataLabel);
            }

            return pbv;
        }
        #endregion

        #region Public Methods 

        /// <summary>
        /// Starts the segment.
        /// </summary>
        /// <param name="atIndex">At index.</param>
        /// <param name="location">The location.</param>
        public override void StartSegment(int atIndex, CorePoint location)
        {
            if (Splitters.Count <= ActiveSplitters)
                Splitters.Add(new LineSegmentSplitter { IsNew = true });

            var splitter = Splitters[ActiveSplitters];
            splitter.SplitterCollectorIndex = SplittersCollector;

            ActiveSplitters++;
            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            var areaLimit = ChartFunctions.ToDrawMargin(double.IsNaN(AreaLimit)
                ? Model.Chart.AxisX[ScalesXAt].FirstSeparator
                : AreaLimit, AxisOrientation.X, Model.Chart, ScalesXAt);

            if (Values != null && atIndex == 0)
            {
                if (Model.Chart.View.DisableAnimations || IsNew)
                    Figure.StartPoint = new Point(areaLimit, location.Y);
                else
                    Figure.BeginPointAnimation(nameof(PathFigure.StartPoint),
                        new Point(areaLimit, location.Y), animSpeed);

                IsNew = false;
            }

            if (atIndex != 0)
            {
                Figure.Segments.Remove(splitter.Bottom);

                if (splitter.IsNew)
                {
                    splitter.Bottom.Point = new Point(Model.Chart.DrawMargin.Width, location.Y);
                    splitter.Left.Point = new Point(Model.Chart.DrawMargin.Width, location.Y);
                }

                if (noAnim)
                    splitter.Bottom.Point = new Point(Model.Chart.DrawMargin.Width, location.Y);
                else
                    splitter.Bottom.BeginPointAnimation(nameof(LineSegment.Point),
                        new Point(Model.Chart.DrawMargin.Width, location.Y), animSpeed);
                Figure.Segments.Insert(atIndex, splitter.Bottom);

                Figure.Segments.Remove(splitter.Left);
                if (noAnim)
                    splitter.Left.Point = location.AsPoint();
                else
                    splitter.Left.BeginPointAnimation(nameof(LineSegment.Point), 
                        location.AsPoint(), animSpeed);
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
                splitter.Left.BeginPointAnimation(nameof(LineSegment.Point),
                    location.AsPoint(), animSpeed);
            Figure.Segments.Insert(atIndex, splitter.Left);
        }

        /// <summary>
        /// Ends the segment.
        /// </summary>
        /// <param name="atIndex">At index.</param>
        /// <param name="location">The location.</param>
        public override void EndSegment(int atIndex, CorePoint location)
        {
            var splitter = Splitters[ActiveSplitters - 1];

            var animSpeed = Model.Chart.View.AnimationsSpeed;
            var noAnim = Model.Chart.View.DisableAnimations;

            var areaLimit = ChartFunctions.ToDrawMargin(double.IsNaN(AreaLimit)
                 ? Model.Chart.AxisX[ScalesXAt].FirstSeparator
                 : AreaLimit, AxisOrientation.X, Model.Chart, ScalesXAt);

            var uw = Model.Chart.AxisY[ScalesYAt].EvaluatesUnitWidth
                ? ChartFunctions.GetUnitWidth(AxisOrientation.Y, Model.Chart, ScalesYAt) / 2
                : 0;
            location.Y -= uw;

            if (splitter.IsNew)
            {
                splitter.Right.Point = new Point(0, location.Y);
            }

            Figure.Segments.Remove(splitter.Right);
            if (noAnim)
                splitter.Right.Point = new Point(areaLimit, location.Y);
            else
                splitter.Right.BeginPointAnimation(nameof(LineSegment.Point),
                    new Point(areaLimit, location.Y), animSpeed);
            Figure.Segments.Insert(atIndex, splitter.Right);

            splitter.IsNew = false;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            Func<ChartPoint, string> defaultLabel = x => Model.CurrentXAxis.GetFormatter()(x.X);
            this.SetIfNotSet(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
            Splitters = new List<LineSegmentSplitter>();
        }

        #endregion
    }
}
