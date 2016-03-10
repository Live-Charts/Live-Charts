using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples.Mvvm
{
    /// <summary>
    /// Interaction logic for MvvmExample.xaml
    /// </summary>
    public partial class MvvmLine
    {
        public MvvmLine()
        {
            InitializeComponent();

            //In this case we are not only plotting double values,
            //instead each point is an instance of MonthSalesData class.

            //We need to let LiveCharts know how to use MonthSalesData class.
            //you can specify wich property to use as X or Y in chart.
            //in this case we are going to use the SoldItems property as Y
            //we are going to use an indexed X 
            //this means first point is 0, second 1, third 2 and so on.
            //X and Y are indexed by default, and it is not necessary to specify it
            //we are doing it just to exmplain how LiveCharts works.
            var config = new SeriesConfiguration<MonthSalesData>()
                .Y(point => point.SoldItems)
                .X((point, index) => index);

            //now we create a new SeriesCollection with this configuration
            Sales = new SeriesCollection(config);

            //we add some default series
            Sales.Add(new LineSeries
            {
                Title = "Charles",
                Values = new ChartValues<MonthSalesData>
                {
                    new MonthSalesData {SoldItems = 15, BestSellers = new[] {"Apple", "Grape"}},
                    new MonthSalesData {SoldItems = 8, BestSellers = new[] {"Orange", "Tomate"}},
                    new MonthSalesData {SoldItems = 8, BestSellers = new[] {"Banana"}}
                }
            });
            
            //Some labels for X axis
            Labels = new List<string> {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Agu", "Sep", "Oct", "Nov", "Dec"};
            //Specify a custom format for Y values.
            YFormatter = y => y + ".00k items";
            //And a custom tooltip
            Chart.DataTooltip = new SalesTooltip();

            DataContext = this;
        }

        public SeriesCollection Sales { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public List<string> Labels { get; set; }

        private void MvvmExample_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Update();
        }

        private void AddSalesManOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            var c = Sales.Count > 0 ? Sales[0].Values.Count : 3;

            var values = new ChartValues<MonthSalesData>();
            for (int i = 0; i < c; i++)
            {
                values.Add(new MonthSalesData
                {
                    SoldItems = r.Next(0, 20),
                    BestSellers = new[] { "A random fruit" }
                });
            }
            Sales.Add(new LineSeries { Values = values });
        }

        private void RemoveSalesManOnClick(object sender, RoutedEventArgs e)
        {
            if (Sales.Count > 0) Sales.RemoveAt(0);
        }

        private void AddPointsOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var salesSeries in Sales)
            {
                salesSeries.Values.Add(new MonthSalesData
                {
                    SoldItems = r.Next(0, 20),
                    BestSellers = new[] {"A random fruit"}
                });
            }
        }

        private void RemovePointsOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var salesSeries in Sales)
            {
                if (salesSeries.Values.Count > 0) salesSeries.Values.RemoveAt(0);
            }
        }

        private void Chart_OnDataClick(ChartPoint point)
        {
            var salesData = point.Instance as MonthSalesData;
            if (salesData == null) return;
            MessageBox.Show("You clicked on: (" + point.X + ", " + point.Y + "), " +
                            "sold items:" + salesData.SoldItems + ", " +
                            "best sellers: " + salesData.BestSellers.Aggregate((x, y) => x + y));
        }
    }
}