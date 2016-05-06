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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Helpers;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

// ReSharper disable once CheckNamespace
namespace LiveCharts.Wpf
{
    public class ScatterSeries : Series
    {
        #region Contructors

        public ScatterSeries()
        {
            Model = new LineAlgorithm(this);
            InitializeDefuaults();
        }

        public ScatterSeries(ConfigurableElement configuration)
        {
            Model = new LineAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Properties

        public static readonly DependencyProperty PointDiameterProperty = DependencyProperty.Register(
            "PointDiameter", typeof (double?), typeof (LineSeries), 
            new PropertyMetadata(default(double?), CalChartUpdater()));

        public double? PointDiameter
        {
            get { return (double?) GetValue(PointDiameterProperty); }
            set { SetValue(PointDiameterProperty, value); }
        }

        public static readonly DependencyProperty MaxBubbleDiameterProperty = DependencyProperty.Register(
            "MaxBubbleDiameter", typeof (double), typeof (ScatterSeries), new PropertyMetadata(default(double)));

        public double MaxBubbleDiameter
        {
            get { return (double) GetValue(MaxBubbleDiameterProperty); }
            set { SetValue(MaxBubbleDiameterProperty, value); }
        }

        public static readonly DependencyProperty MinDoubleDiameterProperty = DependencyProperty.Register(
            "MinDoubleDiameter", typeof (double), typeof (ScatterSeries), new PropertyMetadata(default(double)));

        public double MinDoubleDiameter
        {
            get { return (double) GetValue(MinDoubleDiameterProperty); }
            set { SetValue(MinDoubleDiameterProperty, value); }
        }

        #endregion

        #region Overriden Methods

        public override IChartPointView GetView(IChartPointView view, string label)
        {
            var pbv = (view as Bubble);

            if (pbv == null)
            {
                pbv = new Bubble {IsNew = true};
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

                if (Model.Chart.View.HasDataClickEventAttached)
                {
                    var wpfChart = Model.Chart.View as Chart;
                    if (wpfChart == null) return null;
                    pbv.HoverShape.MouseDown += wpfChart.DataMouseDown;
                }

                Model.Chart.View.AddToDrawMargin(pbv.HoverShape);
            }

                pbv.Ellipse = new Ellipse();

            BindingOperations.SetBinding(pbv.Ellipse, Shape.FillProperty,
                new Binding {Path = new PropertyPath(FillProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
                BindingOperations.SetBinding(pbv.Ellipse, WidthProperty,
                    new Binding {Path = new PropertyPath(PointDiameterProperty), Source = this});
                BindingOperations.SetBinding(pbv.Ellipse, HeightProperty,
                    new Binding {Path = new PropertyPath(PointDiameterProperty), Source = this});

                BindingOperations.SetBinding(pbv.Ellipse, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

                Panel.SetZIndex(pbv.Ellipse, int.MaxValue - 2);

                Model.Chart.View.AddToDrawMargin(pbv.Ellipse);
            

            if (DataLabels && pbv.DataLabel == null)
            {
                pbv.DataLabel = BindATextBlock(0);
                Panel.SetZIndex(pbv.DataLabel, int.MaxValue - 1);

                Model.Chart.View.AddToDrawMargin(pbv.DataLabel);
            }

            if (pbv.DataLabel != null) pbv.DataLabel.Text = label;

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
            SetValue(PointDiameterProperty, 12d);
            SetValue(StrokeThicknessProperty, 0d);
            SetValue(MaxBubbleDiameterProperty, 30d);
            SetValue(MinDoubleDiameterProperty, 6d);
            DefaultFillOpacity = 0.7;
        }

        #endregion
    }
}
