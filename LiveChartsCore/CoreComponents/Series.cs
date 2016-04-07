//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.TypeConverters;

namespace LiveCharts.CoreComponents
{
    public abstract class Series : FrameworkElement, IChartSeries
	{
		internal List<FrameworkElement> Shapes = new List<FrameworkElement>();
	    private Chart _chart;
	    internal bool RequiresAnimation;
	    internal bool RequiresPlot;

        protected Series()
        {           
        }

        protected Series(ISeriesConfiguration configutration)
        {
            Configuration = configutration;
        }

        #region Dependency Properties

	    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
	        "Values", typeof (IChartValues), typeof (Series), 
            new PropertyMetadata(null, ValuesCallBack));
        /// <summary>
        /// Gets or sets chart values.
        /// </summary>
        [TypeConverter(typeof(DefaultValuesConverter))]
        public IChartValues Values
	    {
	        get
	        {
	            var values = (IChartValues) GetValue(ValuesProperty);
#if DEBUG
                if (DesignerProperties.GetIsInDesignMode(this))
                    if (values == null) values = new ChartValues<double> {10, 5, 20, 5};
#endif
	            return values;
	        }
            set { SetValue(ValuesProperty, value); }
	    }

        /// <summary>
        /// Gets collection that owns this series.
        /// </summary>
        public SeriesCollection Collection { get; internal set; }

        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(Series), new PropertyMetadata("An Unnamed Serie"));
        /// <summary>
        /// Gets or sets serie name
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

		public static readonly DependencyProperty StrokeProperty =
			DependencyProperty.Register("Stroke", typeof(Brush), typeof(Series), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets series stroke, if this property is null then a SolidColorBrush will be assigned according to series position in collection and Chart.Colors property
        /// </summary>
		public Brush Stroke
		{
		    get { return ((Brush) GetValue(StrokeProperty)); }
			set { SetValue(StrokeProperty, value); }
		}

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(Series), new PropertyMetadata(2.5));
        /// <summary>
        /// Gets or sets the series stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
			DependencyProperty.Register("Fill", typeof(Brush), typeof(Series), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets series fill color, if this property is null then a SolidColorBrush will be assigned according to series position in collection and Chart.Colors property, also Fill property has a default opacity according to chart type.
        /// </summary>
		public Brush Fill
		{
			get
			{
			    return (Brush) GetValue(FillProperty);
			}
			set { SetValue(FillProperty, value); }
		}

        public static readonly DependencyProperty DataLabelsProperty = DependencyProperty.Register(
            "DataLabels", typeof (bool), typeof (Series), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets if series should include a label over each data point.
        /// </summary>
        public bool DataLabels
        {
            get { return (bool) GetValue(DataLabelsProperty); }
            set { SetValue(DataLabelsProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(Series), new PropertyMetadata(new FontFamily("Calibri")));

        /// <summary>
        /// Gets or sets labels font family
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(Series), new PropertyMetadata(13.0));

        /// <summary>
        /// Gets or sets labels font size
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(Series), new PropertyMetadata(FontWeights.Bold));

        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(Series), new PropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
		public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(Series), new PropertyMetadata(FontStretches.Normal));

        /// <summary>
        /// Gets or sets labels font strech
        /// </summary>
		public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Series), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(55, 71, 79))));

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
		public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof(DoubleCollection), typeof(Series), new PropertyMetadata(default(DoubleCollection)));

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
        #endregion

        #region Properties
        public Chart Chart
        {
            get { return _chart; }
            internal set
            {
                //if (_chart != null) throw new InvalidOperationException("Can't set chart property twice.");
                _chart = value;
            }
        }

        public ISeriesConfiguration Configuration { get; set; }

        public int ScalesXAt { get; set; }
        public int ScalesYAt { get; set; }

        public Axis CurrentXAxis
        {
            get { return Chart.AxisX[ScalesXAt]; }
        }

        public Axis CurrentYAxis
        {
            get { return Chart.AxisY[ScalesYAt]; }
        }

        #endregion

        #region PublicMethods
        /// <summary>
        /// Returns a Color according to an index and Chart.Colors property.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Color GetColorByIndex(int index)
        {
            return Chart.Colors[(int) (index - Chart.Colors.Count*Math.Truncate(index/(decimal) Chart.Colors.Count))];
        }
        /// <summary>
        /// Setup a configuration for this collection, notice this method returns the current instance to support fleunt syntax. if Series.Cofiguration is not null then SeriesCollection.Configuration will be ignored.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public Series Setup<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }
        #endregion

        #region Abstracts
        public abstract void Plot(bool animate = true);
        #endregion

        #region PrivateMethods
        private static void ValuesCallBack(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var series = o as Series;
            if (series == null) return;

            var observable = series.Values as INotifyCollectionChanged;
            if (observable == null) return;
            if (series.Chart == null) return;

            observable.CollectionChanged += series.Chart.OnDataSeriesChanged;

            series.RequiresPlot = true;
            series.RequiresAnimation = true;
            series.Chart.RequiresScale = true;
            series.Chart.EraseSerieBuffer.Add(new DeleteBufferItem {Series = series, Force = true});
            series.Chart.SeriesChanged.Stop();
            series.Chart.SeriesChanged.Start();
        }
        #endregion

        #region Virtual Methods
        internal virtual void Erase(bool force = false)
        {
            foreach (var s in Shapes)
            {
                var p = s.Parent as Canvas;
                if (p != null) p.Children.Remove(s);
            }
            Shapes.Clear();

            var hoverableShapes = Chart.ShapesMapper.Where(x => Equals(x.Series, this)).ToList();
            foreach (var hs in hoverableShapes)
            {
                Chart.Canvas.Children.Remove(hs.HoverShape);
                Chart.ShapesMapper.Remove(hs);
            }
        }
        #endregion

        #region ProtectedMethods

        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <param name="axIndex"></param>
        /// <returns></returns>
        protected double ToPlotArea(double value, AxisTags axis, int axIndex = 0)
        {
            return Methods.ToPlotArea(value, axis, Chart, axIndex);
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <returns></returns>
        protected Point ToPlotArea(Point value, int xIndex = 0, int yIndex = 0)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X, xIndex),
                ToPlotArea(value.Y, AxisTags.Y, yIndex));
        }

        protected Point ToPlotArea(ChartPoint point, int xIndex = 0, int yIndex = 0)
        {
            return new Point(ToPlotArea(point.X, AxisTags.X, xIndex),
                ToPlotArea(point.Y, AxisTags.Y, yIndex));
        }

        protected double ToDrawMargin(double value, AxisTags source, int axis = 0)
        {
            return Methods.ToDrawMargin(value, source, Chart, axis);
        }

        protected ChartPoint ToDrawMargin(ChartPoint point, int axisX = 0, int axisY = 0)
        {
            return new ChartPoint
            {
                X = ToDrawMargin(point.X, AxisTags.X, axisX),
                Y = ToDrawMargin(point.Y, AxisTags.Y, axisY),
                Instance = point.Instance,
                IsMocked = point.IsMocked,
                Key = point.Key
            };
        }
        #endregion

        internal TextBlock BuildATextBlock(int rotate)
        {
            return new TextBlock
            {
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = Foreground,
                RenderTransform = new RotateTransform(rotate)
            };
        }
    }
}
