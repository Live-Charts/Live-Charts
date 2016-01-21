//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lvc.TypeConverters;

namespace lvc
{
    public class Axis : FrameworkElement
    {
        public Axis()
        {
            CleanFactor = 3;
            Separator = new Separator
            {
                IsEnabled = true,
                Color = Color.FromRgb(242, 242, 242),
                Thickness = 1
            };
        }

        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof (IList<string>), typeof (Axis), new PropertyMetadata(default(IList<string>)));

        [TypeConverter(typeof (StringCollectionConverter))]
        public IList<string> Labels
        {
            get
            {
                return (IList<string>) GetValue(LabelsProperty);
            }
            set
            {
                SetValue(LabelsProperty, value);
            }
        }

		public static readonly DependencyProperty LabelFormatterProperty =
			DependencyProperty.Register("LabelFormatter", typeof(Func<double, string>), typeof(Axis), new PropertyMetadata(null));

		public Func<double, string> LabelFormatter
		{
			get { return (Func<double, string>)GetValue(LabelFormatterProperty); }
			set { SetValue(LabelFormatterProperty, value); }
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("Color", typeof(Color), typeof(Axis), new PropertyMetadata(Color.FromRgb(242, 242, 242)));

		/// <summary>
		/// Gets or sets axis color.
		/// </summary>
		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		public static readonly DependencyProperty ThicknessProperty =
			DependencyProperty.Register("Thickness", typeof(int), typeof(Axis), new PropertyMetadata(3));

		/// <summary>
		/// Gets or sets axis thickness.
		/// </summary>
		public int Thickness
		{
			get { return (int)GetValue(ThicknessProperty); }
			set { SetValue(ThicknessProperty, value); }
		}

		public static readonly DependencyProperty ShowLabelsProperty =
			DependencyProperty.Register("ShowLabels", typeof(bool), typeof(Axis), new PropertyMetadata(true));

		/// <summary>
		/// Gets or sets if labels should be printed.
		/// </summary>
		public bool ShowLabels
		{
			get { return (bool)GetValue(ShowLabelsProperty); }
			set { SetValue(ShowLabelsProperty, value); }
		}

		public static readonly DependencyProperty MaxValueProperty =
			DependencyProperty.Register("MaxValue", typeof(double?), typeof(Axis), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets chart max value, set it to null to make this property Auto, default value is null
		/// </summary>
		public double? MaxValue
		{
			get { return (double?)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}

		public static readonly DependencyProperty MinValueProperty =
			DependencyProperty.Register("MinValue", typeof(double?), typeof(Axis), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets chart min value, set it to null to make this property Auto, default value is null
		/// </summary>
		public double? MinValue
		{
			get { return (double?)GetValue(MinValueProperty); }
			set { SetValue(MinValueProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(Axis), new PropertyMetadata(null));

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty FontFamilyProperty =
			DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(Axis), new PropertyMetadata(new FontFamily("Calibri")));

		/// <summary>
		/// Gets or sets labels font family
		/// </summary>
		public FontFamily FontFamily
		{
			get { return (FontFamily)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		public static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(double), typeof(Axis), new PropertyMetadata(11.0));

		/// <summary>
		/// Gets or sets labels font size
		/// </summary>
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		public static readonly DependencyProperty FontWeightProperty =
			DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(Axis), new PropertyMetadata(FontWeights.Normal));

		/// <summary>
		/// Gets or sets labels font weight
		/// </summary>
		public FontWeight FontWeight
		{
			get { return (FontWeight)GetValue(FontWeightProperty); }
			set { SetValue(FontWeightProperty, value); }
		}

		public static readonly DependencyProperty FontStyleProperty =
			DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(Axis), new PropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
		public FontStyle FontStyle
		{
			get { return (FontStyle)GetValue(FontStyleProperty); }
			set { SetValue(FontStyleProperty, value); }
		}

		public static readonly DependencyProperty FontStretchProperty =
			DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(Axis), new PropertyMetadata(FontStretches.Normal));

        /// <summary>
        /// Gets or sets labels font strech
        /// </summary>
		public FontStretch FontStretch
		{
			get { return (FontStretch)GetValue(FontStretchProperty); }
			set { SetValue(FontStretchProperty, value); }
		}

	    public static readonly DependencyProperty ForegroundProperty =
		    DependencyProperty.Register("Foreground", typeof (Brush), typeof (Axis), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(150, 150, 150))));

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
		public Brush Foreground
		{
			get { return (Brush)GetValue(ForegroundProperty); }
			set { SetValue(ForegroundProperty, value); }
		}

		/// <summary>
		/// Get or sets configuration for parallel lines to axis.
		/// </summary>
		public Separator Separator { get; set; }
        /// <summary>
        /// Factor used to calculate label separations. default is 3. increase it to make it 'cleaner'
        /// initialSeparations = Graph.Heigth / (label.Height * cleanFactor)
        /// </summary>
        internal int CleanFactor { get; set; }

        internal bool IgnoresLastLabel { get; set; }

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