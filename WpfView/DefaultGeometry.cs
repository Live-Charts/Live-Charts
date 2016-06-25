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
using System.Windows.Media;

namespace LiveCharts.Wpf
{

    //geometries
    public static class DefaultGeometries
    {
        public static Geometry None
        {
            get { return Geometry.Parse("M 0,0 z"); }
        }

        public static Geometry Circle
        {
            get { return Geometry.Parse("M 0,0 A 180,180 180 1 1 1,1 Z"); }
        }

        public static Geometry Square
        {
            get { return Geometry.Parse("M 1,1 h -2 v -2 h 2 z"); }
        }

        public static Geometry Diamond
        {
            get { return Geometry.Parse("M 1,0 L 2,1  1,2  0,1 z"); }
        }

        public static Geometry Trangle
        {
            get { return Geometry.Parse("M 0,1 l 1,1 h -2 Z"); }
        }
    }


    public class GeometryAttribute : Attribute
    {
        public Geometry Geometry { get; private set; }

        public GeometryAttribute()
        {
            Geometry = Geometry.Parse("M 0,0 z");
        }

        public GeometryAttribute(string source)
        {
            Geometry = Geometry.Parse(source);
        }
    }
}
