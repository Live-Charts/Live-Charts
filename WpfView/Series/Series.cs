using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Chart;
using LiveCharts.Wpf.Converters;

namespace LiveCharts.Wpf.Series
{
    public abstract class Series : FrameworkElement, ISeriesView
    {

        #region Contructors
        protected Series()
        {
            DefaultFillOpacity = 0.35;

            SetValue(TitleProperty, "Some Series");
        }

        protected Series(object configuration)
        {
            Configuration = configuration;

            SetValue(TitleProperty, "Some Series");
        }
        #endregion

        #region Properties

        private IChartValues LastKnownValues { get; set; }
        internal double DefaultFillOpacity { get; set; }
        public SeriesAlgorithm Model { get; set; }
        public bool IsSeriesVisible
        {
            get { return Visibility == Visibility.Visible; }
        }
        public bool IsInVisualTree { get { return Parent != null; } }

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
            "Values", typeof (IChartValues), typeof (Series),
            new PropertyMetadata(default(IChartValues), OnValuesInstanceChanged));
        /// <summary>
        /// Gets or sets chart values.
        /// </summary>
        [TypeConverter(typeof(NumericChartValuesConverter))]
        public IChartValues Values
        {
            get { return (IChartValues) GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (Series),
            new PropertyMetadata(default(string), CallChartUpdater()));
        /// <summary>
        /// Gets or sets serie title
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
            new PropertyMetadata(default(FontFamily), CallChartUpdater()));
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
            typeof (Series), new PropertyMetadata(FontStyles.Normal, CallChartUpdater()));
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
            typeof (Series), new PropertyMetadata(FontStretches.Normal, CallChartUpdater()));
        /// <summary>
        /// Gets or sets labels font strech
        /// </summary>
		public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof (Brush),
            typeof (Series), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(55, 71, 79)), CallChartUpdater()));
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
            new PropertyMetadata(default(DoubleCollection), CallChartUpdater()));
        /// <summary>
        /// Gets or sets the stroke dash array of a series
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public static readonly DependencyProperty ScalesXAtProperty = DependencyProperty.Register(
            "ScalesXAt", typeof (int), typeof (Series), new PropertyMetadata(default(int)));

        public int ScalesXAt
        {
            get { return (int) GetValue(ScalesXAtProperty); }
            set { SetValue(ScalesXAtProperty, value); }
        }

        public static readonly DependencyProperty ScalesYAtProperty = DependencyProperty.Register(
            "ScalesYAt", typeof (int), typeof (Series), new PropertyMetadata(default(int)));

        public int ScalesYAt
        {
            get { return (int) GetValue(ScalesYAtProperty); }
            set { SetValue(ScalesYAtProperty, value); }
        }

        public static readonly DependencyProperty LabelPointProperty = DependencyProperty.Register(
            "LabelPoint", typeof (Func<ChartPoint, string>), typeof (Series), new PropertyMetadata(default(Func<ChartPoint, string>)));

        public Func<ChartPoint, string> LabelPoint
        {
            get { return (Func<ChartPoint, string>) GetValue(LabelPointProperty); }
            set { SetValue(LabelPointProperty, value); }
        }

        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            "Configuration", typeof (object), typeof (Series), new PropertyMetadata(default(object)));

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
                new Binding {Path = new PropertyPath(FontFamilyProperty), Source = this});
            tb.SetBinding(FontSizeProperty,
                new Binding {Path = new PropertyPath(FontSizeProperty), Source = this});
            tb.SetBinding(TextBlock.FontStretchProperty,
                new Binding {Path = new PropertyPath(FontStretchProperty), Source = this});
            tb.SetBinding(TextBlock.FontStyleProperty,
                new Binding {Path = new PropertyPath(FontStyleProperty), Source = this});
            tb.SetBinding(TextBlock.FontWeightProperty,
                new Binding {Path = new PropertyPath(FontWeightProperty), Source = this});
            tb.SetBinding(TextBlock.ForegroundProperty,
                new Binding {Path = new PropertyPath(ForegroundProperty), Source = this});
            tb.SetBinding(TextBlock.VisibilityProperty,
                new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
            return tb;
        }

        #endregion

        public virtual IChartPointView GetPointView(IChartPointView view, ChartPoint point, string label)
        {
            throw new NotImplementedException();
        }

        public virtual void OnSeriesUpdateStart()
        {
            var wpfChart = Model.Chart.View as Chart;
            if (wpfChart == null) return;

            var index = Stroke == null || Fill == null ? wpfChart.SeriesIndexCount++ : 0;

            if (Stroke == null)
                SetValue(StrokeProperty, new SolidColorBrush(Chart.GetDefaultColor(index)));
            if (Fill == null)
                SetValue(FillProperty,
                    new SolidColorBrush(Chart.GetDefaultColor(index)) {Opacity = DefaultFillOpacity});
        }

        public virtual void Erase()
        {
            throw new NotImplementedException();
        }

        public virtual void OnSeriesUpdatedFinish()
        {
        }

        private static void OnValuesInstanceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var series = (Series) dependencyObject;

            if (series.Values != series.LastKnownValues && series.LastKnownValues != null)
            {
                series.LastKnownValues.Points.ForEach(x => x.View.RemoveFromView(series.Model.Chart));
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

        #region Obsoletes

        #endregion
    }
}
