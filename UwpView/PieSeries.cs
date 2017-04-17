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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The pie series should be added only in a pie chart.
    /// </summary>
    public class PieSeries : Series, IPieSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of PieSeries class
        /// </summary>
        public PieSeries()
        {
            Model = new PieAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of PieSeries class with a given mapper.
        /// </summary>
        /// <param name="configuration"></param>
        public PieSeries(object configuration)
        {
            Model = new PieAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        /// <summary>
        /// The push out property
        /// </summary>
        public static readonly DependencyProperty PushOutProperty = DependencyProperty.Register(
            "PushOut", typeof (double), typeof (PieSeries), new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the slice push out, this property highlights the slice
        /// </summary>
        public double PushOut
        {
            get { return (double) GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        /// <summary>
        /// The label position property
        /// </summary>
        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(
            "LabelPosition", typeof(PieLabelPosition), typeof(PieSeries),
            new PropertyMetadata(PieLabelPosition.InsideSlice, CallChartUpdater()));
        /// <summary>
        /// Gets or sets the label position.
        /// </summary>
        /// <value>
        /// The label position.
        /// </value>
        public PieLabelPosition LabelPosition
        {
            get { return (PieLabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
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
            var pbv = (PiePointView) point.View;

            if (pbv == null)
            {
                pbv = new PiePointView
                {
                    IsNew = true,
                    Slice = new PieSlice()
                };
                Model.Chart.View.AddToDrawMargin(pbv.Slice);
            }
            else
            {
                pbv.IsNew = false;
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.Slice);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HoverShape);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.DataLabel);
            }

            pbv.Slice.Fill = Fill;
            pbv.Slice.Stroke = Stroke;
            pbv.Slice.StrokeThickness = StrokeThickness;
            pbv.Slice.StrokeDashArray = StrokeDashArray;
            pbv.Slice.PushOut = PushOut;
            pbv.Slice.Visibility = Visibility;
            Canvas.SetZIndex(pbv.Slice, Canvas.GetZIndex(this));
            
            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new PieSlice
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

            if (DataLabels)
            {
                pbv.DataLabel = UpdateLabelContent(new DataLabelViewModel
                {
                    FormattedText = label,
                    Instance = point.Instance
                }, pbv.DataLabel);
            }

            pbv.OriginalPushOut = PushOut;

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            //this.SetIfNotSet(StrokeThicknessProperty, 2d);
            //this.SetIfNotSet(StrokeProperty, new SolidColorBrush(Windows.UI.Colors.White));
            //this.SetIfNotSet(ForegroundProperty, new SolidColorBrush(Windows.UI.Colors.White));

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            this.SetIfNotSet(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
