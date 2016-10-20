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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp
{
    public class AngularSection : FrameworkElement
    {
        internal AngularGauge Owner { get; set; }

        public static readonly DependencyProperty FromValueProperty = DependencyProperty.Register(
            "FromValue", typeof(double), typeof(AngularSection), new PropertyMetadata(default(double), Redraw));

        public double FromValue
        {
            get { return (double) GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        public static readonly DependencyProperty ToValueProperty = DependencyProperty.Register(
            "ToValue", typeof(double), typeof(AngularSection), new PropertyMetadata(default(double), Redraw));

        public double ToValue
        {
            get { return (double) GetValue(ToValueProperty); }
            set { SetValue(ToValueProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(AngularSection), new PropertyMetadata(default(Brush)));

        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        private static void Redraw(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var angularSection = (AngularSection) dependencyObject;

            angularSection.Owner?.UpdateSections();
        }
    }
}