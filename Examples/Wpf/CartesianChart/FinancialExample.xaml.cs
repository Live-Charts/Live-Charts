using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                        new OhlcPoint(0, 32, 35, 30, 32), //this is an alpha version, the first param should be date time
                        new OhlcPoint(1, 33, 38, 31, 37), //sadly i am having some troubles with datetimes right now...
                        new OhlcPoint(2, 35, 42, 30, 40),
                        new OhlcPoint(3, 37, 40, 35, 38),
                        new OhlcPoint(4, 35, 38, 32, 33)
                    }
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

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
