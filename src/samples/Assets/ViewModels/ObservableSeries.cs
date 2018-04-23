using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assets.Commands;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class ObservableSeries : INotifyPropertyChanged
    {
        private ObservableCollection<ISeries<double>> _seriesCollection;
        private readonly Random _r = new Random();
        private Type _seriesType;

        public ObservableSeries()
        {
            SeriesType = typeof(LineSeries<double>);
            BuildNewSeriesCollection();
            AddSeries = new RelayCommand(o => _addSeries());
            RemoveSeries = new RelayCommand(o => _removeSeries());
            AvailableTypes = new List<Type>
            {
                typeof(LineSeries<double>),
                typeof(BarSeries<double>),
                typeof(StackedBarSeries<double>),
                typeof(ScatterSeries<double>),
                //typeof(BubbleSeries<double>),
                //typeof(HeatSeries<double>)
            };
        }

        public ObservableCollection<ISeries<double>> SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        public Type SeriesType
        {
            get => _seriesType;
            set
            {
                _seriesType = value;
                BuildNewSeriesCollection();
                OnPropertyChanged();
            }
        }

        public List<Type> AvailableTypes { get; set; }

        public ICommand AddSeries { get; }

        public ICommand RemoveSeries { get; }

        private void BuildNewSeriesCollection()
        {
            SeriesCollection = new ObservableCollection<ISeries<double>>
            {
                BuildNewSeries(),
                BuildNewSeries(),
                BuildNewSeries()
            };
        }

        private ISeries<double> BuildNewSeries()
        {
            var series = (ISeries<double>) Activator.CreateInstance(SeriesType);
            series.Values = new double[] {_r.Next(0, 10), _r.Next(0, 10), _r.Next(0, 10)};
            return series;
        }

        private void _addSeries()
        {
            SeriesCollection.Add(BuildNewSeries());
        }

        private void _removeSeries()
        {
            if (SeriesCollection.Count < 1) return;
            SeriesCollection.RemoveAt(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
