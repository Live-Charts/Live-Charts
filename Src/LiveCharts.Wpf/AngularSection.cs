//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.FrameworkElement" />
    public class AngularSection : FrameworkElement
    {
        internal AngularGauge Owner { get; set; }

        /// <summary>
        /// From value property
        /// </summary>
        public static readonly DependencyProperty FromValueProperty = DependencyProperty.Register(
            "FromValue", typeof(double), typeof(AngularSection), new PropertyMetadata(default(double), Redraw));

        /// <summary>
        /// Gets or sets from value.
        /// </summary>
        /// <value>
        /// From value.
        /// </value>
        public double FromValue
        {
            get { return (double) GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        /// <summary>
        /// To value property
        /// </summary>
        public static readonly DependencyProperty ToValueProperty = DependencyProperty.Register(
            "ToValue", typeof(double), typeof(AngularSection), new PropertyMetadata(default(double), Redraw));

        /// <summary>
        /// Gets or sets to value.
        /// </summary>
        /// <value>
        /// To value.
        /// </value>
        public double ToValue
        {
            get { return (double) GetValue(ToValueProperty); }
            set { SetValue(ToValueProperty, value); }
        }

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(AngularSection), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        private static void Redraw(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var angularSection = (AngularSection) dependencyObject;

            if (angularSection.Owner == null) return;

            angularSection.Owner.UpdateSections();
        }

    }
}