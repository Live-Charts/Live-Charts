using System;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for ConstantChangesChart.xaml
    /// </summary>
    public partial class ConstantChangesChart
    {
        public ConstantChangesChart()
        {
            InitializeComponent();

            Buffer = new ChartValues<ObservableValue>();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries {Values = Buffer, StrokeThickness = 4, PointDiameter = 0}
            };

            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };

            Timer.Tick += TimerOnTick;

            IsDataInjectionRunning = false;

            R = new Random();

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<ObservableValue> Buffer { get; set; }
        public DispatcherTimer Timer { get; set; }
        public bool IsDataInjectionRunning { get; set; }
        public Random R { get; set; }

        private void RunDataOnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInjectionRunning)
            {
                Timer.Stop();
                IsDataInjectionRunning = false;
            }
            else
            {
                Timer.Start();
                IsDataInjectionRunning = true;
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            Buffer.Add(new ObservableValue(R.Next(0, 10)));
            if (Buffer.Count > 50) Buffer.RemoveAt(0);
        }
    }
}
