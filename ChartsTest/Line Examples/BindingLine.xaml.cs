using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.Line_Examples
{
    public partial class BindingLine
    {
        public BindingLineViewModel ViewModel { get; set; }
        public BindingLine()
        {
            InitializeComponent();
            ViewModel = new BindingLineViewModel
            {
                FirstSeries = new ObservableCollection<double> { 2, 4, double.NaN, 7, 8, 6, 2, 4, 2, 5 },
                SecondSeries = new ObservableCollection<double> { 7, 3, 4, 1, 5, 6, 8, 5, 1, 3 }
            };
            DataContext = this;
        }

        private void CleanLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class BindingLineViewModel
    {
        public ObservableCollection<double> FirstSeries { get; set; }
        public ObservableCollection<double> SecondSeries { get; set; }
    }
}
