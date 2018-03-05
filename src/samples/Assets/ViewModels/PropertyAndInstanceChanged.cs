using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assets.Commands;
using Assets.Models;
using LiveCharts.Core;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries;
using Point = LiveCharts.Core.Coordinates.Point;

namespace Assets.ViewModels
{
    public class PropertyAndInstanceChanged: INotifyPropertyChanged
    {
        private readonly Random _r = new Random();
        private SeriesCollection _seriesCollection;

        public PropertyAndInstanceChanged()
        {
            Charting.Settings(charting =>
            {
                charting.For<City>((city, index) => new Point(index, city.Population));
                charting.For<City, WeightedPoint>((city, index) =>
                    new WeightedPoint(index, city.Population, _r.Next(0, 10)));
            });

            var series = new BarSeries<City>
            {
                new City
                {
                    Population = 1
                },
                new City
                {
                    Population = 2
                },
                new City
                {
                    Population = 3
                }
            };

            series.StrokeDashArray = new[] {2d, 2d};

            SeriesCollection = new SeriesCollection
            {
                series
            };

            AddPoint = new DelegateCommand(o => _addPoint());
            RemovePoint = new DelegateCommand(o => _removePoint());
            InsertPoint = new DelegateCommand(o => _insertPoint());
            RemoveBetweenPoint = new DelegateCommand(o => _removeBetweenPoint());
            EditPoint = new DelegateCommand(o => _editPoint());
            AddSeries = new DelegateCommand(o => _addSeries());
            RemoveSeries = new DelegateCommand(o => _removeSeries());
            EditSeries = new DelegateCommand(o => _changeSeriesProp());
            SetNewSeriesInstance = new DelegateCommand(o => _setNewSeriesInstance());
            ChangeAllSeriesItems = new DelegateCommand(o => _changeAllSeriesItems());
            RemoveOnDoubleClick = new DelegateCommand(o => _removeOnDoubleClick((IEnumerable<PackedPoint>) o));
        }

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSeries { get; }

        public ICommand RemoveSeries { get; }

        public ICommand SetNewSeriesInstance { get; }

        public ICommand EditSeries { get; }

        public ICommand AddPoint { get; }

        public ICommand InsertPoint { get; }

        public ICommand RemoveBetweenPoint { get; }

        public ICommand RemovePoint { get; }

        public ICommand EditPoint { get; }

        public ICommand ChangeAllSeriesItems { get; }

        public ICommand RemoveOnDoubleClick { get; }

        private void _addPoint()
        {
            if (SeriesCollection.Count < 1) return;
            SeriesCollection[0].Add(new City
            {
                Population = _r.Next(0, 10)
            });
        }

        private void _removePoint()
        {
            var series = SeriesCollection[0];
            if (series.Count == 1) return;
            series.RemoveAt(0);
        }

        private void _insertPoint()
        {
            var series = SeriesCollection[0];
            if (series.Count < 3) return;
            series.Insert(
                series.Count / 2,
                new City
                {
                    Population = _r.Next(0, 10)
                });
        }

        private void _removeBetweenPoint()
        {
            var series = SeriesCollection[0];
            if (series.Count < 3) return;
            series.RemoveAt(series.Count / 2);
        }

        private void _editPoint()
        {
            var series = (LineSeries<City>) SeriesCollection[0];
            if (series.Count < 1) return;
            series[0].Population = _r.Next(0, 10);
        }

        private void _setNewSeriesInstance()
        {
            SeriesCollection = new SeriesCollection
            {
                new BarSeries<City>
                {
                    new City
                    {
                        Population = _r.Next(0, 10)
                    },
                    new City
                    {
                        Population = _r.Next(0, 10)
                    }
                }
            };
        }

        private void _addSeries()
        {
            SeriesCollection.Add(new BarSeries<City>
            {
                new City
                {
                    Population = _r.Next(0, 10)
                },
                new City
                {
                    Population = _r.Next(0, 10)
                }
            });
        }

        private void _removeSeries()
        {
            if (SeriesCollection.Count - 1 < 0) return;
            SeriesCollection.RemoveAt(SeriesCollection.Count - 1);
        }

        private void _changeSeriesProp()
        {
            if (SeriesCollection.Count <= 0) return;
            SeriesCollection[0].Fill =
                Color.FromArgb(255, (byte) _r.Next(0, 255), (byte) _r.Next(0, 255), (byte) _r.Next(0, 255));
        }

        private void _changeAllSeriesItems()
        {
            foreach (var series in SeriesCollection)
            {
                foreach (City city in series)
                {
                    city.Population = _r.Next(0, 10);
                }
            }
        }

        private void _removeOnDoubleClick(IEnumerable<PackedPoint> clickedPoints)
        {
            foreach (var point in clickedPoints)
            {
                SeriesCollection[0].Remove(point.Model);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
