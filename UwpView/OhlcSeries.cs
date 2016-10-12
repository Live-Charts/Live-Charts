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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Points;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The OHCL series defines a financial series, add this series to a cartesian chart
    /// </summary>
    public class OhlcSeries : Series, IFinancialSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of OhclSeries class
        /// </summary>
        public OhlcSeries()
        {
            Model = new OhlcAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of OhclSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public OhlcSeries(object configuration)
        {
            Model = new ColumnAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register(
            "MaxColumnWidth", typeof (double), typeof (OhlcSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the maximum with of a point, a point will be capped to this width.
        /// </summary>
        public double MaxColumnWidth
        {
            get { return (double) GetValue(MaxColumnWidthProperty); }
            set { SetValue(MaxColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty IncreaseBrushProperty = DependencyProperty.Register(
            "IncreaseBrush", typeof (Brush), typeof (OhlcSeries), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the brush of the point when close value is grater than open value
        /// </summary>
        public Brush IncreaseBrush
        {
            get { return (Brush) GetValue(IncreaseBrushProperty); }
            set { SetValue(IncreaseBrushProperty, value); }
        }

        public static readonly DependencyProperty DecreaseBrushProperty = DependencyProperty.Register(
            "DecreaseBrush", typeof (Brush), typeof (OhlcSeries), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the brush of the point when close value is less than open value
        /// </summary>
        public Brush DecreaseBrush
        {
            get { return (Brush) GetValue(DecreaseBrushProperty); }
            set { SetValue(DecreaseBrushProperty, value); }
        }
        
        #endregion

        #region Overridden Methods

        public override void OnSeriesUpdateStart()
        {
            //do nothing on updateStart
        }

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var pbv = (OhlcPointView) view;

            if (pbv == null)
            {
                pbv = new OhlcPointView
                {
                    IsNew = true,
                    HighToLowLine = new Line(),
                    OpenLine = new Line(),
                    CloseLine = new Line()
                };

                Model.Chart.View.AddToDrawMargin(pbv.HighToLowLine);
                Model.Chart.View.AddToDrawMargin(pbv.OpenLine);
                Model.Chart.View.AddToDrawMargin(pbv.CloseLine);
            }
            else
            {
                pbv.IsNew = false;
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HighToLowLine);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.OpenLine);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.CloseLine);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HoverShape);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.DataLabel);
            }

            pbv.HighToLowLine.StrokeThickness = StrokeThickness;
            pbv.CloseLine.StrokeThickness = StrokeThickness;
            pbv.OpenLine.StrokeThickness = StrokeThickness;

            pbv.HighToLowLine.StrokeDashArray = StrokeDashArray;
            pbv.CloseLine.StrokeDashArray = StrokeDashArray;
            pbv.OpenLine.StrokeDashArray = StrokeDashArray;

            pbv.HighToLowLine.Visibility = Visibility;
            pbv.CloseLine.Visibility = Visibility;
            pbv.OpenLine.Visibility = Visibility;

            var i = Canvas.GetZIndex(this);
            Canvas.SetZIndex(pbv.HighToLowLine, i);
            Canvas.SetZIndex(pbv.CloseLine, i);
            Canvas.SetZIndex(pbv.OpenLine, i);

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    StrokeThickness = 0
                };

                Canvas.SetZIndex(pbv.HoverShape, short.MaxValue);

                var uwpfChart = (Chart)Model.Chart.View;
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

            if (point.Open < point.Close)
            {
                pbv.HighToLowLine.Stroke = IncreaseBrush;
                pbv.CloseLine.Stroke = IncreaseBrush;
                pbv.OpenLine.Stroke = IncreaseBrush;
            }
            else
            {
                pbv.HighToLowLine.Stroke = DecreaseBrush;
                pbv.CloseLine.Stroke = DecreaseBrush;
                pbv.OpenLine.Stroke = DecreaseBrush;
            }

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            /*Current*/SetValue(StrokeThicknessProperty, 2.5d);
            /*Current*/SetValue(MaxColumnWidthProperty, 35d);
            /*Current*/SetValue(MaxWidthProperty, 25d);
            /*Current*/SetValue(IncreaseBrushProperty, new SolidColorBrush(Color.FromArgb(255, 254, 178, 0)));
            /*Current*/SetValue(DecreaseBrushProperty, new SolidColorBrush(Color.FromArgb(255, 238, 83, 80)));

            Func<ChartPoint, string> defaultLabel = x =>
                string.Format("O: {0}, H: {1}, L: {2} C: {3}", x.Open, x.High, x.Low, x.Close);
            /*Current*/
            SetValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
