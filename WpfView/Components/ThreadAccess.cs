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

using System;
using System.Windows;

namespace LiveCharts.Wpf.Components
{

    // This is a workaround to prevent a possible threading issue
    // LiveChart was designed to be easy to use, the current design 
    // avoids the usage of DataTemplates, instead we use the same object (UIElement)
    // since UI elements are running the UI thread, it is possible that 
    // when we try to modify a property in a UIElement, i.e. the labels of an axis, 
    // we can't, well we can but we need to use the UI dispatcher.

    internal static class ThreadAccess
    {
        public static T Resolve<T>(DependencyObject dependencyObject,
            DependencyProperty dependencyProperty)
        {
            if (dependencyObject.Dispatcher.CheckAccess())
                return (T) dependencyObject.GetValue(dependencyProperty);

            return (T) dependencyObject.Dispatcher.Invoke(
                new Func<T>(() => (T) dependencyObject.GetValue(dependencyProperty)));
        }
    }
}
