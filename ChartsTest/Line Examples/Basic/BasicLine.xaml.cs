
using System.Windows;

namespace ChartsTest.Line_Examples
{
    public partial class BasicLine
    {
        public BasicLine()
        {
            InitializeComponent();
        }

        private void BasicLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
