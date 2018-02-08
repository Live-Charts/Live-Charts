using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The series class, represents a series to plot in a chart.
    /// </summary>
    /// <seealso cref="IResource" />
    public abstract class BaseSeries : IResource, ISeries, INotifyPropertyChanged, IList
    {
        private readonly List<ChartModel> _usedBy = new List<ChartModel>();
        private bool _isVisible;
        private int _zIndex;
        private int[] _scalesAt;
        private bool _dataLabels;
        private string _title;
        private Color _stroke;
        private double _strokeThickness;
        private Color _fill;
        private Font _font;
        private double _defaultFillOpacity;
        private Geometry _geometry;
        private DataLabelsPosition _dataLabelsPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeries"/> class.
        /// </summary>
        protected BaseSeries()
        {
            IsVisible = true;
            LiveChartsSettings.Build<ISeries>(this);
        }

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
        public double StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
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
        public double DefaultFillOpacity
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
        public abstract Point DefaultPointWidth { get; }

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

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public abstract void UpdateView(ChartModel chart);

        /// <summary>
        /// Fetches the data for the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public abstract void Fetch(ChartModel chart);

        /// <summary>
        /// Gets the points that  its hover area contains the given n dimensions.
        /// </summary>
        /// <param name="selectionMode">The selection mode.</param>
        /// <param name="dimensions">The dimensions.</param>
        /// <returns></returns>
        public abstract IEnumerable<PackedPoint> SelectPointsByDimension(TooltipSelectionMode selectionMode, params double[] dimensions);

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
                        nameof(DataLabelsPosition.HorizontalAlignment), DataLabelsPosition.HorizontalAlignment,
                        null);
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
            OnDisposing();
            Disposed?.Invoke(view, this);
        }

        /// <summary>
        /// Called when the series is disposed.
        /// </summary>
        protected virtual void OnDisposing()
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
}
