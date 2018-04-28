using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;

namespace Assets.ViewModels
{
    public class ScrollBar : INotifyPropertyChanged
    {
        private double _scroll;

        public ScrollBar()
        {
            var random = new Random();
            var values = new ObservableCollection<double>();
            var trend = 0;

            for (var i = 0; i < 50; i++)
            {
                values.Add(trend += random.Next(-5, 10));
            }

            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = values
                }
            };

            ScrollableAxis = new Axis
            {
                MinValue = 0,
                MaxValue = 10
            };

            XAxisCollection = new ObservableCollection<Plane>
            {
                ScrollableAxis
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }
        public ObservableCollection<Plane> XAxisCollection { get; set; }
        public Axis ScrollableAxis { get; set; }

        public double Scroll
        {
            get => _scroll;
            set
            {
                _scroll = value;
                ScrollableAxis.MinValue = _scroll;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}