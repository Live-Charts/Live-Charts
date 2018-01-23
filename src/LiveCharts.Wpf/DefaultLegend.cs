using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Styles;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    /// <inheritdoc cref="WrapPanel" />
    public class DefaultLegend : WrapPanel, ILegend
    {
        /// <summary>
        /// The automatic orientation property.
        /// </summary>
        public static readonly DependencyProperty AutomaticOrientationProperty = DependencyProperty.Register(
            nameof(AutomaticOrientation), typeof(bool), typeof(DefaultLegend), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a value indicating whether the orientation should be decided according to LiveCharts data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the orientation should be set by LiveCharts; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticOrientation
        {
            get => (bool) GetValue(AutomaticOrientationProperty);
            set => SetValue(AutomaticOrientationProperty, value);
        }

        /// <summary>
        /// The geometry size property.
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(DefaultLegend), new PropertyMetadata(default(double)));

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

        private Size BuildControl(IEnumerable<Series> seriesCollection, Orientation orientation)
        {
            Orientation = AutomaticOrientation
                ? orientation.AsWpfOrientation()
                : Orientation;

            MaxWidth = Width;
            MaxHeight = Width;

            Children.Clear();

            foreach (var series in seriesCollection)
            {
                var style = (SeriesStyle) series.Style;
                Children.Add(new StackPanel
                {
                    Children =
                    {
                        new Path
                        {
                            Width = GeometrySize,
                            Height = GeometrySize,
                            StrokeThickness = style.StrokeThickness,
                            Stroke = style.Stroke.AsSolidColorBrush(),
                            Fill = style.Stroke.AsSolidColorBrush(),
                            Stretch = Stretch.Fill,
                            Data = Geometry.Parse(style.Geometry.Data)
                        },
                        new TextBlock
                        {
                            Margin = new Thickness(4, 0, 4, 0),
                            Text = series.Title,
                            VerticalAlignment = VerticalAlignment.Center
                        }
                    }
                });
            }

            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size(DesiredSize.Width, DesiredSize.Height);
        }

        /// <inheritdoc />
        Size ILegend.Measure(IEnumerable<Series> seriesCollection, Orientation orientation)
        {
            return Dispatcher.Invoke(() => BuildControl(seriesCollection, orientation));
        }

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView view)
        {
            var wpfChart = (Chart) view;
            if (wpfChart.Children.Contains(this))
            {
                wpfChart.Children.Remove(this);
            }
        }
    }
}
