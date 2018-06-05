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
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Wpf.Controls;
using Orientation = LiveCharts.Core.Drawing.Styles.Orientation;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default legend class.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.WrapPanel" />
    /// <seealso cref="ILegend" />
    public class ChartLegend : ItemsControl, ILegend
    {
        /// <summary>
        /// Initializes the <see cref="ChartLegend"/> class.
        /// </summary>
        static ChartLegend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ChartLegend),
                new FrameworkPropertyMetadata(typeof(ChartLegend)));
        }

        #region Properties

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(ChartLegend), new PropertyMetadata(default(Orientation)));

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// The geometry size property.
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(ChartLegend), new PropertyMetadata(15d));

        /// <summary>
        /// Gets or sets the size of the geometry.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        public double GeometrySize
        {
            get => (double) GetValue(GeometrySizeProperty);
            set => SetValue(GeometrySizeProperty, value);
        }

        /// <summary>
        /// The actual orientation property
        /// </summary>
        public static readonly DependencyProperty ActualOrientationProperty = DependencyProperty.Register(
            nameof(ActualOrientation), typeof(System.Windows.Controls.Orientation), typeof(ChartLegend), 
            new PropertyMetadata(default(System.Windows.Controls.Orientation)));

        /// <summary>
        /// Gets the actual orientation.
        /// </summary>
        /// <value>
        /// The actual orientation.
        /// </value>
        public System.Windows.Controls.Orientation ActualOrientation =>
            (System.Windows.Controls.Orientation) GetValue(ActualOrientationProperty);

        /// <summary>
        /// The default group style property
        /// </summary>
        public static readonly DependencyProperty DefaultGroupStyleProperty = DependencyProperty.Register(
            nameof(DefaultGroupStyle), typeof(GroupStyle), typeof(ChartLegend),
            new PropertyMetadata(default(GroupStyle), OnDefaultGroupStyleChanged));

        /// <summary>
        /// Gets or sets the default group style.
        /// </summary>
        /// <value>
        /// The default group style.
        /// </value>
        public GroupStyle DefaultGroupStyle
        {
            get => (GroupStyle)GetValue(DefaultGroupStyleProperty);
            set => SetValue(DefaultGroupStyleProperty, value);
        }

        #endregion

        private static void OnDefaultGroupStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue == null) return;

            var tooltip = (ChartToolTip)sender;

            if (tooltip.GroupStyle.Count == 0)
            {
                tooltip.GroupStyle.Add((GroupStyle)args.NewValue);
            }
        }

        float[] ILegend.Measure(
            IEnumerable<ISeries> seriesCollection, Orientation orientation, IChartView chart)
        {
            // return new[] { 0f, 0f };

            if (Parent == null)
            {
                var wpfChart = (Chart) chart;
                wpfChart.Children.Add(this);
                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);
                HorizontalAlignment = HorizontalAlignment.Left;
                VerticalAlignment = VerticalAlignment.Top;
            }

            ItemsSource = seriesCollection;

            if (orientation == Orientation.Auto)
            {
                SetValue(ActualOrientationProperty,
                    orientation == Orientation.Horizontal
                        ? System.Windows.Controls.Orientation.Horizontal
                        : System.Windows.Controls.Orientation.Vertical);
            }
            else
            {
                SetValue(ActualOrientationProperty, orientation.AsWpf());
            }

            UpdateLayout();
            ForceUiToUpdate();

            return new[] {(float) DesiredSize.Width, (float) DesiredSize.Height};
        }

        void ILegend.Move(PointF location, IChartView chart)
        {
            Canvas.SetLeft(this, location.X);
            Canvas.SetTop(this, location.Y);
        }

        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView chart, bool force)
        {
            var content = chart.Content;
            content.DisposeChild(this, false);
            Disposed?.Invoke(chart, this);
        }

        private static void ForceUiToUpdate()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render,
                new DispatcherOperationCallback(o =>
                {
                    frame.Continue = false;
                    return null;
                }), null);
            Dispatcher.PushFrame(frame);
        }
    }
}
