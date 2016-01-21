using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

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
            return v == null ? Colors.Transparent : Series.GetColorByIndex(v.Value);
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

            var index = values[0] as int?;
		    var pieSeriesViewer = values[1] as PieSeriesViewer;
		    if (pieSeriesViewer != null)
		    {
		        var pieSeries = (pieSeriesViewer.Series)[0] as PieSeries;
		        if (pieSeries != null)
		        {
		            var context = pieSeries.Labels;

		            if (index == null || context == null) return null;

		            return context.Count  > index ? context[index.Value] : null;
		        }
		    }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
