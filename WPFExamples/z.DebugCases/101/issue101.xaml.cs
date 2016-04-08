using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LiveCharts.Annotations;

namespace ChartsTest.z.DebugCases._101
{
    /// <summary>
    /// Interaction logic for _101.xaml
    /// </summary>
    public partial class Issue101 : UserControl
    {
        public Issue101()
        {
            InitializeComponent();

            RawData = new ViewModel
            {
                ChartData = new double[]
                {
                    10, 5, 2
                }
            };

            DataContext = this;
        }

        public ViewModel RawData { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            RawData.ChartData = new double[]
            {
                r.Next(0, 10),
                r.Next(0, 10),
                r.Next(0, 10)
            };
        }
    }
}
