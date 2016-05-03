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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LiveCharts.Wpf.Components
{
    public abstract class Series : FrameworkElement, ISeriesView
    {
        public SeriesCore Model { get; set; }

        #region Properties

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
            "Values", typeof (IChartValues), typeof (Series),
            new PropertyMetadata(default(IChartValues), UpdateChart(true)));
        /// <summary>
        /// Gets or sets chart values.
        /// </summary>
        public IChartValues Values
        {
            get { return (IChartValues) GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (Series),
            new PropertyMetadata(default(string), UpdateChart()));
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
            new PropertyMetadata(default(Brush), UpdateChart()));
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
            new PropertyMetadata(default(double), UpdateChart()));
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
            new PropertyMetadata(default(Brush), UpdateChart()));
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
            new PropertyMetadata(default(bool), UpdateChart()));
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
            new PropertyMetadata(default(FontFamily), UpdateChart()));
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
            typeof (Series), new PropertyMetadata(10d, UpdateChart()));
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
            new PropertyMetadata(FontWeights.Bold, UpdateChart()));
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
            typeof (Series), new PropertyMetadata(FontStyles.Normal, UpdateChart()));
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
            typeof (Series), new PropertyMetadata(FontStretches.Normal, UpdateChart()));
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
            typeof (Series), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(55, 71, 79)), UpdateChart()));
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
            new PropertyMetadata(default(DoubleCollection), UpdateChart()));
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

        public SeriesConfiguration Configuration { get; set; }

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

        public virtual IChartPointView RenderPoint(IChartPointView view)
        {
            throw new NotImplementedException();
        }

        public virtual void RemovePointView(object view)
        {
            throw new NotImplementedException();
        }

        public virtual void OnSeriesUpdateStart()
        {
        }

        public void Erase()
        {
            throw new NotImplementedException();
        }

        public virtual void OnSeriesUpdatedFinish()
        {
        }

        public virtual void EraseMeIfYOuSeeMe()
        {
            throw new NotImplementedException();
        }

        protected static PropertyChangedCallback UpdateChart(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfSeries = o as Series;
                if (wpfSeries == null) return;

                if (wpfSeries.Model.Chart != null) wpfSeries.Model.Chart.Updater.Run(animate);
            };
        }
    }
}
