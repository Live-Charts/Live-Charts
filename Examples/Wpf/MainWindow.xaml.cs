using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(10),
                        new ObservableValue(20),
                        new ObservableValue(30),
                        new ObservableValue(40),
                        new ObservableValue(50)
                    },
                    DataLabels = true
                },
                new RowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(10),
                        new ObservableValue(20),
                        new ObservableValue(30),
                        new ObservableValue(40),
                        new ObservableValue(50)
                    },
                    DataLabels = true
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("Hello!");
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                series.Values.Add(new ObservableValue(r.Next(-20, 20)));
            }
        }

        private void RemoveButtonOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in SeriesCollection)
            {
                if (series.Values.Count > 0)
                    series.Values.RemoveAt(0);
            }
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var c = SeriesCollection.Count > 0 ? SeriesCollection[0].Values.Count : 5;

            var values = new List<ObservableValue>();
            var r = new Random();

            for (var i = 0; i < c; i++)
            {
                values.Add(new ObservableValue(r.Next(-20,20)));
            }

            SeriesCollection.Add(new LineSeries
            {
                Values = values.AsChartValues()
            });
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            if (SeriesCollection.Count > 0)
                SeriesCollection.RemoveAt(r.Next(0, SeriesCollection.Count));
        }

        private void RandomizeOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                foreach (var value in series.Values.Cast<ObservableValue>())
                {
                    //value.X = r.NextDouble()*20;
                    value.Value = r.Next(-20, 20);
                }
            }
        }
    }
}
