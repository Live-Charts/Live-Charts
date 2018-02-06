using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Collections;
using LiveCharts.Core.Data;
using LiveCharts.Core.DefaultSettings;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The series class with a defined plot model, represents a series to plot in a chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TPoint">The type of the point.</typeparam>
    /// <seealso cref="IResource" />
    public abstract class Series<TModel, TCoordinate, TViewModel, TPoint> 
        : BaseSeries, IList<TModel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        private IEnumerable<TModel> _itemsSource;
        private IList<TModel> _sourceAsIList;
        private INotifyRangeChanged<TModel> _sourceAsRangeChanged;
        private ModelToPointMapper<TModel, TCoordinate> _mapper;
        private Func<IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>>
            _pointViewProvider;
        private object _chartPointsUpdateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        protected Series()
        {
            ByValTracker = new List<TPoint>();
            _itemsSource = new PlotableCollection<TModel>();
            OnItemsIntancechanged();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        /// <param name="itemsSource">The values.</param>
        protected Series(IEnumerable<TModel> itemsSource)
        {
            _itemsSource = itemsSource;
            OnItemsIntancechanged();
            ByValTracker = new List<TPoint>();
        }

        /// <inheritdoc />
        public new TModel this[int index]
        {
            get
            {
                EnsureIListImplementation();
                return _sourceAsIList[index];
            }
            set
            {
                EnsureIListImplementation();
                _sourceAsIList[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<TModel> ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                OnItemsIntancechanged();
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
        /// Gets the by value tracker.
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

        /// <inheritdoc />
        public int Count
        {
            get
            {
                EnsureIListImplementation();
                return _sourceAsIList.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                EnsureIListImplementation();
                return _sourceAsIList.IsReadOnly;
            }
        }

        /// <summary>
        /// Defaults the point view provider.
        /// </summary>
        /// <returns></returns>
        protected abstract IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>
            DefaultPointViewProvider();

        /// <inheritdoc />
        public override void Fetch(ChartModel model)
        {
            // returned cached points if this method was called from the same updateId.
            if (_chartPointsUpdateId == model.UpdateId) return;
            _chartPointsUpdateId = model.UpdateId;

            // Assign a color if the user did not set it.
            if (Stroke == Color.Empty || Fill == Color.Empty)
            {
                var nextColor = model.GetNextColor();
                if (Stroke == Color.Empty)
                {
                    Stroke = nextColor;
                }

                if (Fill == Color.Empty)
                {
                    Fill = nextColor.SetOpacity(DefaultFillOpacity);
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
                        Collection = ItemsSource.ToArray() // create a copy of the current points.
                    })
                .ToArray();
        }

        /// <inheritdoc />
        public override IEnumerable<PackedPoint> SelectPointsByDimension(TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            return Points.Where(point => point.HoverArea.Contains(selectionMode, dimensions))
                .Select(point => new PackedPoint
                {
                    Key = point.Key,
                    Model = point.Model,
                    Chart = point.Chart,
                    Coordinate = point.Coordinate,
                    Series = point.Series,
                    View = point.View,
                    ViewModel = point.ViewModel
                });
        }


        /// <inheritdoc />
        public IEnumerator<TModel> GetEnumerator()
        {
            return ItemsSource.GetEnumerator();
        }

        /// <inheritdoc />
        protected override IEnumerator OnGetEnumerator()
        {
            return ItemsSource.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(TModel item)
        {
            EnsureIListImplementation();
            return _sourceAsIList.IndexOf(item);
        }

        /// <inheritdoc />
        protected override int OnListIndexOf(object value)
        {
            return IndexOf((TModel) value);
        }

        /// <inheritdoc />
        public void Insert(int index, TModel item)
        {
            EnsureIListImplementation();
            _sourceAsIList.Insert(index, item);
        }

        /// <inheritdoc />
        protected override void OnIListInsert(int index, object value)
        {
            Insert(index, (TModel) value);
        }

        /// <inheritdoc />
        public void Add(TModel item)
        {
            EnsureIListImplementation();
            _sourceAsIList.Add(item);
        }

        /// <inheritdoc />
        protected override int OnIListAdd(object item)
        {
            Add((TModel) item);
            return Count;
        }

        /// <inheritdoc />
        public override void AddRange(IEnumerable items)
        {
            EnsureINotifyRangeImplementation();
            _sourceAsRangeChanged.AddRange((IEnumerable<TModel>) items);
        }

        /// <inheritdoc />
        public new void Clear()
        {
            EnsureIListImplementation();
            _sourceAsIList.Clear();
        }

        /// <inheritdoc />
        protected override void OnIListClear()
        {
            Clear();
        }

        /// <inheritdoc />
        public bool Contains(TModel item)
        {
            EnsureIListImplementation();
            return _sourceAsIList.Contains(item);
        }

        /// <inheritdoc />
        protected override bool OnIListContains(object item)
        {
            return Contains((TModel) item);
        }

        /// <inheritdoc />
        void ICollection<TModel>.CopyTo(TModel[] array, int arrayIndex)
        {
            EnsureIListImplementation();
            _sourceAsIList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        protected override void OnIListCopyTo(Array array, int index)
        {
            ((ICollection<TModel>) this).CopyTo((TModel[]) array, index);
        }

        /// <inheritdoc />
        public bool Remove(TModel item)
        {
            EnsureIListImplementation();
            return _sourceAsIList.Remove(item);
        }

        /// <inheritdoc />
        protected override void OnIListRemove(object item)
        {
            Remove((TModel) item);
        }

        /// <inheritdoc />
        public new void RemoveAt(int index)
        {
            EnsureIListImplementation();
            _sourceAsIList.RemoveAt(index);
        }

        /// <inheritdoc />
        public override void RemoveRange(IEnumerable items)
        {
            EnsureINotifyRangeImplementation();
            _sourceAsRangeChanged.RemoveRange((IEnumerable<TModel>) items);
        }

        /// <inheritdoc />
        protected override void OnIListRemoveAt(int index)
        {
            RemoveAt(index);
        }

        /// <inheritdoc />
        protected override bool OnIListIsReadOnly()
        {
            return _sourceAsIList.IsReadOnly;
        }

        /// <inheritdoc />
        protected override bool OnIListIsFixedSize()
        {
            return ((IList) _sourceAsIList).IsFixedSize;
        }

        /// <inheritdoc />
        protected override int OnIListCount()
        {
            return Count;
        }

        /// <inheritdoc />
        protected override object OnIListSyncRoot()
        {
            return ((IList) _sourceAsIList).SyncRoot;
        }

        /// <inheritdoc />
        protected override bool OnIListIsSynchronized()
        {
            return ((IList) _sourceAsIList).IsSynchronized;
        }

        /// <inheritdoc />
        protected override object GetItem(int index)
        {
            return this[index];
        }

        /// <inheritdoc />
        protected override void SetItem(object value, int index)
        {
            this[index] = (TModel) value;
        }

        private void EnsureIListImplementation([CallerMemberName] string method = null)
        {
            if (_sourceAsIList == null)
            {
                throw new LiveChartsException(
                    $"{nameof(ItemsSource)} property, does not implement {nameof(IList<TModel>)}, " +
                    $"thus the method {method} is not supported.",
                    200);
            }
        }

        private void EnsureINotifyRangeImplementation([CallerMemberName] string method = null)
        {
            if (_sourceAsRangeChanged == null)
            {
                throw new LiveChartsException(
                    $"{nameof(ItemsSource)} property, does not implement {nameof(INotifyRangeChanged<TModel>)}, " +
                    $"thus the method {method} is not supported.",
                    210);
            }
        }

        private void OnItemsIntancechanged()
        {
            _sourceAsIList = _itemsSource as IList<TModel>;
            _sourceAsRangeChanged = ItemsSource as INotifyRangeChanged<TModel>;
        }
    }
}