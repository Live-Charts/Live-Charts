using System.Windows;
using System.Windows.Controls;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for BasicBar.xaml
    /// </summary>
    public partial class BasicBar
    {
        public BasicBar()
        {
            InitializeComponent();
        }

        private void BasicBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
