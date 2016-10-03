using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy
    {
        private int i = 0;

        public JimmyTheTestsGuy()
        {
            InitializeComponent();
        }

        
    }

    public class TestVm
    {
        public TestVm()
        {
            var r = new Random();

            var values = new ChartValues<DateTimePoint>
            {
                new DateTimePoint(DateTime.Now.AddDays(1), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(2), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(3), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(4), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(5), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(6), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(7), r.NextDouble()),
                new DateTimePoint(DateTime.Now.AddDays(8), r.NextDouble())
            };

            SeriesCollection = new SeriesCollection
            {
                new StepLineSeries
                {
                    Values = values
                }
            };

            From = DateTime.Now.AddDays(2).Ticks;
            To = DateTime.Now.AddDays(6).Ticks;
            Min = values.First().DateTime.Ticks;
            Max = values.Last().DateTime.Ticks;
            Formatter = val => new DateTime((long)val).ToString("HH:mm:ss.f");
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}


