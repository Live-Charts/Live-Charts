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
using LiveCharts.Dtos;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The stacked row series compares the proportion of every series in a point
    /// </summary>
    public class StackedRowSeries : Series, IStackedRowSeriesView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of StackedRow series class
        /// </summary>
        public StackedRowSeries()
        {
            Model = new StackedRowAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of StackedRow series class, with a given mapper
        /// </summary>
        public StackedRowSeries(object configuration)
        {
            Model = new StackedRowAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        /// <summary>
        /// The maximum row height property
        /// </summary>
        public static readonly DependencyProperty MaxRowHeightProperty = DependencyProperty.Register(
            "MaxRowHeight", typeof (double), typeof (StackedRowSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the maximum height of row, any row height will be capped at this value.
        /// </summary>
        public double MaxRowHeight
        {
            get { return (double) GetValue(MaxRowHeightProperty); }
            set { SetValue(MaxRowHeightProperty, value); }
        }

        /// <summary>
        /// The row padding property
        /// </summary>
        public static readonly DependencyProperty RowPaddingProperty = DependencyProperty.Register(
            "RowPadding", typeof (double), typeof (StackedRowSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the padding between each row in the series.
        /// </summary>
        public double RowPadding
        {
            get { return (double) GetValue(RowPaddingProperty); }
            set { SetValue(RowPaddingProperty, value); }
        }

        /// <summary>
        /// The stack mode property
        /// </summary>
        public static readonly DependencyProperty StackModeProperty = DependencyProperty.Register(
            "StackMode", typeof (StackMode), typeof (StackedRowSeries), new PropertyMetadata(default(StackMode)));
        /// <summary>
        /// Gets or sets the stacked mode, values or percentage.
        /// </summary>
        public StackMode StackMode
        {
            get { return (StackMode) GetValue(StackModeProperty); }
            set { SetValue(StackModeProperty, value); }
        }

        /// <summary>
        /// The labels position property
        /// </summary>
        public static readonly DependencyProperty LabelsPositionProperty = DependencyProperty.Register(
            "LabelsPosition", typeof(BarLabelPosition), typeof(StackedRowSeries),
            new PropertyMetadata(BarLabelPosition.Parallel, CallChartUpdater()));
        /// <summary>
        /// Gets or sets where the label is placed
        /// </summary>
        public BarLabelPosition LabelsPosition
        {
            get { return (BarLabelPosition)GetValue(LabelsPositionProperty); }
            set { SetValue(LabelsPositionProperty, value); }
        }

        /// <summary>
        /// The Grouping property
        /// </summary>
        public object Grouping
        {
            get { return (object)GetValue(GroupingProperty); }
            set { SetValue(GroupingProperty, value); }
        }

        /// <summary>
        /// Gets or sets which columns are grouped together
        /// </summary>
        public static readonly DependencyProperty GroupingProperty =
            DependencyProperty.Register("Grouping", typeof(object), typeof(StackedRowSeries), new PropertyMetadata(null));
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
            pbv.Rectangle.StrokeThickness = StrokeThickness;
            pbv.Rectangle.Stroke = Stroke;
            pbv.Rectangle.StrokeDashArray = StrokeDashArray;
            pbv.Rectangle.Visibility = Visibility;
            Panel.SetZIndex(pbv.Rectangle, Panel.GetZIndex(this));

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);

                var wpfChart = (Chart)Model.Chart.View;
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

            pbv.LabelPosition = LabelsPosition;

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetCurrentValue(StrokeThicknessProperty, 0d);
            SetCurrentValue(MaxRowHeightProperty, 35d);
            SetCurrentValue(RowPaddingProperty, 2d);
            SetCurrentValue(ForegroundProperty, Brushes.White);

            Func<ChartPoint, string> defaultLabel = x =>  Model.CurrentXAxis.GetFormatter()(x.X);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
