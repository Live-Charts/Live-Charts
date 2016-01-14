using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.Scatter_Examples
{
    public partial class BindingScatter
    {
        public BindingScatter()
        {
            InitializeComponent();

            ViewModel = new BindingScatterViewModel
            {
                FirstPrimaryValues = new ObservableCollection<double> { 2, 4, double.NaN, 7, 6, 4, 5, 2, 7, 9, 1, 4, 4, 5, 6 },
                FirstSecondaryValues = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, double.NaN, 14, 15 },
                SecondPrimaryValues = new ObservableCollection<double> { 7, 3, 4, 1, 4, 7, 2, 7, 3, 8, 9, 2, 7, 9, 3 },
                SecondSecondaryValues = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }
            };

            DataContext = this;
        }

        public BindingScatterViewModel ViewModel { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class BindingScatterViewModel
    {
        public ObservableCollection<double> FirstPrimaryValues { get; set; }
        public IEnumerable<double> FirstSecondaryValues { get; set; } 
        public ObservableCollection<double> SecondPrimaryValues { get; set; }
        public IEnumerable<double> SecondSecondaryValues { get; set; } 
    }
}
