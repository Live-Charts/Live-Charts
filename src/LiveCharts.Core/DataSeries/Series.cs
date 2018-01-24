using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.DefaultSettings;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Styles;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The series class, represents a series to plot in a chart.
    /// </summary>
    /// <seealso cref="IResource" />
    public abstract class Series : IResource, IStylable
    {
        private object _planesUpdateId;
        private IList<Plane> _planes;
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();
        private string _selector;
        private Style _style;
        private bool _isVisible;
        private int _zIndex;
        private int[] _scalesAt;
        private bool _dataLabels;
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class.
        /// </summary>
        protected Series(string selector)
        {
            Style = LiveChartsSettings.GetStyle(selector, "DefaultSeries");
            IsVisible = true;
            Selector = selector;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [data labels].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data labels]; otherwise, <c>false</c>.
        /// </value>
        public bool DataLabels
        {
            get => _dataLabels;
            set
            {
                _dataLabels = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the index of the z.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the scales at array.
        /// </summary>
        /// <value>
        /// The scales at.
        /// </value>
        public int[] ScalesAt
        {
            get => _scalesAt;
            protected set
            {
                _scalesAt = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the plane.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Plane GetPlane(IChartView chart, int index)
        {
            return chart.Dimensions[index][ScalesAt[index]];
        }

        /// <summary>
        /// Gets the planes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public IList<Plane> GetPlanes(IChartView chart)
        {
            if (_planesUpdateId == chart.Model.UpdateId) return _planes;
            _planesUpdateId = chart.Model.UpdateId;
            _planes = ScalesAt.Select((t, i) => chart.Dimensions[i][t]).ToList();
            return _planes;
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public void UpdateView(ChartModel chart)
        {
            OnUpdateView(chart);
        }

        /// <summary>
        /// Fetches the data for the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public void Fetch(ChartModel chart)
        {
            OnFetch(chart);
        }

        /// <summary>
        /// Gets the current values.
        /// </summary>
        /// <returns></returns>
        public virtual object GetValues()
        {
            throw new NotImplementedException();
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
        protected Point GetLabelPosition(
            Point pointLocation,
            Margin pointMargin,
            double betweenBottomLimit,
            Size labelModel,
            DataLabelsPosition labelsPosition)
        {
            const double toRadians = Math.PI / 180;
            var style = (SeriesStyle) Style;
            var rotationAngle = style.DataLabelsPosition.Rotation;

            var xw =
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Width); // width's    horizontal    component
            var yw =
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Width); // width's    vertical      component
            var xh =
                Math.Abs(Math.Sin(rotationAngle * toRadians) * labelModel.Height); // height's   horizontal    component
            var yh =
                Math.Abs(Math.Cos(rotationAngle * toRadians) * labelModel.Height); // height's   vertical      component

            var width = xw + xh;
            var height = yh + yw;

            double left, top;

            switch (style.DataLabelsPosition.HorizontalAlignment)
            {
                case HorizontalAlingment.Centered:
                    left = pointLocation.X - .5 * width;
                    break;
                case HorizontalAlingment.Left:
                    left = pointLocation.X - pointMargin.Left - width;
                    break;
                case HorizontalAlingment.Right:
                    left = pointLocation.X + pointMargin.Right;
                    break;
                case HorizontalAlingment.Between:
                    left = ((pointLocation.X + betweenBottomLimit) / 2) - .5 * width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.HorizontalAlignment), style.DataLabelsPosition.HorizontalAlignment,
                        null);
            }

            switch (style.DataLabelsPosition.VerticalAlignment)
            {
                case VerticalLabelPosition.Centered:
                    top = pointLocation.Y - .5 * height;
                    break;
                case VerticalLabelPosition.Top:
                    top = pointLocation.Y - pointMargin.Top - height;
                    break;
                case VerticalLabelPosition.Bottom:
                    top = pointLocation.Y + pointMargin.Bottom;
                    break;
                case VerticalLabelPosition.Between:
                    top = ((pointLocation.Y + betweenBottomLimit) / 2) - .5 * height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.VerticalAlignment), style.DataLabelsPosition.VerticalAlignment, null);
            }

            return new Point(left, top);
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnDispose(IChartView view)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [fetch].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnFetch(ChartModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [update view].
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnUpdateView(ChartModel chart)
        {
            throw new NotImplementedException();
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
        }

        #region IResource implementation

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            OnDispose(view);
        }

        #endregion

        #region IStylable implementation

        /// <inheritdoc />
        public string Selector
        {
            get => _selector;
            set
            {
                _selector = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Style Style
        {
            get => _style;
            set
            {
                _style = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INPC implementation

        /// <inheritdoc />
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

    /// <summary>
    /// The series class with a defined plot model, represents a series to plot in a chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TPoint">The type of the point.</typeparam>
    /// <seealso cref="IResource" />
    public abstract class Series<TModel, TCoordinate, TViewModel, TPoint> : Series
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        private IEnumerable<TModel> _values;
        private ModelToPointMapper<TModel, TCoordinate> _mapper;
        private Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            _pointViewProvider;
        private object _chartPointsUpdateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        /// <param name="selector"></param>
        protected Series(string selector)
            :base(selector)
        {
            ByValTracker = new List<TPoint>();
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<TModel> Values
        {
            get => _values;
            set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public ModelToPointMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? LiveChartsSettings.GetCurrentMapperFor<TModel, TCoordinate>();
            set
            {
                _mapper = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the by value tracker.
        /// </summary>
        /// <value>
        /// The by value tracker.
        /// </value>
        public IList<TPoint> ByValTracker { get; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IEnumerable<TPoint> Points { get; private set; }

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
        public Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            PointViewProvider
        {
            get => _pointViewProvider ?? DefaultPointViewProvider;
            set
            {
                _pointViewProvider = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the data range.
        /// </summary>
        /// <value>
        /// The data range.
        /// </value>
        public DimensionRange DataRange { get; } = new DimensionRange(double.PositiveInfinity, double.NegativeInfinity);

        /// <summary>
        /// Gets the current values.
        /// </summary>
        /// <returns></returns>
        public override object GetValues()
        {
            return Values;
        }

        /// <summary>
        /// Defaults the point view provider.
        /// </summary>
        /// <returns></returns>
        protected abstract IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>
            DefaultPointViewProvider();

        /// <summary>
        /// Called when [fetch].
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void OnFetch(ChartModel model)
        {
            // returned cached points if this method was called from the same updateId.
            if (_chartPointsUpdateId == model.UpdateId) return;
            _chartPointsUpdateId = model.UpdateId;

            // Assign a color if the user did not set it.
            if (Style.Stroke == Color.Empty || Style.Fill == Color.Empty)
            {
                var nextColor = model.GetNextColor();
                if (Style.Stroke == Color.Empty)
                {
                    Style.Stroke = nextColor;
                }

                if (Style.Fill == Color.Empty)
                {
                    Style.Fill = nextColor.SetOpacity(((SeriesStyle) Style).DefaultFillOpacity);
                }
            }

            // call the factory to fetch our data.
            // Fetch() has 2 main tasks.
            // 1. Calculate each ChartPoint required by the series.
            // 2. Evaluate every dimension to get Max and Min limits.
            Points = LiveChartsSettings.Current.DataFactory
                .FetchData(
                    new DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint>
                    {
                        Series = this,
                        Chart = model,
                        Collection = Values.ToArray() // create a copy of the current points.
                    })
                .ToArray();
        }
    }
}
