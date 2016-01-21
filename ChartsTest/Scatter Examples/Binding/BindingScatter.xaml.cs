using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Scatter_Examples
{
    public partial class BindingScatter
    {
        public BindingScatter()
        {
            InitializeComponent();

            ViewModel = new BindingScatterViewModel
            {
                FirstPrimaryValues = new ChartValues<Point>{ new Point(5, 2), new Point(12, 3), new Point(26, 10), new Point(30, 3), new Point(35, 4) },
                SecondPrimaryValues = new ChartValues<Point> { new Point(5, 3), new Point(8, 7), new Point(22, 14), new Point(28, 8), new Point(40, 8) }
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
        public ObservableCollection<Point> FirstPrimaryValues { get; set; }
        public ObservableCollection<Point> SecondPrimaryValues { get; set; }
    }
}
