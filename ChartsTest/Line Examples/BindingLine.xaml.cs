using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest.Line_Examples
{
    public partial class BindingLine
    {
        public BindingLine()
        {
            InitializeComponent();
            Serie1.DataContext = new ObservableCollection<double> {2, 3, 5, 7};
            Serie2.DataContext = new ObservableCollection<double> {7, 3, 4, 1};
        }

        private void CleanLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
