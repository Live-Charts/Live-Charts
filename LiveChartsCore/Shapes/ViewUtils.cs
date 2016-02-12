//The MIT License(MIT)

//Copyright(c) 2015 Raúl Otaño Hurtado

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
using System.Windows;
using System.Windows.Media;

namespace LiveCharts.Shapes
{
    public static class ViewUtils
    {
        public static bool AnyParent(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return false;

            var visualParent = VisualTreeHelper.GetParent(item);

            return condition(item) || AnyParent(visualParent, condition);
        }

        public static DependencyObject GetParent(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var logicalParent = LogicalTreeHelper.GetParent(item);
            var visualParent = VisualTreeHelper.GetParent(item);

            return condition(item) ? item : GetParent(visualParent, condition);
        }

        public static DependencyObject GetVisualChild(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var q = new Queue<DependencyObject>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
            {
                var t = VisualTreeHelper.GetChild(item, i);
                if (condition(t))
                    return t;
                q.Enqueue(t);
            }

            while (q.Count > 0)
            {
                var subchild = q.Dequeue();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(subchild); i++)
                {
                    var t = VisualTreeHelper.GetChild(subchild, i);
                    if (condition(t))
                        return t;
                    q.Enqueue(t);
                }
            }
            return null;
        }

        public static DependencyObject GetLogicalChild(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var q = new Queue<DependencyObject>();
            foreach (var w in LogicalTreeHelper.GetChildren(item))
            {
                var t = w as DependencyObject;
                if (condition(t))
                    return t;
                q.Enqueue(t);
            }

            while (q.Count > 0)
            {
                var subchild = q.Dequeue();
                foreach (var w in LogicalTreeHelper.GetChildren(subchild))
                {
                    var t = w as DependencyObject;
                    if (condition(t))
                        return t;
                    q.Enqueue(t);
                }
            }
            return null;
        }
    }
}
