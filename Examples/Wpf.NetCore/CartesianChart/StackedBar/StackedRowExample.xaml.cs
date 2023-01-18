﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for StackedRowExample.xaml
    /// </summary>
    public partial class StackedRowExample : UserControl
    {
        public StackedRowExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(2),
                        new ObservableValue(9),
                        new ObservableValue(3)
                    },
                    DataLabels = true
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(7),
                        new ObservableValue(4),
                        new ObservableValue(1),
                        new ObservableValue(7),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(0),
                        new ObservableValue(3)
                    },
                    DataLabels = true
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(6),
                        new ObservableValue(2),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(9),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(3)
                    },
                    DataLabels = true
                }
            };

            Labels = new[]
            {
                "Jan", "Feb","Mar", "Apr", "May", "Jun", "Jul", "Ago"
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }


        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                foreach (var observableValue in series.Values.Cast<ObservableValue>())
                {
                    observableValue.Value = r.Next(-10, 10);
                }
            }
        }
    }
}
