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
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Defines a chart.
    /// </summary>
    public abstract class ChartModel : IDisposable
    {
        private static int _colorCount;
        private Task _delayer;
        private IList<Color> _colors;
        private HashSet<IResource> _resources = new HashSet<IResource>();
        private Dictionary<IEnumerable, EnumerableResource> _enumerableResources = new Dictionary<IEnumerable, EnumerableResource>();
        private PointF _previousTooltipLocation = PointF.Empty;
        private IEnumerable<PackedPoint> _previousHovered;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        protected ChartModel(IChartView view)
        {
            View = view;
            View.Content = Charting.Current.UiProvider.GetChartContent();
            view.ChartViewLoaded += sender =>
            {
                IsViewInitialized = true;
                Invalidate(sender);
            };
            view.ChartViewResized += Invalidate;
            view.PointerMoved += ViewOnPointerMoved;
            view.PropertyChanged += InvalidatePropertyChanged;
            ToolTipTimeoutTimer = new Timer();
            ToolTipTimeoutTimer.Elapsed += (sender, args) =>
            {
                View.InvokeOnUiThread(() => { ToolTip.Hide(View); });
                ToolTipTimeoutTimer.Stop();
            };

            Charting.BuildFromSettings(view);
        }

        /// <summary>
        /// Gets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        public object UpdateId { get; private set; } = new object();

        /// <summary>
        /// Gets or sets the draw area location.
        /// </summary>
        /// <value>
        /// The draw area location.
        /// </value>
        public float[] DrawAreaLocation { get; set; }

        /// <summary>
        /// Gets or sets the size of the draw area.
        /// </summary>
        /// <value>
        /// The size of the draw area.
        /// </value>
        public float[] DrawAreaSize { get; set; }

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
            get => _colors ?? Charting.Current.Colors;
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
        public ICommand UpdatePreviewCommand { get; set; }

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        public event ChartEventHandler Updated;

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        public ICommand UpdatedCommand { get; set; }

        /// <summary>
        /// Occurs when [data pointer enter].
        /// </summary>
        public event DataInteractionHandler DataPointerEnter;

        /// <summary>
        /// Gets or sets the data pointer enter.
        /// </summary>
        /// <value>
        /// The data pointer enter.
        /// </value>
        public ICommand DataPointerEnterCommand { get; set; }

        /// <summary>
        /// Occurs when [data pointer leave].
        /// </summary>
        public event DataInteractionHandler DataPointerLeave;

        /// <summary>
        /// Gets or sets the data pointer leave command.
        /// </summary>
        /// <value>
        /// The data pointer leave command.
        /// </value>
        public ICommand DataPointerLeaveCommand { get; set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public ISeries[] Series { get; internal set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public Plane[][] Dimensions { get; internal set; }

        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        public float[] ControlSize { get; internal set; }

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
        /// Gets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        public IDataToolTip ToolTip { get; internal set; }

        /// <summary>
        /// Gets the legend.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        public ILegend Legend { get; internal set; }

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
        public async void Invalidate(object sender)
        {
            if (!IsViewInitialized)
            {
                return;
            }
            if (_delayer != null && !_delayer.IsCompleted) return;

            var delay = AnimationsSpeed.TotalMilliseconds < 10
                ? TimeSpan.FromMilliseconds(10)
                : AnimationsSpeed;
            _delayer = Task.Delay(delay);

            await _delayer;

            View.InvokeOnUiThread(() =>
            {
                CopyDataFromView();
                using (var context = new UpdateContext(Series.Where(series => series.IsVisible)))
                {
                    var dims = new float[Dimensions.Length][][];
                    for (var dimIndex = 0; dimIndex < Dimensions.Length; dimIndex++)
                    {
                        var dimension = Dimensions[dimIndex];
                        dims[dimIndex] = new float[dimension.Length][];
                        for (var planeIndex = 0; planeIndex < dimension.Length; planeIndex++)
                        {
                            dims[dimIndex][planeIndex] = new[] {float.MaxValue, float.MinValue};
                        }
                    }

                    context.Ranges = dims;

                    Update(false, context);
                }
            });
        }

        /// <summary>
        /// Scales to pixels a data value according to an axis range and a given area, if the area is not present, the chart draw margin size will be used.
        /// </summary>
        /// <param name="dataValue">The value.</param>
        /// <param name="plane">The axis.</param>
        /// <param name="sizeVector">The draw margin, this param is optional, if not set, the current chart's draw margin area will be used.</param>
        /// <returns></returns>
        public abstract float ScaleToUi(double dataValue, Plane plane, float[] sizeVector = null);

        /// <summary>
        /// Scales from pixels to a data value.
        /// </summary>
        /// <param name="pixelsValue">The value.</param>
        /// <param name="plane">The plane.</param>
        /// <param name="sizeVector">The size.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public abstract double ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null);

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
        /// <returns></returns>
        public IEnumerable<PackedPoint> GetHoveredPoints(PointF pointerLocation)
        {
            return Series.SelectMany(series => series.GetHoveredPoints(pointerLocation));
        }

        /// <summary>
        /// Called when the pointer moves over a chart and there is a tooltip in the view.
        /// </summary>
        /// <param name="selectionMode">The selection mode.</param>
        /// <param name="pointerLocation">The dimensions.</param>
        protected virtual void ViewOnPointerMoved(TooltipSelectionMode selectionMode, PointF pointerLocation)
        {
            if (Series == null) return;
            var query = GetHoveredPoints(pointerLocation).ToArray();

            if (selectionMode == TooltipSelectionMode.Auto)
            {
                // ToDo: guess what the user meant here ...
            }

            ToolTip = View.DataToolTip;

            if (View.Hoverable)
            {
                foreach (var leftPoint in _previousHovered?
                                              .Where(x =>
                                                  !x.InteractionArea.Contains(pointerLocation))
                                          ?? Enumerable.Empty<PackedPoint>())
                {
                    leftPoint.Series.ResetPointStyle(leftPoint);
                    OnDataPointerLeave(query);
                }

                _previousHovered = query;
            }

            if (!query.Any())
            {
                ToolTipTimeoutTimer.Start();
                _previousHovered = null;
                return;
            }

            ToolTipTimeoutTimer.Stop();

            View.DataToolTip.ShowAndMeasure(query, View);

            var newTooltipLocation = GetToolTipLocationAndFireHovering(query);

            if (_previousTooltipLocation != newTooltipLocation)
            {
                View.DataToolTip.Move(newTooltipLocation, View);
            }

            OnDataPointerEnter(query);

            _previousTooltipLocation = newTooltipLocation;
        }

        /// <summary>
        /// Gets the tool tip location and fires hovering.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        protected abstract PointF GetToolTipLocationAndFireHovering(
            PackedPoint[] points);

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
                    resource.Dispose(View);
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

            var chartSize = ControlSize;
            float dax = 0f, day = 0f;
            float lw = 0f, lh = 0f;

            // draw and measure legend
            if (Legend != null && LegendPosition != LegendPosition.None)
            {
                RegisterINotifyPropertyChanged(Legend);

                var legendSize = Legend.Measure(Series, DefaultLegendOrientation, View);

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

            DrawAreaLocation = new[] {dax, day};
            DrawAreaSize = Perform.SubstractEach2D(chartSize, new[] {lw, lh});
        }

        /// <summary>
        /// Copies the data from the view o use it in the next update cycle.
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
            foreach (var dimension in View.Dimensions)
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

                    void DisposePropertyChanged(IChartView view, object instance)
                    {
                        inpc.PropertyChanged -= InvalidatePropertyChanged;
                        resource.Disposed -= DisposePropertyChanged;
                    }

                    resource.Disposed += DisposePropertyChanged;
                }
            }

            resource.UpdateId = UpdateId;
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

        internal void CollectResources(bool collectAll = false)
        {
            foreach (var resource in _resources.ToArray())
            {
                if (!collectAll && Equals(resource.UpdateId, UpdateId)) continue;
                _resources.Remove(resource);
                resource.Dispose(View);
            }

            foreach (var resource in _enumerableResources.ToArray())
            {
                if (!collectAll && Equals(resource.Value.UpdateId, UpdateId)) continue;
                _enumerableResources.Remove(resource.Value.Collection);
                resource.Value.Dispose();
            }
        }

        private void InvalidatePropertyChanged(
            object sender, PropertyChangedEventArgs args)
        {
            if (sender is ICartesianChartView && args.PropertyName == nameof(ICartesianChartView.InvertAxes))
            {
                //invert orientation
                foreach (var dimension in View.Dimensions)
                {
                    foreach (var plane in dimension)
                    {
                        if (!(plane is Axis axis)) continue;
                        switch (axis.ActualPosition)
                        {
                            case AxisPosition.Auto:
                                break;
                            case AxisPosition.Top:
                                axis.ActualPosition = AxisPosition.Right;
                                break;
                            case AxisPosition.Left:
                                axis.ActualPosition = AxisPosition.Bottom;
                                break;
                            case AxisPosition.Right:
                                axis.ActualPosition = AxisPosition.Top;
                                break;
                            case AxisPosition.Bottom:
                                axis.ActualPosition = AxisPosition.Left;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            if (!IsViewInitialized) return;
            Invalidate(View);
        }

        private void InvalidateOnCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs args)
        {
            Invalidate(View);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            foreach (var resource in _resources)
            {
                resource.Dispose(View);
            }

            foreach (var resourceCollection in _enumerableResources.Values)
            {
                // ReSharper disable once IdentifierTypo
                if (resourceCollection is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged -= InvalidateOnCollectionChanged;
                }
            }
;
            UpdateId = null;

            _resources = null;
            _enumerableResources = null;
            _colors = null;
            _delayer = null;

            Series = null;
            Dimensions = null;
            ControlSize = null;
            Legend = null;

            _previousHovered = null;
        }

        /// <summary>
        /// Called when [update started].
        /// </summary>
        protected void OnUpdateStarted()
        {
            UpdatePreview?.Invoke(View);
            if (UpdatePreviewCommand != null && UpdatePreviewCommand.CanExecute(View))
            {
                UpdatePreviewCommand.Execute(null);
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
                UpdatedCommand.Execute(null);
            }
        }

        /// <summary>
        /// Called when [data pointer enter].
        /// </summary>
        /// <param name="points">The points.</param>
        protected virtual void OnDataPointerEnter(IEnumerable<PackedPoint> points)
        {
            DataPointerEnter?.Invoke(this, points);
            if (DataPointerEnterCommand != null && DataPointerEnterCommand.CanExecute(points))
            {
                DataPointerEnterCommand.Execute(points);
            }
        }

        /// <summary>
        /// Called when [data pointer leave].
        /// </summary>
        /// <param name="points">The points.</param>
        protected virtual void OnDataPointerLeave(IEnumerable<PackedPoint> points)
        {
            DataPointerLeave?.Invoke(this, points);
            if (DataPointerLeaveCommand != null && DataPointerLeaveCommand.CanExecute(points))
            {
                DataPointerLeaveCommand.Execute(points);
            }
        }
    }
}
