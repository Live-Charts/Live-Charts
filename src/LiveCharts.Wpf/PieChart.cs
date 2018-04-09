using System.Collections.Generic;
using System.Windows;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Generates pie charts.
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.Chart" />
    /// <seealso cref="LiveCharts.Core.Abstractions.IPieChartView" />
    public class PieChart : Chart, IPieChartView
    {
        private readonly IList<IList<Plane>> _planes;

        /// <summary>
        /// Initializes the <see cref="PieChart"/> class.
        /// </summary>
        static PieChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PieChart),
                new FrameworkPropertyMetadata(typeof(PieChart)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public PieChart()
        {
            Model = new PieChartModel(this);
            _planes = new List<IList<Plane>>
            {
                new List<Plane> {new Axis()},
                new List<Plane> {new Axis()}
            };
        }

        #region Dependency properties

        /// <summary>
        /// The starting rotation angle property.
        /// </summary>
        public static readonly DependencyProperty StartingRotationAngleProperty = DependencyProperty.Register(
            nameof(StartingRotationAngle), typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

        /// <summary>
        /// The inner radius property.
        /// </summary>
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            nameof(InnerRadius), typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

        #endregion

        #region Properties

        /// <inheritdoc />
        public double StartingRotationAngle
        {
            get => (double) GetValue(StartingRotationAngleProperty);
            set => SetValue(StartingRotationAngleProperty, value);
        }

        /// <inheritdoc />
        public double InnerRadius
        {
            get => (double) GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        #endregion

        protected override IList<IList<Plane>> GetOrderedDimensions()
        {
            return _planes;
        }
    }
}