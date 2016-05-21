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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Chart;

// ReSharper disable once CheckNamespace
namespace LiveCharts.Wpf
{
    public class StackedAreaSeries : LineSeries, IStackedAreaSeriesViewView
    {
        #region Contructors

        public StackedAreaSeries()
        {
            Model = new StackedAreaAlgorithm(this);
            InitializeDefuaults();
        }

        public StackedAreaSeries(object configuration)
        {
            Model = new StackedAreaAlgorithm(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Private Properties
        #endregion

        #region Properties

        public static readonly DependencyProperty StackModeProperty = DependencyProperty.Register(
            "StackMode", typeof (StackMode), typeof (StackedAreaSeries), 
            new PropertyMetadata(default(StackMode), CallChartUpdater()));

        public StackMode StackMode
        {
            get { return (StackMode) GetValue(StackModeProperty); }
            set { SetValue(StackModeProperty, value); }
        }
        #endregion

        #region Overriden Methods

        public override void OnSeriesUpdateStart()
        {
            ActiveSplitters = 0;

            if (SplittersCollector == int.MaxValue - 1)
            {
                //just in case!
                Splitters.ForEach(s => s.SplitterCollectorIndex = 0);
                SplittersCollector = 0;
            }

            SplittersCollector++;

            if (Figure != null)
            {
                var xIni = ChartFunctions.ToDrawMargin(Values.Limit1.Min, AxisTags.X, Model.Chart, ScalesXAt);

                if (Model.Chart.View.DisableAnimations)
                    Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
                else
                    Figure.BeginAnimation(PathFigure.StartPointProperty,
                        new PointAnimation(new Point(xIni, Model.Chart.DrawMargin.Height),
                            Model.Chart.View.AnimationsSpeed));
            }

            if (IsInView) return;

            IsInView = true;

            Path = new Path();
            BindingOperations.SetBinding(Path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(StrokeProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.FillProperty,
                new Binding { Path = new PropertyPath(FillProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeThicknessProperty,
                new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
            BindingOperations.SetBinding(Path, VisibilityProperty,
                new Binding { Path = new PropertyPath(VisibilityProperty), Source = this });
            BindingOperations.SetBinding(Path, Shape.StrokeDashArrayProperty,
                new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;
            Model.Chart.View.AddToDrawMargin(Path);

            var x = ChartFunctions.ToDrawMargin(Values.Limit1.Min, AxisTags.X, Model.Chart, ScalesXAt);
            Figure.StartPoint = new Point(x, Model.Chart.DrawMargin.Height);

            var wpfChart = Model.Chart.View as Chart;
            if (wpfChart == null) return;

            var index = Stroke == null || Fill == null ? wpfChart.SeriesIndexCount++ : 0;

            var i = Model.Chart.View.Series.IndexOf(this);
            Panel.SetZIndex(Path, Model.Chart.View.Series.Count - i);

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(Chart.GetDefaultColor(index)));
            if (Fill == null)
                SetValue(FillProperty,
                    new SolidColorBrush(Chart.GetDefaultColor(index)) { Opacity = DefaultFillOpacity });
        }

        #endregion

        #region Public Methods 


        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetValue(LineSmoothnessProperty, .7d);
            SetValue(PointDiameterProperty, 0d);
            SetValue(PointForeroundProperty, Brushes.White);
            SetValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(229, 229, 229)));
            SetValue(StrokeThicknessProperty, 0d);
            DefaultFillOpacity = 1;

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            SetValue(LabelPointProperty, defaultLabel);

            Splitters = new List<LineSegmentSplitter>();
        }

        #endregion
    }
}
