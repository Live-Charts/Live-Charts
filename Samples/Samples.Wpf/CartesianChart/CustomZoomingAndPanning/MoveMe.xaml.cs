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

namespace Wpf.CartesianChart.CustomZoomingAndPanning
{
    /// <summary>
    /// Interaction logic for MoveMe.xaml
    /// </summary>
    public partial class MoveMe : UserControl
    {
        public MoveMe()
        {
            InitializeComponent();

            SeriesValues = GetData();

            DataContext = this;
        }

        public ChartValues<double> SeriesValues { get; set; }

        private ChartValues<double> GetData()
        {
            var r = new Random();
            var trend = 100;
            var values = new ChartValues<double>();

            for (var i = 0; i < 160; i++)
            {
                var seed = r.NextDouble();
                if (seed > .8) trend += seed > .9 ? 50 : -50;
                values.Add(trend + r.Next(0, 10));
            }

            return values;
        }
    }
}
