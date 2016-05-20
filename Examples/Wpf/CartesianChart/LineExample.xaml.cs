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
using LiveCharts.Wpf.Series;

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

            SeriesCollection = new SeriesCollection
            {
                new VerticalLineSeries
                {
                    Values = new ChartValues<float>
                    {
                        3,
                        4,
                        6,
                        3,
                        3,
                        3
                    },
                    DataLabels = true
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

    }
}
