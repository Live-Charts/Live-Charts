using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts.Series;

namespace LiveCharts.Viewers
{
    public class PieSerieColorConverter : IValueConverter
    {
		public static PieSerieColorConverter Instance { get; }

	    static PieSerieColorConverter()
	    {
		    Instance = new PieSerieColorConverter();
	    }
		private PieSerieColorConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as int?;
            return v == null ? Colors.Transparent : Serie.GetColorByIndex(v.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PieSerieLabelConverter : IMultiValueConverter
    {
		public static PieSerieLabelConverter Instance { get; }

		static PieSerieLabelConverter()
		{
			Instance = new PieSerieLabelConverter();
		}
		private PieSerieLabelConverter() { }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return null;

            var index = values[0] as int?;
            var context = (((values[1] as PieSeriesViewer)?.Series)?[0] as PieSerie)?.Labels;

            if (index == null || context == null) return null;

            return context.Count  > index ? context[index.Value] : null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
