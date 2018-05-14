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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Collections;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
using Brush = LiveCharts.Core.Drawing.Brush;
using FontStyle = LiveCharts.Core.Drawing.Styles.FontStyle;
#if NET45 || NET46
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The series class with a defined plot model, represents a series to plot in a chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="IResource" />
    public abstract class Series<TModel, TCoordinate, TViewModel, TSeries>
        : ISeries<TModel, TCoordinate, TViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : class, ISeries
    { 
        private IEnumerable<TModel> _values;
        private ModelToCoordinateMapper<TModel, TCoordinate> _mapper;
        private ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries> _viewProvider;
        private object _chartPointsUpdateId;
        private List<ChartModel> _usedBy = new List<ChartModel>();
        private bool _isVisible;
        private bool _dataLabels;
        private string _title;
        private Font _dataLabelsFont;
        private double _defaultFillOpacity;
        private Geometry _geometry;
        private DataLabelsPosition _dataLabelsPosition;
        private Brush _dataLabelsForeground;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TSeries}"/> class.
        /// </summary>
        protected Series()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TSeries}"/> class.
        /// </summary>
        /// <param name="itemsSource">The values.</param>
        protected Series(IEnumerable<TModel> itemsSource)
        {
            Initialize(itemsSource);
        }

#region Properties

        /// <inheritdoc />
        public abstract Type ThemeKey { get; }

        /// <inheritdoc />
        public virtual SeriesStyle Style => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double DefaultFillOpacity
        {
            get => _defaultFillOpacity;
            set
            {
                _defaultFillOpacity = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Geometry Geometry
        {
            get => _geometry;
            set
            {
                _geometry = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool DataLabels
        {
            get => _dataLabels;
            set
            {
                _dataLabels = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public DataLabelsPosition DataLabelsPosition
        {
            get => _dataLabelsPosition;
            set
            {
                _dataLabelsPosition = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Font DataLabelsFont
        {
            get => _dataLabelsFont;
            set
            {
                _dataLabelsFont = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Brush DataLabelsForeground
        {
            get => _dataLabelsForeground;
            set
            {
                _dataLabelsForeground = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public TimeSpan AnimationsSpeed { get; set; }

        /// <inheritdoc />
        public IEnumerable<KeyFrame> AnimationLine { get; set; }

        /// <inheritdoc />
        public DelayRules DelayRule { get; set; }

        /// <inheritdoc />
        int ISeries.GroupingIndex => -1;

        /// <inheritdoc />
        public Dictionary<ChartModel, Dictionary<string, object>> Content { get; protected set; }

        /// <inheritdoc />
        public abstract float[] DefaultPointWidth { get; }
        
        /// <inheritdoc />
        public abstract float PointMargin { get; }

        /// <inheritdoc cref="ISeries.Values" />
        public IEnumerable<TModel> Values
        {
            get => _values;
            set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public SeriesMetadata Metadata { get; protected set; }

        /// <inheritdoc />
        IEnumerable ISeries.Values
        {
            get => Values;
            set => Values = (IEnumerable<TModel>) value;
        }

        /// <inheritdoc />
        public ModelToCoordinateMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? ( _mapper = Charting.GetCurrentMapperFor<TModel, TCoordinate>());
            set
            {
                _mapper = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IEnumerable<ChartPoint<TModel, TCoordinate, TViewModel, TSeries>> Points { get; private set; }

        /// <inheritdoc />
        public int PointsCount { get; private set; }

        /// <summary>
        /// Gets or sets the point builder.
        /// </summary>
        /// <value>
        /// The point builder.
        /// </value>
        public Func<TModel, TViewModel> PointBuilder { get; set; }

        /// <summary>
        /// Gets or sets the point view provider.
        /// </summary>
        /// <value>
        /// The point view provider.
        /// </value>
        public ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries>
            ViewProvider
        {
            get => _viewProvider ?? DefaultViewProvider;
            set
            {
                _viewProvider = value;
                OnPropertyChanged();
            }
        }

        internal LabelStyle LabelsStyle => new LabelStyle
        {
            Font = DataLabelsFont,
            Foreground = DataLabelsForeground,
            LabelsRotation = 0d,
            Padding = new Margin(0f)
        };

        #endregion

        /// <inheritdoc />
        public Func<TCoordinate, string> DataLabelFormatter { get; set; }

        /// <inheritdoc />
        public Func<TCoordinate, string> TooltipFormatter { get; set; }

        /// <inheritdoc />
        string ISeries.GetDataLabel(ICoordinate coordinate)
        {
            return DataLabelFormatter((TCoordinate) coordinate);
        }

        /// <inheritdoc />
        string ISeries.GetTooltipLabel(ICoordinate coordinate)
        {
            return TooltipFormatter((TCoordinate) coordinate);
        }

        /// <inheritdoc />
        void ISeries.UpdateStarted(IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };

            ViewProvider.OnUpdateStarted(chart, this as TSeries, timeLine);
        }

        /// <inheritdoc />
        void ISeries.UpdateFinished(IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };

            ViewProvider.OnUpdateFinished(chart, this as TSeries, timeLine);
        }

        /// <inheritdoc />
        public abstract void UpdateView(ChartModel chart, UpdateContext context);

        /// <inheritdoc />
        void ISeries.UsedBy(ChartModel chart)
        {
            if (Content.ContainsKey(chart)) return;
            var defaultDictionary = new Dictionary<string, object> {[Config.TrackerKey] = 
                new Dictionary<object, ChartPoint<TModel, TCoordinate, TViewModel, TSeries>>()};
            Content[chart] = defaultDictionary;
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
        }

        /// <summary>
        /// Defaults the point view provider.
        /// </summary>
        /// <returns></returns>
        protected abstract ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries> DefaultViewProvider { get; }

        /// <summary>
        /// Sets the default colors.
        /// </summary>
        protected abstract void SetDefaultColors(ChartModel chart);

        /// <inheritdoc />
        public void Fetch(ChartModel chart, UpdateContext context)
        {
            // do not recalculate if this method was called from the same updateId.
            if (_chartPointsUpdateId == chart.UpdateId) return;
            _chartPointsUpdateId = chart.UpdateId;

            // Assign a color if the user didn't.
            SetDefaultColors(chart);

            // call the factory to fetch our data.
            // Fetch() has 2 main tasks.
            // 1. Calculate each ChartPoint required by the series, and count the points.
            // 2. Compare every coordinate in the case of a cartesian chart, to get Max and Min limits, 
            // if stacked, then also do the stacking...

            var tSeries = this as TSeries;
            if (tSeries == null)
            {
                throw new LiveChartsException(122, GetType().Name, typeof(TSeries).Name);
            }
            
            using (var factoryContext = new DataFactoryContext<TModel, TCoordinate, TSeries>
            {
                Series = tSeries,
                Chart = chart,
                Mapper = Mapper,
                UpdateContext = context,
                Collection = Values.ToArray()
            })
            {
                Points = Charting.Current.DataFactory
                    .Fetch<TModel, TCoordinate, TViewModel, TSeries>(
                        factoryContext, out var count);
                PointsCount = count;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<IChartPoint> GetPointsAt(
            PointF pointerLocation, ToolTipSelectionMode selectionMode, bool snapToClosest)
        {
            IEnumerable<ChartPoint<TModel, TCoordinate, TViewModel, TSeries>> query;

            if (!snapToClosest)
            {
                query = Points.Where(point => point.InteractionArea.Contains(pointerLocation, selectionMode));
            }
            else
            {
                var results = Points
                    .Select(point => new
                    {
                        Distance = point.InteractionArea.DistanceTo(pointerLocation, selectionMode),
                        Point = point
                    }).ToArray();
                var min = results.Min(x => x.Distance);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                query = results.Where(x => x.Distance == min).Select(x => x.Point);
            }

            return query;
        }

        void ISeries.OnPointHighlight(IChartPoint point, IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };

            ViewProvider.OnPointHighlight(point, timeLine);
        }

        void ISeries.RemovePointHighlight(IChartPoint point, IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };

            ViewProvider.RemovePointHighlight(point, timeLine);
        }

        /// <summary>
        /// Evaluates the data label.
        /// </summary>
        /// <param name="pointLocation">The point location.</param>
        /// <param name="pointMargin">The point margin.</param>
        /// <param name="betweenBottomLimit">The series bottom limit.</param>
        /// <param name="labelModel">The label model.</param>
        /// <param name="labelsPosition">The labels position.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// horizontal - null
        /// or
        /// vertical - null
        /// </exception>
        protected PointF GetLabelPosition(
            PointF pointLocation,
            Margin pointMargin,
            float betweenBottomLimit,
            Size labelModel,
            DataLabelsPosition labelsPosition)
        {
            const double toRadians = Math.PI / 180;
            var rotationAngle = labelsPosition.Rotation;

            var xw = (float)
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Width); // width's    horizontal    component
            var yw = (float)
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Width); // width's    vertical      component
            var xh = (float)
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Height); // height's   horizontal    component
            var yh = (float)
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Height); // height's   vertical      component

            var width = xw + xh;
            var height = yh + yw;

            float left, top;

            switch (labelsPosition.HorizontalAlignment)
            {
                case HorizontalAlignment.Centered:
                    left = pointLocation.X - .5f * width;
                    break;
                case HorizontalAlignment.Left:
                    left = pointLocation.X - pointMargin.Left - width;
                    break;
                case HorizontalAlignment.Right:
                    left = pointLocation.X + pointMargin.Right;
                    break;
                case HorizontalAlignment.Between:
                    left = (pointLocation.X + betweenBottomLimit) / 2f - .5f * width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.HorizontalAlignment), 
                        DataLabelsPosition.HorizontalAlignment,
                        null);
            }

            switch (DataLabelsPosition.VerticalAlignment)
            {
                case VerticalAlignment.Centered:
                    top = pointLocation.Y - .5f * height;
                    break;
                case VerticalAlignment.Top:
                    top = pointLocation.Y - pointMargin.Top - height;
                    break;
                case VerticalAlignment.Bottom:
                    top = pointLocation.Y + pointMargin.Bottom;
                    break;
                case VerticalAlignment.Between:
                    top = (pointLocation.Y + betweenBottomLimit) / 2f - .5f * height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.VerticalAlignment), 
                        DataLabelsPosition.VerticalAlignment, null);
            }

            return new PointF(left, top);
        }

        private void Initialize(IEnumerable<TModel> itemsSource = null)
        {
            _isVisible = true;
            Content = new Dictionary<ChartModel, Dictionary<string, object>>();
            _dataLabelsFont = new Font("Arial", 11, FontStyle.Regular, FontWeight.Regular);
            _dataLabelsForeground = new SolidColorBrush(Color.FromArgb(30, 30, 30));
            _values = itemsSource ?? new ChartingCollection<TModel>();
            var t = typeof(TModel);
            Metadata = new SeriesMetadata
            {
                ModelType = t,
                IsValueType = t.IsValueType,
                IsObservable = typeof(INotifyPropertyChanged).IsAssignableFrom(t)
            };
            AnimationsSpeed = TimeSpan.MaxValue;
            AnimationLine = null;
            DelayRule = DelayRules.None;
            Charting.BuildFromTheme<ISeries>(this);
        }

#region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view, bool force)
        {
            OnDisposing(view, force);
            Disposed?.Invoke(view, this);
        }

        /// <summary>get
        /// Called when the series is disposed.
        /// </summary>
        protected virtual void OnDisposing(IChartView view, bool force)
        {
            _usedBy = null;
            if (!Content.ContainsKey(view.Model)) return;
            Content[view.Model] = null;
            Content.Remove(view.Model);
        }

#endregion

#region INPC implementation

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#endregion
    }
}
