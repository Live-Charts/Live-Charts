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

#region

using System;
using System.Drawing;

#endregion

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a gradient stop.
    /// </summary>
    public struct GradientStop
    {
        private double _offset;

        /// <summary>
        /// Gets or sets the offset, the value goes from 0 (cold) to 1 (hot).
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public double Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                if (_offset > 1 || _offset < 0)
                {
                    throw new ArgumentOutOfRangeException($"A {nameof(GradientStop)}.{nameof(Offset)} should be greater or equals to 0 and equals or less than 1.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; }
    }
}