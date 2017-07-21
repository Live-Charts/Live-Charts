using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Linq
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinqExample : Page
    {
        public LinqExample()
        {
            InitializeComponent();

            //lets configure the chart to plot cities
            Mapper = Mappers.Xy<City>()
                .X((city, index) => index)
                .Y(city => city.Population);

            MillionFormatter = value => (value / 1000000).ToString("N") + "M";

            this.Loading += LinqExample_Loading;

            DataContext = this;
        }

        private async void LinqExample_Loading(FrameworkElement sender, Object args)
        {
            await DataBase.Initialize();

            //lets take the first 15 records by default;
            var records = DataBase.Cities.OrderByDescending(x => x.Population).Take(15).ToArray();

            Results = records.AsChartValues();
            Labels = new ObservableCollection<string>(records.Select(x => x.Name));
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
