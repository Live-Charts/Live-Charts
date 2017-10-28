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
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts.Uwp.Components.MultiBinding;

namespace LiveCharts.Uwp
{
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
        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(List<SeriesViewModel>), typeof(DefaultLegend), new PropertyMetadata(null));



        /// <summary>
        /// The orientation property
        /// </summary>
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

        /// <summary>
        /// The internal orientation property
        /// </summary>
        public static readonly DependencyProperty InternalOrientationProperty = DependencyProperty.Register(
            "InternalOrientation", typeof (Orientation), typeof (DefaultLegend), 
            new PropertyMetadata(default(Orientation)));

        /// <summary>
        /// Gets or sets the internal orientation.
        /// </summary>
        /// <value>
        /// The internal orientation.
        /// </value>
        public Orientation InternalOrientation
        {
            get { return (Orientation) GetValue(InternalOrientationProperty); }
            set { SetValue(InternalOrientationProperty, value); }
        }

        /// <summary>
        /// The bullet size property
        /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Uwp.Components.MultiBinding.MultiValueConverterBase" />
    public class OrientationConverter : MultiValueConverterBase
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="values">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public override Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Orientation?)values[0] ?? (Orientation)values[1];
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Object[] ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
