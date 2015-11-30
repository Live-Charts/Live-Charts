using System.Windows;

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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
