using System.Windows;

namespace ChartsTest.Pie_Examples
{
    /// <summary>
    /// Interaction logic for BasicBar.xaml
    /// </summary>
    public partial class BasicPie
    {
        public BasicPie()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Redraw();
        }
    }
}
