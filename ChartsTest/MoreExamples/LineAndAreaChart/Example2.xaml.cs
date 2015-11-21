using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts.Series;

namespace ChartsTest.MoreExamples.LineAndAreaChart
{
    /// <summary>
    /// Interaction logic for Example2.xaml
    /// </summary>
    public partial class Example2
    {
        public Example2()
        {
            InitializeComponent();
        }

        private void Example2_OnLoaded(object sender, RoutedEventArgs e)
        {
            Chart.Series = new ObservableCollection<Serie>
            {
                new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double> {15, 30, 23, 29, 45, 5, -10}
                },
                new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double> {5, 2, 4, 3, 6, 2, 29}
                }
            };
        }
    }
}
