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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Points;
using Windows.UI.Xaml;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The Bubble series, draws scatter series, only using X and Y properties or bubble series, if you also use the weight property, this series should be used in a cartesian chart.
    /// </summary>
    public class ScatterSeries : Series, IScatterSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of BubbleSeries class
        /// </summary>
        public ScatterSeries()
        {
            Model = new ScatterAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of BubbleSeries class using a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public ScatterSeries(object configuration)
        {
            Model = new ScatterAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        public static readonly DependencyProperty MaxPointShapeDiameterProperty = DependencyProperty.Register(
            "MaxPointShapeDiameter", typeof (double), typeof (ScatterSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the max shape diameter, the points using the max weight in the series will have this radius.
        /// </summary>
        public double MaxPointShapeDiameter
        {
            get { return (double) GetValue(MaxPointShapeDiameterProperty); }
            set { SetValue(MaxPointShapeDiameterProperty, value); }
        }

        public static readonly DependencyProperty MinPointShapeDiameterProperty = DependencyProperty.Register(
            "MinPointShapeDiameter", typeof (double), typeof (ScatterSeries), 
            new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the min shape diameter, the points using the min weight in the series will have this radius.
        /// </summary>
        public double MinPointShapeDiameter
        {
            get { return (double) GetValue(MinPointShapeDiameterProperty); }
            set { SetValue(MinPointShapeDiameterProperty, value); }
        }

        #endregion

        #region Overridden Methods

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var pbv = (ScatterPointView) view;

            if (pbv == null)
            {
                pbv = new ScatterPointView
                {
                    IsNew = true,
                    Shape = new Path
                    {
                        Stretch = Stretch.Fill,
                        //ClipToBounds = true,
                        StrokeThickness = StrokeThickness
                    }
                };

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
            }

            var p = (Path) pbv.Shape;
            p.Data = PointGeometry;
            p.Fill = Fill;
            p.Stroke = Stroke;
            p.StrokeThickness = StrokeThickness;
            p.Visibility = Visibility;
            Canvas.SetZIndex(p, Canvas.GetZIndex(this));
            p.StrokeDashArray = StrokeDashArray;

            if (Model.Chart.RequiresHoverShape && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    StrokeThickness = 0
                };

                Canvas.SetZIndex(pbv.HoverShape, int.MaxValue);

                var wpfChart = (Chart)Model.Chart.View;
                wpfChart.AttachHoverableEventTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Canvas.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

            if (point.Stroke != null) pbv.Shape.Stroke = (Brush)point.Stroke;
            if (point.Fill != null) pbv.Shape.Fill = (Brush)point.Fill;

            return pbv;
        }

        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            /*Current*/SetValue(StrokeThicknessProperty, 0d);
            /*Current*/SetValue(MaxPointShapeDiameterProperty, 15d);
            /*Current*/SetValue(MinPointShapeDiameterProperty, 10d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentXAxis.GetFormatter()(x.X) + ", "
                                                         + Model.CurrentYAxis.GetFormatter()(x.Y);
            /*Current*/SetValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.7;
        }

        #endregion
    }
}
