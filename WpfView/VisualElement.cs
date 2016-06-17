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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LiveCharts.Wpf.Charts.Chart;

namespace LiveCharts.Wpf
{
    public class VisualElement : FrameworkElement, ICartesianVisualElement
    {
        // ReSharper disable once InconsistentNaming
        public FrameworkElement UIElement { get; set; }
        public IChartView Chart { get; set; }
        public bool RequiresAdd { get; set; }
        public int AxisX { get; set; }
        public int AxisY { get; set; }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof (double), typeof (VisualElement), 
            new PropertyMetadata(default(double), PropertyChangedCallback));

        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof (double), typeof (VisualElement), 
            new PropertyMetadata(default(double), PropertyChangedCallback));

        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public void AddOrMove()
        {
            if (UIElement.Parent == null)
            {
                Chart.AddToDrawMargin(UIElement);
                Panel.SetZIndex(UIElement, 1000);
            }

            var coordinate = new CorePoint(ChartFunctions.ToDrawMargin(X, AxisTags.X, Chart.Model, AxisX),
                ChartFunctions.ToDrawMargin(Y, AxisTags.Y, Chart.Model, AxisY));

            var wpfChart = (CartesianChart) Chart;

            var uw = new CorePoint(
                wpfChart.AxisX[AxisX].Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.X, Chart.Model, AxisX)/2
                    : 0,
                wpfChart.AxisY[AxisY].Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.Y, Chart.Model, AxisY)/2
                    : 0);

            coordinate += uw;

            UIElement.UpdateLayout();

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    coordinate = new CorePoint(coordinate.X, coordinate.Y - UIElement.ActualHeight);
                    break;
                case VerticalAlignment.Center:
                    coordinate = new CorePoint(coordinate.X, coordinate.Y - UIElement.ActualHeight / 2);
                    break;
                case VerticalAlignment.Bottom:
                    coordinate = new CorePoint(coordinate.X, coordinate.Y);
                    break;
                case VerticalAlignment.Stretch:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    coordinate = new CorePoint(coordinate.X - UIElement.ActualWidth, coordinate.Y);
                    break;
                case HorizontalAlignment.Center:
                    coordinate = new CorePoint(coordinate.X - UIElement.ActualWidth / 2, coordinate.Y);
                    break;
                case HorizontalAlignment.Right:
                    coordinate = new CorePoint(coordinate.X, coordinate.Y);
                    break;
                case HorizontalAlignment.Stretch:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Chart.DisableAnimations)
            {
                Canvas.SetLeft(UIElement, coordinate.X);
                Canvas.SetTop(UIElement, coordinate.Y);
            }
            else
            {
                if (double.IsNaN(Canvas.GetLeft(UIElement)))
                {
                    Canvas.SetLeft(UIElement, coordinate.X);
                    Canvas.SetTop(UIElement, coordinate.Y);
                }
                UIElement.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(coordinate.X, wpfChart.AnimationsSpeed));
                UIElement.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(coordinate.Y, wpfChart.AnimationsSpeed));
            }
        }

        public void Remove()
        {
            Chart.RemoveFromView(UIElement);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var wpfChart = dependencyObject as Chart;
            if (wpfChart == null) return;
            if (wpfChart.Model != null) wpfChart.Model.Updater.Run();
        }
    }
}
