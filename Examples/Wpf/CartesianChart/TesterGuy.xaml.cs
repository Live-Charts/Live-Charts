using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class TesterGuy : UserControl
    {
        public TesterGuy()
        {
            InitializeComponent();

            ChartValues = new ChartValues<double> {1, 6, 2, 7, 4};
        }

        public ChartValues<double> ChartValues { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ChartValues.Clear();
        }
    }
}
