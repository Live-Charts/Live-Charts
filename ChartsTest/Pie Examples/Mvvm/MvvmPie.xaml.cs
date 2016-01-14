using System;
using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Pie_Examples
{
    /// <summary>
    /// Interaction logic for MvvmBar.xaml
    /// </summary>
    public partial class MvvmPie
    {
        public MvvmPie()
        {
            InitializeComponent();
            DataContext = Sales = new SalesViewModel();
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
            Salesmen = new ObservableCollection<Series>
            {
                new PieSeries
                {
                    Title = "John",
                    PrimaryValues = new ObservableCollection<double>(new[] {2d, 4, 7, 1, 5}),
                    Labels = _months
                }
            };
        }

        public ObservableCollection<Series> Salesmen { get; set; }
        public string[] AvailableMonths { get; set; }

		public void AddOneMonth()
        {
            var r = new Random();
            if (Salesmen[0].PrimaryValues.Count >= _months.Length) return;
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
