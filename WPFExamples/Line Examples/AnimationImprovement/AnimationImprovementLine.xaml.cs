using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LiveCharts;

namespace ChartsTest.Line_Examples.AnimationImprovement
{
    /// <summary>
    /// Interaction logic for AnimationImprovementLine.xaml
    /// </summary>
    public partial class AnimationImprovementLine 
    {
        public AnimationImprovementLine()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "My Series",
                    Values = new ChartValues<ViewModel>
                    {
                        new ViewModel {YValue = 0},
                        new ViewModel {YValue = 1},
                        new ViewModel {YValue = 0},
                        new ViewModel {YValue = 1},
                        new ViewModel {YValue = 0}
                    },
                    StrokeDashArray = new DoubleCollection { 2 },
                    DataLabels = true
                }
            }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue));

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in Series)
            {
                foreach (var val in series.Values.Cast<ViewModel>())
                {
                    val.YValue = r.Next(0, 11);
                }
            }
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var vals = new ChartValues<ViewModel>();
            var r = new Random();

            for (int i = 0; i < Series[0].Values.Count; i++)
            {
                vals.Add(new ViewModel {YValue = r.Next(0, 11)});
            }

            Series.Add(new LineSeries
            {
                Values = vals,
                //DataLabels = true
            });
        }

        private void AddPointOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in Series)
            {
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
                series.Values.Add(new ViewModel {YValue = r.Next(0, 11)});
            }
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (Series.Count == 1) return;
            Series.RemoveAt(0);
        }

        private void InsertOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in Series)
            {
                if (series.Values.Count > 3)
                    series.Values.Insert(2, new ViewModel
                    {
                        YValue = r.Next(0, 11)
                    });
            }
        }

        private void RemoveOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in Series)
            {
                if(series.Values.Count == 1) continue;
                series.Values.RemoveAt(0);
            }
        }

        private void RemoveMiddleOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in Series)
            {
                if (series.Values.Count > 3) series.Values.RemoveAt(2);
            }
        }

        private void AnimationImprovementLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //This is only to display animation everytime you change the view
            Chart.Update();
        }
    }

    public class ViewModel : IObservableChartPoint
    {
        private double _yValue;

        public double YValue
        {
            get { return _yValue; }
            set
            {
                _yValue = value;
                if (PointChanged != null) PointChanged.Invoke(this);
            }
        }

        public event Action<object> PointChanged;
    }
}
