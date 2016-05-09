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

            var l = new List<ObservableValue>();
            var r = new Random();

            for (int i = 0; i < 10; i++)
            {
                l.Add(new ObservableValue(r.Next(-20, 20)));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = l.AsChartValues(),
                    PointDiameter = 0
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
