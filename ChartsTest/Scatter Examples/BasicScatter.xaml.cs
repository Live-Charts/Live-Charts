using System.Windows;
using LiveCharts;

namespace ChartsTest.Scatter_Examples
{
    public partial class BasicScatter
    {
        public BasicScatter()
        {
            InitializeComponent();
            Chart.PrimaryAxis.LabelFormatter = LabelFormatters.Number;
            Chart.SecondaryAxis.LabelFormatter = value => value + "°";
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
