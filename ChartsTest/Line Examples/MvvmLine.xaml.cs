using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LiveCharts;

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
            Chart.DataContext = Sales.Salesmen;
            SecondaryAxis.DataContext = Sales.AvailableMonths;
            Chart.PrimaryAxis.LabelFormatter = x => x + ".00k items";
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
            Salesmen = new ObservableCollection<Series>
            {
                new LineSeries
                {
                    Title = "John",
                    PrimaryValues = new ObservableCollection<double>(new[] {2d, 4, 7, 1, 5})
                }
            };
        }

        public ObservableCollection<Series> Salesmen { get; set; }
        public string[] AvailableMonths { get; set; }

        public void AddRandomSalesman()
        {
            var r = new Random();

            var values = new List<double>();

            for (var i = 0; i < Salesmen[0].PrimaryValues.Count; i++) values.Add(r.Next(0, 10));

            Salesmen.Add(new LineSeries
            {
                Title = _names[r.Next(0, _names.Count() - 1)],
                PrimaryValues = new ObservableCollection<double>(values)
            });
        }

        public void RemoveLastSalesman()
        {
            if (Salesmen.Count == 1) return;
            Salesmen.RemoveAt(Salesmen.Count-1);
        }

        public void AddOneMonth()
        {
            var r = new Random();
            if (Salesmen[0].PrimaryValues.Count >= _months.Count()) return; 
            foreach (var salesman in Salesmen)
            {
                salesman.PrimaryValues.Add(r.Next(0, 10));
            }
        }

        public void RemoveLastMonth()
        {
            if (Salesmen[0].PrimaryValues.Count == 2) return;
            foreach (var salesman in Salesmen)
            {
                salesman.PrimaryValues.RemoveAt(salesman.PrimaryValues.Count - 1);
            }
        }
    }
}
