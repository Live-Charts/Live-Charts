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

using System.Windows.Media;

namespace LiveChartsDesktop
{
    public static class DefaultAxes
    {
        /// <summary>
        /// Returns default axis
        /// </summary>
        public static Axis DefaultAxis
        {
            get
            {
                return new Axis();
            }
        }

        /// <summary>
        /// Return an axis without separators at all
        /// </summary>
        public static Axis CleanAxis
        {
            get
            {
                var a = new Separator();
                return new Axis
                {
                    IsEnabled = false,
                    Separator = CleanSeparator
                };
            }
        }

        /// <summary>
        /// Returns an axis that only displays a line for zero
        /// </summary>
        public static Axis OnlyZerosAxis
        {
            get
            {
                return new Axis
                {
                    IsEnabled = true,
                    Separator = CleanSeparator
                };
            }
        }

        /// <summary>
        /// Returns an axis that highlights zeros.
        /// </summary>
        public static Axis HighlightZerosAxis
        {
            get
            {
                return new Axis
                {
                    IsEnabled = false,
                    StrokeThickness = 3d,
                    Stroke = new SolidColorBrush(Color.FromRgb(218, 218, 218)),
                    Separator = CleanSeparator
                };
            }
        }

        //Returns a clean separator
        public static Separator CleanSeparator
        {
            get
            {
                return new Separator
                {
                    IsEnabled = false
                };
            }
        }
    }
}