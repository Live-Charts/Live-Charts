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
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Converters;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Bubble series, draws scatter series, only using X and Y properties or bubble series, if you also use the weight property, this series should be used in a cartesian chart.
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
            Model = new BubbleAlgorithm(this);
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
            var pbv = (view as StepLinePointView);

            if (pbv == null)
            {
                pbv = new StepLinePointView
                {
                    IsNew = true,
                    HorizontalLine = new Line(),
                    VerticalLine = new Line()
                };

                BindingOperations.SetBinding(pbv.VerticalLine, Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(AlternativeStrokeProperty), Source = this});
                BindingOperations.SetBinding(pbv.VerticalLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.VerticalLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(pbv.VerticalLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
                BindingOperations.SetBinding(pbv.VerticalLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});

                BindingOperations.SetBinding(pbv.HorizontalLine, Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(pbv.HorizontalLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.HorizontalLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(pbv.HorizontalLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
                BindingOperations.SetBinding(pbv.HorizontalLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});

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

            if (PointGeometry != null && Math.Abs(PointGeometrySize) > 0.1 && pbv.Shape == null)
            {
                if (PointGeometry != null)
                {
                    pbv.Shape = new Path
                    {
                        Stretch = Stretch.Fill,
                        ClipToBounds = true,
                        StrokeThickness = StrokeThickness
                    };
                    BindingOperations.SetBinding(pbv.Shape, Path.DataProperty,
                        new Binding {Path = new PropertyPath(PointGeometryProperty), Source = this});
                }
                else
                {
                    pbv.Shape = new Ellipse();
                }

                BindingOperations.SetBinding(pbv.Shape, Shape.FillProperty,
                    new Binding {Path = new PropertyPath(PointForeroundProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, WidthProperty,
                    new Binding {Path = new PropertyPath(PointGeometrySizeProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, HeightProperty,
                    new Binding {Path = new PropertyPath(PointGeometrySizeProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(pbv.Shape, Panel.ZIndexProperty,
                    new Binding
                    {
                        Path = new PropertyPath(Panel.ZIndexProperty),
                        Source = this,
                        Converter = new NextZIndexConverter()
                    });

                Model.Chart.View.AddToDrawMargin(pbv.Shape);
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

                var wpfChart = (Chart) Model.Chart.View;
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

            if (point.Stroke != null) pbv.Shape.Stroke = (Brush) point.Stroke;
            if (point.Fill != null) pbv.Shape.Fill = (Brush) point.Fill;

            return pbv;
        }

        public override void InitializeColors()
        {
            var wpfChart = (Chart) Model.Chart.View;
            var nextColor = wpfChart.GetNextDefaultColor();

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
            SetCurrentValue(PointGeometrySizeProperty, 8d);
            SetCurrentValue(PointForeroundProperty, Brushes.White);
            SetCurrentValue(StrokeThicknessProperty, 2d);

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
        }
        #endregion
    }
}
