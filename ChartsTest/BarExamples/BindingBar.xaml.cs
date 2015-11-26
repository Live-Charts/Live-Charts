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
            Serie1.DataContext = new ObservableCollection<double> { 2, 3, 5, 7 };
            Serie2.DataContext = new ObservableCollection<double> { 7, 3, 4, 1 };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
