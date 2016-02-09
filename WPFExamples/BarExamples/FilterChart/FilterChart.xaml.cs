using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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

            //create a configuration for City class
            var config = new SeriesConfiguration<City>()
                .Y(city => city.Population); // use Population an Y
                                             // X will use default config, a zero based index
            
            //create a series collection with this config
            Series = new SeriesCollection(config);

            //lets pull some initials results
            var results = DataBase.Cities.OrderByDescending(city => city.Population).Take(15).ToArray();

            PopulationSeries = new BarSeries
            {
                Title = "Population by city 2015",
                Values = results.AsChartValues(),
                DataLabels = true
            };

            //there are 2 types of labels, when we use a formatter, and a strong array mapping
            //in this case instead of a label formatter we use a strong array labels
            //since X is a zero based index LiveCharts automatically maps this array with X
            //so when X = 0 label will be labels[0], when X = 1 labels[1], X = 2 labels[2], X = n labels[n]
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
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FilterChart_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this line is only to display animation every time you change view in this example
            Chart.Update();
        }
    }
}
