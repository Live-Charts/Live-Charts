using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LiveCharts.Series;

namespace LiveCharts.Viewers
{
	internal class SerieConverter : IValueConverter
	{
		public static SerieConverter Instance { get; }

		static SerieConverter()
		{
			Instance = new SerieConverter();
		}
		private SerieConverter() { }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var series = value as IEnumerable<Serie>;
			if (series != null)
				return series.Select(s => new SerieStandin
					{
						Label = s.Label,
						Color = s.Color
					});

			var serie = value as Serie;
			if (serie != null)
				return new SerieStandin
					{
						Label = serie.Label,
						Color = serie.Color
					};

			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}