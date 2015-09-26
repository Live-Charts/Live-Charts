using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Charts;
using Charts.Charts;
using Charts.Series;

namespace ChartsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer;
        private bool _isAlive;
        private int _aliveScalator;

        public MainWindow()
        {
            InitializeComponent();

            //set this property to true so charts use diferent colors (and don't look boring!).
            Chart.RandomizeStartingColor = true;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += (sender, args) =>
            {
                var r = new Random();
                if (r.NextDouble() < .1) _aliveScalator += r.Next(-100, 100);
                foreach (var serie in LineChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(_aliveScalator - 30, _aliveScalator + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }
                var abs = Math.Abs(_aliveScalator);
                foreach (var serie in BarChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(abs, abs + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }

                var f = PieChart.Series.First().PrimaryValues;
                var f1 = PieChart1.Series.First().PrimaryValues;
                f.Add(r.Next(abs , abs + 100));
                f1.Add(r.Next(abs, abs + 100));
                f.RemoveAt(0);
                f1.RemoveAt(0);
            };

            var standardLabels = new[]
            {
                "Day 1","Day 2","Day 3","Day 4","Day 5","Day 6","Day 7","Day 8","Day 9","Day 10","Day 11","Day 12","Day 13","Day 14"
            };


            //Line Chart
            LineChart.PrimaryAxis.LabelFormatter = x => x.ToString("C");
            LineChart.SecondaryAxis.Labels = standardLabels;
            LineChart.Series = new ObservableCollection<Serie>
            {
                new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double>
                    {
                        -10, 5, 9, 28, -3, 2, 0, 5, 10, 1, 7, 2
                    }
                }
            };


            ////Bar Chart
            BarChart.SecondaryAxis.Labels = standardLabels;
            BarChart.Series = new ObservableCollection<Serie>
            {
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 1,2,3,4 }
                },
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 4,3,2,1 }
                },
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 3,1,4,2 }
                }
            };

            //PieChart
            PieChart.Series = new ObservableCollection<Serie>
            {
                //if you add more than one serie to pie chart, they will be overridden
                new PieSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 8, 1, 5 },
                    Labels = standardLabels
                }
            };
            PieChart1.Series = new ObservableCollection<Serie>
            {
                //if you add more than one serie to pie chart, they will be overridden
                //do not mix positive and negative values in a pie chart. It has no sense
                new PieSerie
                {
                    PrimaryValues = new ObservableCollection<double> {8,8,3},
                    Labels = standardLabels
                }
            };

            //func for serie 1
            Func<double, double> fx1 = x => (Math.Pow(x, 2) + 10 * x)*1000;
            Func<double, double> fx2 = x => (Math.Pow(x, 2))*1000;
            ScatterChart.PrimaryAxis.LabelFormatter = LabelFormatters.Currency;
            ScatterChart.Series = new ObservableCollection<Serie>
            {
                new ScatterSerie
                {
                    PrimaryValues = new ObservableCollection<double> {fx1(-10), fx1(-3), fx1(5), fx1(7)},
                    SecondaryValues =  new double[] {-10, -3, 5, 7},
                    PointRadius = 7
                },
                new ScatterSerie
                {
                    PrimaryValues = new ObservableCollection<double> {fx2(-5), fx2(-2), fx2(3), fx2(10)},
                    SecondaryValues =  new double[]{-5, -2, 3, 10},
                    PointRadius = 7
                },
            };
        }

        private void AddPoint(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var s in LineChart.Series)
            {
                s.PrimaryValues.Add(r.Next(-10, 10));
            }
            foreach (var s in BarChart.Series)
            {
                s.PrimaryValues.Add(r.Next(0, 10));
            }
            PieChart.Series.First().PrimaryValues.Add(r.Next(0,10));
            PieChart1.Series.First().PrimaryValues.Add(r.Next(0, 10));
        }

        private void RemovePoint(object sender, RoutedEventArgs e)
        {
            foreach (var s in LineChart.Series.Where(s => s.PrimaryValues.Count > 2))
            {
                s.PrimaryValues.RemoveAt(s.PrimaryValues.Count - 1);
            }
            foreach (var s in BarChart.Series.Where(s => s.PrimaryValues.Count > 2))
            {
                s.PrimaryValues.RemoveAt(s.PrimaryValues.Count - 1);
            }
            var f = PieChart.Series.First().PrimaryValues;
            var f1 = PieChart1.Series.First().PrimaryValues;
            
            if (f.Count > 1)f.RemoveAt(f.Count-1);
            if(f1.Count>1) f1.RemoveAt(f1.Count-1);
        }

        private void AddSerie(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            var s = new LineSerie { PrimaryValues = new ObservableCollection<double>() };
            var lineSerie = LineChart.Series.Count > 0 ? (LineChart.Series[0] as LineSerie) : null;
            if (lineSerie != null)
            {
                var l = LineChart.Series.Count == 0 ? 0 : lineSerie.PrimaryValues.Count;
                for (int i = 0; i < l; i++)
                {
                    s.PrimaryValues.Add(r.Next(-30, 30));
                }
            }
            else
            {
                s = new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double>(new double[] {1, -1, 2, -2, 3, -3, 4, -4, 5, -5})
                };
            }

            LineChart.Series.Add(s);

            var s1 = new BarSerie { PrimaryValues = new ObservableCollection<double>() };
            var barSerie = BarChart.Series.Count > 0 ? BarChart.Series[0] as BarSerie : null;
            if (barSerie != null)
            {
                var l1 = BarChart.Series.Count == 0 ? 0 : barSerie.PrimaryValues.Count;
                for (int i = 0; i < l1; i++)
                {
                    s1.PrimaryValues.Add(r.Next(0, 30));
                }
            }
            else
            {
                s1 = new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double>(new double[] { 1,2,3,4,5 })
                };
            }
            BarChart.Series.Add(s1);
        }

        private void RemoveSerie(object sender, RoutedEventArgs e)
        {
            if (LineChart.Series.Count != 0)
            LineChart.Series.RemoveAt(LineChart.Series.Count - 1);

            if (BarChart.Series.Count != 0)
            BarChart.Series.RemoveAt(BarChart.Series.Count - 1);
        }

        private void IsAlive(object sender, RoutedEventArgs e)
        {
            if (_isAlive)
            {
                _timer.Stop();
                _isAlive = false;

                LineChart.Hoverable = true;
                LineChart.ClearAndPlot(false);

                BarChart.Hoverable = true;
                BarChart.ClearAndPlot(false);

                PieChart.Hoverable = true;
                PieChart.ClearAndPlot();
                PieChart1.Hoverable = true;
                PieChart1.ClearAndPlot();
            }
            else
            {
                _timer.Start();
                _isAlive = true;

                LineChart.Hoverable = false;
                BarChart.Hoverable = false;
                PieChart.Hoverable = false;
                PieChart1.Hoverable = false;
            }
        }

        private void Redraw(object sender, RoutedEventArgs e)
        {
            LineChart.ClearAndPlot();
            BarChart.ClearAndPlot();
            PieChart.ClearAndPlot();
            PieChart1.ClearAndPlot();
        }

        private void PerformanceTest(object sender, RoutedEventArgs e)
        {
            var testLenght = 1000;
            var r = new Random();
            var serie = new ScatterSerie
            {
                PrimaryValues = new ObservableCollection<double>(),
                SecondaryValues = new double[testLenght],
                PointRadius = 3,
                StrokeThickness = 0
            };
            
            for (int i = 0; i < testLenght; i++)
            {
                serie.PrimaryValues.Add(i*r.NextDouble());
                serie.SecondaryValues[i] = i*r.NextDouble();
            }

            var timer = DateTime.Now;
            PerfomranceChart.Series = new ObservableCollection<Serie>
            {
                serie
            };
            MessageBox.Show("drew 1000 pts in " + (DateTime.Now - timer).TotalMilliseconds + " ms");
        }
    }
}
