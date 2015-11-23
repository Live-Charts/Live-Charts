using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ChartsTest.MoreExamples.LineAndAreaChart;
using LiveCharts;
using LiveCharts.Charts;
using LiveCharts.Series;

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

	    public IEnumerable<double> LineValues { get; } = new List<double> {3, 2, 1, 6, 5, 4, 9, 8, 7};
	    public IEnumerable<string> PieLabels { get; } = new List<string> {"Alex", "Betty", "Charlie", "Daniel", "Erin", "Frank", "Geoffrey", "Hector", "Isabella"};

	    public MainWindow()
        {
            InitializeComponent();

			DataContext = this;

			//set this property to true so charts use diferent colors (and don't look boring!).
			Chart.RandomizeStartingColor = true;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += (sender, args) =>
            {
                var r = new Random();
                if (r.NextDouble() < .1) _aliveScalator += r.Next(-100, 100);
                foreach (var serie in LineChart.Series)
                {
                    serie.PrimaryValues.RemoveAt(0);
                    serie.PrimaryValues.Add(r.Next(_aliveScalator - 30, _aliveScalator + 30));
                }
                var abs = Math.Abs(_aliveScalator);
                foreach (var serie in BarChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(abs, abs + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }
                foreach (var serie in StackedBarChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(abs, abs + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }

                //var f = PieChart.Series.First().PrimaryValues;
                //var f1 = PieChart1.Series.First().PrimaryValues;
                //f.Add(r.Next(abs, abs + 100));
                //f1.Add(r.Next(abs, abs + 100));
                //f.RemoveAt(0);
                //f1.RemoveAt(0);

                foreach (var serie in ScatterChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(_aliveScalator - 30, _aliveScalator + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }

                foreach (var serie in RadarChart.Series)
                {
                    serie.PrimaryValues.Add(r.Next(_aliveScalator - 30, _aliveScalator + 30));
                    serie.PrimaryValues.RemoveAt(0);
                }
            };

            var l = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                l.Add("Day number " + (i + 1));
            }
            var standardLabels = l.ToArray();


            //Line Chart
            LineChart.PrimaryAxis.LabelFormatter = (value) => value.ToString("C");
            LineChart.SecondaryAxis.Labels = standardLabels;
		    LineChart.Series.Add(new LineSerie
			                         {
				                         Label = "Vegetables",
				                         PrimaryValues = new ObservableCollection<double> {-10, 5, 9, 28, -3, 2, 0, 5, 10, 1, 7, 2}
			                         });
		    LineChart.Series.Add(new LineSerie
			                         {
				                         Label = "Fruits",
				                         PrimaryValues = new ObservableCollection<double> {-6, 1, 6, 20, -3, -7, -9, 2, 16, 10, 16, 12}
			                         });

            ////Bar Chart
            BarChart.SecondaryAxis.Labels = standardLabels;
		    BarChart.Series.Add(new BarSerie
			                        {
				                        Label = "John",
				                        PrimaryValues = new ObservableCollection<double> {4, 3, 1, 2}
			                        });
		    BarChart.Series.Add(new BarSerie
			    {
				    Label = "Judit",
				    PrimaryValues = new ObservableCollection<double> {3, 1, 4, 3}
			    });

            //Stacked Bar Chart
            StackedBarChart.SecondaryAxis.Labels = standardLabels;
            StackedBarChart.PrimaryAxis.MinValue = 0;
		    StackedBarChart.Series.Add(new StackedBarSerie
			    {
				    Label = "Charles",
				    PrimaryValues = new ObservableCollection<double> {1, 2, 3, 4, 1, 5, 2}
			    });
			StackedBarChart.Series.Add(new StackedBarSerie
			{
				Label = "Susan",
                    PrimaryValues = new ObservableCollection<double> { 4,3,2,1,8,1,4 }
			});
			StackedBarChart.Series.Add(new StackedBarSerie
			{
				Label = "Eli",
                    PrimaryValues = new ObservableCollection<double> { 3,1,4,2,7,3,2 }
			});

			//PieChart
			//if you add more than one serie to pie chart, they will be overridden
		    PieChart.Series.Add(new PieSerie
			    {
				    PrimaryValues = new ObservableCollection<double> {8, 1, 5},
				    Labels = standardLabels
			    });
            PieChart1.PrimaryAxis.LabelFormatter = value => (value/PieChart1.PieTotalSum).ToString("P");
            //if you add more than one serie to pie chart, they will be overridden
            //do not mix positive and negative values in a pie chart. It has no sense
		    PieChart1.Series.Add(new PieSerie
			                         {
				                         PrimaryValues = new ObservableCollection<double> {8, 8, 3},
				                         Labels = standardLabels
			                         });

            RadarChart.SecondaryAxis.Labels = standardLabels;
		    RadarChart.Series.Add(new RadarSerie
			    {
				    PrimaryValues = new ObservableCollection<double> {9, 8, 2, 1, 6, 3, 7}
			    });
			RadarChart.Series.Add(new RadarSerie
			{
				PrimaryValues = new ObservableCollection<double> {4, 2, 7, 5, 10, 5, 2}
                });
		    RadarChart.Series.Add(new RadarSerie
			    {
				    PrimaryValues = new ObservableCollection<double> {2, 2, 4, 3, 2, 9, 5}
			    });

            ////func for serie 1
            Func<double, double> fx1 = x => (Math.Pow(x, 2) + 10 * x) * 1000;
            Func<double, double> fx2 = x => (Math.Pow(x, 2)) * 1000;
            ScatterChart.PrimaryAxis.LabelFormatter = LabelFormatters.Currency;
            ScatterChart.Series.Add(new ScatterSerie
                {
                    PrimaryValues = new ObservableCollection<double> {fx1(-10), fx1(-3), fx1(5), fx1(7)},
                    SecondaryValues =  new double[] {-10, -3, 5, 7},
                    PointRadius = 7
                });
		    ScatterChart.Series.Add(new ScatterSerie
			    {
				    PrimaryValues = new ObservableCollection<double> {fx2(-5), fx2(-2), fx2(3), fx2(10)},
				    SecondaryValues = new double[] {-5, -2, 3, 10},
				    PointRadius = 7
			    });
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
            foreach (var s in StackedBarChart.Series)
            {
                s.PrimaryValues.Add(r.Next(0, 10));
            }
            PieChart.Series.First().PrimaryValues.Add(r.Next(0, 10));
            PieChart1.Series.First().PrimaryValues.Add(r.Next(0, 10));
            foreach (var s in RadarChart.Series)
            {
                s.PrimaryValues.Add(r.Next(-10, 10));
            }
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
            foreach (var s in StackedBarChart.Series.Where(s => s.PrimaryValues.Count > 2))
            {
                s.PrimaryValues.RemoveAt(s.PrimaryValues.Count - 1);
            }
            var f = PieChart.Series.First().PrimaryValues;
            var f1 = PieChart1.Series.First().PrimaryValues;

            if (f.Count > 1) f.RemoveAt(f.Count - 1);
            if (f1.Count > 1) f1.RemoveAt(f1.Count - 1);

            foreach (var s in RadarChart.Series.Where(s => s.PrimaryValues.Count > 2))
            {
                s.PrimaryValues.RemoveAt(s.PrimaryValues.Count - 1);
            }
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
                    PrimaryValues = new ObservableCollection<double>(new double[] { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5 })
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
                    PrimaryValues = new ObservableCollection<double>(new double[] { 1, 2, 3, 4, 5 })
                };
            }
            BarChart.Series.Add(s1);

            var s2 = new StackedBarSerie { PrimaryValues = new ObservableCollection<double>() };
            var stackedSerie = StackedBarChart.Series.Count > 0 ? StackedBarChart.Series[0] as StackedBarSerie : null;
            if (stackedSerie != null)
            {
                var l1 = StackedBarChart.Series.Count == 0 ? 0 : stackedSerie.PrimaryValues.Count;
                for (int i = 0; i < l1; i++)
                {
                    s2.PrimaryValues.Add(r.Next(0, 30));
                }
            }
            else
            {
                s2 = new StackedBarSerie
                {
                    PrimaryValues = new ObservableCollection<double>(new double[] { 1, 2, 3, 4, 5 })
                };
            }
            StackedBarChart.Series.Add(s2);

            var s8 = new RadarSerie { PrimaryValues = new ObservableCollection<double>() };
            var radarSerie = RadarChart.Series.Count > 0 ? RadarChart.Series[0] as RadarSerie : null;
            if (radarSerie != null)
            {
                var l1 = RadarChart.Series.Count == 0 ? 0 : radarSerie.PrimaryValues.Count;
                for (int i = 0; i < l1; i++)
                {
                    s8.PrimaryValues.Add(r.Next(0, 30));
                }
            }
            else
            {
                s8 = new RadarSerie
                {
                    PrimaryValues = new ObservableCollection<double>(new double[] { 1, 2, 3, 4, 5 })
                };
            }
            RadarChart.Series.Add(s8);
        }

        private void RemoveSerie(object sender, RoutedEventArgs e)
        {
            if (LineChart.Series.Count != 0)
                LineChart.Series.RemoveAt(LineChart.Series.Count - 1);

            if (BarChart.Series.Count != 0)
                BarChart.Series.RemoveAt(BarChart.Series.Count - 1);

            if (StackedBarChart.Series.Count != 0)
                StackedBarChart.Series.RemoveAt(StackedBarChart.Series.Count - 1);

            if (RadarChart.Series.Count != 0)
                RadarChart.Series.RemoveAt(RadarChart.Series.Count - 1);
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

                StackedBarChart.Hoverable = true;
                StackedBarChart.ClearAndPlot(false);

                PieChart.Hoverable = true;
                PieChart.ClearAndPlot();
                PieChart1.Hoverable = true;
                PieChart1.ClearAndPlot();

                ScatterChart.Hoverable = true;
                ScatterChart.ClearAndPlot();

                RadarChart.Hoverable = true;
                RadarChart.ClearAndPlot();
            }
            else
            {
                _timer.Start();
                _isAlive = true;

                LineChart.Hoverable = false;
                BarChart.Hoverable = false;
                StackedBarChart.Hoverable = false;
                PieChart.Hoverable = false;
                PieChart1.Hoverable = false;
                ScatterChart.Hoverable = false;
                ScatterChart.PrimaryAxis.MinValue = null;
                RadarChart.Hoverable = false;
            }
        }

        private void Redraw(object sender, RoutedEventArgs e)
        {
            LineChart.ClearAndPlot();
            BarChart.ClearAndPlot();
            PieChart.ClearAndPlot();
            PieChart1.ClearAndPlot();
            StackedBarChart.ClearAndPlot();
            ScatterChart.ClearAndPlot();
            RadarChart.ClearAndPlot();
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
                serie.PrimaryValues.Add(i * r.NextDouble());
                serie.SecondaryValues[i] = i * r.NextDouble();
            }

            var timer = DateTime.Now;
	        PerformanceChart.Series.Add(serie);
            MessageBox.Show("drew 1000 pts in " + (DateTime.Now - timer).TotalMilliseconds + " ms");
        }

        private void IncreaseScale(object sender, RoutedEventArgs e)
        {
            foreach (var serie in ScatterChart.Series.Cast<ScatterSerie>())
            {
                for (var index = 0; index < serie.PrimaryValues.Count; index++)
                {
                    serie.PrimaryValues[index] = serie.PrimaryValues[index] * 10;
                }
            }
        }

        private void DecreaseScale(object sender, RoutedEventArgs e)
        {
            foreach (var serie in ScatterChart.Series.Cast<ScatterSerie>())
            {
                for (var index = 0; index < serie.PrimaryValues.Count; index++)
                {
                    serie.PrimaryValues[index] = serie.PrimaryValues[index] / 10;
                }
            }
        }

        private void MoreLineChartExmaplesOnClick(object sender, RoutedEventArgs e)
        {
            new LineAndAreaExamples().ShowDialog();
        }
    }
}
