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
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// Default data tool tip class.
    /// </summary>
    public class ChartToolTip : ItemsControl, IDataToolTip
    {
        static ChartToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ChartToolTip),
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

        SizeF IDataToolTip.ShowAndMeasure(IEnumerable<PackedPoint> selected, IChartView chart)
        {
            var wpfChart = (Chart) chart;

            if (Parent == null)
            {
                wpfChart.TooltipPopup.Child = this;
            }

            ItemsSource = selected;
            wpfChart.TooltipPopup.IsOpen = true;
            UpdateLayout();

            return new SizeF((float) DesiredSize.Width, (float) DesiredSize.Height);
        }

        /// <inheritdoc />
        void IDataToolTip.Hide(IChartView chart)
        {
            var wpfChart = (Chart) chart;
            wpfChart.TooltipPopup.IsOpen = false;
        }

        void IDataToolTip.Move(PointF location, IChartView chart)
        {
            var wpfChart = (Chart) chart;
            wpfChart.TooltipPopup.Animate(
                    new TimeLine
                    {
                        Duration = chart.AnimationsSpeed,
                        AnimationLine = chart.AnimationLine
                    })
                .Property(
                    Popup.HorizontalOffsetProperty, wpfChart.TooltipPopup.HorizontalOffset, location.X)
                .Property(
                    Popup.VerticalOffsetProperty, wpfChart.TooltipPopup.VerticalOffset, location.Y)
                .Begin();
        }
    }
}