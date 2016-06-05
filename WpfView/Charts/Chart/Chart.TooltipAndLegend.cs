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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart
    {
        private DispatcherTimer TooltipTimeoutTimer { get; set; }

        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            "TooltipTimeout", typeof(TimeSpan), typeof(Chart),
            new PropertyMetadata(default(TimeSpan), TooltipTimeoutCallback));

        /// <summary>
        /// Gets or sets the time a tooltip takes to hide when the user leaves the data point.
        /// </summary>
        public TimeSpan TooltipTimeout
        {
            get { return (TimeSpan)GetValue(TooltipTimeoutProperty); }
            set { SetValue(TooltipTimeoutProperty, value); }
        }

        internal void AttachEventsTo(FrameworkElement element)
        {
            element.MouseDown -= DataMouseDown;
            element.MouseEnter -= DataMouseEnter;
            element.MouseLeave -= DataMouseLeave;

            element.MouseDown += DataMouseDown;
            element.MouseEnter += DataMouseEnter;
            element.MouseLeave += DataMouseLeave;
        }

        private void DataMouseDown(object sender, MouseEventArgs e)
        {
            var result = Series.SelectMany(x => x.Values.Points).FirstOrDefault(x =>
            {
                var pointView = x.View as PointView;
                return pointView != null && Equals(pointView.HoverShape, sender);
            });
            if (DataClick != null) DataClick.Invoke(sender, result);
        }

        private void DataMouseEnter(object sender, EventArgs e)
        {
            if (DataTooltip == null) return;

            TooltipTimeoutTimer.Stop();

            if (DataTooltip.Parent == null)
            {
                Panel.SetZIndex(DataTooltip, int.MaxValue);
                AddToDrawMargin(DataTooltip);
                Canvas.SetTop(DataTooltip, 0d);
                Canvas.SetLeft(DataTooltip, 0d);
            }

            var source = Series.SelectMany(x => x.Values.Points);
            var senderPoint =
                source.FirstOrDefault(x => x.View != null && Equals(((PointView) x.View).HoverShape, sender));

            if (senderPoint == null) return;

            var ax = AxisX[senderPoint.SeriesView.ScalesXAt];
            var ay = AxisY[senderPoint.SeriesView.ScalesYAt];

            senderPoint.View.OnHover(senderPoint);

            var pointsToHighlight = Enumerable.Empty<ChartPoint>();
            double? shares = null;

            //ToDo: the tooltip should be smart enough to detect the best SelectionMode
            switch (DataTooltip.SelectionMode)
            {
                case TooltipSelectionMode.OnlySender:
                    pointsToHighlight = new List<ChartPoint> { senderPoint };
                    break;
                case TooltipSelectionMode.SharedXValues:
                    pointsToHighlight = Series.Where(x => x.ScalesXAt == senderPoint.SeriesView.ScalesXAt)
                        .SelectMany(x => x.Values.Points)
                        .Where(x => Math.Abs(x.X - senderPoint.X) < ax.Model.S * .01);
                    shares = (this as PieChart) == null ? (double?) senderPoint.X : null;
                    break;
                case TooltipSelectionMode.SharedYValues:
                    pointsToHighlight = Series.Where(x => x.ScalesYAt == senderPoint.SeriesView.ScalesYAt)
                        .SelectMany(x => x.Values.Points)
                        .Where(x => Math.Abs(x.Y - senderPoint.Y) < ay.Model.S * .01);
                    shares = senderPoint.Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DataTooltip.Data = new TooltipData
            {
                XFormatter = ax.Model.GetFormatter(),
                YFormatter = ay.Model.GetFormatter(),
                SharedValue = shares,
                SelectionMode = DataTooltip.SelectionMode,
                Points = pointsToHighlight.Select(x => new DataPointViewModel
                {
                    Series = new Wpf.SeriesViewModel
                    {
                        Fill = ((Series.Series)x.SeriesView).Fill,
                        Stroke = ((Series.Series)x.SeriesView).Stroke,
                        StrokeThickness = ((Series.Series)x.SeriesView).StrokeThickness,
                        Title = ((Series.Series)x.SeriesView).Title,
                    },
                    ChartPoint = x
                }).ToList()
            };

            DataTooltip.Visibility = Visibility.Visible;
            DataTooltip.UpdateLayout();

            var location = GetTooltipPosition(senderPoint);

            DataTooltip.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(location.X, TimeSpan.FromMilliseconds(200)));
            DataTooltip.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(location.Y, TimeSpan.FromMilliseconds(200)));
        }

        private void DataMouseLeave(object sender, EventArgs e)
        {
            TooltipTimeoutTimer.Stop();
            TooltipTimeoutTimer.Start();

            var source = Series.SelectMany(x => x.Values.Points);
            var senderPoint =
                source.FirstOrDefault(x => x.View != null && Equals(((PointView) x.View).HoverShape, sender)); 

            if (senderPoint == null) return;

            senderPoint.View.OnHoverLeave(senderPoint);
        }

        private void TooltipTimeoutTimerOnTick(object sender, EventArgs eventArgs)
        {
            TooltipTimeoutTimer.Stop();

            if (DataTooltip == null) return;

            DataTooltip.Visibility = Visibility.Hidden;
        }

        private static void TooltipTimeoutCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var chart = (Chart)dependencyObject;

            if (chart == null) return;

            chart.TooltipTimeoutTimer.Interval = chart.TooltipTimeout;
        }

        public void HideTooltop()
        {
            if (DataTooltip == null) return;

            DataTooltip.Visibility = Visibility.Hidden;
        }

        protected virtual Point GetTooltipPosition(ChartPoint senderPoint)
        {
            var xt = senderPoint.ChartLocation.X;
            var yt = senderPoint.ChartLocation.Y;

            xt = xt > DrawMargin.Width / 2 ? xt - DataTooltip.ActualWidth - 5 : xt + 5;
            yt = yt > DrawMargin.Height / 2 ? yt - DataTooltip.ActualHeight - 5 : yt + 5;

            return new Point(xt, yt);
        }

    }
}
