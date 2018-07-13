﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class ZoomingAndPanning
    {
        private double _trend;

        public ZoomingAndPanning()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double>(GetRecords(30))
                }
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }

        private IEnumerable<double> GetRecords(int count)
        {
            var r = new Random();

            for (int i = 0; i < count; i++)
            {
                yield return _trend += r.Next(-5, 10);
            }
        }
    }
}