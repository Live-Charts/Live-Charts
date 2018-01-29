using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using Point = LiveCharts.Core.Drawing.Point;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// Default data tool tip class.
    /// </summary>
    public class ChartToolTip : ItemsControl, IDataToolTip
    {
        static ChartToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartToolTip),
                new FrameworkPropertyMetadata(typeof(ChartToolTip)));
        }

        #region Properties

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(ChartToolTip), new PropertyMetadata(15d));

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
            nameof(SelectionMode), typeof(TooltipSelectionMode), typeof(ChartToolTip), new PropertyMetadata(default(TooltipSelectionMode)));

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

        /// <summary>
        /// The default group style property
        /// </summary>
        public static readonly DependencyProperty DefaultGroupStyleProperty = DependencyProperty.Register(
            nameof(DefaultGroupStyle), typeof(GroupStyle), typeof(ChartToolTip), 
            new PropertyMetadata(default(GroupStyle), OnDefaultGroupStyleChanged));

        /// <summary>
        /// Gets or sets the default group style.
        /// </summary>
        /// <value>
        /// The default group style.
        /// </value>
        public GroupStyle DefaultGroupStyle
        {
            get => (GroupStyle) GetValue(DefaultGroupStyleProperty);
            set => SetValue(DefaultGroupStyleProperty, value);
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(double), typeof(ChartToolTip), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public double CornerRadius
        {
            get => (double) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
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

        TooltipSelectionMode IDataToolTip.SelectionMode => SelectionMode;

        Size IDataToolTip.ShowAndMeasure(IEnumerable<PackedPoint> selected, IChartView chart)
        {
            return Dispatcher.Invoke(() =>
            {
                var wpfChart = (Chart)chart;

                if (Parent == null)
                {
                    wpfChart.TooltipPopup.Child = this;
                }

                ItemsSource = selected;
                wpfChart.TooltipPopup.IsOpen = true;
                UpdateLayout();

                return new Size(DesiredSize.Width, DesiredSize.Height);
            });
        }

        /// <inheritdoc />
        void IDataToolTip.Hide(IChartView chart)
        {
            Dispatcher.Invoke(() =>
            {
                var wpfChart = (Chart)chart;
                wpfChart.TooltipPopup.IsOpen = false;
            });
        }

        void IDataToolTip.Move(Point location, IChartView chart)
        {
            Dispatcher.Invoke(() =>
            {
                var wpfChart = (Chart)chart;
                wpfChart.TooltipPopup.AsStoryboardTarget()
                    .AtSpeed(TimeSpan.FromMilliseconds(150))
                    .Property(Popup.HorizontalOffsetProperty, location.X)
                    .Property(Popup.VerticalOffsetProperty, location.Y)
                    .Begin();
            });
        }
    }
}