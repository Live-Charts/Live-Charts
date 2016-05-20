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
    /// Interaction logic for BubbleSeries.xaml
    /// </summary>
    public partial class BubblesExample : UserControl
    {
        public BubblesExample()
        {
            InitializeComponent();
                
            SeriesCollection = new SeriesCollection
            {
                new BubbleSeries
                {
                    Values = new ChartValues<BubblePoint>
                    {
                        new BubblePoint(5, 5, 2),
                        new BubblePoint(3, 4, 8),
                        new BubblePoint(7, 2, 2),
                        new BubblePoint(2, 6, 6),
                        new BubblePoint(8, 2, 7),
                    },
                    DataLabels = true
                },
                new BubbleSeries
                {
                    Values = new ChartValues<BubblePoint>
                    {
                        new BubblePoint(7, 5, 5),
                        new BubblePoint(2, 2, 4),
                        new BubblePoint(1, 1, 2),
                        new BubblePoint(6, 3, 1),
                        new BubblePoint(8, 8, 5),
                    }
                }
            };

            DataContext = this;
        }
        public SeriesCollection SeriesCollection { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                foreach (var bubble in series.Values.Cast<BubblePoint>())
                {
                    bubble.X = r.NextDouble()*10;
                    bubble.Y = r.NextDouble()*10;
                    bubble.Weight = r.NextDouble()*10;
                }
            }
        }
    }
}
