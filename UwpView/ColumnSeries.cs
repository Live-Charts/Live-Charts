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
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// Use the column series to plot horizontal bars in a cartesian chart
    /// </summary>
    public class ColumnSeries : Series, IColumnSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of ColumnSeries class
        /// </summary>
        public ColumnSeries()
        {
            Model = new ColumnAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of ColumnSeries class, using a given mapper
        /// </summary>
        public ColumnSeries(object configuration)
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
            "MaxColumnWidth", typeof (double), typeof (ColumnSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the MaxColumnWidht in pixels, the column width will be capped at this value.
        /// </summary>
        public double MaxColumnWidth
        {
            get { return (double) GetValue(MaxColumnWidthProperty); }
            set { SetValue(MaxColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty ColumnPaddingProperty = DependencyProperty.Register(
            "ColumnPadding", typeof (double), typeof (ColumnSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the padding between the columns in the series.
        /// </summary>
        public double ColumnPadding
        {
            get { return (double) GetValue(ColumnPaddingProperty); }
            set { SetValue(ColumnPaddingProperty, value); }
        }

        public static readonly DependencyProperty LabelsPositionProperty = DependencyProperty.Register(
            "LabelsPosition", typeof (BarLabelPosition), typeof (ColumnSeries), 
            new PropertyMetadata(default(BarLabelPosition), CallChartUpdater()));
        /// <summary>
        /// Gets or sets where the label is placed
        /// </summary>
        public BarLabelPosition LabelsPosition
        {
            get { return (BarLabelPosition) GetValue(LabelsPositionProperty); }
            set { SetValue(LabelsPositionProperty, value); }
        }

        #endregion

        #region Overridden Methods

        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var pbv = (ColumnPointView) point.View;

            if (pbv == null)
            {
                pbv = new ColumnPointView
                {
                    IsNew = true,
                    Rectangle = new Rectangle(),
                    Data = new CoreRectangle()
                };

                Model.Chart.View.AddToDrawMargin(pbv.Rectangle);
            }
            else
            {
                pbv.IsNew = false;
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.Rectangle);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HoverShape);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.DataLabel);
            }

            pbv.Rectangle.Fill = Fill;
            pbv.Rectangle.StrokeThickness = StrokeThickness;
            pbv.Rectangle.Stroke = Stroke;
            pbv.Rectangle.StrokeDashArray = StrokeDashArray;

            pbv.Rectangle.Visibility = Visibility;
            var zIndex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(pbv.Rectangle, zIndex);

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

            if (point.Stroke != null) pbv.Rectangle.Stroke = (Brush)point.Stroke;
            if (point.Fill != null) pbv.Rectangle.Fill = (Brush)point.Fill;

            pbv.LabelPosition = LabelsPosition;

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            this.SetIfNotSet(StrokeThicknessProperty, 0d);
            this.SetIfNotSet(MaxColumnWidthProperty, 35d);
            this.SetIfNotSet(ColumnPaddingProperty, 2d);
            this.SetIfNotSet(LabelsPositionProperty, BarLabelPosition.Top);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            this.SetIfNotSet(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
