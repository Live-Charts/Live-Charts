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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf
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

        #region Dependency properties

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(ChartToolTip),
            new FrameworkPropertyMetadata(15d, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The selection mode property
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            nameof(SelectionMode), typeof(ToolTipSelectionMode), typeof(ChartToolTip), new PropertyMetadata(default(ToolTipSelectionMode)));

        /// <summary>
        /// The position property
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(ToolTipPosition), typeof(ChartToolTip),
            new FrameworkPropertyMetadata(default(ToolTipPosition), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

               /// <summary>
        /// The default group style property
        /// </summary>
        public static readonly DependencyProperty DefaultGroupStyleProperty = DependencyProperty.Register(
            nameof(DefaultGroupStyle), typeof(GroupStyle), typeof(ChartToolTip), 
            new PropertyMetadata(default(GroupStyle), OnDefaultGroupStyleChanged));

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(double), typeof(ChartToolTip), new PropertyMetadata(default(double)));
        
        /// <summary>
        /// The show series names property
        /// </summary>
        public static readonly DependencyProperty ShowSeriesNamesProperty = DependencyProperty.Register(
            nameof(ShowSeriesNames), typeof(bool), typeof(ChartToolTip), new PropertyMetadata(default(bool)));

        /// <summary>
        /// The wedge property
        /// </summary>
        public static readonly DependencyProperty WedgeProperty = DependencyProperty.Register(
            nameof(Wedge), typeof(double), typeof(ChartToolTip), new PropertyMetadata(default(double)));

        /// <summary>
        /// The wedge hypotenuse property
        /// </summary>
        public static readonly DependencyProperty WedgeHypotenuseProperty = DependencyProperty.Register(
            nameof(WedgeHypotenuse), typeof(double), typeof(ChartToolTip), new PropertyMetadata(default(double)));

        /// <summary>
        /// The snap to closest property
        /// </summary>
        public static readonly DependencyProperty SnapToClosestProperty = DependencyProperty.Register(
            nameof(SnapToClosest), typeof(bool), typeof(ChartToolTip), new PropertyMetadata(default(bool)));

        /// <summary>
        /// The show series geometry property
        /// </summary>
        public static readonly DependencyProperty ShowSeriesGeometryProperty = DependencyProperty.Register(
            "ShowSeriesGeometry", typeof(bool), typeof(ChartToolTip), new PropertyMetadata(default(bool)));

        #endregion

        #region Properties


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
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        public ToolTipSelectionMode SelectionMode
        {
            get => (ToolTipSelectionMode) GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        /// <inheritdoc />
        public ToolTipPosition Position
        {
            get => (ToolTipPosition) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
 
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
        
        /// <summary>
        /// Gets or sets a value indicating whether the series name should be displayed in the tooltip.
        /// </summary>
        public bool ShowSeriesNames
        {
            get => (bool) GetValue(ShowSeriesNamesProperty);
            set => SetValue(ShowSeriesNamesProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the series geometry is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show series geometry]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSeriesGeometry
        {
            get => (bool) GetValue(ShowSeriesGeometryProperty);
            set => SetValue(ShowSeriesGeometryProperty, value);
        }
      
        /// <summary>
        /// Gets or sets the wedge, in degrees.
        /// </summary>
        /// <value>
        /// The wedge.
        /// </value>
        public double Wedge
        {
            get => (double) GetValue(WedgeProperty);
            set => SetValue(WedgeProperty, value);
        }
  
        /// <summary>
        /// Gets or sets the wedge hypotenuse.
        /// </summary>
        /// <value>
        /// The wedge hypotenuse.
        /// </value>
        public double WedgeHypotenuse
        {
            get => (double) GetValue(WedgeHypotenuseProperty);
            set => SetValue(WedgeHypotenuseProperty, value);
        }
        
        /// <inheritdoc />
        public bool SnapToClosest
        {
            get => (bool) GetValue(SnapToClosestProperty);
            set => SetValue(SnapToClosestProperty, value);
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

        ToolTipSelectionMode IDataToolTip.SelectionMode => SelectionMode;

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

            var x = location.X + (float) Canvas.GetLeft(wpfChart.VisualDrawMargin);
            var y = location.Y + (float) Canvas.GetTop(wpfChart.VisualDrawMargin);

            location = new PointF(x, y);

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
