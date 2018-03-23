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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Collections;
using LiveCharts.Core.Content;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Events;
using Font = LiveCharts.Core.Abstractions.Font;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The data set class, represents a series to plot in a chart.
    /// </summary>
    /// <seealso cref="IResource" />
    public abstract class Series : IResource, ISeries, INotifyPropertyChanged, IList
    {
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();
        private bool _isVisible;
        private int _zIndex;
        private int[] _scalesAt;
        private bool _dataLabels;
        private string _title;
        private Color _stroke;
        private float _strokeThickness;
        private Color _fill;
        private Font _font;
        private float _defaultFillOpacity;
        private Geometry _geometry;
        private DataLabelsPosition _dataLabelsPosition;
        private IEnumerable<double> _strokeDashArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class.
        /// </summary>
        protected Series()
        {
            IsVisible = true;
            Charting.BuildFromSettings<ISeries>(this);
        }

        #region Properties

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public abstract Type ResourceKey { get; }

        /// <inheritdoc />
        public object this[int index]
        {
            get => GetItem(index);
            set => SetItem(value, index);
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
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Color Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public float StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        public IEnumerable<double> StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                _strokeDashArray = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Color Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        public float DefaultFillOpacity
        {
            get => _defaultFillOpacity;
            set
            {
                _defaultFillOpacity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default geometry.
        /// </summary>
        /// <value>
        /// The default geometry.
        /// </value>
        public Geometry Geometry
        {
            get => _geometry;
            set
            {
                _geometry = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the data labels position.
        /// </summary>
        /// <value>
        /// The data labels position.
        /// </value>
        public DataLabelsPosition DataLabelsPosition
        {
            get => _dataLabelsPosition;
            set
            {
                _dataLabelsPosition = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the default width of the point.
        /// </summary>
        /// <value>
        /// The default width of the point.
        /// </value>
        public abstract float[] DefaultPointWidth { get; }

        /// <summary>
        /// Gets the point margin.
        /// </summary>
        /// <value>
        /// The point margin.
        /// </value>
        public abstract float[] PointMargin { get; }

        /// <summary>
        /// Gets the range by dimension.
        /// </summary>
        /// <value>
        /// The range by dimension.
        /// </value>
        public RangeF[] RangeByDimension { get; protected set; }

        /// <inheritdoc />
        bool IList.IsReadOnly => OnIListIsReadOnly();

        /// <summary>
        /// Called when [i list is read only].
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract bool OnIListIsReadOnly();

        /// <inheritdoc />
        bool IList.IsFixedSize => OnIListIsFixedSize();

        /// <summary>
        /// Called when [i list is fixed size].
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnIListIsFixedSize();

        /// <inheritdoc />
        public int Count => OnIListCount();

        /// <summary>
        /// Called when [i list count].
        /// </summary>
        /// <returns></returns>
        protected abstract int OnIListCount();

        /// <inheritdoc />
        object ICollection.SyncRoot => OnIListSyncRoot();

        /// <summary>
        /// Called when [i list synchronize root].
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract object OnIListSyncRoot();

        /// <inheritdoc />
        bool ICollection.IsSynchronized => OnIListIsSynchronized();

        #endregion

        /// <summary>
        /// Called when [i list is synchronized].
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract bool OnIListIsSynchronized();

        /// <inheritdoc />
        public int Add(object item)
        {
            return OnIListAdd(item);
        }

        /// <summary>
        /// Called when [i list add].
        /// </summary>
        /// <returns></returns>
        protected abstract int OnIListAdd(object item);

        /// <inheritdoc />
        public bool Contains(object value)
        {
            return OnIListContains(value);
        }

        /// <summary>
        /// Called when [i list contains].
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnIListContains(object value);

        /// <inheritdoc />
        public void Clear()
        {
            OnIListClear();
        }

        /// <summary>
        /// Called when [i list clear].
        /// </summary>
        protected abstract void OnIListClear();

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            return OnListIndexOf(value);
        }

        /// <summary>
        /// Called when [list index of].
        /// </summary>
        /// <returns></returns>
        protected abstract int OnListIndexOf(object item);

        /// <inheritdoc />
        public void Insert(int index, object value)
        {
            OnIListInsert(index, value);
        }

        /// <summary>
        /// Called when [i list insert].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        protected abstract void OnIListInsert(int index, object value);

        /// <inheritdoc />
        public void Remove(object item)
        {
            OnIListRemove(item);
        }

        /// <summary>
        /// Called when [i list remove].
        /// </summary>
        /// <param name="item">The item.</param>
        protected abstract void OnIListRemove(object item);

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            OnIListRemoveAt(index);
        }

        /// <summary>
        /// Called when [i list remove at].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract void OnIListRemoveAt(int index);

        /// <summary>
        /// Adds the given range of items.
        /// </summary>
        /// <param name="items">The items.</param>
        public abstract void AddRange(IEnumerable items);

        /// <summary>
        /// Removes the given range of items.
        /// </summary>
        /// <param name="items">The items.</param>
        public abstract void RemoveRange(IEnumerable items);

        internal abstract void UpdateStarted(IChartView chart);

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public abstract void UpdateView(ChartModel chart);

        internal abstract void UpdateFinished(IChartView chart);

        internal abstract void UsedBy(ChartModel chart);

        /// <summary>
        /// Fetches the data for the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public abstract void Fetch(ChartModel chart);

        /// <summary>
        /// Gets the points that  its hover area contains the given n dimensions.
        /// </summary>
        /// <param name="dimensions">The dimensions.</param>
        /// <returns></returns>
        public abstract IEnumerable<PackedPoint> GetInteractedPoints(params double[] dimensions);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return OnGetEnumerator();
        }

        /// <summary>
        /// Called when [get enumerator].
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator OnGetEnumerator();

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
            Point pointLocation,
            Margin pointMargin,
            float betweenBottomLimit,
            Size labelModel,
            DataLabelsPosition labelsPosition)
        {
            const double toRadians = Math.PI / 180;
            var rotationAngle = DataLabelsPosition.Rotation;

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

            switch (DataLabelsPosition.HorizontalAlignment)
            {
                case HorizontalAlingment.Centered:
                    left = pointLocation.X - .5f * width;
                    break;
                case HorizontalAlingment.Left:
                    left = pointLocation.X - pointMargin.Left - width;
                    break;
                case HorizontalAlingment.Right:
                    left = pointLocation.X + pointMargin.Right;
                    break;
                case HorizontalAlingment.Between:
                    left = (pointLocation.X + betweenBottomLimit) / 2f - .5f * width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.HorizontalAlignment), DataLabelsPosition.HorizontalAlignment,
                        null);
            }

            switch (DataLabelsPosition.VerticalAlignment)
            {
                case VerticalLabelPosition.Centered:
                    top = pointLocation.Y - .5f * height;
                    break;
                case VerticalLabelPosition.Top:
                    top = pointLocation.Y - pointMargin.Top - height;
                    break;
                case VerticalLabelPosition.Bottom:
                    top = pointLocation.Y + pointMargin.Bottom;
                    break;
                case VerticalLabelPosition.Between:
                    top = (pointLocation.Y + betweenBottomLimit) / 2f - .5f * height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(DataLabelsPosition.VerticalAlignment), DataLabelsPosition.VerticalAlignment, null);
            }

            return new PointF(left, top);
        }

        internal void AddChart(ChartModel chart)
        {
            if (_usedBy.Contains(chart)) return;
            _usedBy.Add(chart);
        }

        /// <inheritdoc />
        void ICollection.CopyTo(Array array, int index)
        {
            OnIListCopyTo(array, index);
        }

        /// <summary>
        /// Called when [i list copy to].
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract void OnIListCopyTo(Array array, int index);

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected abstract object GetItem(int index);

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <returns></returns>
        protected abstract void SetItem(object value, int index);

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            OnDisposing(view);
            Disposed?.Invoke(view, this);
        }

        /// <summary>
        /// Called when the series is disposed.
        /// </summary>
        protected virtual void OnDisposing(IChartView view)
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
    public abstract class Series<TModel, TCoordinate, TViewModel, TPoint> 
        : Series, IList<TModel>, INotifyCollectionChanged
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    { 
        private IEnumerable<TModel> _itemsSource;
        private IEnumerable<TModel> _previousItemsSource;
        private IList<TModel> _sourceAsIList;
        private INotifyRangeChanged<TModel> _sourceAsRangeChanged;
        private ModelToPointMapper<TModel, TCoordinate> _mapper;
        private ISeriesViewProvider<TModel, TCoordinate, TViewModel> _viewProvider;
        private object _chartPointsUpdateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        protected Series()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        /// <param name="itemsSource">The values.</param>
        protected Series(IEnumerable<TModel> itemsSource)
        {
            Initialize(itemsSource);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Properties

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
                OnItemsInstanceChanged();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public SeriesMetatada Metadata { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public ModelToPointMapper<TModel, TCoordinate> Mapper
        {
            get => _mapper ?? Charting.GetCurrentMapperFor<TModel, TCoordinate>();
            set
            {
                _mapper = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the visual content.
        /// </summary>
        /// <value>
        /// The tracker.
        /// </value>
        public Dictionary<ChartModel, SeriesContent<TModel, TCoordinate, TViewModel, TPoint>> Content { get; private set; }

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
        public ISeriesViewProvider<TModel, TCoordinate, TViewModel>
            ViewProvider
        {
            get => _viewProvider ?? DefaultViewProvider;
            set
            {
                _viewProvider = value;
                OnPropertyChanged();
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

        #endregion

        internal override void UsedBy(ChartModel chart)
        {
            if (Content.ContainsKey(chart)) return;
            Content[chart] = new SeriesContent<TModel, TCoordinate, TViewModel, TPoint>();
        }

        internal override void UpdateStarted(IChartView chart)
        {
            ViewProvider.OnUpdateStarted(chart, this);
        }

        internal override void UpdateFinished(IChartView chart)
        {
            ViewProvider.OnUpdateFinished(chart, this);
        }

        /// <summary>
        /// Defaults the point view provider.
        /// </summary>
        /// <returns></returns>
        protected abstract ISeriesViewProvider<TModel, TCoordinate, TViewModel> DefaultViewProvider { get; }

        /// <inheritdoc />
        public override void Fetch(ChartModel model)
        {
            // returned cached points if this method was called from the same updateId.
            if (_chartPointsUpdateId == model.UpdateId) return;
            _chartPointsUpdateId = model.UpdateId;

            // Assign a color if the user didn't.
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
            Points = Charting.Current.DataFactory
                .Fetch(
                    new DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint>
                    {
                        Series = this,
                        Chart = model,
                        Collection = ItemsSource.ToArray() // create a copy of the current points.
                    })
                .ToArray();
        }

        /// <inheritdoc />
        public override IEnumerable<PackedPoint> GetInteractedPoints(params double[] dimensions)
        {
            return Points
                .Where(point => point.InteractionArea.Contains(dimensions))
                .Select(point => new PackedPoint
                {
                    Key = point.Key,
                    Model = point.Model,
                    Chart = point.Chart,
                    Coordinate = point.Coordinate,
                    Series = point.Series,
                    View = point.View,
                    ViewModel = point.ViewModel,
                    InteractionArea = point.InteractionArea
                });
        }

        #region Items interaction

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
            EnsureIListImplementation();
            return _sourceAsIList.Count;
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

        #endregion

        /// <inheritdoc />
        protected override void OnDisposing(IChartView view)
        {
            if (!Content.ContainsKey(view.Model)) return;
            Content[view.Model].Dispose();
            Content.Remove(view.Model);
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

        private void OnItemsInstanceChanged()
        {
            _sourceAsIList = _itemsSource as IList<TModel>;
            _sourceAsRangeChanged = ItemsSource as INotifyRangeChanged<TModel>;

            if (_itemsSource is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += InccOnCollectionChanged;
            }

            if (_previousItemsSource is INotifyCollectionChanged pincc)
            {
                pincc.CollectionChanged -= InccOnCollectionChanged;
            }

            _previousItemsSource = _itemsSource;
        }

        private void InccOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            CollectionChanged?.Invoke(sender, notifyCollectionChangedEventArgs);
        }

        private void Initialize(IEnumerable<TModel> itemsSource = null)
        {
            Content = new Dictionary<ChartModel, SeriesContent<TModel, TCoordinate, TViewModel, TPoint>>();
            _itemsSource = itemsSource ?? new ChartingCollection<TModel>();
            OnItemsInstanceChanged();
            var t = typeof(TModel);
            Metadata = new SeriesMetatada
            {
                ModelType = t,
                IsValueType = t.IsValueType
            };
        }
    }
}