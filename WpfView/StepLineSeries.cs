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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Step line series.
    /// </summary>
    public class StepLineSeries : Series, IFondeable, IAreaPoint
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

        /// <summary>
        /// The point geometry size property
        /// </summary>
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

        /// <summary>
        /// The point foreround property
        /// </summary>
        public static readonly DependencyProperty PointForegroundProperty = DependencyProperty.Register(
            "PointForeground", typeof(Brush), typeof(StepLineSeries),
            new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the point shape foreground.
        /// </summary>
        public Brush PointForeground
        {
            get { return (Brush)GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); }
        }

        /// <summary>
        /// The alternative stroke property
        /// </summary>
        public static readonly DependencyProperty AlternativeStrokeProperty = DependencyProperty.Register(
            "AlternativeStroke", typeof (Brush), typeof (StepLineSeries), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Gets or sets the alternative stroke.
        /// </summary>
        /// <value>
        /// The alternative stroke.
        /// </value>
        public Brush AlternativeStroke
        {
            get { return (Brush) GetValue(AlternativeStrokeProperty); }
            set { SetValue(AlternativeStrokeProperty, value); }
        }

        /// <summary>
        /// The inverted mode property
        /// </summary>
        public static readonly DependencyProperty InvertedModeProperty = DependencyProperty.Register(
            "InvertedMode", typeof(bool), typeof(StepLineSeries), new PropertyMetadata(default(bool), CallChartUpdater()));
        /// <summary>
        /// Gets or sets a value indicating whether the series should be drawn using the inverted mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [inverted mode]; otherwise, <c>false</c>.
        /// </value>
        public bool InvertedMode
        {
            get { return (bool) GetValue(InvertedModeProperty); }
            set { SetValue(InvertedModeProperty, value); }
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Gets the view of a given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var pbv = (StepLinePointView) point.View;

            if (pbv == null)
            {
                pbv = new StepLinePointView
                {
                    IsNew = true,
                    Line2 = new Line(),
                    Line1 = new Line()
                };

                Model.Chart.View.AddToDrawMargin(pbv.Line2);
                Model.Chart.View.AddToDrawMargin(pbv.Line1);
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
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.Line2);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.Line1);
            }

            pbv.Line1.StrokeThickness = StrokeThickness;
            pbv.Line1.Stroke = AlternativeStroke;
            pbv.Line1.StrokeDashArray = StrokeDashArray;
            pbv.Line1.Visibility = Visibility;
            Panel.SetZIndex(pbv.Line1, Panel.GetZIndex(this));

            pbv.Line2.StrokeThickness = StrokeThickness;
            pbv.Line2.Stroke = Stroke;
            pbv.Line2.StrokeDashArray = StrokeDashArray;
            pbv.Line2.Visibility = Visibility;
            Panel.SetZIndex(pbv.Line2, Panel.GetZIndex(this));

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
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeDashArray = StrokeDashArray;
                pbv.Shape.Visibility = Visibility;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry;
                Panel.SetZIndex(pbv.Shape, Panel.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush) point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush) point.Fill;
            }

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);

                var wpfChart = (Chart) Model.Chart.View;
                wpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

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
        /// Initializes the series colors if they are not set
        /// </summary>
        public override void InitializeColors()
        {
            var wpfChart = (Chart) Model.Chart.View;

            if (Stroke != null && AlternativeStroke != null) return;

            var nextColor = wpfChart.GetNextDefaultColor();
            
            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(nextColor));
            if (AlternativeStroke == null)
                SetValue(AlternativeStrokeProperty, new SolidColorBrush(nextColor));
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Gets the point diameter.
        /// </summary>
        /// <returns></returns>
        public double GetPointDiameter()
        {
            return PointGeometrySize/2;
        }
        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetCurrentValue(PointGeometrySizeProperty, 8d);
            SetCurrentValue(PointForegroundProperty, Brushes.White);
            SetCurrentValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
        }
        #endregion
    }
}
