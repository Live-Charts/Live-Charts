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

namespace LiveCharts.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Events.RangeChangedEventArgs" />
    public class PreviewRangeChangedEventArgs : RangeChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewRangeChangedEventArgs"/> class.
        /// </summary>
        public PreviewRangeChangedEventArgs()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewRangeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="args">The <see cref="RangeChangedEventArgs"/> instance containing the event data.</param>
        public PreviewRangeChangedEventArgs(RangeChangedEventArgs args)
        {
            LeftLimitChange = args.LeftLimitChange;
            RightLimitChange = args.RightLimitChange;
            Range = args.Range;
            Axis = args.Axis;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis change was canceled by the user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        public bool Cancel { get; set; }
        /// <summary>
        /// Gets the preview minimum value.
        /// </summary>
        /// <value>
        /// The preview minimum value.
        /// </value>
        public double PreviewMinValue { get; internal set; }
        /// <summary>
        /// Gets the preview maximum value.
        /// </summary>
        /// <value>
        /// The preview maximum value.
        /// </value>
        public double PreviewMaxValue { get; internal set; }
    }
}