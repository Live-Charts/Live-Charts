using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assets.Models;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.Themes;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class CartesianChart : IChartView
    {
        static CartesianChart()
        {
            LiveChartsSettings.Set(settings =>
            {
                settings.DataFactory = new DefaultDataFactory();
                settings.UiProvider = new UiProvider();
                settings.UseMaterialDesignLightTheme()
                    .Has2DPlotFor<City>((city, index) => new Point2D(index, city.Population));
            });
        }

        private IEnumerable<DataSet> _series;
        private TimeSpan _animationsSpeed;
        private TimeSpan _tooltipTimeOut;
        private ILegend _legend;
        private LegendPosition _legendPosition;
        private IDataToolTip _dataToolTip;
        private IList<IList<Plane>> _dimensions;

        public CartesianChart()
        {
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
            ControlSize = new Size(600, 600);
            Model = new CartesianChartModel(this);
            DrawMargin = Margin.Empty;
            Legend = null;
            DataToolTip = null;
            ChartViewLoaded?.Invoke(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ChartEventHandler ChartViewLoaded;
        public event ChartEventHandler ChartViewResized;
        public event PointerMovedHandler PointerMoved;
        /// <inheritdoc />
        public event ChartEventHandler UpdatePreview
        {
            add => Model.UpdatePreview += value;
            remove => Model.UpdatePreview -= value;
        }

        /// <inheritdoc cref="IChartView.UpdatePreview" />
        public ICommand UpdatePreviewCommand
        {
            get => Model.UpdatePreviewCommand;
            set => Model.UpdatePreviewCommand = value;
        }

        /// <inheritdoc />
        public event ChartEventHandler Updated
        {
            add => Model.Updated += value;
            remove => Model.Updated -= value;
        }

        /// <inheritdoc cref="IChartView.Updated" />
        public ICommand UpdatedCommand
        {
            get => Model.UpdatedCommand;
            set => Model.UpdatedCommand = value;
        }
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

        public IEnumerable<DataSet> Series
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
