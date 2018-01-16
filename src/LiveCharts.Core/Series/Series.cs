using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TPoint">The type of the chart point.</typeparam>
    /// <seealso cref="ISeries{TModel,TCoordinate,TViewModel,TPoint}" />
    public abstract class Series<TModel, TCoordinate, TViewModel, TPoint>
        : ISeries<TModel, TCoordinate, TViewModel, TPoint>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        private readonly string _key;
        private object _chartPointsUpdateId;
        private IEnumerable<TModel> _values;
        private object _planesUpdateId;
        private IList<Plane> _planes;
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();
        private ModelToPointMapper<TModel, TCoordinate> _mapper;
        private Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>> _pointViewProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{T, U, V, W}"/> class.
        /// </summary>
        /// <param name="key">the series key.</param>
        protected Series(string key)
        {
            _key = key;
            Geometry = Geometry.Empty;
            Fill = Color.Empty;
            Stroke = Color.Empty;
        }

        /// <inheritdoc />
        string ISeries.Key => _key;

        /// <inheritdoc />
        public IEnumerable<TModel> Values
        {
            get => _values;
            set
            {
                // unsubscribe previous instance from INCC.
                if (_values is INotifyCollectionChanged previousIncc)
                {
                    previousIncc.CollectionChanged -= OnValuesCollectionChanged;
                }

                // assign the new instance
                _values = value;

                // subscribe new instance to INCC.
                if (_values is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged += OnValuesCollectionChanged;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<ChartModel> UsedBy { get; internal set; }

        /// <inheritdoc />
        public bool DataLabels { get; set; }

        /// <inheritdoc />
        public DataLabelsPosition DataLabelsPosition { get; set; }

        /// <inheritdoc />
        public Font DataLabelsFont { get; set; }

        /// <inheritdoc cref="ISeries.Geometry"/>
        public Geometry Geometry { get; set; }

        /// <inheritdoc />
        public bool IsVisible { get; set; }

        /// <inheritdoc />
        public ModelToPointMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? LiveChartsSettings.GetMapper<TModel, TCoordinate>();
            set => _mapper = value;
        }

        /// <inheritdoc />
        IList<TPoint> ISeries<TModel, TCoordinate, TViewModel, TPoint>.ByValTracker { get; set; }

        /// <inheritdoc />
        public IEnumerable<TPoint> Points { get; private set; }

        /// <inheritdoc />
        public Func<TModel, TViewModel> PointBuilder { get; set; }

        /// <inheritdoc />
        public Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            PointViewProvider
        {
            get => _pointViewProvider ?? DefaultPointViewProvider;
            set => _pointViewProvider = value;
        }

        /// <inheritdoc cref="ISeries.Title"/>
        public string Title { get; set; }

        /// <inheritdoc cref="ISeries.StrokeThickness"/>
        public double StrokeThickness { get; set; }

        /// <inheritdoc cref="ISeries.Stroke"/>
        public Color Stroke { get; set; }

        /// <inheritdoc cref="ISeries.Fill"/>
        public Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the index at Z position.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        public int ZIndex { get; set; }

        /// <inheritdoc />
        protected int[] ScalesAt { get; set; }

        int[] ISeries.ScaleAtByDimension => ScalesAt;

        /// <inheritdoc cref="ISeries.ScaleAtByDimension"/>
        public DimensionRange DataRange { get; } = new DimensionRange(double.PositiveInfinity, double.NegativeInfinity);
        Func<IPointView<TModel, TPoint, TCoordinate, TViewModel>> ISeries<TModel, TCoordinate, TViewModel, TPoint>.PointViewProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Fetches the data for a given chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public void Fetch(ChartModel chart)
        {
            // returned cached points if this method was called from the same updateId.
            if (_chartPointsUpdateId == chart.UpdateId) return;
            _chartPointsUpdateId = chart.UpdateId;

            // Assign a color if the user did not set it.
            if (Stroke == Color.Empty || Fill == Color.Empty)
            {
                var nextColor = chart.GetNextColor();
                if (Stroke == Color.Empty)
                {
                    Stroke = nextColor;
                }
                if (Fill == Color.Empty)
                {
                    Fill = nextColor.SetOpacity(LiveChartsSettings.GetSeriesDefault(((ISeries) this).Key).FillOpacity);
                }
            }

            // call the factory to fetch our data.
            // Fetch() has 2 main tasks.
            // 1. Calculate each ChartPoint required by the series.
            // 2. Evaluate every dimension and every axis to get Max and Min limits.
            Points = LiveChartsSettings.Current.DataFactory
                .FetchData(
                    new DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint>
                    {
                        Series = this,
                        Chart = chart,
                        Collection = new List<TModel>(Values),
                        PropertyChangedEventHandler = OnValuesItemPropertyChanged
                    });
        }
        
        /// <inheritdoc />
        public Plane GetPlane(IChartView chart, int index)
        {
            return chart.PlanesArrayByDimension[index][ScalesAt[index]];
        }

        /// <inheritdoc />
        public IList<Plane> GetPlanes(IChartView chart)
        {
            if (_planesUpdateId == chart.ChartModel.UpdateId) return _planes;
            _planesUpdateId = chart.ChartModel.UpdateId;
            _planes = ScalesAt.Select((t, i) => chart.PlanesArrayByDimension[i][t]).ToList();
            return _planes;
        }

        /// <param name="chart"></param>
        /// <inheritdoc cref="ISeries.UpdateView"/>
        void ISeries.UpdateView(ChartModel chart)
        {
            OnUpdateView(chart);
        }

        /// <inheritdoc cref="ISeries.UpdateView"/>
        protected virtual void OnUpdateView(ChartModel chart)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_values is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged -= OnValuesCollectionChanged;
            }

            var notifiesChange = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TModel));
            if (!notifiesChange) return;
            foreach (var value in _values)
            {
                var npc = (INotifyPropertyChanged) value;
                npc.PropertyChanged -= OnValuesItemPropertyChanged;
            }
            _values = null;
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
        }

        /// <summary>
        /// Gets the default point view.
        /// </summary>
        /// <returns></returns>
        protected abstract IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>
            DefaultPointViewProvider();

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
            var rotationAngle = DataLabelsPosition.Rotation;

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

            switch (DataLabelsPosition.HorizontalAlignment)
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
                        nameof(DataLabelsPosition.HorizontalAlignment), DataLabelsPosition.HorizontalAlignment, null);
            }

            switch (DataLabelsPosition.VerticalAlignment)
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
                        nameof(DataLabelsPosition.VerticalAlignment), DataLabelsPosition.VerticalAlignment, null);
            }

            return new Point(left, top);
        }

        private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (var chartModel in UsedBy)
            {
                chartModel.Invalidate();
            }
        }

        private void OnValuesItemPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            foreach (var chartModel in UsedBy)
            {
                chartModel.Invalidate();
            }
        }
    }
}
