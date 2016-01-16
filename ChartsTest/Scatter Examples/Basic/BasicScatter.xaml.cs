using System.Windows;
using lvc;

namespace ChartsTest.Scatter_Examples
{
    public partial class BasicScatter
    {
        public BasicScatter()
        {
            InitializeComponent();
            Chart.AxisX.LabelFormatter = LabelFormatters.Number;
            Chart.AxisY.LabelFormatter = value => value + "°";
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
