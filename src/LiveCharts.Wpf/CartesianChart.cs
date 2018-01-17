using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LiveCharts.Core;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Series;

namespace LiveCharts.Wpf
{
    public class CartesianChart : Chart
    {
        static CartesianChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CartesianChart),
                new FrameworkPropertyMetadata(typeof(CartesianChart)));
        }

         public CartesianChart()
        {
            Model = new CartesianChartModel(this);
            SetValue(SeriesProperty, new ObservableCollection<Series>());
            SetValue(XAxisProperty, new ObservableCollection<Plane>
            {
                new Axis()
            });
            SetValue(YAxisProperty, new ObservableCollection<Plane>
            {
                new Axis()
            });
        }

        #region Dependency properties

        /// <summary>
        /// The x axis property.
        /// </summary>
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(
            nameof(XAxis), typeof(IEnumerable<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, BuildInstanceChangedCallback(p => p.XAxis, nameof(XAxis))));

        /// <summary>
        /// The y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(
            nameof(YAxis), typeof(IEnumerable<Plane>), typeof(CartesianChart),
            new PropertyMetadata(null, BuildInstanceChangedCallback(p => p.YAxis, nameof(YAxis))));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x axis.
        /// </summary>
        /// <value>
        /// The x axis.
        /// </value>
        public IEnumerable<Plane> XAxis
        {
            get => (IEnumerable<Plane>) GetValue(XAxisProperty);
            set => SetValue(XAxisProperty, value);
        }

        /// <summary>
        /// Gets or sets the y axis.
        /// </summary>
        /// <value>
        /// The y axis.
        /// </value>
        public IEnumerable<Plane> YAxis
        {
            get => (IEnumerable<Plane>) GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        #endregion

        /// <inheritdoc cref="Chart.GetPlanes"/>
        protected override IList<IList<Plane>> GetPlanes()
        {
            if (XAxis == null || !XAxis.Any() ||
                YAxis == null || !YAxis.Any())
            {
                throw new LiveChartsException(
                    "No axis was found, please ensure your chart contains at least an axis.", 190);
            }

            return new List<IList<Plane>>
            {
                XAxis.Select(axis => new Plane(PlaneTypes.X)).ToList(),
                YAxis.Select(axis => new Plane(PlaneTypes.Y)).ToList()
            };
        }
    }
}
