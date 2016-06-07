using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Bubbles
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
                        new BubblePoint(5, 5, 20),
                        new BubblePoint(3, 4, 80),
                        new BubblePoint(7, 2, 20),
                        new BubblePoint(2, 6, 60),
                        new BubblePoint(8, 2, 70),
                        new BubblePoint(5, 5, 20),
                        new BubblePoint(3, 4, 80),
                        new BubblePoint(7, 2, 20),
                        new BubblePoint(2, 6, 60),
                        new BubblePoint(8, 2, 70),
                        new BubblePoint(5, 5, 20),
                        new BubblePoint(3, 4, 80),
                        new BubblePoint(7, 2, 20),
                        new BubblePoint(2, 6, 60),
                        new BubblePoint(8, 2, 70)
                    }
                },
                new BubbleSeries
                {
                    Values = new ChartValues<BubblePoint>
                    {
                        new BubblePoint(7, 5, 1),
                        new BubblePoint(2, 2, 1),
                        new BubblePoint(1, 1, 1),
                        new BubblePoint(6, 3, 1),
                        new BubblePoint(8, 8, 1),
                        new BubblePoint(7, 5, 1),
                        new BubblePoint(2, 2, 1),
                        new BubblePoint(1, 1, 1),
                        new BubblePoint(6, 3, 1),
                        new BubblePoint(8, 8, 1),
                        new BubblePoint(7, 5, 1),
                        new BubblePoint(2, 2, 1),
                        new BubblePoint(1, 1, 1),
                        new BubblePoint(6, 3, 1),
                        new BubblePoint(8, 8, 1)
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
