using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using lvc;

namespace ChartsTest.Scatter_Examples
{
    /// <summary>
    /// Interaction logic for MvvmExample.xaml
    /// </summary>
    public partial class MvvmScatter
    {
        public MvvmScatter()
        {
            InitializeComponent();
            Math = new MathViewModel();
            Chart.DataContext = Math.Functions;
            Chart.AxisX.MinValue = 0;
            Chart.AxisY.MinValue = 0;
        }

        public MathViewModel Math { get; set; }

        private void AddFunctionOnClick(object sender, RoutedEventArgs e)
        {
            Math.AddRandomFunction();
        }

        private void RemoveFunctionOnClick(object sender, RoutedEventArgs e)
        {
            Math.RemoveLastFunction();
        }

        private void AddPointOnClick(object sender, RoutedEventArgs e)
        {
            Math.AddPoint();
        }
        private void RemovePointOnClick(object sender, RoutedEventArgs e)
        {
            Math.RemoveLastMonth();
        }

        private void MvvmExample_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

    public class MathViewModel
    {
        private readonly Func<double, int, double> baseFunc = (val, offset) => Math.Pow(val, 2) + offset*20;

        private readonly List<double> _secondaryValues = new List<double>
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15
        };

        public MathViewModel()
        {
            Functions = new ObservableCollection<Series>
            {
                new ScatterSeries
                {
                    Values = _secondaryValues.Select(x => baseFunc(x, 0)).AsChartValues(),
                    SecondaryValues = _secondaryValues,
                    PointRadius = 0,
                    StrokeThickness = 4
                }
            };
        }

        public ObservableCollection<Series> Functions { get; set; }

        public void AddRandomFunction()
        {
            Functions.Add(new ScatterSeries
            {
                Values = _secondaryValues.Select(x => baseFunc(x, Functions.Count)).AsChartValues(),
                SecondaryValues = _secondaryValues,
                PointRadius = 0,
                StrokeThickness = 4
            });
        }

        public void RemoveLastFunction()
        {
            if (Functions.Count == 1) return;
            Functions.RemoveAt(Functions.Count-1);
        }

        public void AddPoint()
        {
            var nextVal = _secondaryValues[_secondaryValues.Count - 1] + 1;
            _secondaryValues.Add(nextVal);

            foreach (var func in Functions.Cast<ScatterSeries>())
            {
                var evaluation = baseFunc(nextVal, Functions.IndexOf(func));
                func.Values.Add(evaluation);
                func.SecondaryValues = _secondaryValues;
            }
        }

        public void RemoveLastMonth()
        {
            if (Functions[0].Values.Count == 2) return;
            foreach (var func in Functions)
            {
                func.Values.RemoveAt(func.Values.Count - 1);
            }
        }
    }
}
