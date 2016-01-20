using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using lvc;

namespace ChartsTest.Line_Examples
{
    /// <summary>
    /// Interaction logic for MvvmExample.xaml
    /// </summary>
    public partial class MvvmLine
    {
        public MvvmLine()
        {
            InitializeComponent();
            Sales = new SalesViewModel();
            DataContext = this;
            Chart.AxisY.LabelFormatter = x => x + ".00k items";
        }

        public SalesViewModel Sales { get; set; }

        private void AddSalesmanOnClick(object sender, RoutedEventArgs e)
        {
            Sales.AddRandomSalesman();
        }

        private void RemoveSalesmanOnClick(object sender, RoutedEventArgs e)
        {
            Sales.RemoveLastSalesman();
        }

        private void AddMonthOnClick(object sender, RoutedEventArgs e)
        {
            Sales.AddOneMonth();
        }
        private void RemoveMonthOnClick(object sender, RoutedEventArgs e)
        {
            Sales.RemoveLastMonth();
        }

        private void MvvmExample_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class SalesData
    {
        public int ItemsSold { get; set; }
        public decimal ItemsAverageSellPrice { get; set; }
        public double ItemsAverageCost { get; set; }
        public bool IsAboveAverage { get; set; }
    }

    public class SalesViewModel
    {
        private readonly string[] _names =
        {
            "Charles", "Susan", "Edit", "Roger", "Peter", "James", "Ana", "Alice", "Maria",
            "Jesus", "Jose", "Miriam", "Aristoteles", "Socrates", "Isaac", "Thomas", "Nicholas"
        };

        private readonly string[] _months =
        {
            "Jan 65", "Feb 65", "Mar 65", "Apr 65", "May 65", "Jun 65", "Jul 65", "Ago 65", "Sep 65", "Oct 65", "Nov 65", "Dec 65",
            "Jan 66", "Feb 66", "Mar 66", "Apr 66", "May 66", "Jun 66", "Jul 66", "Ago 66", "Sep 66", "Oct 66", "Nov 66", "Dec 66",
            "Jan 67", "Feb 67", "Mar 67", "Apr 67", "May 67", "Jun 67", "Jul 67", "Ago 67", "Sep 67", "Oct 67", "Nov 67", "Dec 67"
        };

        public SalesViewModel()
        {
            AvailableMonths = _months;
            SalesmenSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<SalesData>
                    {
                        new SalesData {ItemsSold = 15, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 16, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 22, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 25, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 20, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 10, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true},
                        new SalesData {ItemsSold = 12, ItemsAverageCost = .15, ItemsAverageSellPrice = 5000, IsAboveAverage = true}
                    }
                }
            }.Setup(new SeriesConfiguration<SalesData>().Y(x => x.ItemsSold));
        }

        public SeriesCollection SalesmenSeries { get; set; }
        public string[] AvailableMonths { get; set; }

        public void AddRandomSalesman()
        {
            var r = new Random();

            var values = new ChartValues<SalesData>();
            for (var i = 0; i < SalesmenSeries[0].Values.Count; i++) values.Add(new SalesData
            {
                ItemsSold = r.Next(5,20),
                ItemsAverageCost = .15,
                ItemsAverageSellPrice = 5000,
                IsAboveAverage = true
            });

            SalesmenSeries.Add(new LineSeries
            {
                Title = _names[r.Next(0, _names.Count() - 1)],
                Values = values
            });
        }

        public void RemoveLastSalesman()
        {
            if (SalesmenSeries.Count == 1) return;
            SalesmenSeries.RemoveAt(SalesmenSeries.Count-1);
        }

        public void AddOneMonth()
        {
            var r = new Random();
            if (SalesmenSeries[0].Values.Count >= _months.Count()) return; 
            foreach (var salesman in SalesmenSeries)
            {
                salesman.Values.Add(new SalesData
                {
                    ItemsSold = r.Next(5,20),
                    ItemsAverageCost = .15,
                    ItemsAverageSellPrice = 5000,
                    IsAboveAverage = true
                });
            }
        }

        public void RemoveLastMonth()
        {
            if (SalesmenSeries[0].Values.Count == 2) return;
            foreach (var salesman in SalesmenSeries)
            {
                salesman.Values.RemoveAt(salesman.Values.Count - 1);
            }
        }
    }
   
}
