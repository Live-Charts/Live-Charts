
using System.Windows;

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
    }
}
