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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Points;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Candle series defines a financial series, add this series to a cartesian chart
    /// </summary>
    public class CandleSeries : Series, IOhlcSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of OhclSeries class
        /// </summary>
        public CandleSeries()
        {
            Model = new CandleAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of OhclSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public CandleSeries(object configuration)
        {
            Model = new CandleAlgorithm(this);
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
            var pbv = (view as CandlePointView);

            if (pbv == null)
            {
                pbv = new CandlePointView
                {
                    IsNew = true,
                    HighToLowLine = new Line(),
                    OpenToCloseRectangle = new Rectangle()
                };

                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});
                BindingOperations.SetBinding(pbv.HighToLowLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
                BindingOperations.SetBinding(pbv.HighToLowLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.FillProperty,
                    new Binding { Path = new PropertyPath(FillProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(StrokeProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.StrokeDashArrayProperty,
                    new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Panel.ZIndexProperty,
                    new Binding { Path = new PropertyPath(Panel.ZIndexProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, VisibilityProperty,
                    new Binding { Path = new PropertyPath(VisibilityProperty), Source = this });

                Model.Chart.View.AddToDrawMargin(pbv.HighToLowLine);
                Model.Chart.View.AddToDrawMargin(pbv.OpenToCloseRectangle);
            }
            else
            {
                pbv.IsNew = false;
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.HighToLowLine);
                point.SeriesView.Model.Chart.View
                    .EnsureElementBelongsToCurrentDrawMargin(pbv.OpenToCloseRectangle);
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
                    StrokeThickness = 0
                };

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);
                BindingOperations.SetBinding(pbv.HoverShape, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                var wpfChart = (Chart)Model.Chart.View;
                wpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Panel.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

            if (point.Open <= point.Close)
            {
                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.FillProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
            }
            else
            {
                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(DecreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(DecreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenToCloseRectangle, Shape.FillProperty,
                    new Binding { Path = new PropertyPath(DecreaseBrushProperty), Source = this });
            }

            return pbv;
        }

        public override void Erase()
        {
            Values.Points.ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Model.Chart);
            });
            Model.Chart.View.RemoveFromView(this);
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetValue(StrokeThicknessProperty, 1d);
            SetValue(MaxColumnWidthProperty, 35d);
            SetValue(MaxWidthProperty, 25d);
            SetValue(IncreaseBrushProperty, new SolidColorBrush(Color.FromRgb(254, 178, 0)));
            SetValue(DecreaseBrushProperty, new SolidColorBrush(Color.FromRgb(238, 83, 80)));

            Func<ChartPoint, string> defaultLabel = x =>
                string.Format("O: {0}, H: {1}, C: {2} L: {3}", x.Open, x.High, x.Close, x.Low);
            SetValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }

        #endregion
    }
}
