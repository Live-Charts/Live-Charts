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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Uwp.Charts.Base;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// Defines a visual element, a visual element is a UI element that is placed and scaled in the chart.
    /// </summary>
    public class VisualElement : FrameworkElement, ICartesianVisualElement
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets or sets the user interface element.
        /// </summary>
        public FrameworkElement UIElement { get; set; }
        /// <summary>
        /// Gets the chart that owns the visual element
        /// </summary>
        public IChartView Chart { get; set; }
        /// <summary>
        /// Gets if the elements requires to be added to the chart, this property should normally only be used internally by LiveCharts
        /// </summary>
        public bool RequiresAdd { get; set; }
        /// <summary>
        /// Gets or sets the index of the axis in X that owns the element, the axis position must exist.
        /// </summary>
        public int AxisX { get; set; }
        /// <summary>
        /// Gets or sets the index of the axis in Y that owns the element, the axis position must exist.
        /// </summary>
        public int AxisY { get; set; }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof (double), typeof (VisualElement), 
            new PropertyMetadata(default(double), PropertyChangedCallback));
        /// <summary>
        /// Gets or sets the X value of the UiElement
        /// </summary>
        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof (double), typeof (VisualElement), 
            new PropertyMetadata(default(double), PropertyChangedCallback));
        /// <summary>
        /// Gets or sets the Y value of the UiElement
        /// </summary>
        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public void AddOrMove(ChartCore chart)
        {
            if (UIElement.Parent == null)
            {
                chart.View.AddToDrawMargin(UIElement);
                Canvas.SetZIndex(UIElement, 1000);
            }

            var coordinate = new CorePoint(ChartFunctions.ToDrawMargin(X, AxisOrientation.X, chart, AxisX),
                ChartFunctions.ToDrawMargin(Y, AxisOrientation.Y, chart, AxisY));

            var uwpChart = (CartesianChart) chart.View;

            var uw = new CorePoint(
                uwpChart.AxisX[AxisX].Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisOrientation.X, chart, AxisX)/2
                    : 0,
                uwpChart.AxisY[AxisY].Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisOrientation.Y, chart, AxisY)/2
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

            if (chart.View.DisableAnimations)
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

                var x = AnimationsHelper.CreateDouble(coordinate.X, uwpChart.AnimationsSpeed, "(Canvas.Left)");
                var y = AnimationsHelper.CreateDouble(coordinate.Y, uwpChart.AnimationsSpeed, "(Canvas.Top)");
                AnimationsHelper.CreateStoryBoardAndBegin(UIElement, x, y);
            }
        }

        public void Remove(ChartCore chart)
        {
            chart.View.RemoveFromView(UIElement);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var wpfChart = dependencyObject as Chart;
            if (wpfChart == null) return;
            if (wpfChart.Model != null) wpfChart.Model.Updater.Run();
        }
    }
}
