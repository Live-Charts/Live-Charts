using System;
using System.Linq;
using System.Windows;
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
                        new ViewModel {YValue = 0}
                    }
                }
            }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue));

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var val in Series[0].Values.Cast<ViewModel>())
            {
                val.YValue = r.Next(0, 2);
            }
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
                if (ValueChanged!= null) ValueChanged.Invoke();
            }
        }

        public event Action ValueChanged;
    }
}
