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
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Chart;
using LiveCharts.Wpf.Points;

// ReSharper disable once CheckNamespace
namespace LiveCharts.Wpf
{
    public class OhlcSeries : Series.Series, IOhlcSeriesView
    {
        #region Contructors

        public OhlcSeries()
        {
            Model = new OhlcAlgorithm(this);
            InitializeDefuaults();
        }

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

        public double MaxColumnWidth
        {
            get { return (double) GetValue(MaxColumnWidthProperty); }
            set { SetValue(MaxColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty IncreaseBrushProperty = DependencyProperty.Register(
            "IncreaseBrush", typeof (Brush), typeof (OhlcSeries), new PropertyMetadata(default(Brush)));

        public Brush IncreaseBrush
        {
            get { return (Brush) GetValue(IncreaseBrushProperty); }
            set { SetValue(IncreaseBrushProperty, value); }
        }

        public static readonly DependencyProperty DecreaseBrushProperty = DependencyProperty.Register(
            "DecreaseBrush", typeof (Brush), typeof (OhlcSeries), new PropertyMetadata(default(Brush)));

        public Brush DecreaseBrush
        {
            get { return (Brush) GetValue(DecreaseBrushProperty); }
            set { SetValue(DecreaseBrushProperty, value); }
        }
        
        #endregion

        #region Overriden Methods

        public override void OnSeriesUpdateStart()
        {
            //do nothing on updateStart
        }

        public override IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            var pbv = (view as OhlcPointView);

            if (pbv == null)
            {
                pbv = new OhlcPointView
                {
                    IsNew = true,
                    HighToLowLine = new Line(),
                    OpenLine = new Line(),
                    CloseLine = new Line()
                };

                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.CloseLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});
                BindingOperations.SetBinding(pbv.OpenLine, Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(StrokeThicknessProperty), Source = this});

                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});
                BindingOperations.SetBinding(pbv.CloseLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});
                BindingOperations.SetBinding(pbv.OpenLine, Shape.StrokeDashArrayProperty,
                    new Binding {Path = new PropertyPath(StrokeDashArrayProperty), Source = this});

                BindingOperations.SetBinding(pbv.HighToLowLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
                BindingOperations.SetBinding(pbv.CloseLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});
                BindingOperations.SetBinding(pbv.OpenLine, Panel.ZIndexProperty,
                    new Binding {Path = new PropertyPath(Panel.ZIndexProperty), Source = this});

                BindingOperations.SetBinding(pbv.HighToLowLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(pbv.CloseLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
                BindingOperations.SetBinding(pbv.OpenLine, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                Model.Chart.View.AddToDrawMargin(pbv.HighToLowLine);
                Model.Chart.View.AddToDrawMargin(pbv.OpenLine);
                Model.Chart.View.AddToDrawMargin(pbv.CloseLine);
            }
            else
            {
                pbv.IsNew = false;
            }

            if ((Model.Chart.View.HasTooltip || Model.Chart.View.HasDataClickEventAttached) && pbv.HoverShape == null)
            {
                pbv.HoverShape = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };

                Panel.SetZIndex(pbv.HoverShape, int.MaxValue);
                BindingOperations.SetBinding(pbv.HoverShape, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                var wpfChart = Model.Chart.View as Chart;
                if (wpfChart == null) return null;
                wpfChart.AttachEventsTo(pbv.HoverShape);

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Panel.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

            if (point.Open < point.Close)
            {
                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.CloseLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(IncreaseBrushProperty), Source = this });
            }
            else
            {
                BindingOperations.SetBinding(pbv.HighToLowLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(DecreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.CloseLine, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(DecreaseBrushProperty), Source = this });
                BindingOperations.SetBinding(pbv.OpenLine, Shape.StrokeProperty,
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
            SetValue(StrokeThicknessProperty, 2.5d);
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
