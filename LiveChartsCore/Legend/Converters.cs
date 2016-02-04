//The MIT License(MIT)

//Copyright(c) 2015 Greg Dennis

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
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts.CoreComponents;

namespace LiveCharts.Viewers
{
    public class PieSerieColorConverter : IValueConverter
    {
		public static PieSerieColorConverter Instance { get; set; }

        static PieSerieColorConverter()
	    {
		    Instance = new PieSerieColorConverter();
	    }
		private PieSerieColorConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as int?;
            return v == null ? Colors.Transparent : Colors.Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PieSerieLabelConverter : IMultiValueConverter
    {
		public static PieSerieLabelConverter Instance { get; set; }

        static PieSerieLabelConverter()
		{
			Instance = new PieSerieLabelConverter();
		}
		private PieSerieLabelConverter() { }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return null;

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
