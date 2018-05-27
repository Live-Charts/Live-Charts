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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Wpf.Events;
using Point = System.Windows.Point;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Defines a chart class.
    /// </summary>
    /// <seealso cref="Canvas" />
    /// <seealso cref="IChartView" />
    public abstract class Chart : Canvas, IChartView, IWpfChart
    {
        /// <summary>
        /// Initializes the <see cref="Chart"/> class.
        /// </summary>
        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Chart),
                new FrameworkPropertyMetadata(typeof(Chart)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        protected Chart()
        {
            VisualDrawMargin = new ContentPresenter();
            DrawMargin = Core.Drawing.Margin.Empty;
            SetValue(LegendProperty, new ChartLegend());
            SetValue(DataTooltipProperty, new ChartToolTip());
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            Unloaded += OnUnloaded;
            TooltipPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.RelativePoint
            };
            Children.Add(TooltipPopup);
            Children.Add(VisualDrawMargin);
        }

        /// <summary>
        /// Gets the tooltip popup.
        /// </summary>
        /// <value>
        /// The tooltip popup.
        /// </value>
        public Popup TooltipPopup { get; protected set; }

        public ContentPresenter VisualDrawMargin { get; set; }

        #region Dependency properties

        /// <summary>
        /// The draw margin property, default is LiveCharts.Core.Drawing.Margin.Empty which means that the library will calculate it.
        /// </summary>
        public static readonly DependencyProperty DrawMarginProperty = DependencyProperty.Register(
            nameof(DrawMargin), typeof(Margin), typeof(Chart), 
            new PropertyMetadata(Core.Drawing.Margin.Empty, RaiseOnPropertyChanged(nameof(DrawMargin))));

        /// <summary>
        /// The series property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series), typeof(IEnumerable<ISeries>), typeof(Chart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(Series))));

        /// <summary>
        /// The animations speed property, default value is 550 milliseconds.
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            nameof(AnimationsSpeed), typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(TimeSpan.FromMilliseconds(1000), RaiseOnPropertyChanged(nameof(AnimationsSpeed))));

        /// <summary>
        /// The tooltip timeout property, default is 150 ms.
        /// </summary>
        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            nameof(TooltipTimeOut), typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(TimeSpan.FromMilliseconds(150)));

        /// <summary>
        /// The legend property, default is DefaultLegend class.
        /// </summary>
        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(
            nameof(Legend), typeof(ILegend), typeof(Chart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(Legend))));

        /// <summary>
        /// The legend position property
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            nameof(LegendPosition), typeof(LegendPosition), typeof(Chart),
            new PropertyMetadata(LegendPosition.None, RaiseOnPropertyChanged(nameof(LegendPosition))));

        /// <summary>
        /// The updater state property
        /// </summary>
        public static readonly DependencyProperty UpdaterStateProperty = DependencyProperty.Register(
            nameof(UpdaterState), typeof(UpdaterStates), typeof(Chart),
            new PropertyMetadata(UpdaterStates.Running, RaiseOnPropertyChanged(nameof(UpdaterState))));

        /// <summary>
        /// The data tooltip property.
        /// </summary>
        public static readonly DependencyProperty DataTooltipProperty = DependencyProperty.Register(
            nameof(DataToolTip), typeof(IDataToolTip), typeof(Chart),
            new PropertyMetadata(null));

        /// <summary>
        /// The animation line property, default is bounce medium.
        /// </summary>
        public static readonly DependencyProperty AnimationLineProperty = DependencyProperty.Register(
            nameof(AnimationLine), typeof(IEnumerable<Core.Animations.KeyFrame>), typeof(Chart),
            new PropertyMetadata(TimeLines.Ease));

        /// <summary>
        /// The chart update preview command property
        /// </summary>
        public static readonly DependencyProperty ChartUpdatePreviewCommandProperty = DependencyProperty.Register(
            nameof(ChartUpdatePreviewCommand), typeof(ICommand), typeof(Chart), 
            new PropertyMetadata(default(ICommand), (o, args) =>
            {
                var chart = (Chart) o;
                chart.Model.UpdatePreviewCommand = chart.ChartUpdatePreviewCommand;
            }));

        /// <summary>
        /// The chart updated command property
        /// </summary>
        public static readonly DependencyProperty ChartUpdatedCommandProperty = DependencyProperty.Register(
            nameof(ChartUpdatedCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand), (o, args) =>
            {
                var chart = (Chart) o;
                chart.Model.UpdatedCommand = chart.ChartUpdatedCommand;
            }));

        /// <summary>
        /// The data pointer entered command property
        /// </summary>
        public static readonly DependencyProperty DataMouseEnteredCommandProperty = DependencyProperty.Register(
            nameof(DataMouseEnteredCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand), (o, args) =>
            {
                var chart = (Chart) o;
                chart.Model.DataPointerEnteredCommand = chart.DataMouseEnteredCommand;
            }));

        /// <summary>
        /// The data pointer left command property
        /// </summary>
        public static readonly DependencyProperty DataMouseLeftCommandProperty = DependencyProperty.Register(
            nameof(DataMouseLeftCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand), (o, args) =>
            {
                var chart = (Chart) o;
                chart.Model.DataPointerLeftCommand = chart.DataMouseLeftCommand;
            }));

        /// <summary>
        /// The data pointer down command property
        /// </summary>
        public static readonly DependencyProperty DataMouseDownCommandProperty = DependencyProperty.Register(
            nameof(DataMouseDownCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand), (o, args) =>
            {
                var chart = (Chart) o;
                chart.Model.DataPointerDownCommand = chart.DataMouseDownCommand;
            }));

        private ChartModel _model;

        #endregion

        #region private and protected methods

        /// <summary>
        /// Gets the planes in the current chart.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract IList<IList<Plane>> GetOrderedDimensions();

        /// <summary>
        /// Notifies that the specified property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected static PropertyChangedCallback RaiseOnPropertyChanged(string propertyName)
        {
            return (sender, eventArgs) =>
            {
                var chart = (Chart) sender;
                chart.OnPropertyChanged(propertyName);
            };
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLoaded(object sender, EventArgs eventArgs)
        {
            ChartViewLoaded?.Invoke(this);
        }

        /// <summary>
        /// Called when [unloaded].
        /// </summary>
        /// <param name="sender1">The sender1.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUnloaded(object sender1, RoutedEventArgs routedEventArgs)
        {
            Dispose();
        }

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="sizeChangedEventArgs">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSizeChanged(object o, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ChartViewResized?.Invoke(this);
        }

        /// <summary>
        /// Called when [model set].
        /// </summary>
        protected virtual void OnModelSet()
        {
            Model.DataPointerDown += (chart, points, args) =>
            {
                DataMouseDown?.Invoke(chart, points, (MouseButtonEventArgs) args);
            };
            Model.DataPointerEntered += (chart, points, args) =>
            {
                DataMouseEntered?.Invoke(chart, points, (MouseEventArgs) args);
            };
            Model.DataPointerLeft += (chart, points, args) =>
            {
                DataMouseLeft?.Invoke(chart, points, (MouseEventArgs) args);
            };
        }

        #endregion

        #region IChartView implementation

        private event ChartEventHandler ChartViewLoaded;

        event ChartEventHandler IChartView.ChartViewLoaded
        {
            add => ChartViewLoaded += value;
            remove => ChartViewLoaded -= value;
        }

        private event ChartEventHandler ChartViewResized;

        event ChartEventHandler IChartView.ChartViewResized
        {
            add => ChartViewResized += value;
            remove => ChartViewResized -= value;
        }

        /// <inheritdoc />
        public event ChartEventHandler ChartUpdatePreview
        {
            add => Model.UpdatePreview += value;
            remove => Model.UpdatePreview -= value;
        }

        /// <inheritdoc cref="IChartView.ChartUpdatePreview" />
        public ICommand ChartUpdatePreviewCommand
        {
            get => (ICommand) GetValue(ChartUpdatePreviewCommandProperty);
            set => SetValue(ChartUpdatePreviewCommandProperty, value);
        }

        /// <inheritdoc />
        public event ChartEventHandler ChartUpdated
        {
            add => Model.Updated += value;
            remove => Model.Updated -= value;
        }

        /// <inheritdoc cref="IChartView.ChartUpdated" />
        public ICommand ChartUpdatedCommand
        {
            get => (ICommand) GetValue(ChartUpdatedCommandProperty);
            set => SetValue(ChartUpdatedCommandProperty, value);
        }

        /// <inheritdoc cref="IChartView.Model"/>
        public ChartModel Model
        {
            get => _model;
            protected set
            {
                _model = value;
                OnModelSet();
            }
        }

        public IChartContent Content
        {
            get => (IChartContent) VisualDrawMargin.Content;
            set => 
                VisualDrawMargin.Content = value;
        }

        /// <inheritdoc cref="IChartView.ControlSize"/>
        float[] IChartView.ControlSize => new[] {(float) ActualWidth, (float) ActualHeight};

        /// <inheritdoc cref="IChartView.DrawMargin"/>
        public Margin DrawMargin
        {
            get => (Margin) GetValue(DrawMarginProperty);
            set => SetValue(DrawMarginProperty, value);
        }

        /// <inheritdoc />
        IList<IList<Plane>> IChartView.Dimensions => GetOrderedDimensions();

        /// <inheritdoc cref="IChartView.Series"/>
        public IEnumerable<ISeries> Series
        {
            get => (IEnumerable<ISeries>) GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <inheritdoc />
        public UpdaterStates UpdaterState
        {
            get => (UpdaterStates) GetValue(UpdaterStateProperty);
            set => SetValue(UpdaterStateProperty, value);
        }

        /// <inheritdoc cref="IChartView.AnimationsSpeed"/>
        public TimeSpan AnimationsSpeed
        {
            get => (TimeSpan) GetValue(AnimationsSpeedProperty);
            set => SetValue(AnimationsSpeedProperty, value);
        }

        /// <inheritdoc />
        public IEnumerable<Core.Animations.KeyFrame> AnimationLine
        {
            get => (IEnumerable<Core.Animations.KeyFrame>) GetValue(AnimationLineProperty);
            set => SetValue(AnimationLineProperty, value);
        }

        /// <inheritdoc />
        public TimeSpan TooltipTimeOut
        {
            get => (TimeSpan) GetValue(TooltipTimeoutProperty);
            set => SetValue(TooltipTimeoutProperty, value);
        }

        /// <inheritdoc cref="IChartView.Legend"/>
        public ILegend Legend
        {
            get => (ILegend) GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }

        /// <inheritdoc cref="IChartView.LegendPosition"/>
        public LegendPosition LegendPosition
        {
            get => (LegendPosition) GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        /// <inheritdoc cref="IChartView.DataToolTip"/>
        public IDataToolTip DataToolTip
        {
            get => (IDataToolTip) GetValue(DataTooltipProperty);
            set => SetValue(DataTooltipProperty, value);
        }

        /// <inheritdoc />
        public void ForceUpdate(bool restartAnimations = false)
        {
            Model.Invalidate(restartAnimations, true);
        }

        void IChartView.InvokeOnUiThread(Action action)
        {
            Dispatcher.Invoke(action);
        }

        #endregion

        #region IWPFChart implementation

        /// <inheritdoc />
        public event MouseDataInteractionHandler DataMouseEntered;

        /// <inheritdoc />
        public ICommand DataMouseEnteredCommand
        {
            get => (ICommand) GetValue(DataMouseEnteredCommandProperty);
            set => SetValue(DataMouseEnteredCommandProperty, value);
        }

        /// <inheritdoc />
        public event MouseDataInteractionHandler DataMouseLeft;

        /// <inheritdoc />
        public ICommand DataMouseLeftCommand
        {
            get => (ICommand) GetValue(DataMouseLeftCommandProperty);
            set => SetValue(DataMouseLeftCommandProperty, value);
        }

        /// <inheritdoc />
        public event MouseButtonDataInteractionHandler DataMouseDown;

        /// <inheritdoc />
        public ICommand DataMouseDownCommand
        {
            get => (ICommand) GetValue(DataMouseDownCommandProperty);
            set => SetValue(DataMouseDownCommandProperty, value);
        }

        #endregion

        #region INPC implementation

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public virtual void Dispose()
        {
            Model.Dispose();
            Series = null;
            Children.Remove(TooltipPopup);
            Children.Remove(VisualDrawMargin);
            Loaded -= OnLoaded;
            DrawMargin = Core.Drawing.Margin.Empty;
            SetValue(LegendProperty, null);
            SetValue(DataTooltipProperty, null);
            Loaded -= OnLoaded;
            SizeChanged -= OnSizeChanged;
            TooltipPopup = null;
            VisualDrawMargin = null;
            GC.Collect();
        }
    }
}
