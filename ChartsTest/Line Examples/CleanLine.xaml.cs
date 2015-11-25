using System.Windows;

namespace ChartsTest.Line_Examples
{
    public partial class CleanLine
    {
        public CleanLine()
        {
            InitializeComponent();
        }

        private void CleanLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
