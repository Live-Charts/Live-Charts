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

namespace LiveCharts.Configurations
{
    /// <summary>
    /// Mapper to configure X and Y points
    /// </summary>
    /// <typeparam name="T">Type to configure</typeparam>
    public class GanttMapper<T> : IPointEvaluator<T>
    {
        private Func<T, int, double> _x = (v, i) => i;
        private Func<T, int, double> _y = (v, i) => i;
        private Func<T, int, double> _startX = (v, i) => 0;
        private Func<T, int, double> _startY = (v, i) => 0;
        private Func<T, int, object> _stroke;
        private Func<T, int, object> _fill;

        /// <summary>
        /// Sets values for a specific point
        /// </summary>
        /// <param name="point">Point to set</param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        public void Evaluate(int key, T value, ChartPoint point)
        {
            point.X = _x(value, key);
            point.Y = _y(value, key);
            point.XStart = _startX(value, key);
            point.YStart = _startY(value, key);
            if (_stroke != null) point.Stroke = _stroke(value, key);
            if (_fill != null) point.Fill = _fill(value, key);

            point.EvaluatesGantt = true;
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> X(Func<T, double> predicate)
        {
            return X((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> X(Func<T, int, double> predicate)
        {
            _x = predicate;
            return this;
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> XStart(Func<T, double> predicate)
        {
            return XStart((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> XStart(Func<T, int, double> predicate)
        {
            _startX = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> Y(Func<T, double> predicate)
        {
            return Y((t, i) => predicate(t));
        }
        
        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> Y(Func<T, int, double> predicate)
        {
            _y = predicate;
            return this;
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> YStart(Func<T, double> predicate)
        {
            return YStart((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public GanttMapper<T> YStart(Func<T, int, double> predicate)
        {
            _startY = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GanttMapper<T> Stroke(Func<T, object> predicate)
        {
            return Stroke((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GanttMapper<T> Stroke(Func<T, int, object> predicate)
        {
            _stroke = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GanttMapper<T> Fill(Func<T, object> predicate)
        {
            return Fill((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GanttMapper<T> Fill(Func<T, int, object> predicate)
        {
            _fill = predicate;
            return this;
        }
    }
}