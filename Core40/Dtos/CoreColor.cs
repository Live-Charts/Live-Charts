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


namespace LiveCharts.Dtos
{
    /// <summary>
    /// Defines a portable color
    /// </summary>
    public struct CoreColor
    {
        /// <summary>
        /// Initializes a new instance of CoreColor
        /// </summary>
        /// <param name="a">alpha component</param>
        /// <param name="r">red component</param>
        /// <param name="g">green component</param>
        /// <param name="b">blue component</param>
        public CoreColor(byte a, byte r, byte g, byte b) : this()
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Alpha component
        /// </summary>
        public byte A { get; set; }
        /// <summary>
        /// Red component
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Green component
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Red component
        /// </summary>
        public byte B { get; set; }
    }
}