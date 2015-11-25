using System.Windows;

namespace ChartsTest.Line_Examples
{
    public partial class JustAreasAndZoomable
    {
        public JustAreasAndZoomable()
        {
            InitializeComponent();
        }

        private void JustAreasAndZoomable_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
