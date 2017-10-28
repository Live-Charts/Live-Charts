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
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for LineExample.xaml
    /// </summary>
    public partial class LineExample : UserControl
    {
        public LineExample()
        {
            InitializeComponent();

            Values = Values = new ChartValues<float>
            {
                3,
                4,
                6,
                3,
                2,
                6
            };
            
            DataContext = this;
        }

        public ChartValues<float> Values { get; set; }

    }
}
