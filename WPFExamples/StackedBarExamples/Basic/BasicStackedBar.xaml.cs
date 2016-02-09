using System.Windows;

namespace ChartsTest.StackedBarExamples
{
    /// <summary>
    /// Interaction logic for BasicBar.xaml
    /// </summary>
    public partial class BasicStackedBar
    {
        public BasicStackedBar()
        {
            InitializeComponent();
        }

        private void BasicStackedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Update();
        }
    }
}
