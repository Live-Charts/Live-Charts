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
    public class ChartPoint
    {

        #region Cartesian 

        /// <summary>
        /// Gets the X point value
        /// </summary>
        public double X { get; internal set; }
        /// <summary>
        /// Gets the Y point value
        /// </summary>
        public double Y { get; internal set; }

        #endregion

        #region bubbles

        /// <summary>
        /// Gets the Weight of the point
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region stacked

        /// <summary>
        /// Gets where the stacked value started from
        /// </summary>
        public double From { get; internal set; }
        /// <summary>
        /// Gets where the stacked value finishes
        /// </summary>
        public double To { get; internal set; }
        /// <summary>
        /// Get the total sum of the stacked elements
        /// </summary>
        public double Sum { get; internal set; }
        /// <summary>
        /// Get the participation of the point in the stacked elements
        /// </summary>
        public double Participation { get; internal set; }
        /// <summary>
        /// gets the stacked participation of a point
        /// </summary>
        public double StackedParticipation { get; internal set; }

        #endregion

        #region Financial

        /// <summary>
        /// Gets the Open value of the point
        /// </summary>
        public double Open { get; internal set; }
        /// <summary>
        /// Gets the High value of the point
        /// </summary>
        public double High { get; internal set; }
        /// <summary>
        /// Gets the Low value of the point
        /// </summary>
        public double Low { get; internal set; }
        /// <summary>
        /// Gets the Close value of the point
        /// </summary>
        public double Close { get; internal set; }

        #endregion

        #region Polar

        public double Radius { get; set; }
        public double Angle { get; set; }
        
        #endregion

        /// <summary>
        /// Gets the coordinate where the value is placed at chart
        /// </summary>
        public CorePoint ChartLocation { get; internal set; }
        /// <summary>
        /// Gets the index of this point in the chart
        /// </summary>
        public int Key { get; internal set; }
        /// <summary>
        /// Gets the object where the chart pulled the point
        /// </summary>
        public object Instance { get; internal set; }
        /// <summary >
        /// Gets or sets the view of this chart point
        /// </summary>
        public IChartPointView View { get; internal set; }
        /// <summary>
        /// Gets the series where the point belongs to
        /// </summary>
        public ISeriesView SeriesView { get; internal set; }

        internal double GarbageCollectorIndex { get; set; }
    }

    public enum ChartPointType
    {
        Bezier, Bar, PieSlice
    }
}