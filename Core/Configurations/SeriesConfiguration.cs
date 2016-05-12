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
using System.Collections.Generic;
using LiveCharts.Charts;

namespace LiveCharts.Configurations
{
    [Obsolete("Instead use LiveCharts.Configurations.SeriesMappers class")]
    public class SeriesConfiguration<T> : IPointEvaluator<T>
    {
        #region Properties

        public ChartCore Chart { get; set; }
        internal Func<KeyValuePair<int, T>, double> XMapper { get; set; }
        internal Func<KeyValuePair<int, T>, double> YMapper { get; set; }

        #endregion

        public void SetAll(KeyValuePair<int, T> valuePair, ChartPoint point)
        {
            point.X = XMapper(valuePair);
            point.Y = YMapper(valuePair);
        }

        public Xyw[] GetEvaluation(KeyValuePair<int, T> valuePair)
        {
            var xyw = new Xyw(XMapper(valuePair), YMapper(valuePair), 0);
            return new[] {xyw, xyw};
        }

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
            XMapper = vp => predicate(vp.Value, vp.Key);
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
            YMapper = vp => predicate(vp.Value, vp.Key);
            return this;
        }
    }
}