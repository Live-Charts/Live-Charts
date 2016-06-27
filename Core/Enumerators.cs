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

namespace LiveCharts
{
    /// <summary>
    /// Tooltip selection modes
    /// </summary>
    public enum TooltipSelectionMode
    {
        /// <summary>
        /// Gets only the hovered point 
        /// </summary>
        OnlySender,
        /// <summary>
        /// Gets all the points that shares the value X in the chart
        /// </summary>
        SharedXValues,
        /// <summary>
        /// Gets all the points that shares the value Y in the chart
        /// </summary>
        SharedYValues,
        /// <summary>
        /// Gets all the points that shares the value X in the hovered series
        /// </summary>
        SharedXInSeries,
        /// <summary>
        /// Gets all the points that shares the value Y in the hovered series
        /// </summary>
        SharedYInSeries
    }

    /// <summary>
    /// The series orientation
    /// </summary>
    public enum SeriesOrientation
    {
        /// <summary>
        /// Both, horizontal and vertical orientation
        /// </summary>
        All,
        /// <summary>
        /// Horizontal orientation
        /// </summary>
        Horizontal,
        /// <summary>
        /// Vertical orientation
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Axis position
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// Left for Y axis, Bottom for X axis
        /// </summary>
        LeftBottom,
        /// <summary>
        /// Right for Y axis, Top for X axis
        /// </summary>
        RightTop
    }

    /// <summary>
    /// Separator current state
    /// </summary>
    public enum SeparationState
    {
        /// <summary>
        /// Remove the separator from the chart
        /// </summary>
        Remove,
        /// <summary>
        /// Kepp the separator in the chart
        /// </summary>
        Keep,
        /// <summary>
        /// no animated add
        /// </summary>
        InitialAdd
    }

    /// <summary>
    /// Chart zooming options
    /// </summary>
    public enum ZoomingOptions
    {
        /// <summary>
        /// Disables zoom
        /// </summary>
        None,
        /// <summary>
        /// Only X axis
        /// </summary>
        X,
        /// <summary>
        /// Only Y axis
        /// </summary>
        Y,
        /// <summary>
        /// Both, X and Y axes
        /// </summary>
        Xy
    }

    /// <summary>
    /// Charts legend locations
    /// </summary>
    public enum LegendLocation
    {
        /// <summary>
        /// Disables legend
        /// </summary>
        None,
        /// <summary>
        /// PLaces legend at top
        /// </summary>
        Top,
        /// <summary>
        /// Places legend at bottom
        /// </summary>
        Bottom,
        /// <summary>
        /// Places legend at left
        /// </summary>
        Left,
        /// <summary>
        /// Places legend at right
        /// </summary>
        Right
    }

    /// <summary>
    /// Stacked mode, for stacked series
    /// </summary>
    public enum StackMode
    {
        /// <summary>
        /// Stacks the values, eg: if values are 1,2,3 the stacked total is 6
        /// </summary>
        Values,
        /// <summary>
        /// Stacks percentage, eg: if values are 1,2,3, they are actually being stacked as (1/6), (2/6), (3/6) [value/totalSum]
        /// </summary>
        Percentage
    }
    
}