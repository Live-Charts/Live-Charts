using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts.Series;

namespace ChartsTest.MoreExamples.LineAndAreaChart
{
    /// <summary>
    /// Interaction logic for Example1.xaml
    /// </summary>
    public partial class Example1
    {
        public Example1()
        {
            InitializeComponent();
        }

        private void Example1_OnLoaded(object sender, RoutedEventArgs e)
        {
            Chart.Series = new ObservableCollection<Serie>
            {
                new LineSerie
                {
                    Name = "Washington",
                    PrimaryValues = new ObservableCollection<double> {15, 30, 23, 29, 45, 5, -10}
                },
                new LineSerie
                {
                    Name = "Tokio",
                    PrimaryValues = new ObservableCollection<double> {5, 2, 4, 3, 6, 2, 29}
                }
            };
            Chart.PrimaryAxis.LabelFormatter = value => value + " °C";
            Chart.SecondaryAxis.Labels = new[] {"January", "February", "March", "April", "May", "June", "July"};
        }
    }
}
