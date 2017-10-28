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

namespace LiveCharts.Configurations
{
    /// <summary>
    /// Gets the already built point mappers
    /// </summary>
    public static class Mappers
    {
        /// <summary>
        /// Gets a mapper to configure X, Y points
        /// </summary>
        /// <typeparam name="T">Type to map</typeparam>
        /// <returns>A new cartesian mapper instance</returns>
        public static CartesianMapper<T> Xy<T>()
        {
            return new CartesianMapper<T>();
        }

        /// <summary>
        /// Gets a mapper to configure financial points
        /// </summary>
        /// <typeparam name="T">type to map</typeparam>
        /// <returns>a new financial mapper instance</returns>
        public static FinancialMapper<T> Financial<T>()
        {
            return new FinancialMapper<T>();
        }

        /// <summary>
        /// Gets a mapper to configure X, Y and Weight points
        /// </summary>
        /// <typeparam name="T">type to map</typeparam>
        /// <returns>a new weighted mapper instance</returns>
        public static WeightedMapper<T> Weighted<T>()
        {
            return new WeightedMapper<T>();
        }

        /// <summary>
        /// Gets a Gantt Mapper
        /// </summary>
        /// <typeparam name="T">type to amp</typeparam>
        /// <returns>a new polar mapper insance</returns>
        public static GanttMapper<T> Gantt<T>()
        {
            return new GanttMapper<T>();
        }

        /// <summary>
        /// Gets a mapper to configure Radius and Angle
        /// </summary>
        /// <typeparam name="T">type to amp</typeparam>
        /// <returns>a new polar mapper insance</returns>
        public static PolarMapper<T> Polar<T>()
        {
            return new PolarMapper<T>();
        }

        /// <summary>
        /// PGets a mapper to configure a pie chart
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PieMapper<T> Pie<T>()
        {
            return new PieMapper<T>();
        }
    }
}