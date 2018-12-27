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
using LiveCharts.Animations;
using LiveCharts.Charts;
using LiveCharts.Collections;
using LiveCharts.Coordinates;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Drawing.Styles;
using LiveCharts.Interaction.Controls;
using LiveCharts.Interaction.Events;
using LiveCharts.Interaction.Points;
using LiveCharts.Interaction.Series;
using LiveCharts.Updating;
using FontStyle = LiveCharts.Drawing.Styles.FontStyle;
#if NET45 || NET46
using Font = LiveCharts.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// The series class with a defined plot model, represents a series to plot in a chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    /// <typeparam name="TPointShape">The type of the point shape in hte UI.</typeparam>
    /// <seealso cref="IResource" />
    public abstract class Series<TModel, TCoordinate, TPointShape>
        : ISeries<TCoordinate>
        where TCoordinate : ICoordinate
        where TPointShape : class, IShape
    {
        private IEnumerable<TModel> _values = Enumerable.Empty<TModel>();
        private ModelToCoordinateMapper<TModel, TCoordinate>? _mapper;
        private object _chartPointsUpdateId = new object();
        private bool _isVisible;
        private bool _dataLabels;
        private string _title = string.Empty;
        private Font _dataLabelsFont = Font.Default;
        private double _defaultFillOpacity;
        private Geometry _geometry;
        private DataLabelsPosition _dataLabelsPosition;
        private IBrush _dataLabelsForeground = UIFactory.GetNewSolidColorBrush(255, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TShape}"/> class.
        /// </summary>
        protected Series()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TShape}"/> class.
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
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        /// <inheritdoc />
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <inheritdoc />
        public double DefaultFillOpacity
        {
            get => _defaultFillOpacity > 1 ? 1 : (_defaultFillOpacity < 0 ? 0 : _defaultFillOpacity);
            set
            {
                _defaultFillOpacity = value;
                OnPropertyChanged(nameof(DefaultFillOpacity));
            }
        }

        /// <inheritdoc />
        public Geometry Geometry
        {
            get => _geometry;
            set
            {
                _geometry = value;
                OnPropertyChanged(nameof(Geometry));
            }
        }

        /// <inheritdoc />
        public bool DataLabels
        {
            get => _dataLabels;
            set
            {
                _dataLabels = value;
                OnPropertyChanged(nameof(DataLabelsForeground));
            }
        }

        /// <inheritdoc />
        public DataLabelsPosition DataLabelsPosition
        {
            get => _dataLabelsPosition;
            set
            {
                _dataLabelsPosition = value;
                OnPropertyChanged(nameof(DataLabelsPosition));
            }
        }

        /// <inheritdoc />
        public Font DataLabelsFont
        {
            get => _dataLabelsFont;
            set
            {
                _dataLabelsFont = value;
                OnPropertyChanged(nameof(DataLabelsFont));
            }
        }

        /// <inheritdoc />
        public IBrush DataLabelsForeground
        {
            get => _dataLabelsForeground;
            set
            {
                _dataLabelsForeground = value;
                OnPropertyChanged(nameof(DataLabelsForeground));
            }
        }

        /// <inheritdoc />
        public TimeSpan AnimationsSpeed { get; set; }

        /// <inheritdoc />
        public IEnumerable<KeyFrame> AnimationLine { get; set; } = TimeLine.Completed;

        /// <inheritdoc />
        public DelayRules DelayRule { get; set; }

        /// <inheritdoc />
        int ISeries.GroupingIndex => -1;

        /// <inheritdoc />
        public Dictionary<ChartModel, Dictionary<string, object>> Content { get; protected set; } =
            new Dictionary<ChartModel, Dictionary<string, object>>();

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
                OnPropertyChanged(nameof(Values));
            }
        }

        /// <inheritdoc />
        public SeriesMetadata Metadata { get; protected set; }

        /// <inheritdoc />
        IEnumerable ISeries.Values
        {
            get => Values;
            set => Values = (IEnumerable<TModel>)value;
        }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public ModelToCoordinateMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? (_mapper = Global.Settings.GetCurrentMapperFor<TModel, TCoordinate>());
            set
            {
                _mapper = value;
                OnPropertyChanged(nameof(Mapper));
            }
        }

        /// <summary>
        /// Gets the points count.
        /// </summary>
        /// <value>
        /// The points count.
        /// </value>
        public int PointsCount { get; private set; }

        #endregion

        /// <inheritdoc />
        public Func<TCoordinate, string>? DataLabelFormatter { get; set; }

        /// <inheritdoc />
        public Func<TCoordinate, string>? TooltipFormatter { get; set; }

        /// <inheritdoc />
        string ISeries.GetDataLabel(ICoordinate coordinate)
        {
            return DataLabelFormatter?.Invoke((TCoordinate)coordinate) ?? "";
        }

        /// <inheritdoc />
        string ISeries.GetTooltipLabel(ICoordinate coordinate)
        {
            return TooltipFormatter?.Invoke((TCoordinate)coordinate) ?? "";
        }

        /// <inheritdoc />
        void ISeries.UpdateStarted(IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };

        }

        /// <inheritdoc />
        void ISeries.UpdateFinished(IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };
        }

        /// <inheritdoc />
        public abstract void UpdateView(ChartModel chart, UpdateContext context);

        /// <inheritdoc />
        void ISeries.UsedBy(ChartModel chart)
        {
            if (Content.ContainsKey(chart)) return;
            Dictionary<string, object> defaultDictionary = new Dictionary<string, object>
            {
                [Config.TrackerKey] =
                    new Dictionary<object,
                        ChartPoint<TModel, TCoordinate, TPointShape>>()
            };
            Content[chart] = defaultDictionary;
        }

        /// <summary>
        /// Sets the default colors.
        /// </summary>
        protected abstract void SetDefaultColors(ChartModel chart);

        /// <summary>
        /// Gets the points for the given view.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IEnumerable<ChartPoint<TModel, TCoordinate, TPointShape>>
            GetPoints(IChartView chart)
        {
            Dictionary<object, ChartPoint<TModel, TCoordinate, TPointShape>> tracker =
                (Dictionary<object, ChartPoint<TModel, TCoordinate, TPointShape>>)
                Content[chart.Model][Config.TrackerKey];
            return tracker.Values;
        }

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

            using (var factoryContext = new DataFactoryContext<TModel, TCoordinate>(
                    chart, this, context, Mapper, Values.ToArray()))
            {
                Dictionary<object, ChartPoint<TModel, TCoordinate, TPointShape>> tracker =
                    (Dictionary<object, ChartPoint<TModel, TCoordinate, TPointShape>>)
                    Content[chart][Config.TrackerKey];
                Global.Settings.DataFactory.Fetch(factoryContext, tracker, out int count);
                PointsCount = count;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<IChartPoint>? GetPointsAt(
            PointF pointerLocation, ToolTipSelectionMode selectionMode, bool snapToClosest, IChartView chart)
        {
            IEnumerable<ChartPoint<TModel, TCoordinate, TPointShape>>? query;

            if (!snapToClosest)
            {
                query = GetPoints(chart).Where(point => point.InteractionArea.Contains(pointerLocation, selectionMode));
            }
            else
            {
                var results = GetPoints(chart)
                    .Select(point => new
                    {
                        Distance = point.InteractionArea.DistanceTo(pointerLocation, selectionMode),
                        Point = point
                    }).ToArray();
                float min = results.Min(x => x.Distance);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                query = results?.Where(x => x.Distance == min).Select(x => x.Point);
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
        }

        void ISeries.RemovePointHighlight(IChartPoint point, IChartView chart)
        {
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.AnimationLine
            };
        }

        /// <summary>
        /// Draws the point label.
        /// </summary>
        /// <param name="chartPoint">The chart point.</param>
        protected void DrawPointLabel(ChartPoint<TModel, TCoordinate, TPointShape> chartPoint)
        {
            var chart = chartPoint.Chart;

            if (chartPoint.Label == null)
            {
                chartPoint.Label = UIFactory.GetNewLabel(chartPoint.Chart.Model);
                chart.Canvas.AddChild(chartPoint.Label, true);
            }

            chartPoint.Label.Content = chartPoint.Series.GetDataLabel(chartPoint.Coordinate);

            chartPoint.Label.FontFamily = DataLabelsFont.FamilyName;
            chartPoint.Label.FontSize = DataLabelsFont.Size;
            chartPoint.Label.FontStyle = DataLabelsFont.Style;
            chartPoint.Label.FontWeight = DataLabelsFont.Weight;

            chartPoint.Label.Paint(DataLabelsForeground, null);
            PlaceLabel(chartPoint, chartPoint.Label.Measure());
        }

        /// <summary>
        /// Places the label.
        /// </summary>
        /// <param name="chartPoint">The chart point.</param>
        /// <param name="size">The size.</param>
        protected virtual void PlaceLabel(
            ChartPoint<TModel, TCoordinate, TPointShape> chartPoint,
            SizeF size)
        {
            if (chartPoint.Label == null) return;
            chartPoint.Label.Left = chartPoint.Coordinate[0][0];
            chartPoint.Label.Top = chartPoint.Coordinate[0][1];
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
            double rotationAngle = labelsPosition.Rotation;

            float xw = (float)
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Width); // width's    horizontal    component
            float yw = (float)
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Width); // width's    vertical      component
            float xh = (float)
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Height); // height's   horizontal    component
            float yh = (float)
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Height); // height's   vertical      component

            float width = xw + xh;
            float height = yh + yw;

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

        private void Initialize(IEnumerable<TModel>? itemsSource = null)
        {
            _isVisible = true;
            Content = new Dictionary<ChartModel, Dictionary<string, object>>();
            _dataLabelsFont = new Font("Arial", 11, FontStyle.Regular, FontWeight.Regular);
            _dataLabelsForeground = UIFactory.GetNewSolidColorBrush(255, 30, 30, 30);
            _values = itemsSource ?? new ChartingCollection<TModel>();
            var t = typeof(TModel);
            Metadata = new SeriesMetadata
            {
                ModelType = t,
                IsValueType = t.IsValueType,
                IsObservable = typeof(INotifyPropertyChanged).IsAssignableFrom(t)
            };
            AnimationsSpeed = TimeSpan.MaxValue;
            AnimationLine = TimeLine.Completed;
            DelayRule = DelayRules.None;
            Global.Settings.BuildFromTheme<ISeries>(this);
        }

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; } = new object();

        void IResource.Dispose(IChartView chart, bool force)
        {
            OnDisposing(chart, force);
            Dictionary<string, object> viewContent = Content[chart.Model];
            viewContent.Remove(Config.TrackerKey);
            _values = Enumerable.Empty<TModel>();
            Disposed?.Invoke(chart, this, force);
        }

        /// <summary>get
        /// Called when the series is disposed.
        /// </summary>
        protected virtual void OnDisposing(IChartView chart, bool force)
        {
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
