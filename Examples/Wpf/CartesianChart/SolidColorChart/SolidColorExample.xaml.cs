using System.Windows;
using System.Windows.Controls;
using LiveCharts;

namespace Wpf.CartesianChart.SolidColorChart
{
    /// <summary>
    /// Interaction logic for SolidColorExample.xaml
    /// </summary>
    public partial class SolidColorExample : UserControl
    {
        public SolidColorExample()
        {
            InitializeComponent();

            Values = new ChartValues<double> {150, 375, 420, 500, 160, 140};

            DataContext = this;
        }

        public ChartValues<double> Values { get; set; }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true);
        }
    }
}
