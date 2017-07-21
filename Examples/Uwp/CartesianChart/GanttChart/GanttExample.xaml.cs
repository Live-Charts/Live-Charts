using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.GanttChart
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GanttExample : Page, INotifyPropertyChanged
    {
        private double _from;
        private double _to;
        private readonly ChartValues<GanttPoint> _values;

        public GanttExample()
        {
            this.InitializeComponent();

            var now = DateTime.Now;

            _values = new ChartValues<GanttPoint>
            {
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks)
            };

            Series = new SeriesCollection
            {
                new RowSeries
                {
                    Values = _values,
                    DataLabels = true
                }
            };
            Formatter = value => new DateTime((long)value).ToString("dd MMM");

            var labels = new List<string>();
            for (var i = 0; i < 12; i++)
                labels.Add("Task " + i);
            Labels = labels.ToArray();

            ResetZoomOnClick(null, null);

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
        public Func<double, string> Formatter { get; set; }
        public string[] Labels { get; set; }

        public double From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            From = _values.First().StartPoint;
            To = _values.Last().EndPoint;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
