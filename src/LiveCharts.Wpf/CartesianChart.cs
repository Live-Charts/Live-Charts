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
using System.Windows.Input;
using LiveCharts.Core;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Wpf.Controls;
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
            MouseWheel += OnCartesianChartMouseWheelMoved;
            MouseDown += OnCartesianChartMouseDown;
            MouseMove += OnCartesianChartMouseMove;
            MouseUp += OnCartesianChartMouseUp;
            SetValue(SeriesProperty, new ChartingCollection<ISeries>());
            SetValue(XAxisProperty, new ChartingCollection<Plane> { new Axis() });
            SetValue(YAxisProperty, new ChartingCollection<Plane> { new Axis() });
            SetValue(WeightPlaneProperty, new ChartingCollection<Plane> { new Plane() });
            Charting.BuildFromTheme(this);
            InitializeScrolling();
        }

        #region ScrollBar behavior

        private void InitializeScrolling()
        {
            XFrom = new HorizontalDraggable();
            XTo = new HorizontalDraggable();
            XThumb = new HorizontalDraggableThumb();
            YFrom = new VerticalDraggable();
            YTo = new VerticalDraggable();
            YThumb = new VerticalDraggableThumb();

            XFrom.Dragging += args =>
            {
                if (args.Point.X >= XTo.Left) return;

                if (args.Point.X < Content.DrawArea.X)
                {
                    args.Point = new Point(Content.DrawArea.X, args.Point.Y);
                }
                if (args.Point.X > Content.DrawArea.Width + Content.DrawArea.X)
                {
                    args.Point = new Point(Content.DrawArea.Width + Content.DrawArea.X, args.Point.Y);
                }

                for (var index = 0; index < ScrollsX.Count; index++)
                {
                    ScrollsX[index].MinValue = Model.ScaleFromUi((float)args.Point.X - Content.DrawArea.X, XAxis[index]);
                }

                XFrom.Left = args.Point.X;
                XThumb.Left = args.Point.X;
                XThumb.Width = XTo.Left - XFrom.Left;
            };

            XTo.Dragging += args =>
            {
                if (XFrom.Left + XFrom.ActualWidth >= args.Point.X) return;

                if (args.Point.X < Content.DrawArea.X)
                {
                    args.Point = new Point(Content.DrawArea.X, args.Point.Y);
                }
                if (args.Point.X > Content.DrawArea.Width + Content.DrawArea.X)
                {
                    args.Point = new Point(Content.DrawArea.Width + Content.DrawArea.X, args.Point.Y);
                }

                for (var index = 0; index < ScrollsX.Count; index++)
                {
                    ScrollsX[index].MaxValue = Model.ScaleFromUi((float) args.Point.X, XAxis[index]);
                }

                XTo.Left = args.Point.X;
                XThumb.Width = XTo.Left - XFrom.Left;
            };

            XThumb.Dragging += args =>
            {
                var i = args.Point.X - XThumb.StartLeftOffset;
                var j = i + XThumb.Width;
                if (i <= Content.DrawArea.X) return;
                if (j >= Content.DrawArea.Width) return;

                for (var index = 0; index < ScrollsX.Count; index++)
                {
                    ScrollsX[index].MinValue = Model.ScaleFromUi((float) args.Point.X, XAxis[index]);
                    ScrollsX[index].MaxValue = Model.ScaleFromUi((float) args.Point.X, XAxis[index]) +
                                               ScrollsX[index].ActualRange;
                }
                XThumb.Left = i;
                XFrom.Left = i;
                XTo.Left = i + XThumb.Width;
            };
        }

        private void AttachUpdateFromSourceHandlers()
        {
            if (ScrollsX != null)
            {
                if (ScrollsX.Count > 1)
                {
                    throw new LiveChartsException("The source to scroll could only contain 1 plane.", 950);
                }

                OnScrollsXRangeSolved(ScrollsX[0]);
                ScrollsX[0].RangeSolved += OnScrollsXRangeSolved;
            }

            if (ScrollsY != null)
            {
                if (ScrollsY.Count > 1)
                {
                    throw new LiveChartsException("The source to scroll could only contain 1 plane.", 950);
                }

                OnScrollsYRangeSolved(ScrollsY[0]);
                ScrollsY[0].RangeSolved += OnScrollsYRangeSolved;
            }
        }

        private void UpdateScrollingView()
        {
            if (ScrollsX == null && ScrollsY == null) return;

            if (ScrollsX != null)
            {
                if (XFrom.Parent == null)
                {
                    Children.Add(XThumb);
                    Children.Add(XFrom);
                    Children.Add(XTo);
                }
            }
            else
            {
                if (XFrom.Parent != null)
                {
                    Children.Remove(XFrom);
                    Children.Remove(XTo);
                    Children.Remove(XThumb);
                }
            }

            if (ScrollsY != null)
            {
                if (YFrom.Parent == null)
                {
                    Children.Add(YThumb);
                    Children.Add(YFrom);
                    Children.Add(YTo);
                }
            }
            else
            {
                if (YFrom.Parent != null)
                {
                    Children.Remove(YFrom);
                    Children.Remove(YTo);
                    Children.Remove(YThumb);
                }
            }

            SetTop(XFrom, Content.DrawArea.Y + Content.DrawArea.Height * .5 - XFrom.ActualHeight * .5);
            SetTop(XTo, Content.DrawArea.Y + Content.DrawArea.Height * .5 - XTo.ActualHeight * .5);
            XThumb.Height = ActualHeight;

            SetLeft(YFrom, Content.DrawArea.X + Content.DrawArea.Width * .5 - YFrom.ActualWidth * .5);
            SetLeft(YTo, Content.DrawArea.X + Content.DrawArea.Width * .5 - YTo.ActualWidth * .5);

            AttachUpdateFromSourceHandlers();
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

        /// <summary>
        /// Called when [cartesian chart mouse wheel moved].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCartesianChartMouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            if (Zooming == Zooming.None) return;

            var pivot = e.GetPosition(VisualDrawMargin);

            e.Handled = true;

            var cartesianModel = (CartesianChartModel)Model;

            if (e.Delta > 0)
            {
                cartesianModel.ZoomIn(new PointF((float)pivot.X, (float)pivot.Y));
            }
            else
            {
                cartesianModel.ZoomOut(new PointF((float)pivot.X, (float)pivot.Y));
            }
        }

        private bool _isDragging;
        private Point _previous;

        /// <summary>
        /// Called when [cartesian chart mouse down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCartesianChartMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Panning == Panning.None) return;
            _previous = e.GetPosition(VisualDrawMargin);
            _isDragging = true;
            CaptureMouse();
        }

        /// <summary>
        /// Called when [cartesian chart mouse move].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCartesianChartMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var cartesianModel = (CartesianChartModel)Model;

            var current = e.GetPosition(VisualDrawMargin);

            cartesianModel.Drag(
                new PointF(
                    (float)(_previous.X - current.X),
                    (float)(_previous.Y - current.Y)
                ));

            _previous = current;
        }

        /// <summary>
        /// Called when [cartesian chart mouse up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCartesianChartMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Panning == Panning.None) return;
            _isDragging = false;
            ReleaseMouseCapture();
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
                ScrollsX[0].RangeSolved -= OnScrollsXRangeSolved;
                ScrollsX = null;
            }

            if (ScrollsY != null)
            {
                ScrollsY[0].RangeSolved -= OnScrollsYRangeSolved;
                ScrollsY = null;
            }
        }

        private void OnScrollsYRangeSolved(Plane sender)
        {
            throw new NotImplementedException();
        }

        private void OnScrollsXRangeSolved(Plane sender)
        {
            if (XFrom.IsDragging || XTo.IsDragging || XThumb.IsDragging) return;

            var i = Model.ScaleToUi(ScrollsX[0].ActualMinValue, XAxis[0]);
            var j = Model.ScaleToUi(ScrollsX[0].ActualMaxValue, XAxis[0]);
            XFrom.Left = i;
            XTo.Left = j;
            XThumb.Left = i;
            XThumb.Width = Math.Abs(j - i);
        }
    }
}
