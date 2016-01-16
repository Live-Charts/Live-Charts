using System.Windows;

namespace ChartsTest.Pie_Examples
{
    /// <summary>
    /// Interaction logic for CustomBar.xaml
    /// </summary>
    public partial class CustomPie 
    {
        public CustomPie()
        {
            InitializeComponent();
            Chart.AxisX.LabelFormatter = val => (val/Chart.PieTotalSum).ToString("P");
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
