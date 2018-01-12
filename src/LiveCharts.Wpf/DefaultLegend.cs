using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Interaction logic for DefaultLegend.xaml
    /// </summary>
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

        public void RemoveLegend(IChartView chart)
        {
            var wpfChart = (Chart)chart;
            if (wpfChart.Children.Contains(this))
            {
                wpfChart.Children.Remove(this);
            }
        }

        /// <inheritdoc cref="ILegend.UpdateLayoutAsync"/>
        async Task<Size> ILegend.UpdateLayoutAsync(IEnumerable<ISeries> seriesCollection, Orientation orientation)
        {
            // we really don't care about the wpf cycle, we don't need bindings,
            // we could actually get a sync error if we use bindings in this control
            // our chart is updated every time UpdateLayoutAsync method is called
            // we should only update the legend when this method is called.

            // wait for the UI thread to calculate the new size of the control
            await Dispatcher.InvokeAsync(() =>
            {

                Orientation = AutomaticOrientation
                    ? orientation.AsWpfOrientation()
                    : Orientation;

                MaxWidth = Width;
                MaxHeight = Width;

                Children.Clear();

                foreach (var series in seriesCollection)
                {
                    var g = series.Geometry == Core.Drawing.Svg.Geometry.Empty
                        ? Core.LiveCharts.GetDefault(series.Key).Geometry
                        : series.Geometry;

                    Children.Add(new StackPanel
                    {
                        Children =
                        {
                            new Path
                            {
                                Width = GeometrySize,
                                Height = GeometrySize,
                                StrokeThickness = series.StrokeThickness,
                                Stroke = series.Stroke.AsSolidColorBrush(),
                                Fill = series.Stroke.AsSolidColorBrush(),
                                Stretch = Stretch.Fill,
                                Data = Geometry.Parse(g.Data)
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

                UpdateLayout();
            });

            // and once it is finished, we return the size of our legend with the new data.
            return new Size(ActualWidth, ActualHeight);
        }
    }
}
