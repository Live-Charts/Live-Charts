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
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts.Uwp.Components.MultiBinding;

namespace LiveCharts.Uwp
{
    public interface IChartLegend
    {
        List<SeriesViewModel> Series { get; set; }
    }

    /// <summary>
    /// The default legend control, by default a new instance of this control is created for every chart that requires a legend.
    /// </summary>
    public partial class DefaultLegend : IChartLegend
    {
        /// <summary>
        /// Initializes a new instance of DefaultLegend class
        /// </summary>
        public DefaultLegend()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the series displayed in the legend.
        /// </summary>
        public List<SeriesViewModel> Series
        {
            get { return (List<SeriesViewModel>)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Series.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(List<SeriesViewModel>), typeof(DefaultLegend), new PropertyMetadata(null));



        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof (Orientation?), typeof (DefaultLegend), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the orientation of the legend, default is null, if null LiveCharts will decide which orientation to use, based on the Chart.Legend location property.
        /// </summary>
        public Orientation? Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty InternalOrientationProperty = DependencyProperty.Register(
            "InternalOrientation", typeof (Orientation), typeof (DefaultLegend), 
            new PropertyMetadata(default(Orientation)));

        public Orientation InternalOrientation
        {
            get { return (Orientation) GetValue(InternalOrientationProperty); }
            set { SetValue(InternalOrientationProperty, value); }
        }

        public static readonly DependencyProperty BulletSizeProperty = DependencyProperty.Register(
            "BulletSize", typeof(double), typeof(DefaultLegend), new PropertyMetadata(15d));
        /// <summary>
        /// Gets or sets the bullet size, the bullet size modifies the drawn shape size.
        /// </summary>
        public double BulletSize
        {
            get { return (double)GetValue(BulletSizeProperty); }
            set { SetValue(BulletSizeProperty, value); }
        }
    }

    public class OrientationConverter : MultiValueConverterBase
    {
        public override Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Orientation?)values[0] ?? (Orientation)values[1];
        }

        public override Object[] ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
