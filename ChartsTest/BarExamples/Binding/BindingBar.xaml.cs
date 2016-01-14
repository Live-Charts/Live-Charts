using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for BindingBar.xaml
    /// </summary>
    public partial class BindingBar
    {
        public BindingBar()
        {
            InitializeComponent();
            ViewModel = new BindingBarViewModel
            {
                FirstSerie = new ObservableCollection<double> { 2, 3, 5, 7 },
                SecondSerie = new ObservableCollection<double> { 7, 3, 4, 1 }
            };
            DataContext = this;
        }

        public BindingBarViewModel ViewModel { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class BindingBarViewModel
    {
        public ObservableCollection<double> FirstSerie { get; set; }
        public ObservableCollection<double> SecondSerie { get; set; }
    }
}
