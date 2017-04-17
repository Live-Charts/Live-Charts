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
    /// The Row series plots horizontal bars in a cartesian chart
    /// </summary>
    public class RowSeries : Series, IRowSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of RowSeries class
        /// </summary>
        public RowSeries()
        {
            Model = new RowAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of RowSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public RowSeries(object configuration)
        {
            Model = new RowAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        /// <summary>
        /// The maximum row heigth property
        /// </summary>
        public static readonly DependencyProperty MaxRowHeigthProperty = DependencyProperty.Register(
            "MaxRowHeigth", typeof (double), typeof (RowSeries), new PropertyMetadata(35d));
        /// <summary>
        /// Gets or sets the maximum row height, the height of a column will be capped at this value
        /// </summary>
        public double MaxRowHeigth
        {
            get { return (double) GetValue(MaxRowHeigthProperty); }
            set { SetValue(MaxRowHeigthProperty, value); }
        }

        /// <summary>
        /// The row padding property
        /// </summary>
        public static readonly DependencyProperty RowPaddingProperty = DependencyProperty.Register(
            "RowPadding", typeof (double), typeof (RowSeries), new PropertyMetadata(2d));
        /// <summary>
        /// Gets or sets the padding between rows in this series
        /// </summary>
        public double RowPadding
        {
            get { return (double) GetValue(RowPaddingProperty); }
            set { SetValue(RowPaddingProperty, value); }
        }

        /// <summary>
        /// The label position property
        /// </summary>
        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(
            "LabelPosition", typeof(BarLabelPosition), typeof(RowSeries), 
            new PropertyMetadata(BarLabelPosition.Top, CallChartUpdater()));
        /// <summary>
        /// Gets or sets where the label is placed
        /// </summary>
        public BarLabelPosition LabelPosition
        {
            get { return (BarLabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }

        /// <summary>
        /// The shares position property
        /// </summary>
        public static readonly DependencyProperty SharesPositionProperty = DependencyProperty.Register(
            "SharesPosition", typeof(bool), typeof(RowSeries), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether this row shares space with all the row series in the same position
        /// </summary>
        /// <value>
        /// <c>true</c> if [shares position]; otherwise, <c>false</c>.
        /// </value>
        public bool SharesPosition
        {
            get { return (bool) GetValue(SharesPositionProperty); }
            set { SetValue(SharesPositionProperty, value); }
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
            var pbv = (RowPointView) point.View;

            if (pbv == null)
            {
                pbv = new RowPointView
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
            pbv.Rectangle.Stroke = Stroke;
            pbv.Rectangle.StrokeThickness = StrokeThickness;
            pbv.Rectangle.StrokeDashArray = StrokeDashArray;
            pbv.Rectangle.Visibility = Visibility;
            Canvas.SetZIndex(pbv.Rectangle, Canvas.GetZIndex(this));

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

            if (DataLabels)
            {
                pbv.DataLabel = UpdateLabelContent(new DataLabelViewModel
                {
                    FormattedText = label,
                    Instance = point.Instance
                }, pbv.DataLabel);
            }

            if (point.Stroke != null) pbv.Rectangle.Stroke = (Brush) point.Stroke;
            if (point.Fill != null) pbv.Rectangle.Fill = (Brush) point.Fill;

            pbv.LabelPosition = LabelPosition;

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            //this.SetIfNotSet(StrokeThicknessProperty, 0d);

            Func<ChartPoint, string> defaultLabel = x => x.EvaluatesGantt
                ? string.Format("starts {0}, ends {1}", Model.CurrentXAxis.GetFormatter()(x.XStart),
                    Model.CurrentXAxis.GetFormatter()(x.X))
                : Model.CurrentXAxis.GetFormatter()(x.X);
            this.SetIfNotSet(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
