using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Line_Examples
{
    public partial class BindingLine
    {
        public BindingLine()
        {
            InitializeComponent();
            Serie1.DataContext = new ObservableCollection<double> {2, 3, 5, 7};
            Serie2.DataContext = new ObservableCollection<double> {7, 3, 4, 1};
            Chart.Series.Add(new LineSeries
            {
                Title = "Charles",
                PrimaryValues = new ObservableCollection<double> { 5, 8, 1, 9}
            });
        }

        private void CleanLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
