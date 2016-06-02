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
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Basic_Bars
{
    /// <summary>
    /// Interaction logic for BasicRowExample.xaml
    /// </summary>
    public partial class BasicRowExample : UserControl
    {
        public BasicRowExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>
                    {

                    }
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

    }
}
