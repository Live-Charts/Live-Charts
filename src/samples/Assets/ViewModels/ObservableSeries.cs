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
    public class ObservableSeriesAndValues : INotifyPropertyChanged
    {
        private ObservableCollection<ISeries> _seriesCollection;
        private readonly Random _r = new Random();
        private Type _seriesType;

        public ObservableSeriesAndValues()
        {
            SeriesType = typeof(LineSeries<double>);
            BuildNewSeriesCollection();

            AddSeries = new RelayCommand(o => _addSeries());
            RemoveSeries = new RelayCommand(o => _removeSeries());
            AddPointFirst = new RelayCommand(o => _addFirst());
            AddPointMiddle = new RelayCommand(o => _addMiddle());
            AddPointLast = new RelayCommand(o => _addLast());
            RemovePointFirst = new RelayCommand(o => _removeFirst());
            RemovePointMiddle = new RelayCommand(o => _removeMiddle());
            RemovePointLast = new RelayCommand(o => _removeLast());

            AvailableTypes = new List<Type>
            {
                typeof(LineSeries<double>),
                typeof(BarSeries<double>),
                typeof(StackedBarSeries<double>),
                typeof(ScatterSeries<double>)
            };
        }

        public ObservableCollection<ISeries> SeriesCollection
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

        public ICommand AddPointFirst { get; }

        public ICommand AddPointMiddle { get; }

        public ICommand AddPointLast { get; }

        public ICommand RemovePointFirst { get; }

        public ICommand RemovePointMiddle { get; }

        public ICommand RemovePointLast { get; }

        private void BuildNewSeriesCollection()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                BuildNewSeries(),
                BuildNewSeries(),
                BuildNewSeries()
            };
        }

        private ISeries BuildNewSeries()
        {
            var series = (ISeries) Activator.CreateInstance(SeriesType);
            series.Values = new ObservableCollection<double> {_r.Next(0, 10), _r.Next(0, 10), _r.Next(0, 10)};
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

        private void _addFirst()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            values.Insert(0, _r.Next(0, 10));
        }

        private void _addMiddle()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            values.Insert(values.Count / 2, _r.Next(0, 10));
        }

        private void _addLast()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            values.Add(_r.Next(0, 10));
        }

        private void _removeFirst()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            if (values.Count == 0) return;
            values.RemoveAt(0);
        }

        private void _removeMiddle()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            if (values.Count == 0) return;
            values.RemoveAt(values.Count/2);
        }

        private void _removeLast()
        {
            if (SeriesCollection.Count == 0) return;
            var values = (ObservableCollection<double>)SeriesCollection[0].Values;
            if (values.Count == 0) return;
            values.RemoveAt(values.Count - 1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
