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

using Brush = LiveCharts.Core.Drawing.Brush;

#endregion

namespace LiveCharts.Core.Interaction.Styles
{
    /// <summary>
    /// Defines a shape style.
    /// </summary>
    public class SeparatorStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorStyle"/> struct.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        public SeparatorStyle(Brush stroke, Brush fill, float strokeThickness)
        {
            Stroke = stroke;
            Fill = fill;
            StrokeThickness = strokeThickness;
        }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke { get; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public float StrokeThickness { get;}

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill { get; }
    }
}
