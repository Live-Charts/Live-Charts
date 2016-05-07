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
using LiveCharts.Charts;

namespace LiveCharts
{
    public class SeriesConfiguration<T> : ISeriesConfiguration
    {
        #region Properties

        public ChartCore Chart { get; set; }

        internal Func<T, int, double> Value1 { get; set; }

        internal Func<T, int, double> Value2 { get; set; }

        internal Func<T, int, double> Value3 { get; set; } 

        internal Func<T, int, double> Value4 { get; set; }

        #endregion
        
        /// <summary>
        /// Maps X value in cartesian charts
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, double> predicate)
        {
            return X((t, i) => predicate(t));
        }
        public SeriesConfiguration<T> X(Func<T, int, double> predicate)
        {
            Value1 = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Value in cartesian charts
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, double> predicate)
        {
            return Y((t, i) => predicate(t));
        }
        public SeriesConfiguration<T> Y(Func<T, int, double> predicate)
        {
            Value2 = predicate;
            return this;
        }

        /// <summary>
        /// Maps weight in cartesian charts
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Weight(Func<T, double> predicate)
        {
            return Weight((t, i) => predicate(t));
        }

        public SeriesConfiguration<T> Weight(Func<T, int, double> predicate)
        {
            Value3 = predicate;
            return this;
        }
    }

    public interface ISeriesConfiguration
    {
        ChartCore Chart { get; set; }
    }
}