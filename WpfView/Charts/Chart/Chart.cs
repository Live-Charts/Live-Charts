//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart : UserControl, IChartView
    {
        protected ChartCore ChartCoreModel;

        #region Contructors

        protected Chart()
        {
            Canvas = new Canvas {ClipToBounds = true};
            Content = Canvas;

            DrawMargin = new Canvas {ClipToBounds = true};
            Canvas.Children.Add(DrawMargin);

            TooltipTimeoutTimer = new DispatcherTimer();

            SetValue(MinHeightProperty, 125d);
            SetValue(MinWidthProperty, 125d);

            SetValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(300));
            SetValue(ChartLegendProperty, new DefaultLegend());
            SetValue(DataTooltipProperty, new DefaultTooltip());
            SetValue(TooltipTimeoutProperty, TimeSpan.FromMilliseconds(800));

            SetValue(AxisXProperty, new List<Axis>());
            SetValue(AxisYProperty, new List<Axis>());

            SetValue(SeriesProperty, new SeriesCollection());

            if (RandomizeStartingColor)
                SeriesIndexCount = Randomizer.Next(0, Colors.Count);

            SizeChanged += OnSizeChanged;
            MouseWheel += MouseWheelOnRoll;
            Loaded += OnLoaded;
            TooltipTimeoutTimer.Tick += TooltipTimeoutTimerOnTick;

            DrawMargin.Background = Brushes.Transparent; // if this line is not set, then it does not detect mouse down event...
            DrawMargin.MouseDown += OnDraggingStart;
            DrawMargin.MouseUp += OnDraggingEnd;
        }

        static Chart()
        {
            Colors = new List<Color>
            {
                Color.FromRgb(33, 149, 242),
                Color.FromRgb(243, 67, 54),
                Color.FromRgb(254, 192, 7),
                Color.FromRgb(96, 125, 138),
                Color.FromRgb(155, 39, 175),
                Color.FromRgb(0, 149, 135),
                Color.FromRgb(76, 174, 80),
                Color.FromRgb(121, 85, 72),
                Color.FromRgb(157, 157, 157),
                Color.FromRgb(232, 30, 99),
                Color.FromRgb(63, 81, 180),
                Color.FromRgb(0, 187, 211),
                Color.FromRgb(254, 234, 59),
                Color.FromRgb(254, 87, 34)
            };
            Randomizer = new Random();
        }

        #endregion

        #region Property Changed

        protected static PropertyChangedCallback CallChartUpdater(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfChart = o as Chart;
                if (wpfChart == null) return;
                if (wpfChart.Model != null) wpfChart.Model.Updater.Run(animate);
            };
        }

        private static void UpdateChartFrequency(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var wpfChart = (Chart) o;

            var freq = wpfChart.DisableAnimations ? TimeSpan.FromMilliseconds(10) : wpfChart.AnimationsSpeed;

            if (wpfChart.Model == null || wpfChart.Model.Updater == null) return;

            wpfChart.Model.Updater.UpdateFrequency(freq);

            CallChartUpdater(true)(o, e);
        }

        #endregion

        #region Obsoletes, this properties will dissapear in future versions

        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register("Hoverable", typeof (bool), typeof (Chart), new PropertyMetadata(true));

        [Obsolete("This property is obsolete, if you need to disable tooltips, set the Chart.DataTooltip to null")]
        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        #endregion

#if DEBUG

        public void CountElements()
        {
            //Trying to avoid memory leaks
            Debug.WriteLine("Canvas Elements -> " + Canvas.Children.Count);
            Debug.WriteLine("Draw Margin Elements ->" + DrawMargin.Children.Count);
        }
#endif
    }
}
