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
            Serie1.DataContext = new ObservableCollection<double> { 2, 3, 5, 7, 6, 2 };
            Serie2.DataContext = new ObservableCollection<double> { 7, 3, 4, 1, 6, 2 };
        }

        private void BindingStackedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
