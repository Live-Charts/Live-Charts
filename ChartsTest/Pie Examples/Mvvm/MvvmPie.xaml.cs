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
            Chart.ClearAndPlot();
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
        private readonly string[] _months =
        {
            "Jan 65", "Feb 65", "Mar 65", "Apr 65", "May 65", "Jun 65", "Jul 65", "Ago 65", "Sep 65", "Oct 65", "Nov 65", "Dec 65",
            "Jan 66", "Feb 66", "Mar 66", "Apr 66", "May 66", "Jun 66", "Jul 66", "Ago 66", "Sep 66", "Oct 66", "Nov 66", "Dec 66",
            "Jan 67", "Feb 67", "Mar 67", "Apr 67", "May 67", "Jun 67", "Jul 67", "Ago 67", "Sep 67", "Oct 67", "Nov 67", "Dec 67"
        };

        public SalesViewModel()
        {
            AvailableMonths = _months;
            Salesmen = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<SalesData>
                    {
                        new SalesData {ItemsSold = 15, Rentability = .15, ItemsAverageSellPrice = 5000},
                        new SalesData {ItemsSold = 16, Rentability = .12, ItemsAverageSellPrice = 5200},
                        new SalesData {ItemsSold = 22, Rentability = .11, ItemsAverageSellPrice = 5100},
                        new SalesData {ItemsSold = 25, Rentability = .13, ItemsAverageSellPrice = 5400},
                        new SalesData {ItemsSold = 20, Rentability = .12, ItemsAverageSellPrice = 5100},
                        new SalesData {ItemsSold = 10, Rentability = .11, ItemsAverageSellPrice = 5200},
                        new SalesData {ItemsSold = 12, Rentability = .13, ItemsAverageSellPrice = 5400}
                    }
                }.Setup(new SeriesConfiguration<SalesData>().Y(data => data.ItemsSold))
            };
        }

        public SeriesCollection Salesmen { get; set; }
        public string[] AvailableMonths { get; set; }

		public void AddOneMonth()
        {
            var r = new Random();
            if (Salesmen[0].Values.Count >= _months.Length) return;
            foreach (var salesman in Salesmen)
            {
                salesman.Values.Add(new SalesData
                {
                    ItemsSold = r.Next(5, 30),
                    Rentability = .15,
                    ItemsAverageSellPrice = 5000
                });
            }
        }

        public void RemoveLastMonth()
        {
            if (Salesmen[0].Values.Count == 2) return;
            foreach (var salesman in Salesmen)
            {
                salesman.Values.RemoveAt(salesman.Values.Count - 1);
            }
        }
    }
}
