//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.Windows.Controls;

namespace LiveCharts.Tooltip
{
    public class ScatterTooltip : UserControl
    {
        public ScatterTooltip()
        {
            DataContext = this;
        }

        public static readonly DependencyProperty PrimaryAxisTitleProperty =
            DependencyProperty.Register("PrimaryAxisTitle", typeof(string), typeof(ScatterTooltip), new PropertyMetadata(null));

        public string PrimaryAxisTitle
        {
            get { return (string)GetValue(PrimaryAxisTitleProperty); }
            set { SetValue(PrimaryAxisTitleProperty, value); }
        }

        public static readonly DependencyProperty PrimaryValueProperty =
            DependencyProperty.Register("PrimaryValue", typeof(string), typeof(ScatterTooltip), new PropertyMetadata(null));

        public string PrimaryValue
        {
            get { return (string)GetValue(PrimaryValueProperty); }
            set { SetValue(PrimaryValueProperty, value); }
        }

        public static readonly DependencyProperty SecondaryAxisTitleProperty =
            DependencyProperty.Register("SecondaryAxisTitle", typeof(string), typeof(ScatterTooltip), new PropertyMetadata(null));

        public string SecondaryAxisTitle
        {
            get { return (string)GetValue(SecondaryAxisTitleProperty); }
            set { SetValue(SecondaryAxisTitleProperty, value); }
        }

        public static readonly DependencyProperty SecondaryValueProperty =
            DependencyProperty.Register("SecondaryValue", typeof(string), typeof(ScatterTooltip), new PropertyMetadata(null));

        public string SecondaryValue
        {
            get { return (string)GetValue(SecondaryValueProperty); }
            set { SetValue(SecondaryValueProperty, value); }
        }
    }
}