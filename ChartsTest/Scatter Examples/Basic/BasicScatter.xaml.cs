using System.Windows;
using LiveCharts;

namespace ChartsTest.Scatter_Examples
{
    public partial class BasicScatter
    {
        public BasicScatter()
        {
            InitializeComponent();
            Chart.AxisY.LabelFormatter = LabelFormatters.Number;
            Chart.AxisX.LabelFormatter = value => value + "°";
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
