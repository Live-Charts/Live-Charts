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
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The stacked area compares trends and percentage, add this series to a cartesian chart
    /// </summary>
    public class StackedAreaSeries : LineSeries, IStackedAreaSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of StackedAreaSeries class
        /// </summary>
        public StackedAreaSeries()
        {
            Model = new StackedAreaAlgorithm(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of StackedAreaSeries class, with a given mapper
        /// </summary>
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

        /// <summary>
        /// The stack mode property
        /// </summary>
        public static readonly DependencyProperty StackModeProperty = DependencyProperty.Register(
            "StackMode", typeof (StackMode), typeof (StackedAreaSeries), 
            new PropertyMetadata(default(StackMode), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the series stacked mode, values or percentage
        /// </summary>
        public StackMode StackMode
        {
            get { return (StackMode) GetValue(StackModeProperty); }
            set { SetValue(StackModeProperty, value); }
        }
        #endregion

        #region Overridden Methods

        /// <summary>
        /// This method runs when the update starts
        /// </summary>
        public override void OnSeriesUpdateStart()
        {
            ActiveSplitters = 0;

            if (SplittersCollector == short.MaxValue - 1)
            {
                //just in case!
                Splitters.ForEach(s => s.SplitterCollectorIndex = 0);
                SplittersCollector = 0;
            }

            SplittersCollector++;

            if (Figure != null && Values != null)
            {
                var xIni = ChartFunctions.ToDrawMargin(Values.GetTracker(this).XLimit.Min, AxisOrientation.X, Model.Chart, ScalesXAt);

                if (Model.Chart.View.DisableAnimations)
                    Figure.StartPoint = new Point(xIni, Model.Chart.DrawMargin.Height);
                else
                    Figure.BeginPointAnimation(nameof(PathFigure.StartPoint), new Point(xIni, Model.Chart.DrawMargin.Height), Model.Chart.View.AnimationsSpeed);
            }

            if (IsPathInitialized)
            {
                Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
                Path.Stroke = Stroke;
                Path.StrokeThickness = StrokeThickness;
                Path.Fill = Fill;
                Path.Visibility = Visibility;
                Path.StrokeDashArray = StrokeDashArray;
                return;
            }

            IsPathInitialized = true;

            Path = new Path
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Fill = Fill,
                Visibility = Visibility,
                StrokeDashArray = StrokeDashArray
            };

            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;
            Model.Chart.View.AddToDrawMargin(Path);

            var x = ChartFunctions.ToDrawMargin(ActualValues.GetTracker(this).XLimit.Min, AxisOrientation.X, Model.Chart, ScalesXAt);
            Figure.StartPoint = new Point(x, Model.Chart.DrawMargin.Height);

            var i = Model.Chart.View.Series.IndexOf(this);
            Canvas.SetZIndex(Path, Model.Chart.View.Series.Count - i);
        }

        #endregion

        #region Public Methods 


        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {;
            //this.SetIfNotSet(PointGeometrySizeProperty, 0d);
            //this.SetIfNotSet(ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 229, 229, 229)));
            //this.SetIfNotSet(StrokeThicknessProperty, 0d);

            DefaultFillOpacity = 1;

            Func<ChartPoint, string> defaultLabel = x => Model.CurrentYAxis.GetFormatter()(x.Y);
            this.SetIfNotSet(LabelPointProperty, defaultLabel);

            Splitters = new List<LineSegmentSplitter>();
        }

        #endregion
    }
}
