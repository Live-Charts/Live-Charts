using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.Scatter_Examples
{
    public partial class BindingScatter
    {
        public BindingScatter()
        {
            InitializeComponent();
            var secondaryValues = new double[] {16, 32, 64, 128};
            Serie1.DataContext = new ObservableCollection<double> {2, 3, 5, 7};
            Serie1.SecondaryValues = secondaryValues;
            Serie2.DataContext = new ObservableCollection<double> {7, 3, 4, 1};
            Serie2.SecondaryValues = secondaryValues;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
