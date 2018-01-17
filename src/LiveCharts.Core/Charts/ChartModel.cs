using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Defines a chart.
    /// </summary>
    public abstract class ChartModel : IDisposable
    {
        private static int _colorCount;
        private ILegend _previousLegend;
        private Task _delayer;
        private readonly Dictionary<string, object> _propertyReferences = new Dictionary<string, object>();
        private IList<Color> _colors;
        private readonly Dictionary<IDisposableChartingResource, object> _resources =
            new Dictionary<IDisposableChartingResource, object>();

        private object _updateId = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        protected ChartModel(IChartView view)
        {
            View = view;
            view.ChartViewInitialized += ChartViewOnInitialized;
            view.UpdaterFrequencyChanged += ChartViewOnUpdaterFreqChanged;
            view.DataInstanceChanged += ChartViewOnPropertyInstanceChanged;
        }

        /// <summary>
        /// Gets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        public object UpdateId
        {
            get { return _updateId; }
            private set { _updateId = value; }
        }

        /// <summary>
        /// Gets or sets the draw area location.
        /// </summary>
        /// <value>
        /// The draw area location.
        /// </value>
        public Point DrawAreaLocation { get; set; }

        /// <summary>
        /// Gets or sets the size of the draw area.
        /// </summary>
        /// <value>
        /// The size of the draw area.
        /// </value>
        public Size DrawAreaSize { get; set; }

        /// <summary>
        /// Gets the data range matrix.
        /// </summary>
        /// <value>
        /// The data range matrix.
        /// </value>
        public DimensionRange[][] DataRangeMatrix { get; protected set; }

        /// <summary>
        /// Gets the chart view.
        /// </summary>
        /// <value>
        /// The chart view.
        /// </value>
        public IChartView View { get; }

        /// <summary>
        /// Gets a value indicating whether this instance view is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance view is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsViewInitialized { get; private set; }

        /// <summary>
        /// Gets or sets the colors.
        /// </summary>
        /// <value>
        /// The colors.
        /// </value>
        public IList<Color> Colors
        {
            get => _colors ?? LiveChartsSettings.Current.Colors;
            set => _colors = value;
        }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public IEnumerable<ISeries> Series => View.Series;

        /// <summary>
        /// Gets or sets the default legend orientation.
        /// </summary>
        /// <value>
        /// The default legend orientation.
        /// </value>
        public Orientation DefaultLegendOrientation =>
            View.LegendPosition == LegendPositions.Top || View.LegendPosition == LegendPositions.Bottom
                ? Orientation.Horizontal
                : Orientation.Vertical;

        /// <summary>
        /// Invalidates this instance, the chart will queue an update request.
        /// </summary>
        /// <returns></returns>
        public async void Invalidate()
        {
            if (_delayer == null || !_delayer.IsCompleted) return;
            var delay = View.AnimationsSpeed.TotalMilliseconds < 10 
                ? TimeSpan.FromMilliseconds(10)
                : View.AnimationsSpeed;
            _delayer = Task.Delay(delay);
            await _delayer;
            Update(false);
        }

        /// <summary>
        /// Scales a number according to an axis range, to a given area, if the area is not present, the chart draw margin size will be used.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="plane">The axis.</param>
        /// <param name="size">The draw margin, this param is optional, if not set, the current chart's draw margin area will be used.</param>
        /// <returns></returns>
        public virtual double ScaleToUi(double value, Plane plane, Size? size = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scales a point to the UI.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public Point2D ScaleToUi(Point2D point, Plane x, Plane y)
        {
            return new Point2D(ScaleToUi(point.X, x), ScaleToUi(point.Y, y));
        }

        /// <summary>
        /// Updates the chart.
        /// </summary>
        /// <param name="restart">if set to <c>true</c> all the elements in the view will be redrawn.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void Update(bool restart)
        {
            UpdateId = new object();

            if (!IsViewInitialized)
            {
                Invalidate();
                return;
            }

            if (restart)
            {
                foreach (var resource in _resources)
                {
                    resource.Key.Dispose(View);
                }
                _resources.Clear();
            }

            // [ x: [x1: range, x2: range, x3: range, ..., xn: range], y: [...], z[...], w[...] ]
            DataRangeMatrix = View.PlanesArrayByDimension.Select(
                    x => x.Select(
                            y => new DimensionRange(
                                double.IsNaN(y.MinValue) ? double.PositiveInfinity : y.MinValue,
                                double.IsNaN(y.MaxValue) ? double.NegativeInfinity : y.MaxValue))
                        .ToArray())
                .ToArray();

            foreach (var series in View.Series.Where(x => x.IsVisible))
            {
                series.Fetch(this);
                RegisterResource(series);
            }

            var chartSize = View.ControlSize;
            double dax = 0, day = 0;
            var legendSize = new Size(0, 0);

            // draw and measure legend
            if (View.Legend != null && View.LegendPosition != LegendPositions.None)
            {
                if (_previousLegend != View.Legend)
                {
                    RegisterResource(View.Legend);
                }
                legendSize = View.Legend.Measure(Series, DefaultLegendOrientation);

                switch (View.LegendPosition)
                {
                    case LegendPositions.None:
                        dax = 0;
                        day = 0;
                        break;
                    case LegendPositions.Top:
                        dax = chartSize.Width * .5 - legendSize.Width * .5;
                        day = chartSize.Height - legendSize.Height;
                        break;
                    case LegendPositions.Bottom:
                        dax = chartSize.Width * .5 - legendSize.Width * .5;
                        day = 0;
                        break;
                    case LegendPositions.Left:
                        dax = chartSize.Width - legendSize.Width;
                        day = chartSize.Height * .5 - legendSize.Height * .5;
                        break;
                    case LegendPositions.Right:
                        dax = 0;
                        day = chartSize.Height * .5 - legendSize.Height * .5;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _previousLegend = View.Legend;
            }

            DrawAreaLocation = new Point(dax, day);
            DrawAreaSize = chartSize - legendSize;
        }

        internal Color GetNextColor()
        {
            return Colors[_colorCount++ % Colors.Count];
        }

        internal void RegisterResource(IDisposableChartingResource disposable)
        {
            if (!_resources.ContainsKey(disposable))
            {
                _resources.Add(disposable, UpdateId);
                return;
            }
            _resources[disposable] = UpdateId;
        }

        internal void CollectResources()
        {
            foreach (var disposable in _resources.ToArray())
            {
                if (disposable.Value == UpdateId) continue;
                disposable.Key.Dispose(View);
                _resources.Remove(disposable.Key);
            }
        }

        private void ChartViewOnInitialized()
        {
            IsViewInitialized = true;
            Update(false);
        }

        private void ChartViewOnPropertyInstanceChanged(object instance, string propertyName)
        {
            if (_propertyReferences.TryGetValue(propertyName, out object previousInstance))
            {
                if (previousInstance is INotifyCollectionChanged previousIncc)
                {
                    previousIncc.CollectionChanged -= OnCollectionChangedUpdate;
                }
            }
            if (instance is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += OnCollectionChangedUpdate;
            }
            _propertyReferences[propertyName] = instance;
        }

        private void OnCollectionChangedUpdate(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Invalidate();
        }

        private void ChartViewOnUpdaterFreqChanged(TimeSpan newValue)
        {
            Invalidate();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            View.ChartViewInitialized -= ChartViewOnInitialized;
            View.UpdaterFrequencyChanged -= ChartViewOnUpdaterFreqChanged;
            foreach (var reference in _propertyReferences)
            {
                if (reference.Value is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged -= OnCollectionChangedUpdate;
                }
            }
        }
    }
}
