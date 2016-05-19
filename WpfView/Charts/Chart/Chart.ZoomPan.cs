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

using System.Windows;
using System.Windows.Input;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart
    {
        private Point DragOrigin { get; set; }

        private void MouseWheelOnRoll(object sender, MouseWheelEventArgs e)
        {
            if (Zoom == ZoomingOptions.None) return;

            var p = e.GetPosition(this);

            var corePoint = new CorePoint(p.X, p.Y);

            e.Handled = true;

            if (e.Delta > 0)
                Model.ZoomIn(corePoint);
            else
                Model.ZoomOut(corePoint);
        }

        private void OnDraggingStart(object sender, MouseButtonEventArgs e)
        {
            DragOrigin = e.GetPosition(this);
            DragOrigin = new Point(
                ChartFunctions.FromDrawMargin(DragOrigin.X, AxisTags.X, Model),
                ChartFunctions.FromDrawMargin(DragOrigin.Y, AxisTags.Y, Model));
        }

        private void OnDraggingEnd(object sender, MouseButtonEventArgs e)
        {
            if (Zoom == ZoomingOptions.None) return;

            var end = e.GetPosition(this);

            end = new Point(
                ChartFunctions.FromDrawMargin(end.X, AxisTags.X, Model),
                ChartFunctions.FromDrawMargin(end.Y, AxisTags.Y, Model));

            Model.Drag(new CorePoint(DragOrigin.X - end.X, DragOrigin.Y - end.Y));
        }
    }
}
