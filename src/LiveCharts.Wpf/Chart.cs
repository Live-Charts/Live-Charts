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
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.Interaction;
using DataInteractionHandler = LiveCharts.Core.Interaction.Events.DataInteractionHandler;
using Point = System.Windows.Point;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Defines a chart class.
    /// </summary>
    /// <seealso cref="Canvas" />
    /// <seealso cref="IChartView" />
    /// <seealso cref="IDesktopChart" />
    public abstract class Chart : Canvas, IChartView, IDesktopChart
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
            MouseMove += OnMouseMove;
            MouseLeftButtonUp += OnLeftButtonUp;
            MouseLeftButtonDown += OnLeftButtonDown;
            TooltipPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.RelativePoint
            };
            Children.Add(TooltipPopup);
            Children.Add(VisualDrawMargin);
        }

        /// <summary>
        /// Gets or sets the visual draw margin.
        /// </summary>
        /// <value>
        /// The visual draw margin.
        /// </value>
        protected ContentPresenter VisualDrawMargin { get; set; }

        /// <summary>
        /// Gets the tooltip popup.
        /// </summary>
        /// <value>
        /// The tooltip popup.
        /// </value>
        public Popup TooltipPopup { get; protected set; }

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
            new PropertyMetadata(TimeSpan.FromMilliseconds(550), RaiseOnPropertyChanged(nameof(AnimationsSpeed))));

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
        /// The hoverable property
        /// </summary>
        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            nameof(Hoverable), typeof(bool), typeof(Chart), new PropertyMetadata(true));

        /// <summary>
        /// The animation line property, default is bounce medium.
        /// </summary>
        public static readonly DependencyProperty AnimationLineProperty = DependencyProperty.Register(
            nameof(AnimationLine), typeof(IEnumerable<Core.Animations.Frame>), typeof(Chart),
            new PropertyMetadata(TimeLines.Ease));

        #endregion

        #region private and protected methods

        /// <summary>
        /// Gets the planes in the current chart.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual IList<IList<Plane>> GetOrderedDimensions()
        {
            throw new NotImplementedException();
        }

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
        /// Raises the <see cref="E:DataClick" /> event.
        /// </summary>
        /// <param name="args">The <see cref="DataInteractionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDataClick(DataInteractionEventArgs args)
        {
            DataClick?.Invoke(this, args.Points);
            if (DataClickCommand != null && DataClickCommand.CanExecute(args.Points))
            {
                DataClickCommand.Execute(args.Points);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DataDoubleClick" /> event.
        /// </summary>
        /// <param name="args">The <see cref="DataInteractionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDataDoubleClick(DataInteractionEventArgs args)
        {
            DataDoubleClick?.Invoke(this, args.Points);
            if (DataDoubleClickCommand != null && DataDoubleClickCommand.CanExecute(args.Points))
            {
                DataDoubleClickCommand.Execute(args.Points);
            }
        }

        private void OnLoaded(object sender, EventArgs eventArgs)
        {
            ChartViewLoaded?.Invoke(this);
        }

        private void OnSizeChanged(object o, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ChartViewResized?.Invoke(this);
        }

        private void OnMouseMove(object o, MouseEventArgs args)
        {
            if (DataToolTip == null) return;
            var p = args.GetPosition(this);
            var c = new Point(
                p.X - GetLeft(VisualDrawMargin),
                p.Y - GetTop(VisualDrawMargin));
            PointerMovedOverPlot?.Invoke(DataToolTip.SelectionMode, new PointF((float) c.X, (float) c.Y));
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            if (args.ClickCount != 2) return;
            var p = args.GetPosition(this);
            var c = new Point(
                p.X + GetLeft(VisualDrawMargin),
                p.Y + GetTop(VisualDrawMargin));
            var points = Model.GetHoveredPoints(new PointF((float) c.X, (float) c.Y));
            var e = new DataInteractionEventArgs(args.MouseDevice, args.Timestamp, args.ChangedButton, points)
            {
                RoutedEvent = MouseDownEvent
            };
            OnDataDoubleClick(e);
        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            var p = args.GetPosition(this);
            var c = new Point(
                p.X + GetLeft(VisualDrawMargin),
                p.Y + GetTop(VisualDrawMargin));
            var points = Model.GetHoveredPoints(new PointF((float) c.X, (float) c.Y));
            var e = new DataInteractionEventArgs(args.MouseDevice, args.Timestamp, args.ChangedButton, points)
            {
                RoutedEvent = MouseDownEvent,
                Handled = args.Handled
            };
            OnDataClick(e);
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

        private event PointerMovedHandler PointerMovedOverPlot;

        event PointerMovedHandler IChartView.PointerMoved
        {
            add => PointerMovedOverPlot += value;
            remove => PointerMovedOverPlot -= value;
        }

        /// <inheritdoc />
        public event ChartEventHandler UpdatePreview
        {
            add => Model.UpdatePreview += value;
            remove => Model.UpdatePreview -= value;
        }

        /// <inheritdoc cref="IChartView.UpdatePreview" />
        public ICommand UpdatePreviewCommand
        {
            get => Model.UpdatePreviewCommand;
            set => Model.UpdatePreviewCommand = value;
        }

        /// <inheritdoc />
        public event ChartEventHandler Updated
        {
            add => Model.Updated += value;
            remove => Model.Updated -= value;
        }

        /// <inheritdoc cref="IChartView.Updated" />
        public ICommand UpdatedCommand
        {
            get => Model.UpdatedCommand;
            set => Model.UpdatedCommand = value;
        }

        /// <inheritdoc cref="IChartView.Model"/>
        public ChartModel Model { get; protected set; }

        IChartContent IChartView.Content
        {
            get => (IChartContent) VisualDrawMargin.Content;
            set => VisualDrawMargin.Content = value;
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
        public IEnumerable<Core.Animations.Frame> AnimationLine
        {
            get => (IEnumerable<Core.Animations.Frame>) GetValue(AnimationLineProperty);
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

        /// <inheritdoc />
        public bool Hoverable
        {
            get => (bool) GetValue(HoverableProperty);
            set => SetValue(HoverableProperty, value);
        }

        /// <inheritdoc cref="IChartView.DataToolTip"/>
        public IDataToolTip DataToolTip
        {
            get => (IDataToolTip) GetValue(DataTooltipProperty);
            set => SetValue(DataTooltipProperty, value);
        }

        /// <inheritdoc />
        public void Update(bool restartAnimations = false)
        {
            Model.Invalidate(restartAnimations, true);
        }

        void IChartView.SetDrawArea(RectangleF drawArea)
        {
            SetTop(VisualDrawMargin, drawArea.Top);
            SetLeft(VisualDrawMargin, drawArea.Left);
            VisualDrawMargin.Width = drawArea.Width;
            VisualDrawMargin.Height = drawArea.Height;
        }

        void IChartView.InvokeOnUiThread(Action action)
        {
            Dispatcher.Invoke(action);
        }

        #endregion

        #region PlatformSpecific events 

        ///<summary>
        /// Occurs when the user clicks in a data point.
        /// </summary>
        public event DataInteractionHandler DataClick;

        public static readonly DependencyProperty DataClickCommandProperty = DependencyProperty.Register(
            nameof(DataClickCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand)));

        ///<summary>
        /// Occurs when the user clicks in a data point.
        /// </summary>
        public ICommand DataClickCommand
        {
            get => (ICommand) GetValue(DataClickCommandProperty);
            set => SetValue(DataClickCommandProperty, value);
        }

        ///<summary>
        /// Occurs when the user double clicks in a data point.
        /// </summary>
        public event DataInteractionHandler DataDoubleClick;

        public static readonly DependencyProperty DataDoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DataDoubleClickCommand), typeof(ICommand), typeof(Chart),
            new PropertyMetadata(default(ICommand)));

        ///<summary>
        /// Occurs when the user double clicks in a data point.
        /// </summary>
        public ICommand DataDoubleClickCommand
        {
            get => (ICommand) GetValue(DataDoubleClickCommandProperty);
            set => SetValue(DataDoubleClickCommandProperty, value);
        }

        #endregion

        #region IDesktopChart implementation

        /// <inheritdoc />
        public event DataInteractionHandler DataMouseEnter
        {
            add => Model.DataPointerEnter += value;
            remove => Model.DataPointerLeave -= value;
        }

        public ICommand DataMouseEnterCommand
        {
            get => Model.DataPointerEnterCommand;
            set => Model.DataPointerEnterCommand = value;
        }

        /// <inheritdoc />
        public event DataInteractionHandler DataMouseLeave
        {
            add => Model.DataPointerLeave += value;
            remove => Model.DataPointerLeave -= value;
        }

        public ICommand DataMouseLeaveCommand
        {
            get => Model.DataPointerLeaveCommand;
            set => Model.DataPointerLeaveCommand = value;
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

        public void Dispose()
        {
            Model.Dispose();
            Series = null;
            Children.Remove(TooltipPopup);
            Children.Remove(VisualDrawMargin);
            Loaded -= OnLoaded;
            VisualDrawMargin = null;
            DrawMargin = Core.Drawing.Margin.Empty;
            SetValue(LegendProperty, null);
            SetValue(DataTooltipProperty, null);
            Loaded -= OnLoaded;
            SizeChanged -= OnSizeChanged;
            MouseMove -= OnMouseMove;
            MouseLeftButtonUp -= OnLeftButtonUp;
            MouseLeftButtonDown -= OnLeftButtonDown;
            TooltipPopup = null;
        }
    }
}
