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

    public abstract class Series : IDisposableChartingResource
    {
        private object _planesUpdateId;
        private IList<Plane> _planes;
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class.
        /// </summary>
        protected Series(string key)
        {
            Geometry = Geometry.Empty;
            Fill = Color.Empty;
            Stroke = Color.Empty;
            IsVisible = true;
            Key = key;
        }

        public string Key { get; }

        public IEnumerable<ChartModel> UsedBy { get; internal set; }

        public bool DataLabels { get; set; }

        public DataLabelsPosition DataLabelsPosition { get; set; }

        public Font DataLabelsFont { get; set; }

        public Geometry Geometry { get; set; }

        public bool IsVisible { get; set; }

        public string Title { get; set; }

        public double StrokeThickness { get; set; }

        public Color Stroke { get; set; }

        public Color Fill { get; set; }

        public int ZIndex { get; set; }

        public int[] ScalesAt { get; protected set; }

        public Plane GetPlane(IChartView chart, int index)
        {
            return chart.PlanesArrayByDimension[index][ScalesAt[index]];
        }

        public IList<Plane> GetPlanes(IChartView chart)
        {
            if (_planesUpdateId == chart.Model.UpdateId) return _planes;
            _planesUpdateId = chart.Model.UpdateId;
            _planes = ScalesAt.Select((t, i) => chart.PlanesArrayByDimension[i][t]).ToList();
            return _planes;
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
        }

        public void UpdateView(ChartModel chart)
        {
            OnUpdateView(chart);
        }

        public void Fetch(ChartModel chart)
        {
            OnFetch(chart);
        }

        protected virtual void OnUpdateView(ChartModel chart)
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

        protected virtual void OnDispose(IChartView view)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnFetch(ChartModel model)
        {
            throw new  NotImplementedException();
        }

        void IDisposableChartingResource.Dispose(IChartView view)
        {
            OnDispose(view);
        }
    }

    public abstract class Series<TModel, TCoordinate, TViewModel, TPoint> : Series
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        private IEnumerable<TModel> _values;
        private ModelToPointMapper<TModel, TCoordinate> _mapper;
        private Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            _pointViewProvider;
        private object _chartPointsUpdateId;

        protected Series(string key)
            :base(key)
        {
        }

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

        public ModelToPointMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? LiveChartsSettings.GetCurrentMapperFor<TModel, TCoordinate>();
            set => _mapper = value;
        }

        public IList<TPoint> ByValTracker { get; set; }

        public IEnumerable<TPoint> Points { get; private set; }

        public Func<TModel, TViewModel> PointBuilder { get; set; }

        public Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            PointViewProvider
        {
            get => _pointViewProvider ?? DefaultPointViewProvider;
            set => _pointViewProvider = value;
        }

        public DimensionRange DataRange { get; } = new DimensionRange(double.PositiveInfinity, double.NegativeInfinity);

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
                    Fill = nextColor.SetOpacity(LiveChartsSettings.GetSeriesDefault(((Series) this).Key).FillOpacity);
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

        protected abstract IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>
            DefaultPointViewProvider();

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

        protected override void OnDispose(IChartView view)
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
    }
}
