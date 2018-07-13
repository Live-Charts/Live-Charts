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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Core;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Wpf.Controls;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Cartesian chart class supports X,Y based plots.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Charts.ICartesianChartView" />
    /// <seealso cref="LiveCharts.Wpf.Chart" />
    /// <seealso cref="ICartesianChartView" />
    public class CartesianChart : Chart, ICartesianChartView
    {
        /// <summary>
        /// Initializes the <see cref="CartesianChart"/> class.
        /// </summary>
        static CartesianChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CartesianChart),
                new FrameworkPropertyMetadata(typeof(CartesianChart)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianChart"/> class.
        /// </summary>
        public CartesianChart()
        {
            Model = new CartesianChartModel(this);
            SetValue(SeriesProperty, new ChartingCollection<ISeries>());
            SetValue(XAxisProperty, new ChartingCollection<Plane> { new Axis() });
            SetValue(YAxisProperty, new ChartingCollection<Plane> { new Axis() });
            SetValue(WeightPlaneProperty, new ChartingCollection<Plane> { new Plane() });
            Charting.BuildFromTheme(this);
            InitializeScrolling();
        }

        #region ScrollBar behavior

        private Canvas _scrollingCanvas;
        private Popup _scrollingPopup;
        private Window _window;

        private void InitializeScrolling()
        {
            XFrom = new HorizontalDraggable();
            XTo = new HorizontalDraggable();
            XThumb = new HorizontalDraggableThumb();
            YFrom = new VerticalDraggable();
            YTo = new VerticalDraggable();
            YThumb = new VerticalDraggableThumb();

            XFrom.Dragging += OnXFromOnDragging;
            XTo.Dragging += OnXToOnDragging;
            XThumb.Dragging += OnXThumbOnDragging;
            YFrom.Dragging += OnYFromOnDragging;
            YTo.Dragging += OnYToOnDragging;
            YThumb.Dragging += OnYThumbOnDragging;
        }

        private void OnXThumbOnDragging(DraggableArgs args)
        {
            double i = args.Point.X - XThumb.StartLeftOffset;
            double j = i + XThumb.ActualWidth;
            if (i <= Content.DrawArea.X) return;
            if (j >= Content.DrawArea.Width) return;

            for (int index = 0; index < ScrollsX.Count; index++)
            {
                double xi = Model.ScaleFromUi((float) i, XAxis[index]);
                ScrollsX[index].SetRange(xi, xi + ScrollsX[index].ActualRange);
            }

            XThumb.Left = i;
            XFrom.Left = i;
            XTo.Left = i + XThumb.Width;
        }

        private void OnYToOnDragging(DraggableArgs args)
        {
            if (args.Point.Y >= YFrom.Top) return;

            EnsureYIsInRange(args);

            for (int index = 0; index < ScrollsY.Count; index++)
            {
                ScrollsY[index].SetRange(ScrollsY[index].ActualMinValue, Model.ScaleFromUi((float) args.Point.Y, YAxis[index]));
            }

            YTo.Top = args.Point.Y;
            YThumb.Top = args.Point.Y;
            YThumb.Height = YFrom.Top - YTo.Top;
        }

        private void OnYThumbOnDragging(DraggableArgs args)
        {
            double i = args.Point.Y - YThumb.StartTopOffset;
            double j = i + YThumb.ActualHeight;
            if (i < Content.DrawArea.Y) return;
            if (j > Content.DrawArea.Y + Content.DrawArea.Height) return;

            for (int index = 0; index < ScrollsY.Count; index++)
            {
                double yi = Model.ScaleFromUi((float) i, YAxis[index]);
                ScrollsY[index].SetRange(yi - ScrollsY[index].ActualRange, yi);
            }

            YFrom.Top = i;
            YThumb.Top = i;
            YTo.Top = i + YThumb.Height;
        }

        private void OnYFromOnDragging(DraggableArgs args)
        {
            if (args.Point.Y <= YTo.Top + YFrom.ActualHeight) return;

            EnsureYIsInRange(args);

            for (int index = 0; index < ScrollsY.Count; index++)
            {
                ScrollsY[index].SetRange(Model.ScaleFromUi((float) args.Point.Y - Content.DrawArea.Y, YAxis[index]), ScrollsY[index].ActualMaxValue);
            }

            YFrom.Top = args.Point.Y;
            YThumb.Height = YFrom.Top - YTo.Top;
        }

        private void OnXToOnDragging(DraggableArgs args)
        {
            if (XFrom.Left + XFrom.ActualWidth >= args.Point.X) return;

            EnsureXIsInRange(args);

            for (int index = 0; index < ScrollsX.Count; index++)
            {
                ScrollsX[index].SetRange(ScrollsX[index].ActualMinValue, Model.ScaleFromUi((float) args.Point.X, XAxis[index]));
            }

            XTo.Left = args.Point.X;
            XThumb.Width = XTo.Left - XFrom.Left;
        }

        private void OnXFromOnDragging(DraggableArgs args)
        {
            if (args.Point.X >= XTo.Left) return;

            EnsureXIsInRange(args);

            for (int index = 0; index < ScrollsX.Count; index++)
            {
                ScrollsX[index].SetRange(Model.ScaleFromUi((float) args.Point.X - Content.DrawArea.X, XAxis[index]), ScrollsX[index].ActualMaxValue);
            }

            XFrom.Left = args.Point.X;
            XThumb.Left = args.Point.X;
            XThumb.Width = XTo.Left - XFrom.Left;
        }

        private void EnsureXIsInRange(DraggableArgs args)
        {
            if (args.Point.X < Content.DrawArea.X)
            {
                args.Point = new Point(Content.DrawArea.X, args.Point.Y);
            }

            if (args.Point.X > Content.DrawArea.Width + Content.DrawArea.X)
            {
                args.Point = new Point(Content.DrawArea.Width + Content.DrawArea.X, args.Point.Y);
            }
        }

        private void EnsureYIsInRange(DraggableArgs args)
        {
            if(args.Point.Y < Content.DrawArea.Y)
            {
                args.Point = new Point(args.Point.X, Content.DrawArea.Y);
            }

            if (args.Point.Y > Content.DrawArea.Height + Content.DrawArea.Y)
            {
                args.Point = new Point(args.Point.X, Content.DrawArea.Height + Content.DrawArea.Y);
            }
        }

        private void AttachUpdateFromSourceHandlers()
        {
            if (ScrollsX != null)
            {
                if (ScrollsX.Count > 1)
                {
                    throw new LiveChartsException(142);
                }

                XTo.UpdateLayout();
                XFrom.UpdateLayout();
                UpdateScrollXFromDataSourceChange(ScrollsX[0], 0, 0);
                ScrollsX[0].RangeChanged += UpdateScrollXFromDataSourceChange;
            }

            if (ScrollsY != null)
            {
                if (ScrollsY.Count > 1)
                {
                    throw new LiveChartsException(142);
                }

                YTo.UpdateLayout();
                YFrom.UpdateLayout();
                UpdateScrollYFromDataSourceChange(ScrollsY[0], 0, 0);
                ScrollsY[0].RangeChanged += UpdateScrollYFromDataSourceChange;
            }
        }

        private void UpdateScrollingView()
        {
            if (ScrollsX == null && ScrollsY == null) return;

            if (_scrollingPopup == null)
            {
                _scrollingPopup = new Popup
                {
                    IsOpen = false,
                    AllowsTransparency = true,
                    Placement = PlacementMode.AbsolutePoint
                };
                Children.Add(_scrollingPopup);
                _scrollingPopup.IsOpen = true;
                _window = Window.GetWindow(this);
                _window.LocationChanged += WindowOnLocationChanged;
            }

            _scrollingPopup.Width = ActualWidth;
            _scrollingPopup.Height = ActualHeight;

            UpdateScrollBarPopup();

            if (_scrollingCanvas == null)
            {
                _scrollingCanvas = new Canvas();
                _scrollingPopup.Child = _scrollingCanvas;
            }

            if (ScrollsX != null)
            {
                if (XFrom.Parent == null)
                {
                    _scrollingCanvas.Children.Add(XThumb);
                    _scrollingCanvas.Children.Add(XFrom);
                    _scrollingCanvas.Children.Add(XTo);
                }
            }
            else
            {
                if (XFrom.Parent != null)
                {
                    _scrollingCanvas.Children.Remove(XFrom);
                    _scrollingCanvas.Children.Remove(XTo);
                    _scrollingCanvas.Children.Remove(XThumb);
                }
            }

            if (ScrollsY != null)
            {
                if (YFrom.Parent == null)
                {
                    _scrollingCanvas.Children.Add(YThumb);
                    _scrollingCanvas.Children.Add(YFrom);
                    _scrollingCanvas.Children.Add(YTo);
                }
            }
            else
            {
                if (YFrom.Parent != null)
                {
                    _scrollingCanvas.Children.Remove(YFrom);
                    _scrollingCanvas.Children.Remove(YTo);
                    _scrollingCanvas.Children.Remove(YThumb);
                }
            }

            _scrollingCanvas.Width = ActualWidth;
            _scrollingCanvas.Height = ActualHeight;

            SetTop(XFrom, Content.DrawArea.Y + Content.DrawArea.Height * .5 - XFrom.ActualHeight * .5);
            SetTop(XTo, Content.DrawArea.Y + Content.DrawArea.Height * .5 - XTo.ActualHeight * .5);
            XThumb.Height = ActualHeight;

            SetLeft(YFrom, Content.DrawArea.X + Content.DrawArea.Width * .5 - YFrom.ActualWidth * .5);
            SetLeft(YTo, Content.DrawArea.X + Content.DrawArea.Width * .5 - YTo.ActualWidth * .5);
            YThumb.Width = ActualWidth;

            AttachUpdateFromSourceHandlers();
        }

        private void WindowOnLocationChanged(object sender, EventArgs eventArgs)
        {
            _scrollingPopup.IsOpen = true;
            UpdateScrollBarPopup();
        }

        private void UpdateScrollBarPopup()
        {
            var p = PointToScreen(new Point(0, 0));
            _scrollingPopup.HorizontalOffset = p.X;
            _scrollingPopup.VerticalOffset = p.Y;
        }

        private void UpdateScrollYFromDataSourceChange(Plane sender, double min, double max)
        {
            if (YFrom.IsDragging || YTo.IsDragging || YThumb.IsDragging) return;

            float i = Model.ScaleToUi(ScrollsY[0].ActualMinValue, YAxis[0]);
            float j = Model.ScaleToUi(ScrollsY[0].ActualMaxValue, YAxis[0]);

            int tolerance = 10;

            if (j < Content.DrawArea.Y) j = Content.DrawArea.Y;
            if (i < Content.DrawArea.Y) i = Content.DrawArea.Y + tolerance;
            if (i > Content.DrawArea.Y + Content.DrawArea.Height) i = Content.DrawArea.Y + Content.DrawArea.Height + tolerance;
            if (j > Content.DrawArea.Y + Content.DrawArea.Height) j = Content.DrawArea.Y + Content.DrawArea.Height;

            // ToDo: Animate???

            YFrom.Top = i;
            YTo.Top = j;
            YThumb.Top = j;
            YThumb.Height = Math.Abs(i - j);
        }

        private void UpdateScrollXFromDataSourceChange(Plane sender, double min, double max)
        {
            if (XFrom.IsDragging || XTo.IsDragging || XThumb.IsDragging) return;

            float i = Model.ScaleToUi(ScrollsX[0].ActualMinValue, XAxis[0]);
            float j = Model.ScaleToUi(ScrollsX[0].ActualMaxValue, XAxis[0]);

            int tolerance = 10;

            if (i < Content.DrawArea.X) i = Content.DrawArea.X;
            if (j < Content.DrawArea.X) j = Content.DrawArea.X + tolerance;
            if (i > Content.DrawArea.X + Content.DrawArea.Width) i = Content.DrawArea.X + Content.DrawArea.Width;
            if (j > Content.DrawArea.X + Content.DrawArea.Width) j = Content.DrawArea.X + Content.DrawArea.Width + tolerance;

            XFrom.Left = i;
            XTo.Left = j;
            XThumb.Left = i;
            XThumb.Width = Math.Abs(j - i);
        }

        private Draggable XFrom { get; set; }

        private Draggable XTo { get; set; }

        private Draggable XThumb { get; set; }

        private Draggable YFrom { get; set; }

        private Draggable YTo { get; set; }

        private Draggable YThumb { get; set; }

        #endregion

        #region Dependency properties

        /// <summary>
        /// The x axis property.
        /// </summary>
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(
            nameof(XAxis), typeof(IList<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(IChartView.Dimensions))));

        /// <summary>
        /// The y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(
            nameof(YAxis), typeof(IList<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(IChartView.Dimensions))));

        /// <summary>
        /// The weight plane property
        /// </summary>
        public static readonly DependencyProperty WeightPlaneProperty = DependencyProperty.Register(
            nameof(WeightPlane), typeof(IList<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, RaiseOnPropertyChanged(nameof(IChartView.Dimensions))));

        /// <summary>
        /// The invert axes property
        /// </summary>
        public static readonly DependencyProperty InvertAxesProperty = DependencyProperty.Register(
            nameof(InvertAxes), typeof(bool), typeof(CartesianChart),
            new PropertyMetadata(false, RaiseOnPropertyChanged(nameof(InvertAxes))));

        /// <summary>
        /// The zooming property
        /// </summary>
        public static readonly DependencyProperty ZoomingProperty = DependencyProperty.Register(
            nameof(Zooming), typeof(Zooming), typeof(CartesianChart), new PropertyMetadata(Zooming.None));

        /// <summary>
        /// The zooming speed property
        /// </summary>
        public static readonly DependencyProperty ZoomingSpeedProperty = DependencyProperty.Register(
            nameof(ZoomingSpeed), typeof(double), typeof(CartesianChart), new PropertyMetadata(0.8d));

        /// <summary>
        /// The panning property
        /// </summary>
        public static readonly DependencyProperty PanningProperty = DependencyProperty.Register(
            nameof(Panning), typeof(Panning), typeof(CartesianChart), new PropertyMetadata(Panning.Unset));

        /// <summary>
        /// The scrolls x property
        /// </summary>
        public static readonly DependencyProperty ScrollsXProperty = DependencyProperty.Register(
            nameof(ScrollsX), typeof(IList<Plane>), typeof(CartesianChart), new PropertyMetadata(default(IList<Plane>)));

        /// <summary>
        /// The scrolls y property
        /// </summary>
        public static readonly DependencyProperty ScrollsYProperty = DependencyProperty.Register(
            nameof(ScrollsY), typeof(IList<Plane>), typeof(CartesianChart), new PropertyMetadata(default(IList<Plane>)));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x axis.
        /// </summary>
        /// <value>
        /// The x axis.
        /// </value>
        public IList<Plane> XAxis
        {
            get => (IList<Plane>)GetValue(XAxisProperty);
            set => SetValue(XAxisProperty, value);
        }

        /// <summary>
        /// Gets or sets the y axis.
        /// </summary>
        /// <value>
        /// The y axis.
        /// </value>
        public IList<Plane> YAxis
        {
            get => (IList<Plane>)GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        /// <summary>
        /// Gets or sets the weight plane.
        /// </summary>
        /// <value>
        /// The weight plane.
        /// </value>
        public IList<Plane> WeightPlane
        {
            get => (IList<Plane>)GetValue(WeightPlaneProperty);
            set => SetValue(WeightPlaneProperty, value);
        }

        /// <inheritdoc />
        public bool InvertAxes
        {
            get => (bool)GetValue(InvertAxesProperty);
            set => SetValue(InvertAxesProperty, value);
        }

        /// <inheritdoc />
        public Panning Panning
        {
            get => (Panning)GetValue(PanningProperty);
            set => SetValue(PanningProperty, value);
        }

        /// <inheritdoc />
        public Zooming Zooming
        {
            get => (Zooming)GetValue(ZoomingProperty);
            set => SetValue(ZoomingProperty, value);
        }

        /// <inheritdoc />
        public double ZoomingSpeed
        {
            get => (double)GetValue(ZoomingSpeedProperty);
            set => SetValue(ZoomingSpeedProperty, value);
        }

        /// <summary>
        /// Gets or sets the axes to scroll when the scroll bar of this chart is moved in the X direction.
        /// </summary>
        /// <value>
        /// The scrolls x.
        /// </value>
        public IList<Plane> ScrollsX
        {
            get => (IList<Plane>)GetValue(ScrollsXProperty);
            set => SetValue(ScrollsXProperty, value);
        }


        /// <summary>
        /// Gets or sets the axes to scroll when the scroll bar of this chart is moved in the Y direction.
        /// </summary>
        /// <value>
        /// The scrolls y.
        /// </value>
        public IList<Plane> ScrollsY
        {
            get => (IList<Plane>)GetValue(ScrollsYProperty);
            set => SetValue(ScrollsYProperty, value);
        }

        #endregion

        /// <summary>
        /// Scales a point in the target planes units to the chart UI coordinates.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="xPlaneIndex">Index of the x plane.</param>
        /// <param name="yPlaneIndex">Index of the y plane.</param>
        /// <returns></returns>
        public Point ScaleToUi(Point point, int xPlaneIndex = 0, int yPlaneIndex = 0)
        {
            return new Point(
                Content.DrawArea.X + Model.ScaleToUi(point.X, XAxis[xPlaneIndex]),
                Content.DrawArea.Y + Model.ScaleToUi(point.Y, YAxis[yPlaneIndex]));
        }

        /// <summary>
        /// Scales a point in the chart Ui coordinates to the target planes units.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="xPlaneIndex">Index of the x plane.</param>
        /// <param name="yPlaneIndex">Index of the y plane.</param>
        /// <returns></returns>
        public Point ScaleFromUi(Point point, int xPlaneIndex = 0, int yPlaneIndex = 0)
        {
            var corrected = new Point(point.X - Content.DrawArea.X, point.Y - Content.DrawArea.Y);
            return new Point(
                Model.ScaleFromUi((float)corrected.X, XAxis[xPlaneIndex]),
                Model.ScaleFromUi((float)corrected.Y, YAxis[yPlaneIndex]));
        }

        /// <inheritdoc cref="Chart.GetOrderedDimensions"/>
        protected override IList<IList<Plane>> GetOrderedDimensions()
        {
            return new List<IList<Plane>>
            {
                XAxis,
                YAxis,
                WeightPlane
            };
        }

        /// <inheritdoc />
        protected override void OnModelSet()
        {
            base.OnModelSet();
            ChartUpdated += chart =>
            {
                UpdateScrollingView();
            };
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            if (ScrollsX != null)
            {
                ScrollsX[0].RangeChanged -= UpdateScrollXFromDataSourceChange;
                ScrollsX = null;
            }

            if (ScrollsY != null)
            {
                ScrollsY[0].RangeChanged -= UpdateScrollYFromDataSourceChange;
                ScrollsY = null;
            }

            XFrom.Dragging -= OnXFromOnDragging;
            XTo.Dragging -= OnXToOnDragging;
            XThumb.Dragging -= OnXThumbOnDragging;
            YFrom.Dragging -= OnYFromOnDragging;
            YTo.Dragging -= OnYToOnDragging;
            YThumb.Dragging -= OnYThumbOnDragging;

            if (_window != null)
            {
                _window.LocationChanged -= WindowOnLocationChanged;
            }

            _window = null;
        }
    }
}
