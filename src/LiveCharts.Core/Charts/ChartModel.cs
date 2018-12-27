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
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using LiveCharts.DataSeries;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Styles;
using LiveCharts.Interaction.Controls;
using LiveCharts.Interaction.Events;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;

#endregion

namespace LiveCharts.Charts
{
    /// <summary>
    /// Defines a chart.
    /// </summary>
    public abstract class ChartModel : IDisposable
    {
        private static int _colorCount;
        private Task _delayer = Task.FromResult(false);
        private IList<Color> _colors = new List<Color>();
        private readonly HashSet<IResource> _resources = new HashSet<IResource>();
        private readonly Dictionary<IEnumerable, EnumerableResource> _enumerableResources =
            new Dictionary<IEnumerable, EnumerableResource>();
        private PointF _previousTooltipLocation = PointF.Empty;
        private IEnumerable<IChartPoint>? _previousHovered;
        private HashSet<IChartPoint> _previousEntered = new HashSet<IChartPoint>(Enumerable.Empty<IChartPoint>());

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        protected ChartModel(IChartView view)
        {
            View = view;
            View.Canvas = UIFactory.GetNewChartContent(view);
            view.Canvas.ContentLoaded += OnContentOnContentLoaded;
            view.ViewResized += OnViewOnChartViewResized;
            view.Canvas.PointerMoved += ViewOnPointerMoved;
            view.Canvas.PointerDown += ViewOnPointerDown;
            view.PropertyChanged += InvalidatePropertyChanged;
            ToolTipTimeoutTimer = new Timer();
            ToolTipTimeoutTimer.Elapsed += OnToolTipTimeoutTimerOnElapsed;
            Global.Settings.BuildFromTheme(view);
        }

        /// <summary>
        /// Gets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        public object UpdateId { get; private set; } = new object();

        /// <summary>
        /// Gets a value indicating whether this instance is disposing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposing; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposing { get; private set; }

        /// <summary>
        /// Gets or sets the draw area location.
        /// </summary>
        /// <value>
        /// The draw area location.
        /// </value>
        public float[] DrawAreaLocation { get; set; } = new[] { 0f, 0f };

        /// <summary>
        /// Gets or sets the size of the draw area.
        /// </summary>
        /// <value>
        /// The size of the draw area.
        /// </value>
        public float[] DrawAreaSize { get; set; } = new[] { 0f, 0f };

        /// <summary>
        /// Gets the chart view.
        /// </summary>
        /// <value>
        /// The chart view.
        /// </value>
        public IChartView View { get; private set; }

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
            get => _colors ?? Global.Settings.Colors;
            set => _colors = value;
        }

        /// <summary>
        /// Gets or sets the default legend orientation.
        /// </summary>
        /// <value>
        /// The default legend orientation.
        /// </value>
        public Orientation DefaultLegendOrientation =>
            LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom
                ? Orientation.Horizontal
                : Orientation.Vertical;

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        public event ChartEventHandler UpdatePreview;

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        public ICommand? UpdatePreviewCommand { get; set; }

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        public event ChartEventHandler Updated;

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        public ICommand? UpdatedCommand { get; set; }

        /// <summary>
        /// Occurs when the users pointer goes down over a data point.
        /// </summary>
        public event DataInteractionHandler DataPointerDown;

        /// <summary>
        /// Gets or sets the data pointer down command., the command will be executed when the user pointer
        /// goes down over a data point.
        /// </summary>
        /// <value>
        /// The data pointer down command.
        /// </value>
        public ICommand? DataPointerDownCommand { get; set; }

        /// <summary>
        /// Occurs when [data pointer enter].
        /// </summary>
        public event DataInteractionHandler DataPointerEntered;

        /// <summary>
        /// Gets or sets the data pointer enter.
        /// </summary>
        /// <value>
        /// The data pointer enter.
        /// </value>
        public ICommand? DataPointerEnteredCommand { get; set; }

        /// <summary>
        /// Occurs when [data pointer leave].
        /// </summary>
        public event DataInteractionHandler DataPointerLeft;

        /// <summary>
        /// Gets or sets the data pointer leave command.
        /// </summary>
        /// <value>
        /// The data pointer leave command.
        /// </value>
        public ICommand? DataPointerLeftCommand { get; set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public ISeries[] Series { get; internal set; } = new ISeries[0];

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public Plane[][] Dimensions { get; internal set; } = new Plane[0][];

        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        public float[] ControlSize { get; internal set; } = new float[] { 0f, 0f };

        /// <summary>
        /// Gets the draw margin.
        /// </summary>
        /// <value>
        /// The draw margin.
        /// </value>
        public Margin DrawMargin { get; internal set; }

        /// <summary>
        /// Gets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        public TimeSpan AnimationsSpeed { get; internal set; }

        /// <summary>
        /// Gets the legend.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        public ILegend? Legend { get; internal set; }

        /// <summary>
        /// Gets the legend position.
        /// </summary>
        /// <value>
        /// The legend position.
        /// </value>
        public LegendPosition LegendPosition { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [invert xy].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [invert xy]; otherwise, <c>false</c>.
        /// </value>
        public bool InvertXy { get; internal set; }

        internal Timer ToolTipTimeoutTimer { get; set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        protected abstract int DimensionsCount { get; }

        /// <summary>
        /// Invalidates this instance, the chart will queue an update request.
        /// </summary>
        /// <returns></returns>
        public async void Invalidate(bool restart = false, bool force = false)
        {
            if (!IsViewInitialized || IsDisposing)
            {
                return;
            }

            if (!force)
            {
                if (View.UpdaterState == UpdaterStates.Paused)
                {
                    return;
                }

                if (_delayer != null && !_delayer.IsCompleted)
                {
                    return;
                }
            }

            var delay = AnimationsSpeed.TotalMilliseconds < 10
                ? TimeSpan.FromMilliseconds(10)
                : AnimationsSpeed;
            _delayer = Task.Delay(delay);

            await _delayer;

            View.InvokeOnUiThread(() =>
            {
                CopyDataFromView();

                var context = new UpdateContext(Series.Where(series => series.IsVisible));
                float[][][] dims = new float[Dimensions.Length][][];
                for (int dimIndex = 0; dimIndex < Dimensions.Length; dimIndex++)
                {
                    Plane[] dimension = Dimensions[dimIndex];
                    dims[dimIndex] = new float[dimension.Length][];
                    for (int planeIndex = 0; planeIndex < dimension.Length; planeIndex++)
                    {
                        dims[dimIndex][planeIndex] = new[] { float.MaxValue, float.MinValue };
                    }
                }
                context.Ranges = dims;

                Update(restart, context);
            });
        }

        /// <summary>
        /// Scales to pixels a data value according to an axis range and a given area, if the area is not present, the chart draw margin size will be used.
        /// </summary>
        /// <param name="dataValue">The value.</param>
        /// <param name="plane">The axis.</param>
        /// <param name="sizeVector">The draw margin, this param is optional, if not set, the current chart's draw margin area will be used.</param>
        /// <returns></returns>
        public abstract float ScaleToUi(double dataValue, Plane plane, float[]? sizeVector = null);

        /// <summary>
        /// Scales from pixels to a data value.
        /// </summary>
        /// <param name="pixelsValue">The value.</param>
        /// <param name="plane">The plane.</param>
        /// <param name="sizeVector">The size.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public abstract double ScaleFromUi(float pixelsValue, Plane plane, float[]? sizeVector = null);

        /// <summary>
        /// Get2s the width of the d UI unit.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public virtual float[] Get2DUiUnitWidth(Plane x, Plane y)
        {
            return new[]
            {
                Math.Abs(ScaleToUi(0, x) - ScaleToUi(x.ActualPointLength[x.Dimension], x)),
                Math.Abs(ScaleToUi(0, y) - ScaleToUi(y.ActualPointLength[y.Dimension], y))
            };
        }

        /// <summary>
        /// Selects the points.
        /// </summary>
        /// <param name="pointerLocation">The dimensions.</param>
        /// <param name="selectionMode">The selection mode.</param>
        /// <param name="snapToClosest">Specifies if the result should only get the closest point.</param>
        /// <returns></returns>
        public IEnumerable<IChartPoint>? GetPointsAt(
            PointF pointerLocation, ToolTipSelectionMode selectionMode, bool snapToClosest)
        {
            if (!snapToClosest)
            {
                return Series.SelectMany(series => series.GetPointsAt(pointerLocation, selectionMode, false, View));
            }

            var results = Series.SelectMany(series => series.GetPointsAt(pointerLocation, selectionMode, true, View))
                .Select(point => new
                {
                    Distance = point.InteractionArea.DistanceTo(pointerLocation, selectionMode),
                    Point = point
                }).ToArray();
            float min = results.Min(x => x.Distance);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return results?.Where(x => x.Distance == min).Select(x => x.Point);
        }

        /// <summary>
        /// Called when the pointer moves over a chart and there is a tooltip in the view.
        /// </summary>
        /// <param name="pointerLocation">The dimensions.</param>
        /// <param name="args">The args.</param>
        protected virtual void ViewOnPointerMoved(PointF pointerLocation, EventArgs args)
        {
            if (Series == null) return;
            if (!IsViewInitialized) return;
            bool requiresDataLeft = DataPointerLeft != null && DataPointerLeftCommand != null;
            bool requiresDataEnter = DataPointerEntered != null && DataPointerEnteredCommand != null;
            bool requiresTooltip = View.DataToolTip != null;
            if (!requiresTooltip && !requiresDataEnter && !requiresDataLeft) return;

            if (requiresTooltip)
            {
                EvaluateTooltip(pointerLocation);
            }

            if (!requiresDataEnter && !requiresDataLeft) return;

            EvaluateEnterLeftPoints(pointerLocation, args);
        }

        /// <summary>
        /// Called when the pointer goes down in the tooltip
        /// </summary>
        /// <param name="pointerLocation">The pointer location.</param>
        /// /// <param name="args">The event args.</param>
        protected virtual void ViewOnPointerDown(PointF pointerLocation, EventArgs args)
        {
            if (Series == null) return;
            if (!IsViewInitialized) return;
            if (DataPointerDown == null && DataPointerDownCommand == null && View.DataToolTip == null)
            {
                return;
            }

            IChartPoint[] query = GetPointsAt(pointerLocation, ToolTipSelectionMode.SharedXy, false).ToArray();

            if (query.Length < 1)
            {
                ToolTipTimeoutTimer.Start();
                return;
            }

            OnDataPointerDown(query, args);

            if (View.DataToolTip != null)
            {
                ShowTooltip(query);
            }
        }

        /// <summary>
        /// Gets the tool tip location and fires hovering.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        protected abstract PointF GetTooltipLocation(
            IChartPoint[] points);

        /// <summary>
        /// Updates the chart.
        /// </summary>
        /// <param name="restart">if set to <c>true</c> all the elements in the view will be redrawn.</param>
        /// <param name="context">the update context.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void Update(bool restart, UpdateContext context)
        {
            UpdateId = new object();

            if (restart)
            {
                foreach (var resource in _resources)
                {
                    resource.Dispose(View, true);
                }

                _resources.Clear();
            }

            foreach (var series in Series.Where(x => x.IsVisible))
            {
                series.UsedBy(this);
                series.Fetch(this, context);
                RegisterINotifyPropertyChanged(series);
                RegisterINotifyCollectionChanged(series.Values);
                OnPreparingSeries(context, series);
            }

            float[] chartSize = ControlSize;
            //View.Content.ControlSize = new SizeF(ControlSize[0], ControlSize[1]);
            float dax = 0f, day = 0f;
            float lw = 0f, lh = 0f;

            // draw and measure legend
            if (Legend != null && LegendPosition != LegendPosition.None)
            {
                RegisterINotifyPropertyChanged(Legend);

                float[] legendSize = Legend.Measure(Series, DefaultLegendOrientation, View);

                float lx = 0f, ly = 0f;

                switch (LegendPosition)
                {
                    case LegendPosition.None:
                        break;
                    case LegendPosition.Top:
                        day = legendSize[1];
                        lh = legendSize[1];
                        lx = ControlSize[0] * .5f - legendSize[0] * .5f;
                        break;
                    case LegendPosition.Bottom:
                        lh = legendSize[1];
                        lx = chartSize[0] * .5f - legendSize[0] * .5f;
                        ly = chartSize[1] - legendSize[1];
                        break;
                    case LegendPosition.Left:
                        dax = legendSize[0];
                        lw = legendSize[0];
                        ly = chartSize[1] * .5f - legendSize[1] * .5f;
                        break;
                    case LegendPosition.Right:
                        lw = legendSize[0];
                        lx = chartSize[0] - legendSize[0];
                        ly = chartSize[1] * .5f - legendSize[1] * .5f;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Legend.Move(new PointF(lx, ly), View);
            }

            DrawAreaLocation = new[] { dax, day };
            DrawAreaSize = Perform.SubstractEach2D(chartSize, new[] { lw, lh });
            View.SetContentArea(
                new RectangleF(
                    new PointF(DrawAreaLocation[0], DrawAreaLocation[1]),
                    new SizeF(DrawAreaSize[0], DrawAreaSize[1])));
        }

        /// <summary>
        /// Copies the data from the view to use it in the next update cycle.
        /// </summary>
        protected virtual void CopyDataFromView()
        {
            Series = (View.Series ?? Enumerable.Empty<ISeries>()).ToArray();
            Dimensions = View.Dimensions.Select(x => x.ToArray()).ToArray();
            ControlSize = View.ControlSize;
            DrawMargin = View.DrawMargin;
            AnimationsSpeed = View.AnimationsSpeed;
            LegendPosition = View.LegendPosition;
            Legend = View.Legend;
            RegisterINotifyCollectionChanged(View.Series);
            foreach (IList<Plane> dimension in View.Dimensions)
            {
                RegisterINotifyCollectionChanged(dimension);
            }
        }

        /// <summary>
        /// Called when the series was fetched and registered in the resources collector.
        /// </summary>
        /// <param name="context">The update context.</param>
        /// <param name="series">The series.</param>
        protected virtual void OnPreparingSeries(UpdateContext context, ISeries series)
        {
        }

        internal Color GetNextColor()
        {
            if (Series.Length - 1 < _colorCount) _colorCount = 0;
            return Colors[_colorCount++ % Colors.Count];
        }

        internal void RegisterINotifyPropertyChanged(IResource resource)
        {
            if (!_resources.Contains(resource))
            {
                _resources.Add(resource);

                // ReSharper disable once IdentifierTypo
                if (resource is INotifyPropertyChanged inpc)
                {
                    inpc.PropertyChanged += InvalidatePropertyChanged;

                    void DisposePropertyChanged(IChartView view, object instance, bool force)
                    {
                        inpc.PropertyChanged -= InvalidatePropertyChanged;
                        resource.Disposed -= DisposePropertyChanged;
                    }

                    resource.Disposed += DisposePropertyChanged;
                }
            }

            // posible bug in C#8 beta compiler
#pragma warning disable CS8602 // Possible dereference of a null reference.
            resource.UpdateId = UpdateId;
#pragma warning restore CS8602 // Possible dereference of a null reference.
        }

        internal void RegisterINotifyCollectionChanged(IEnumerable collection)
        {
            // ReSharper disable once IdentifierTypo
            if (!(collection is INotifyCollectionChanged incc)) return;

            // ReSharper disable once PossibleMultipleEnumeration
            if (!_enumerableResources.TryGetValue(collection, out var resource))
            {
                resource = new EnumerableResource
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    Collection = collection,
                    UpdateId = UpdateId
                };

                // ReSharper disable once PossibleMultipleEnumeration
                _enumerableResources.Add(collection, resource);

                incc.CollectionChanged += InvalidateOnCollectionChanged;

                void DisposeCollectionChanged()
                {
                    incc.CollectionChanged -= InvalidateOnCollectionChanged;
                    resource.Disposed -= DisposeCollectionChanged;
                }

                resource.Disposed += DisposeCollectionChanged;
            }

            resource.UpdateId = UpdateId;
        }

        internal void CollectResources(bool collectAll = false, bool wasChartDisposed = false)
        {
            foreach (var resource in _resources.ToArray())
            {
                if (!collectAll && Equals(resource.UpdateId, UpdateId)) continue;
                _resources.Remove(resource);
                resource.Dispose(View, wasChartDisposed);
            }

            foreach (KeyValuePair<IEnumerable, EnumerableResource> resource in _enumerableResources.ToArray())
            {
                if (!collectAll && Equals(resource.Value.UpdateId, UpdateId)) continue;
                _enumerableResources.Remove(resource.Value.Collection);
                resource.Value.Dispose();
            }
        }

        private void InvalidatePropertyChanged(
            object sender, PropertyChangedEventArgs args)
        {
            if (!IsViewInitialized) return;
            Invalidate();
        }

        private void InvalidateOnCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs args)
        {
            Invalidate();
        }

        private void EvaluateTooltip(PointF pointerLocation)
        {
            var selectionMode = View.DataToolTip.SelectionMode;
            IChartPoint[] query = GetPointsAt(pointerLocation, selectionMode, View.DataToolTip.SnapToClosest).ToArray();

            IEnumerable<IChartPoint> notHoveredAnymore = _previousHovered?.Where(x =>
                                        !x.InteractionArea.Contains(pointerLocation, selectionMode))
                                    ?? Enumerable.Empty<IChartPoint>();

            foreach (var leftHoveredPoint in notHoveredAnymore)
            {
                leftHoveredPoint.Series.RemovePointHighlight(leftHoveredPoint, View);
            }

            if (!query.Any())
            {
                ToolTipTimeoutTimer.Start();
                _previousHovered = null;
                return;
            }

            ToolTipTimeoutTimer.Stop();
            ShowTooltip(query);

            _previousHovered = query;
        }

        private void EvaluateEnterLeftPoints(PointF pointerLocation, EventArgs args)
        {
            IChartPoint[] q = GetPointsAt(pointerLocation, ToolTipSelectionMode.SharedXy, false).ToArray();

            IChartPoint[] leftPoints = _previousEntered
                .Where(x => !x.InteractionArea.Contains(pointerLocation, ToolTipSelectionMode.SharedXy)).ToArray();
            if (leftPoints.Any())
            {
                OnDataPointerLeft(leftPoints, args);
            }

            if (!q.Any())
            {
                _previousEntered = new HashSet<IChartPoint>(Enumerable.Empty<IChartPoint>());
                return;
            }

            IChartPoint[] enteredPoints = q.Where(x => !_previousEntered.Contains(x)).ToArray();

            if (enteredPoints.Any())
            {
                OnDataPointerEnter(enteredPoints, args);
                _previousEntered = new HashSet<IChartPoint>(enteredPoints);
            }
        }

        private void ShowTooltip(IChartPoint[] points)
        {
            var newToolTipLocation = CorrectTooltipLocationByPosition(
                View.DataToolTip.ShowAndMeasure(points, View),
                GetTooltipLocation(points));

            foreach (var chartPoint in points)
            {
                chartPoint.Series.OnPointHighlight(chartPoint, View);
            }

            if (_previousTooltipLocation != newToolTipLocation)
            {
                View.DataToolTip.Move(newToolTipLocation, View);
            }

            _previousTooltipLocation = newToolTipLocation;
        }

        private PointF CorrectTooltipLocationByPosition(SizeF tooltipSize, PointF toolTipLocation)
        {
            float xCorrection = 0f;
            float yCorrection = 0f;

            switch (View.DataToolTip.Position)
            {
                case ToolTipPosition.Top:
                    xCorrection = -tooltipSize.Width * .5f;
                    yCorrection = -tooltipSize.Height;
                    break;
                case ToolTipPosition.Bottom:
                    xCorrection = -tooltipSize.Width * .5f;
                    break;
                case ToolTipPosition.Left:
                    xCorrection = -tooltipSize.Width;
                    yCorrection = -tooltipSize.Height * .5f;
                    break;
                case ToolTipPosition.Right:
                    yCorrection = -tooltipSize.Height * .5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new PointF(toolTipLocation.X + xCorrection, toolTipLocation.Y + yCorrection);
        }

        private void OnToolTipTimeoutTimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            View.InvokeOnUiThread(() => { View.DataToolTip.Hide(View); });
            ToolTipTimeoutTimer.Stop();
        }

        private void OnViewOnChartViewResized(IChartView sender)
        {
            Invalidate();
        }

        private void OnContentOnContentLoaded(IChartView sender)
        {
            IsViewInitialized = true;
            Invalidate();
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            IsDisposing = true;

            foreach (var resourceCollection in _enumerableResources.Values)
            {
                // ReSharper disable once IdentifierTypo
                if (resourceCollection.Collection is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged -= InvalidateOnCollectionChanged;
                }
            }

            foreach (var resource in _resources)
            {
                resource.Dispose(View, true);
            }

            _resources.Clear();
            _enumerableResources.Clear();

            Series = new ISeries[0];
            Dimensions = new Plane[0][];
            Legend = null;

            _previousHovered = null;

            View.Canvas.ContentLoaded -= OnContentOnContentLoaded;
            View.ViewResized -= OnViewOnChartViewResized;
            View.Canvas.PointerMoved -= ViewOnPointerMoved;
            View.Canvas.PointerDown -= ViewOnPointerDown;
            ToolTipTimeoutTimer.Elapsed -= OnToolTipTimeoutTimerOnElapsed;

            View.Canvas.Dispose();
        }

        /// <summary>
        /// Called when [update started].
        /// </summary>
        protected void OnUpdateStarted()
        {
            UpdatePreview?.Invoke(View);
            if (UpdatePreviewCommand != null && UpdatePreviewCommand.CanExecute(View))
            {
                UpdatePreviewCommand.Execute(View);
            }
        }

        /// <summary>
        /// Called when [update finished].
        /// </summary>
        protected void OnUpdateFinished()
        {
            Updated?.Invoke(View);
            if (UpdatedCommand != null && UpdatedCommand.CanExecute(View))
            {
                UpdatedCommand.Execute(View);
            }
        }

        /// <summary>
        /// Called when [data pointer enter].
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="args">The event args.</param>
        protected virtual void OnDataPointerEnter(IChartPoint[] points, EventArgs args)
        {
            DataPointerEntered?.Invoke(View, points, args);
            if (DataPointerEnteredCommand != null && DataPointerEnteredCommand.CanExecute(points))
            {
                DataPointerEnteredCommand.Execute(points);
            }
        }

        /// <summary>
        /// Called when [data pointer leave].
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="args">The event args.</param>
        protected virtual void OnDataPointerLeft(IChartPoint[] points, EventArgs args)
        {
            DataPointerLeft?.Invoke(View, points, args);
            if (DataPointerLeftCommand != null && DataPointerLeftCommand.CanExecute(points))
            {
                DataPointerLeftCommand.Execute(points);
            }
        }

        /// <summary>
        /// Called when [data pointer down].
        /// </summary>
        /// <param name="points">The points.</param>
        /// /// <param name="args">The event args.</param>
        protected virtual void OnDataPointerDown(IChartPoint[] points, EventArgs args)
        {
            DataPointerDown?.Invoke(View, points, args);
            if (DataPointerDownCommand != null && DataPointerDownCommand.CanExecute(points))
            {
                DataPointerDownCommand.Execute(points);
            }
        }
    }
}
