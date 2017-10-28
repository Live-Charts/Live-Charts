using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;

namespace Wpf.CartesianChart.Linq
{

    public partial class LinqExample : UserControl
    {

        public LinqExample()
        {
            InitializeComponent();

            //lets configure the chart to plot cities
            Mapper = Mappers.Xy<City>()
                .X((city, index) => index)
                .Y(city => city.Population);

            //lets take the first 15 records by default;
            var records = DataBase.Cities.OrderByDescending(x => x.Population).Take(15).ToArray();

            Results = records.AsChartValues();
            Labels = new ObservableCollection<string>(records.Select(x => x.Name));

            MillionFormatter = value => (value/1000000).ToString("N") + "M";

            DataContext = this;
        }

        public ChartValues<City> Results { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public Func<double, string> MillionFormatter { get; set; }

        public object Mapper { get; set; }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var q = (Query.Text ?? string.Empty).ToUpper();

            var records = DataBase.Cities
                .Where(x => x.Name.ToUpper().Contains(q) || x.Country.ToUpper().Contains(q))
                .OrderByDescending(x => x.Population)
                .Take(15)
                .ToArray();

            Results.Clear();
            Results.AddRange(records);

            Labels.Clear();
            foreach (var record in records) Labels.Add(record.Name);

        }
    }
}
