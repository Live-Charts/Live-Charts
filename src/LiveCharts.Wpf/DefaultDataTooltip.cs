using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using Point = LiveCharts.Core.Drawing.Point;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default data tool tip class.
    /// </summary>
    public class DefaultDataTooltip : ItemsControl, IDataTooltip
    {
        static DefaultDataTooltip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultDataTooltip),
                new FrameworkPropertyMetadata(typeof(DefaultDataTooltip)));
        }

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(DefaultDataTooltip), new PropertyMetadata(15d));

        /// <summary>
        /// Gets or sets the size of the bullet.
        /// </summary>
        /// <value>
        /// The size of the bullet.
        /// </value>
        public double GeometrySize
        {
            get => (double) GetValue(GeometrySizeProperty);
            set => SetValue(GeometrySizeProperty, value);
        }

        /// <summary>
        /// The selection mode property
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            nameof(SelectionMode), typeof(TooltipSelectionMode), typeof(DefaultDataTooltip), new PropertyMetadata(default(TooltipSelectionMode)));

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        public TooltipSelectionMode SelectionMode
        {
            get => (TooltipSelectionMode) GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            throw new System.NotImplementedException();
        }

        TooltipSelectionMode IDataTooltip.SelectionMode => SelectionMode;

        Size IDataTooltip.Measure(IEnumerable<PackedPoint> selected)
        {
            return Dispatcher.Invoke(() =>
            {
                Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                return new Size(DesiredSize.Width, DesiredSize.Height);
            });
        }

        void IDataTooltip.Move(Point location)
        {
            throw new System.NotImplementedException();
        }
    }
}