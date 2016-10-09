﻿//The MIT License(MIT)

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
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Converters;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// Base WPF and WinForms series, this class is abstract
    /// </summary>
    public abstract class Series : FrameworkElement, ISeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new Instance of Series
        /// </summary>
        protected Series()
        {
            DefaultFillOpacity = 0.35;
            /*Current*/SetValue(TitleProperty, "Series");
            RegisterPropertyChangedCallback(VisibilityProperty, OnIsVisibleChanged);
        }

        /// <summary>
        /// Initializes a new Instance of series, with a given configuration
        /// </summary>
        /// <param name="configuration"></param>
        protected Series(object configuration)
        {
            Configuration = configuration;
            SetValue(TitleProperty, "Series");
            PreviousVisibility = Visibility;
            RegisterPropertyChangedCallback(VisibilityProperty, OnIsVisibleChanged);
        }
        #endregion

        #region Properties

        private IChartValues LastKnownValues { get; set; }
        internal double DefaultFillOpacity { get; set; }
        /// <summary>
        /// THe Model is set by every series type, it is the motor of the series, it is the communication with the core of the library
        /// </summary>
        public SeriesAlgorithm Model { get; set; }
        /// <summary>
        /// Gets the Actual values in the series, active or visible series only
        /// </summary>
        public IChartValues ActualValues
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled && (Values == null || Values.Count == 0))
                    SetValue(ValuesProperty, GetValuesForDesigner());

                return Values ?? new ChartValues<double>();
            }
        }

        /// <summary>
        /// Gets whether the series is visible
        /// </summary>
        public bool IsSeriesVisible
        {
            get { return Visibility == Visibility.Visible; }
        }

        /// <summary>
        /// Gets the current chart points in the series
        /// </summary>
        public IEnumerable<ChartPoint> ChartPoints
        {
            get { return ActualValues.GetPoints(this); }
        }
        
        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
            "Values", typeof (IChartValues), typeof (Series),
            new PropertyMetadata(default(IChartValues), OnValuesInstanceChanged));
        /// <summary>
        /// Gets or sets chart values.
        /// </summary>
        //[TypeConverter(typeof(NumericChartValuesConverter))]
        public IChartValues Values
        {
            get { return (IChartValues) GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (Series),
            new PropertyMetadata(default(string), CallChartUpdater()));
        /// <summary>
        /// Gets or sets series title
        /// </summary>
        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (Series), 
            new PropertyMetadata(default(Brush), CallChartUpdater()));
        /// <summary>
        /// Gets or sets series stroke, if this property is null then a SolidColorBrush will be assigned according to series position in collection and Chart.Colors property
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (Series), 
            new PropertyMetadata(default(double), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the series stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof (Brush), typeof (Series), 
            new PropertyMetadata(default(Brush), CallChartUpdater()));
        /// <summary>
        /// Gets or sets series fill color, if this property is null then a SolidColorBrush will be assigned according to series position in collection and Chart.Colors property, also Fill property has a default opacity according to chart type.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty DataLabelsProperty = DependencyProperty.Register(
            "DataLabels", typeof (bool), typeof (Series), 
            new PropertyMetadata(default(bool), CallChartUpdater()));
        /// <summary>
        /// Gets or sets if series should include a label over each data point.
        /// </summary>
        public bool DataLabels
        {
            get { return (bool) GetValue(DataLabelsProperty); }
            set { SetValue(DataLabelsProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
            "FontFamily", typeof (FontFamily), typeof (Series), 
            new PropertyMetadata(default(FontFamily)));
        /// <summary>
        /// Gets or sets labels font family
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily) GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof (double),
            typeof (Series), new PropertyMetadata(10d, CallChartUpdater()));
        /// <summary>
        /// Gets or sets labels font size
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight", typeof (FontWeight), typeof (Series),
            new PropertyMetadata(FontWeights.Bold, CallChartUpdater()));
        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(
            "FontStyle", typeof (FontStyle),
            typeof (Series), new PropertyMetadata(FontStyle.Normal, CallChartUpdater()));
        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
		public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty = DependencyProperty.Register(
            "FontStretch", typeof (FontStretch),
            typeof (Series), new PropertyMetadata(FontStretch.Normal, CallChartUpdater()));
        /// <summary>
        /// Gets or sets labels font stretch
        /// </summary>
		public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof (Brush),
            typeof (Series), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 55, 71, 79))));
        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
		public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof(DoubleCollection), typeof(Series), 
            new PropertyMetadata(default(DoubleCollection)));
        /// <summary>
        /// Gets or sets the stroke dash array of a series, sue this property to draw dashed strokes
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public static readonly DependencyProperty PointGeometryProperty =
            DependencyProperty.Register("PointGeometry", typeof (Geometry), typeof (Series),
                new PropertyMetadata(DefaultGeometries.Circle, CallChartUpdater()));
        /// <summary>
        /// Gets or sets the point geometry, this shape will be drawn in the Tooltip, Legend, and if line series in every point also.
        /// </summary>
        public Geometry PointGeometry
        {
            get { return ((Geometry)GetValue(PointGeometryProperty)); }
            set { SetValue(PointGeometryProperty, value); }
        }

        public static readonly DependencyProperty ScalesXAtProperty = DependencyProperty.Register(
            "ScalesXAt", typeof (int), typeof (Series), new PropertyMetadata(default(int), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the axis where series is scaled at, the axis must exist in the collection
        /// </summary>
        public int ScalesXAt
        {
            get { return (int) GetValue(ScalesXAtProperty); }
            set { SetValue(ScalesXAtProperty, value); }
        }

        public static readonly DependencyProperty ScalesYAtProperty = DependencyProperty.Register(
            "ScalesYAt", typeof (int), typeof (Series), new PropertyMetadata(default(int), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the axis where series is scaled at, the axis must exist in the collection
        /// </summary>
        public int ScalesYAt
        {
            get { return (int) GetValue(ScalesYAtProperty); }
            set { SetValue(ScalesYAtProperty, value); }
        }

        public static readonly DependencyProperty LabelPointProperty = DependencyProperty.Register(
            "LabelPoint", typeof (Func<ChartPoint, string>), typeof (Series), new PropertyMetadata(default(Func<ChartPoint, string>)));
        /// <summary>
        /// Gets or sets the label formatter for the data label and tooltip, this property is set by default according to the series
        /// </summary>
        public Func<ChartPoint, string> LabelPoint
        {
            get { return (Func<ChartPoint, string>) GetValue(LabelPointProperty); }
            set { SetValue(LabelPointProperty, value); }
        }

        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            "Configuration", typeof (object), typeof (Series), 
            new PropertyMetadata(default(object), CallChartUpdater()));
        /// <summary>
        /// Gets or sets series mapper, if this property is set then the library will ignore the SeriesCollection mapper and global mappers.
        /// </summary>
        public object Configuration
        {
            get { return GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        #endregion

        #region Internal Helpers

        internal TextBlock BindATextBlock(int rotate)
        {
            var tb = new TextBlock();

            tb.SetBinding(TextBlock.FontFamilyProperty,
                new Binding {Path = new PropertyPath("FontFamily"), Source = this});
            tb.SetBinding(TextBlock.FontSizeProperty,
                new Binding {Path = new PropertyPath("FontSize"), Source = this});
            tb.SetBinding(TextBlock.FontStretchProperty,
                new Binding {Path = new PropertyPath("FontStretch"), Source = this});
            tb.SetBinding(TextBlock.FontStyleProperty,
                new Binding {Path = new PropertyPath("FontStyle"), Source = this});
            tb.SetBinding(TextBlock.FontWeightProperty,
                new Binding {Path = new PropertyPath("FontWeight"), Source = this});
            tb.SetBinding(TextBlock.ForegroundProperty,
                new Binding {Path = new PropertyPath("Foreground"), Source = this});
            tb.SetBinding(TextBlock.VisibilityProperty,
                new Binding {Path = new PropertyPath("Visibility"), Source = this});
            return tb;
        }

        #endregion

        #region Publics

        /// <summary>
        /// Gets the view of a given point
        /// </summary>
        /// <param name="view"></param>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method runs when the update starts
        /// </summary>
        public virtual void OnSeriesUpdateStart()
        {
        }

        /// <summary>
        /// Erases series
        /// </summary>
        public virtual void Erase(bool removeFromView = true)
        {
            Values.GetPoints(this).ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Model.Chart);
            });
            if (removeFromView) Model.Chart.View.RemoveFromView(this);
        }

        /// <summary>
        /// This method runs when the update finishes
        /// </summary>
        public virtual void OnSeriesUpdatedFinish()
        {
        }

        /// <summary>
        /// Initializes the series colors if they are not set
        /// </summary>
        public virtual void InitializeColors()
        {
            var wpfChart = (Chart) Model.Chart.View;
            var nextColor = wpfChart.GetNextDefaultColor();

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(nextColor));
            if (Fill == null)
                SetValue(FillProperty, new SolidColorBrush(nextColor) {Opacity = DefaultFillOpacity});
        }

        /// <summary>
        /// Defines special elements to draw according to the series type
        /// </summary>
        public virtual void DrawSpecializedElements()
        {

        }

        /// <summary>
        /// Places specializes items
        /// </summary>
        public virtual void PlaceSpecializedElements()
        {

        }

        public Func<ChartPoint, string> GetLabelPointFormatter()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return x => "Label";

            return LabelPoint;
        }
        #endregion

        private static void OnValuesInstanceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var series = (Series) dependencyObject;

            if (series.Values != series.LastKnownValues && series.LastKnownValues != null)
            {
                series.LastKnownValues.GetPoints(series).ForEach(
                    x => { if (x.View != null) x.View.RemoveFromView(series.Model.Chart); });
            }

            CallChartUpdater()(dependencyObject, dependencyPropertyChangedEventArgs);
            series.LastKnownValues = series.Values;
        }

        protected static PropertyChangedCallback CallChartUpdater(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfSeries = o as Series;

                if (wpfSeries == null) return;
                if (wpfSeries.Model == null) return;

                if (wpfSeries.Model.Chart != null) wpfSeries.Model.Chart.Updater.Run(animate);
            };
        }

        private Visibility PreviousVisibility { get; set; }
       
        private void OnIsVisibleChanged(DependencyObject sender, DependencyProperty e)
        {
            if (Model.Chart != null && PreviousVisibility != Visibility)
            {
                PreviousVisibility = Visibility;
                Model.Chart.Updater.Run(false, true);
                if (Visibility == Visibility.Collapsed)
                {
                    Erase(false);
                }
            }
        }

        private static ChartValues<ObservableValue> GetValuesForDesigner()
        {
            var r = new Random();
            return new ChartValues<ObservableValue>
            {
                new ObservableValue(r.Next(0, 100)),
                new ObservableValue(r.Next(0, 100)),
                new ObservableValue(r.Next(0, 100)),
                new ObservableValue(r.Next(0, 100)),
                new ObservableValue(r.Next(0, 100)),
                new ObservableValue(r.Next(0, 100))
            };
        }

        #region Obsoletes

        #endregion
    }
}
