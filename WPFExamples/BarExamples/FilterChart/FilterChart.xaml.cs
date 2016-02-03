using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Annotations;

namespace ChartsTest.BarExamples.FilterChart
{
    /// <summary>
    /// Interaction logic for FilterChart.xaml
    /// </summary>
    public partial class FilterChart : INotifyPropertyChanged
    {
        private string[] _labels;

        public FilterChart()
        {
            InitializeComponent();

            var config = new SeriesConfiguration<City>()
                .Y(city => city.Population);
            
            Series = new SeriesCollection(config);

            var results = DataBase.Cities.OrderByDescending(city => city.Population).Take(15).ToArray();

            PopulationSeries = new BarSeries
            {
                Title = "Population by city 2015",
                Values = results.AsChartValues()
            };

            Labels = results.Select(city => city.Name).ToArray();

            Series.Add(PopulationSeries);

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
        public BarSeries PopulationSeries { get; set; }
        public string Criteria { get; set; }

        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var c = Criteria.ToUpper();

            var results = DataBase.Cities
                .Where(city => city.Name.ToUpper().Contains(c) || city.Country.ToUpper().Contains(c))
                .OrderByDescending(city => city.Population)
                .Take(15)
                .ToArray();

            Labels = results.Select(city => city.Name).ToArray();
            PopulationSeries.Values = results.AsChartValues();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
