using System.Windows;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for CustomBar.xaml
    /// </summary>
    public partial class CustomBar
    {
        public CustomBar()
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
