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

using System.Windows.Media;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Contains an already defined collection of geometries, useful to set the Series.PointGeomety property
    /// </summary>
    public static class DefaultGeometries
    {
        /// <summary>
        /// Returns a null geometry
        /// </summary>
        public static Geometry None
        {
            get { return null; }
        }

        /// <summary>
        /// Returns a circle geometry
        /// </summary>
        public static Geometry Circle
        {
            get
            {
                var g = Geometry.Parse("M 0,0 A 180,180 180 1 1 1,1 Z");
                g.Freeze();
                return g;
            }
        }

        /// <summary>
        /// Returns a square geometry
        /// </summary>
        public static Geometry Square
        {
            get
            {
                var g = Geometry.Parse("M 1,1 h -2 v -2 h 2 z");
                g.Freeze();
                return g;
            }
        }

        /// <summary>
        /// Returns a diamond geometry
        /// </summary>
        public static Geometry Diamond
        {
            get
            {
                var g = Geometry.Parse("M 1,0 L 2,1  1,2  0,1 z");
                g.Freeze();
                return g;
            }
        }

        /// <summary>
        /// Returns a triangle geometry
        /// </summary>
        public static Geometry Triangle
        {
            get
            {
                var g = Geometry.Parse("M 0,1 l 1,1 h -2 Z");
                g.Freeze();
                return g;
            }
        }

        /// <summary>
        /// Returns a cross geometry
        /// </summary>
        public static Geometry Cross
        {
            get
            {
                var g = Geometry.Parse("M0,0 L1,1 M0,1 l1,-1");
                g.Freeze();
                return g;
            }
        }
    }
}
