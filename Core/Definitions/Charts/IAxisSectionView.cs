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

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAxisSectionView
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        AxisSectionCore Model { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        double Value { get; set; }
        /// <summary>
        /// Gets or sets the width of the section.
        /// </summary>
        /// <value>
        /// The width of the section.
        /// </value>
        double SectionWidth { get; set; }
        /// <summary>
        /// Gets or sets the section offset.
        /// </summary>
        /// <value>
        /// The section offset.
        /// </value>
        double SectionOffset { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IAxisSectionView"/> is draggable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if draggable; otherwise, <c>false</c>.
        /// </value>
        bool Draggable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the section is animated
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        bool DisableAnimations { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the section should display a label that displays its current value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data label]; otherwise, <c>false</c>.
        /// </value>
        bool DataLabel { get; set; }

        /// <summary>
        /// Draws the or move.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="axis">The axis.</param>
        void DrawOrMove(AxisOrientation source, int axis);
        /// <summary>
        /// Removes this instance.
        /// </summary>
        void Remove();
        /// <summary>
        /// Ases the core element.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        AxisSectionCore AsCoreElement(AxisCore axis, AxisOrientation source);
    }
}