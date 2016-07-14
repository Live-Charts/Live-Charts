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
using LiveCharts.Dtos;

namespace LiveCharts.Configurations
{
    /// <summary>
    /// Mapper to configure X and Y points
    /// </summary>
    /// <typeparam name="T">Type to configure</typeparam>
    public class CartesianMapper<T> : IPointEvaluator<T>
    {
        private Func<T, int, double> _x = (v, i) => i;
        private Func<T, int, double> _y = (v, i) => i;
        private Func<T, int, object> _stroke;
        private Func<T, int, object> _fill;

        /// <summary>
        /// Sets values for a specific point
        /// </summary>
        /// <param name="valuePair">Key and value</param>
        /// <param name="point">Point to set</param>
        public void SetAll(KeyValuePair<int, T> valuePair, ChartPoint point)
        {
            point.X = _x(valuePair.Value, valuePair.Key);
            point.Y = _y(valuePair.Value, valuePair.Key);
            if (_stroke != null) point.Stroke = _stroke(valuePair.Value, valuePair.Key);
            if (_fill != null) point.Fill = _fill(valuePair.Value, valuePair.Key);
        }

        /// <summary>
        /// Evaluates a point with a given value and key
        /// </summary>
        /// <param name="valuePair">Value and Key</param>
        /// <returns>evaluated point</returns>
        public Xyw[] GetEvaluation(KeyValuePair<int, T> valuePair)
        {
            var xyw = new Xyw(_x(valuePair.Value, valuePair.Key), _y(valuePair.Value, valuePair.Key), 0);
            return new [] {xyw, xyw};
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public CartesianMapper<T> X(Func<T, double> predicate)
        {
            return X((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public CartesianMapper<T> X(Func<T, int, double> predicate)
        {
            _x = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public CartesianMapper<T> Y(Func<T, double> predicate)
        {
            return Y((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public CartesianMapper<T> Y(Func<T, int, double> predicate)
        {
            _y = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> Stroke(Func<T, object> predicate)
        {
            return Stroke((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> Stroke(Func<T, int, object> predicate)
        {
            _stroke = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> Fill(Func<T, object> predicate)
        {
            return Fill((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CartesianMapper<T> Fill(Func<T, int, object> predicate)
        {
            _fill = predicate;
            return this;
        }
    }
}