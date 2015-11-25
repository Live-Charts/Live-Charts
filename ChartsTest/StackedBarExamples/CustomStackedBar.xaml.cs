using System.Windows;
using System.Windows.Controls;

namespace ChartsTest.StackedBarExamples
{
    /// <summary>
    /// Interaction logic for CustomBar.xaml
    /// </summary>
    public partial class CustomStackedBar
    {
        public CustomStackedBar()
        {
            InitializeComponent();
        }

        private void CustomStackedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
