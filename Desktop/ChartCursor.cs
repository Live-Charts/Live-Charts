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
using LiveChartsCore;

namespace LiveChartsDesktop
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
                Stroke = new SolidColorBrush(Color.FromRgb(55, 71, 79)),
                Fill = new SolidColorBrush(Color.FromRgb(55, 71, 79)) { Opacity = .3 }
            };

            Canvas.SetLeft(Shape, 0d);
            Canvas.SetTop(Shape, 0d);
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

                Update();
            }
        }

        public void Update()
        {
            if (Value == null) return;

            if (_tag == AxisTags.X)
            {
                //The Chart must be already drawn or this wont work!!!
                Canvas.SetTop(Shape, 0d);
                Shape.Height = _chart.DrawMargin.Height;

                Shape.Width = _chart.Model.HasUnitaryPoints && !_chart.Model.Invert
                    ? ChartFunctions.GetUnitWidth(AxisTags.X, _chart.Model, Axis)
                    : 2;

                var chartValue = ChartFunctions.ToDrawMargin((double) Value, AxisTags.X, _chart.Model, Axis);

                if (_chart.DisableAnimations)
                    Canvas.SetLeft(Shape, chartValue - Shape.Width);
                else
                    Shape.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(chartValue - Shape.Width, _chart.AnimationsSpeed));
            }
            else
            {
                Canvas.SetLeft(Shape, 0d);
                Shape.Width = _chart.DrawMargin.Width;

                Shape.Height = _chart.Model.HasUnitaryPoints && _chart.Model.Invert
                    ? ChartFunctions.GetUnitWidth(AxisTags.Y, _chart.Model, Axis)
                    : 2;

                var chartValue = ChartFunctions.ToDrawMargin((double) Value, AxisTags.Y, _chart.Model, Axis);

                if (_chart.DisableAnimations)
                    Canvas.SetTop(Shape, chartValue - Shape.Height);
                else
                    Shape.BeginAnimation(Canvas.TopProperty,
                        new DoubleAnimation(chartValue - Shape.Height, _chart.AnimationsSpeed));
            }
        }
    }
}
