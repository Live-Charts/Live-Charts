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

namespace LiveCharts.Defaults
{
    /// <summary>
    /// An already configured weighted chart point, this class notifies the chart to update every time a property changes
    /// </summary>
    public class HeatPoint : ScatterPoint
    {
        /// <summary>
        /// Initializes a new instance of HeatPoint class
        /// </summary>
        public HeatPoint()
        {

        }
        
        /// <summary>
        /// _initializes a new instance of HeatPoint class, giving x, y and weight
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="weight"></param>
        public HeatPoint(double x, double y, double weight)
        {
            X = x;
            Y = y;
            Weight = weight;
        }
    }
}