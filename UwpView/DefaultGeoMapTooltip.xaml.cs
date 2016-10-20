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
using System.Globalization;
using Windows.UI.Xaml;
using LiveCharts.Uwp.Components.MultiBinding;

namespace LiveCharts.Uwp
{
    public partial class DefaultGeoMapTooltip
    {
        public DefaultGeoMapTooltip()
        {
            InitializeComponent();

            /*Current*/SetValue(CornerRadiusProperty, 4d);

            DataContext = this;
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (double), typeof (DefaultGeoMapTooltip), new PropertyMetadata(default(double)));

        public double CornerRadius
        {
            get { return (double) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof (Func<double, string>), typeof (DefaultGeoMapTooltip), new PropertyMetadata(default(Func<double, string>)));

        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>) GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        public static readonly DependencyProperty GeoDataProperty = DependencyProperty.Register(
            "GeoData", typeof (GeoData), typeof (DefaultGeoMapTooltip), new PropertyMetadata(default(GeoData)));

        public GeoData GeoData
        {
            get { return (GeoData) GetValue(GeoDataProperty); }
            set { SetValue(GeoDataProperty, value); }
        }

    }

    public class GeoData
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public class GeoDataLabelConverter : MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Func<double, string> defF = x => x.ToString(CultureInfo.InvariantCulture);
            var f = values[1] as Func<double, string> ?? defF;
            return f(values[0] as double? ?? 0);
        }

        public override Object[] ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
