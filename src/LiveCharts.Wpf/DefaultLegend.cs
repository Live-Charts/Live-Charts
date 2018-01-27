using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Point = LiveCharts.Core.Drawing.Point;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default legend class.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.WrapPanel" />
    /// <seealso cref="LiveCharts.Core.Abstractions.ILegend" />
    public class DefaultLegend : ItemsControl, ILegend
    {
        /// <summary>
        /// Initializes the <see cref="DefaultLegend"/> class.
        /// </summary>
        static DefaultLegend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultLegend),
                new FrameworkPropertyMetadata(typeof(DefaultLegend)));
        }

        #region Properties

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(DefaultLegend), new PropertyMetadata(default(Orientation)));

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
            nameof(GeometrySize), typeof(double), typeof(DefaultLegend), new PropertyMetadata(15d));

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
            nameof(ActualOrientation), typeof(System.Windows.Controls.Orientation), typeof(DefaultLegend), 
            new PropertyMetadata(default(System.Windows.Controls.Orientation)));

        /// <summary>
        /// Gets the actual orientation.
        /// </summary>
        /// <value>
        /// The actual orientation.
        /// </value>
        public System.Windows.Controls.Orientation ActualOrientation =>
            (System.Windows.Controls.Orientation) GetValue(ActualOrientationProperty);

        #endregion

        Size ILegend.Measure(
            IEnumerable<Series> seriesCollection, 
            Orientation orientation,
            IChartView chart)
        {
            return Dispatcher.Invoke(() =>
            {
                if (Parent == null)
                {
                    var wpfChart = (Chart) chart;
                    wpfChart.Children.Add(this);
                }
                ItemsSource = seriesCollection;
                if (Orientation == Orientation.Auto)
                {
                    SetValue(ActualOrientationProperty,
                        orientation == Orientation.Horizontal
                            ? System.Windows.Controls.Orientation.Horizontal
                            : System.Windows.Controls.Orientation.Vertical);
                }
                else
                {
                    SetValue(ActualOrientationProperty, Orientation.AsWpfOrientation());
                }
                UpdateLayout();
                return new Size(DesiredSize.Width, DesiredSize.Width);
            });
        }

        void ILegend.Move(Point location, IChartView chart)
        {
            Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this, location.X);
                Canvas.SetTop(this, location.Y);
            });
        }

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView view)
        {
            Dispatcher.Invoke(() =>
            {
                var wpfChart = (Chart) view;
                if (wpfChart.Children.Contains(this))
                {
                    wpfChart.Children.Remove(this);
                }
            });
        }
    }
}
