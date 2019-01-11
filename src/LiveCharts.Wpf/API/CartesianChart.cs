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

using System.Collections.Generic;
using System.Windows;
using LiveCharts.Charts;
using LiveCharts.Collections;
using LiveCharts.DataSeries;
using LiveCharts.Dimensions;
using LiveCharts.Interaction;
using Point = System.Windows.Point;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Cartesian chart class supports X,Y based plots.
    /// </summary>
    /// <seealso cref="ICartesianChartView" />
    /// <seealso cref="Chart" />
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
            Global.Settings.BuildFromTheme(this);
        }

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
                Canvas.DrawArea.X + Model.ScaleToUi(point.X, XAxis[xPlaneIndex]),
                Canvas.DrawArea.Y + Model.ScaleToUi(point.Y, YAxis[yPlaneIndex]));
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
            var corrected = new Point(point.X - Canvas.DrawArea.X, point.Y - Canvas.DrawArea.Y);
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
    }
}
