using System.Windows;
using System.Windows.Controls;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for ZoomableBar.xaml
    /// </summary>
    public partial class ZoomableBar
    {
        public ZoomableBar()
        {
            InitializeComponent();
        }

        private void ZoomableBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
