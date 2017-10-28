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
using System.Globalization;
using Windows.UI.Xaml;
using LiveCharts.Uwp.Components.MultiBinding;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public partial class DefaultGeoMapTooltip
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGeoMapTooltip"/> class.
        /// </summary>
        public DefaultGeoMapTooltip()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (double), typeof (DefaultGeoMapTooltip), new PropertyMetadata(4d));

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public double CornerRadius
        {
            get { return (double) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// The label formatter property
        /// </summary>
        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof (Func<double, string>), typeof (DefaultGeoMapTooltip), new PropertyMetadata(default(Func<double, string>)));

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        /// <value>
        /// The label formatter.
        /// </value>
        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>) GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        /// <summary>
        /// The geo data property
        /// </summary>
        public static readonly DependencyProperty GeoDataProperty = DependencyProperty.Register(
            "GeoData", typeof (GeoData), typeof (DefaultGeoMapTooltip), new PropertyMetadata(default(GeoData)));

        /// <summary>
        /// Gets or sets the geo data.
        /// </summary>
        /// <value>
        /// The geo data.
        /// </value>
        public GeoData GeoData
        {
            get { return (GeoData) GetValue(GeoDataProperty); }
            set { SetValue(GeoDataProperty, value); }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class GeoData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Uwp.Components.MultiBinding.MultiValueConverterBase" />
    public class GeoDataLabelConverter : MultiValueConverterBase
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
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Func<double, string> defF = x => x.ToString(CultureInfo.InvariantCulture);
            var f = values[1] as Func<double, string> ?? defF;
            return f(values[0] as double? ?? 0);
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
