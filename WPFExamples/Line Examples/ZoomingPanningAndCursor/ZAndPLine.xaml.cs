using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples.ZoomingAndPanning
{
    /// <summary>
    /// Interaction logic for ZAndPLine.xaml
    /// </summary>
    public partial class ZAndPLine
    {
        public ZAndPLine()
        {
            InitializeComponent();

            var mercedes = new LineSeries
            {
                Title = "Mercedes",
                Values = new ChartValues<MotorTemperatureMeasurement>()
            };
            var ferrari = new LineSeries
            {
                Title = "Ferrari",
                Values = new ChartValues<MotorTemperatureMeasurement>()
            };

            var r = new Random();

            var mercedesValues = new List<MotorTemperatureMeasurement>(1000);
            var ferrariValues = new List<MotorTemperatureMeasurement>(1000);

            for (int i = 0; i < 50; i++)
            {
                mercedesValues.Add(new MotorTemperatureMeasurement
                {
                    DateTime = DateTime.Now,
                    MeasuredBy = "Computer",
                    Temperature = r.Next(40, 200)
                });
                ferrariValues.Add(new MotorTemperatureMeasurement
                {
                    DateTime = DateTime.Now,
                    MeasuredBy = "Computer",
                    Temperature = r.Next(40, 200)
                });
            }

            mercedes.Values = mercedesValues.AsChartValues();
            ferrari.Values = ferrariValues.AsChartValues();

            Motors = new SeriesCollection {mercedes, ferrari}
                .Setup(new SeriesConfiguration<MotorTemperatureMeasurement>()
                    .Y(m => m.Temperature));

            TempFormat = temp => temp + " C°";
            XFormatter = index => Math.Round(index).ToString("N");

            DataContext = this;
        }

        public SeriesCollection Motors { get; set; }
        public Func<double, string> TempFormat { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private void Chart_OnDataClick(ChartPoint point)
        {
            Chart.CursorX.Value = point.X;
            Chart.CursorY.Value = point.Y;
        }
    }
}
