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

using System;
using System.Collections.Generic;

namespace LiveCharts.Configurations
{
    public class CartesianMapper<T> : IPointEvaluator<T>
    {
        private Func<T, int, double> _x = (v, i) => i;
        private Func<T, int, double> _y = (v, i) => i;

        public void SetAll(KeyValuePair<int, T> valuePair, ChartPoint point)
        {
            point.X = _x(valuePair.Value, valuePair.Key);
            point.Y = _y(valuePair.Value, valuePair.Key);
        }

        public Xyw[] GetEvaluation(KeyValuePair<int, T> valuePair)
        {
            var xyw = new Xyw(_x(valuePair.Value, valuePair.Key), _y(valuePair.Value, valuePair.Key), 0);
            return new [] {xyw, xyw};
        }

        /// <summary>
        /// Maps X value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> X(Func<T, double> predicate)
        {
            return X((t, i) => predicate(t));
        }
        public CartesianMapper<T> X(Func<T, int, double> predicate)
        {
            _x = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> Y(Func<T, double> predicate)
        {
            return Y((t, i) => predicate(t));
        }
        public CartesianMapper<T> Y(Func<T, int, double> predicate)
        {
            _y = predicate;
            return this;
        }
    }
}