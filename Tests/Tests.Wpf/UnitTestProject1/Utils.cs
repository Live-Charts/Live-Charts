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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using LiveCharts.Wpf;

namespace Tests.Wpf
{
    public static class Utils
    {
        public static int GetDesendantCount(this FrameworkElement element)
        {
            if (element == null) return 0;

            var count = 0;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = (FrameworkElement) VisualTreeHelper.GetChild(element, i);
                count++;
                count += child.GetDesendantCount();
            }
            return count;
        }

        public static IEnumerable<Series> TestSeries
        {
            get
            {
                var liveChartsWpf = Assembly.Load("LiveCharts.Wpf");

                var enumerable = liveChartsWpf.GetTypes()
                    .Where(t => t.BaseType != null && t.BaseType.IsAssignableFrom(typeof(Series)));

                foreach (var seriesType in enumerable)
                {
                    yield return (Series) Activator.CreateInstance(seriesType);
                }
            }
        }
    }
}
