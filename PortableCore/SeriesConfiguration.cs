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

using System;

namespace LiveChartsCore
{
    public class SeriesConfiguration<T> : SeriesConfiguration
    {
        public SeriesConfiguration()
        {
            XValueMapper = (value, index) => index;
            YValueMapper = (value, index) => index;
        }

        /// <summary>
        /// Gets or sets optimization method
        /// </summary>
        public IDataOptimization<T> DataOptimization { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls X value from T
        /// </summary>
        internal Func<T, int, double> XValueMapper { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls Y value from T
        /// </summary>
        internal Func<T, int, double> YValueMapper { get; set; }

        /// <summary>
        /// Maps X value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, double> predicate)
        {
            XValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, int, double> predicate)
        {
            XValueMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, double> predicate)
        {
            YValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Max X Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, int, double> predicate)
        {
            YValueMapper = predicate;
            return this;
        }

        public SeriesConfiguration<T> HasHighPerformanceMethod(IDataOptimization<T> optimization)
        {
            DataOptimization = optimization;
            return this;
        }
    }

    public class SeriesConfiguration
    {
        public IChartModel Chart { get; set; }
    }
}