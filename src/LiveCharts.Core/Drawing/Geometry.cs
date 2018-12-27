#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Defines an SVG geometry.
    /// </summary>
    public struct Geometry
    {
        private readonly bool _isEmpty;

        private Geometry(bool isEmpty)
        {
            _isEmpty = isEmpty;
            Data = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry"/> class.
        /// </summary>
        public Geometry(string data)
        {
            _isEmpty = false;
            Data = data;
        }

        /// <summary>
        /// The empty
        /// </summary>
        public static Geometry Empty = new Geometry(true);

        /// <summary>
        /// Gets the circle.
        /// </summary>
        /// <value>
        /// The circle.
        /// </value>
        public static Geometry Circle = new Geometry("M 0,0 A 180,180 180 1 1 1,1 Z");

        /// <summary>
        /// Gets the square.
        /// </summary>
        /// <value>
        /// The square.
        /// </value>
        public static Geometry Square = new Geometry("M 0,0 h 1 v -1 h -1 Z");

        /// <summary>
        /// Gets the diamond.
        /// </summary>
        /// <value>
        /// The diamond.
        /// </value>
        public static Geometry Diamond = new Geometry("M 1,0 L 2,1  1,2  0,1 Z");

        /// <summary>
        /// Gets the triangle.
        /// </summary>
        /// <value>
        /// The triangle.
        /// </value>
        public static Geometry Triangle = new Geometry("M 0,1 L 1,1 H -2 Z");

        /// <summary>
        /// Gets the cross.
        /// </summary>
        /// <value>
        /// The cross.
        /// </value>
        public static Geometry Cross = new Geometry("M0, 0 L1, 1 M0, 1 L1, -1");

        /// <summary>
        /// Gets the cross.
        /// </summary>
        /// <value>
        /// The cross.
        /// </value>
        public static Geometry HorizontalLine = new Geometry("M 0,0.5 H 1,0.5 Z");

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="g1">The g1.</param>
        /// <param name="g2">The g2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Geometry g1, Geometry g2)
        {
            return Equals(g1, g2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="g1">The g1.</param>
        /// <param name="g2">The g2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Geometry g1, Geometry g2)
        {
            return !Equals(g1, g2);
        }

        /// <summary>
        /// Compares the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Geometry other)
        {
            return _isEmpty == other._isEmpty && string.Equals(Data, other.Data);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Geometry geometry && Equals(geometry);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_isEmpty.GetHashCode() * 397) ^ (Data != null ? Data.GetHashCode() : 0);
            }
        }
    }
}
