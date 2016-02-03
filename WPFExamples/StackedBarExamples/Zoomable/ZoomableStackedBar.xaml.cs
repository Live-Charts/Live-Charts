using System.Windows;

namespace ChartsTest.StackedBarExamples
{
    /// <summary>
    /// Interaction logic for ZoomableBar.xaml
    /// </summary>
    public partial class ZoomableStackedBar
    {
        public ZoomableStackedBar()
        {
            InitializeComponent();
        }

        private void ZoomableBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Redraw();
        }
    }
}
