using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for FinancialSeries.xaml
    /// </summary>
    public partial class FinancialExample
    {
        
        public FinancialExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new OhlcSeries
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(DateTime.Now, 32, 35, 30, 32), 
                        new OhlcPoint(DateTime.Now.AddDays(1), 33, 38, 31, 37),
                        new OhlcPoint(DateTime.Now.AddDays(2), 35, 42, 30, 40),
                        new OhlcPoint(DateTime.Now.AddDays(3), 37, 40, 35, 38),
                        new OhlcPoint(DateTime.Now.AddDays(4), 35, 38, 32, 33)
                    }
                }
            };

            DateTimeFormatter = x => ChartFunctions.FromChartDay(x).ToString("dd MMM");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var point in SeriesCollection[0].Values.Cast<OhlcPoint>())
            {
                point.Open = r.Next((int) point.Low, (int) point.High);
                point.Close = r.Next((int) point.Low, (int) point.High);
            }
        }
    }
}
