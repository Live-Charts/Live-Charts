using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.UnitTests
{
    public class MockedChart : IChartView
    {
        private IEnumerable<BaseSeries> _series;
        private TimeSpan _animationsSpeed;
        private TimeSpan _tooltipTimeOut;
        private ILegend _legend;
        private LegendPosition _legendPosition;
        private IDataToolTip _dataToolTip;
        private IList<IList<Plane>> _dimensions;

        public MockedChart()
        {
            Model = new CartesianChartModel(this);
            Dimensions = new List<IList<Plane>>
            {
                new List<Plane>
                {
                    new Axis()
                },
                new List<Plane>
                {
                    new Axis()
                }
            };
            ChartViewLoaded?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action ChartViewLoaded;
        public event Action ChartViewResized;
        public event PointerMovedHandler PointerMoved;
        public ChartModel Model { get; set; }
        public Size ControlSize { get; set; }
        public Margin DrawMargin { get; set; }

        public IList<IList<Plane>> Dimensions
        {
            get => _dimensions;
            set
            {
                _dimensions = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<BaseSeries> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan AnimationsSpeed
        {
            get => _animationsSpeed;
            set
            {
                _animationsSpeed = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan TooltipTimeOut
        {
            get => _tooltipTimeOut;
            set
            {
                _tooltipTimeOut = value;
                OnPropertyChanged();
            }
        }

        public ILegend Legend
        {
            get => _legend;
            set
            {
                _legend = value;
                OnPropertyChanged();
            }
        }

        public LegendPosition LegendPosition
        {
            get => _legendPosition;
            set
            {
                _legendPosition = value; 
                OnPropertyChanged();
            }
        }

        public IDataToolTip DataToolTip
        {
            get => _dataToolTip;
            set
            {
                _dataToolTip = value;
                OnPropertyChanged();
            }
        }

        public void UpdateDrawArea(Rectangle model)
        {
            
        }

        public void InvokeOnUiThread(Action action)
        {
            action();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
