using System.Windows;
using System.Windows.Controls;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for CustomBar.xaml
    /// </summary>
    public partial class CustomBar : UserControl
    {
        public CustomBar()
        {
            InitializeComponent();
        }

        private void CustomBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
