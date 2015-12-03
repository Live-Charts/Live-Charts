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

using System.Windows.Media;

namespace LiveCharts
{
    public class Separator
    {
        public Separator()
        {
            Step = null;
        }

        /// <summary>
        /// Indicates weather to draw separators or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets color separators color 
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets separatos thickness
        /// </summary>
        public int Thickness { get; set; }

        /// <summary>
        /// Gets or sets sepator step, use null to make it auto
        /// </summary>
        public double? Step { get; set; }
    }
}