#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assets.Commands;
using Assets.Models;
using LiveCharts.Core;
using LiveCharts.Core.Collections;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using Point = LiveCharts.Core.Coordinates.Point;

#endregion

namespace Assets.ViewModels
{
    public class PropertyAndInstanceChanged: INotifyPropertyChanged
    {
        private readonly Random _r = new Random();
        private ChartingCollection<Series> _seriesCollection;
        private ChartingCollection<Plane> _x;

        public PropertyAndInstanceChanged()
        {
            Charting.Settings(charting =>
            {
                charting.For<City>((city, index) => new Point(index, city.Population));
                charting.For<City, WeightedPoint>((city, index) =>
                    new WeightedPoint(index, city.Population, _r.Next(0, 10)));
            });

            X = new ChartingCollection<Plane>
            {
                new Axis
                {
                    Step = 1
                }
            };

            SeriesCollection = new ChartingCollection<Series>
            {
                new ScatterSeries<double>
                {
                    1,
                    2,
                    3
                }
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

        public ChartingCollection<Series> SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        public ChartingCollection<Plane> X
        {
            get => _x;
            set
            {
                _x = value;
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
            SeriesCollection = new ChartingCollection<Series>
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
