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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Components;
using LiveCharts.Uwp.Points;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// ThesStep line series 
    /// </summary>
    public class StepLineSeries : Series, IFondeable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of BubbleSeries class
        /// </summary>
        public StepLineSeries()
        {
            Model = new StepLineAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of BubbleSeries class using a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public StepLineSeries(object configuration)
        {
            Model = new ScatterAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        public static readonly DependencyProperty PointGeometrySizeProperty = DependencyProperty.Register(
           "PointGeometrySize", typeof(double), typeof(StepLineSeries),
           new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the point geometry size, increasing this property will make the series points bigger
        /// </summary>
        public double PointGeometrySize
        {
            get { return (double)GetValue(PointGeometrySizeProperty); }
            set { SetValue(PointGeometrySizeProperty, value); }
        }

        public static readonly DependencyProperty PointForeroundProperty = DependencyProperty.Register(
            "PointForeround", typeof(Brush), typeof(StepLineSeries),
            new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the point shape foreground.
        /// </summary>
        public Brush PointForeround
        {
            get { return (Brush)GetValue(PointForeroundProperty); }
            set { SetValue(PointForeroundProperty, value); }
        }

        public static readonly DependencyProperty AlternativeStrokeProperty = DependencyProperty.Register(
            "AlternativeStroke", typeof (Brush), typeof (StepLineSeries), new PropertyMetadata(default(Brush)));

        public Brush AlternativeStroke
        {
            get { return (Brush) GetValue(AlternativeStrokeProperty); }
            set { SetValue(AlternativeStrokeProperty, value); }
        }

        #endregion

        #region Overridden Methods

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var pbv = (StepLinePointView) view;

            if (pbv == null)
            {
                pbv = new StepLinePointView
                {
                    IsNew = true,
                    HorizontalLine = new Line(),
                    VerticalLine = new Line()
                };

                Model.Chart.View.AddToDrawMargin(pbv.HorizontalLine);
                Model.Chart.View.AddToDrawMargin(pbv.VerticalLine);
                Model.Chart.View.AddToDrawMargin(pbv.Shape);
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
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HorizontalLine);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.VerticalLine);
            }

            pbv.VerticalLine.StrokeThickness = StrokeThickness;
            pbv.VerticalLine.Stroke = AlternativeStroke;
            pbv.VerticalLine.StrokeDashArray = StrokeDashArray;
            pbv.VerticalLine.Visibility = Visibility;
            Canvas.SetZIndex(pbv.VerticalLine, Canvas.GetZIndex(this));

            pbv.HorizontalLine.StrokeThickness = StrokeThickness;
            pbv.HorizontalLine.Stroke = Stroke;
            pbv.HorizontalLine.StrokeDashArray = StrokeDashArray;
            pbv.HorizontalLine.Visibility = Visibility;
            Canvas.SetZIndex(pbv.HorizontalLine, Canvas.GetZIndex(this));

            if (PointGeometry != null && Math.Abs(PointGeometrySize) > 0.1 && pbv.Shape == null)
            {
                if (PointGeometry != null)
                {
                    pbv.Shape = new Path
                    {
                        Stretch = Stretch.Fill,
                        //ClipToBounds = true,
                        StrokeThickness = StrokeThickness
                    };
                }
                Model.Chart.View.AddToDrawMargin(pbv.Shape);
            }

            if (pbv.Shape != null)
            {
                pbv.Shape.Fill = PointForeround;
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeDashArray = StrokeDashArray;
                pbv.Shape.Visibility = Visibility;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry;
                Canvas.SetZIndex(pbv.Shape, Canvas.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush) point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush) point.Fill;
            }

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    StrokeThickness = 0
                };

                Canvas.SetZIndex(pbv.HoverShape, short.MaxValue);

                var uwpfChart = (Chart) Model.Chart.View;
                uwpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Canvas.SetZIndex(pbv.DataLabel, short.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

            return pbv;
        }

        public override void InitializeColors()
        {
            var uwpfChart = (Chart) Model.Chart.View;

            if (Stroke != null && AlternativeStroke != null) return;

            var nextColor = uwpfChart.GetNextDefaultColor();

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(nextColor));
            if (AlternativeStroke == null)
                SetValue(AlternativeStrokeProperty, new SolidColorBrush(nextColor));
            if (Fill == null)
                SetValue(FillProperty, new SolidColorBrush(nextColor) { Opacity = DefaultFillOpacity });
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            /*Current*/SetValue(PointGeometrySizeProperty, 8d);
            /*Current*/SetValue(PointForeroundProperty, new SolidColorBrush(Windows.UI.Colors.White));
            /*Current*/SetValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            /*Current*/SetValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
        }
        #endregion
    }
}
