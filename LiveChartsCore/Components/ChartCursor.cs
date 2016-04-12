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

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;

namespace LiveCharts.Components
{
    public class ChartCursor
    {
        private double? _value;
        private readonly Chart _chart;
        private readonly AxisTags _tag;

        public ChartCursor(Chart chart, AxisTags tag)
        {
            _chart = chart;
            _tag = tag;

            Shape = new Rectangle
            {
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.Red),
                Fill = new SolidColorBrush(Colors.Red) {Opacity = .3}
            };
        }

        /// <summary>
        /// Gets the cursor shape (System.Window.Shapes.Rectangle)
        /// </summary>
        public Rectangle Shape { get; private set; }

        /// <summary>
        /// Gets or sets the axis where the cursor is scalated,  default is 0
        /// </summary>
        public int Axis { get; set; }

        /// <summary>
        /// Gets or sets the value where the cursor is positioned
        /// </summary>
        public double? Value
        {
            get { return _value; }
            set
            {
                _value = value;

                if (_value == null)
                {
                    _chart.DrawMargin.Children.Remove(Shape);
                    return;
                }

                if (Shape.Parent == null)
                    _chart.DrawMargin.Children.Add(Shape);

                if (_tag == AxisTags.X)
                {
                    //The Chart must be already drawn or this wont work!!!
                    Canvas.SetTop(Shape, 0d);
                    Shape.Height = _chart.DrawMargin.Height;
                    Shape.Width = _chart is IUnitaryPoints && !_chart.Invert
                        ? Methods.GetUnitWidth(AxisTags.X, _chart, Axis)
                        : 1;

                    var chartValue = Methods.FromDrawMargin((double) _value, AxisTags.X, _chart, Axis);

                    if (_chart.DisableAnimations)
                        Canvas.SetLeft(Shape, chartValue - Shape.Width);
                    else
                        Shape.BeginAnimation(Canvas.LeftProperty,
                            new DoubleAnimation(chartValue - Shape.Width, _chart.AnimationsSpeed));
                }
                else
                {
                    Canvas.SetLeft(Shape, 0d);
                    Shape.Width = _chart.DrawMargin.Height;
                    Shape.Height = _chart is IUnitaryPoints && _chart.Invert
                        ? Methods.GetUnitWidth(AxisTags.Y, _chart, Axis)
                        : 1;

                    var chartValue = Methods.FromDrawMargin((double) _value, AxisTags.Y, _chart, Axis);

                    if (_chart.DisableAnimations)
                        Canvas.SetTop(Shape, chartValue - Shape.Height);
                    else
                        Shape.BeginAnimation(Canvas.TopProperty,
                            new DoubleAnimation(chartValue - Shape.Height, _chart.AnimationsSpeed));
                }
            }
        }
    }
}
