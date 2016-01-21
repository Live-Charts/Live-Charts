using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Pie_Examples
{
    /// <summary>
    /// Interaction logic for BindingBar.xaml
    /// </summary>
    public partial class BindingPie
    {
        public BindingPie()
        {
            InitializeComponent();
            ViewModel = new BindingPieViewModel
            {
                FirstSeries = new ChartValues<double> { 2, 3, 5, 7 }
            };
            DataContext = this;
        }

        public BindingPieViewModel ViewModel { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class BindingPieViewModel
    {
        public ObservableCollection<double> FirstSeries { get; set; }
    }
}
