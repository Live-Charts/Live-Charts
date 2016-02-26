using System;
using System.Windows;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace ChartsTest.Pie_Examples.Mvvm
{
    /// <summary>
    /// Interaction logic for MvvmBar.xaml
    /// </summary>
    public partial class MvvmPie
    {
        public MvvmPie()
        {
            InitializeComponent();
            Sales = new SalesViewModel();
            Chart.DataTooltip = new SalesTooltip();
            DataContext = this;
        }

        public SalesViewModel Sales { get; set; }

        private void AddMonthOnClick(object sender, RoutedEventArgs e)
        {
            Sales.AddOneMonth();
        }
        private void RemoveMonthOnClick(object sender, RoutedEventArgs e)
        {
            Sales.RemoveLastMonth();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Update();
        }
    }

    public class SalesData
    {
        public int ItemsSold { get; set; }
        public decimal ItemsAverageSellPrice { get; set; }
        public double Rentability { get; set; }
    }

    public class SalesViewModel
    {
        public SalesViewModel()
        {
            var config = new SeriesConfiguration<SalesData>().Y(data => data.ItemsSold);
            Salesmen = new SeriesCollection(config)
            {
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<SalesData>
                    {
                        new SalesData {ItemsSold = 15, Rentability = .15, ItemsAverageSellPrice = 5000}
                    }
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<SalesData>
                    {
                        new SalesData {ItemsSold = 16, Rentability = .12, ItemsAverageSellPrice = 5200}
                    }
                },
                new PieSeries
                {
                    Title = "George",
                    Values = new ChartValues<SalesData>
                    {
                        new SalesData {ItemsSold = 22, Rentability = .11, ItemsAverageSellPrice = 5100}
                    }
                }
            };
        }

        public SeriesCollection Salesmen { get; set; }

		public void AddOneMonth()
        {
            var r = new Random();
            if (Salesmen.Count >= 20) return;
		    Salesmen.Add(new PieSeries
		    {
		        Title = "A Dynamic Name",
		        Values = new ChartValues<SalesData>
		        {
		            new SalesData
		            {
		                ItemsSold = r.Next(5, 30),
		                Rentability = .15,
		                ItemsAverageSellPrice = 5000
		            }
		        }
		    });
        }

        public void RemoveLastMonth()
        {
            if (Salesmen.Count == 2) return;
            Salesmen.RemoveAt(0);
        }
    }
}
