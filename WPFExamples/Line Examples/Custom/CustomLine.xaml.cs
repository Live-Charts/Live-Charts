
using System.Windows;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples
{
    public partial class CustomLine
    {
        public CustomLine()
        {
            InitializeComponent();
        }

        private void CustomLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }

        private void Chart_OnDataClick(ChartPoint point)
        {
            MessageBox.Show("you clicked (" + point.X + "," + point.Y + ")");

            // point.Instance contains the value as object, in case you passed a class, or any other object
        }
    }
}
