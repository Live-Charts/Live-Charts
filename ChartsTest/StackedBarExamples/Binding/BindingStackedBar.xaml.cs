using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.StackedBarExamples
{
    /// <summary>
    /// Interaction logic for BindingBar.xaml
    /// </summary>
    public partial class BindingStackedBar
    {
        public BindingStackedBar()
        {
            InitializeComponent();

            ViewModel = new BindingStackedBarViewModel
            {
                FirstSeries = new ObservableCollection<double> { 2, 3, 5, 7, 6, 2 },
                SecondSeries = new ObservableCollection<double> { 7, 3, 4, 1, 6, 2 }
            };

            DataContext = this;
        }

        public BindingStackedBarViewModel ViewModel { get; set; }

        private void BindingStackedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class BindingStackedBarViewModel
    {
        public ObservableCollection<double> FirstSeries { get; set; }
        public ObservableCollection<double> SecondSeries { get; set; }
    }
}
