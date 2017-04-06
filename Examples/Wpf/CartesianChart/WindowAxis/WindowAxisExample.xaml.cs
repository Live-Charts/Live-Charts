using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
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
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.WindowAxis
{

    public class OtherWindow : AxisWindow
    {
        public override double MinimumSeparatorWidth
        {
            get { return 10; }
        }

        public override bool IsHeader(double x)
        {
            return x % 100 == 0;
        }

        public override bool IsSeparator(double x)
        {
            return x % 10 == 0;
        }

        public override string FormatAxisLabel(double x)
        {
            return x.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class PresidentWindow : AxisWindow
    {
        private Dictionary<double, string> _presidents = new Dictionary<double, string>();

        public PresidentWindow()
        {
            _presidents.Add(1789, "George Washington");
            _presidents.Add(1797, "John Adams");
            _presidents.Add(1801, "Thomas Jefferson");
            _presidents.Add(1809, "James Madison");
            _presidents.Add(1817, "James Monroe");
            _presidents.Add(1825, "John Quincy Adams");
            _presidents.Add(1829, "Andrew Jackson");
            _presidents.Add(1837, "Martin van Buren");
            _presidents.Add(1841, "John Tyler"); // William Henry Harrison died in the first month
            _presidents.Add(1845, "James K. Polk");
            _presidents.Add(1849, "Zachary Taylor");
            _presidents.Add(1850, "Millard Fillmore");
            _presidents.Add(1853, "Franklin Pierce");
            _presidents.Add(1857, "James Buchanan");
            _presidents.Add(1861, "Abraham Lincoln");
            _presidents.Add(1865, "Andrew Johnson");
            _presidents.Add(1869, "Ulysses S. Grant");
            _presidents.Add(1877, "Rutherford B. hayes");
            _presidents.Add(1881, "Chester A. Arthur"); // James A. Garfield was assassinated in this first year of office
            _presidents.Add(1885, "Grover Cleveland");
            _presidents.Add(1889, "Benjamin Harrison");
            _presidents.Add(1893, "Grover Cleveland");
            _presidents.Add(1897, "William McKinley");
            _presidents.Add(1901, "Theodore Roosevelt");
            _presidents.Add(1909, "William Howard Taft");
            _presidents.Add(1913, "Woodrow Wilson");
            _presidents.Add(1921, "Warren G. Harding");
            _presidents.Add(1923, "Calvin Coolidge");
            _presidents.Add(1929, "Herbert Hoover");
            _presidents.Add(1933, "Franklin D. Roosevelt");
            _presidents.Add(1945, "Harry S. Truman");
            _presidents.Add(1953, "Dwight D. Eisenhower");
            _presidents.Add(1961, "John F. Kennedy");
            _presidents.Add(1963, "Lyndon B. Johnson");
            _presidents.Add(1969, "Richard nixon");
            _presidents.Add(1974, "Gerald Ford");
            _presidents.Add(1977, "Jimmy Carter");
            _presidents.Add(1981, "Ronald Reagan");
            _presidents.Add(1989, "George H. W. Bush");
            _presidents.Add(1993, "Bill Clinton");
            _presidents.Add(2001, "George W. Bush");
            _presidents.Add(2009, "Barack Obama");
        }

        public override double MinimumSeparatorWidth
        {
            get { return 30; }
        }

        public override bool IsHeader(double x)
        {
            return Math.Floor(x) % 10 == 0;
        }

        public override bool IsSeparator(double x)
        {
            // Return if this is a president, or if this is a period of 10 years
            return _presidents.ContainsKey(x) || Math.Floor(x) % 10 == 0;
        }

        public override string FormatAxisLabel(double x)
        {
            string president;
            return _presidents.TryGetValue(x, out president)
                ? string.Format("{0}-{1}", x.ToString(CultureInfo.InvariantCulture), president)
                : x.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Interaction logic for WindowAxisExample.xaml
    /// </summary>
    public partial class WindowAxisExample : UserControl, INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowAxisExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Population",
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(1790, 3513183),
                        new ObservablePoint(1800, 5010172),
                        new ObservablePoint(1810, 6710919),
                        new ObservablePoint(1820, 9090219),
                        new ObservablePoint(1830, 12556760),
                        new ObservablePoint(1840, 16676537),
                        new ObservablePoint(1850, 22652981),
                        new ObservablePoint(1860, 30664152),
                        new ObservablePoint(1870, 38115641),
                        new ObservablePoint(1880, 49371340),
                        new ObservablePoint(1890, 74607225),
                        new ObservablePoint(1900, 74607225),
                        new ObservablePoint(1910, 91109542),
                        new ObservablePoint(1920, 105273049),
                        new ObservablePoint(1930, 122288177),
                        new ObservablePoint(1940, 131006184),
                        new ObservablePoint(1950, 149895183),
                        new ObservablePoint(1960, 178559219),
                        new ObservablePoint(1970, 202545363),
                        new ObservablePoint(1980, 225903767),
                        new ObservablePoint(1990, 248102973),
                        new ObservablePoint(2000, 280849847),
                    },
                    PointGeometry = null
                },
            };

            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
