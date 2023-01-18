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

using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// Defines a point in the chart
    /// </summary>
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

        /// <summary>
        /// Gets the Gantt x start value
        /// </summary>
        public double XStart { get; set; }

        /// <summary>
        /// Gets the Gantt y start value
        /// </summary>
        public double YStart { get; set; }

        internal bool EvaluatesGantt { get; set; }

        #region Gantt

        #endregion

        #region Weighted

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

        /// <summary>
        /// Gets the Radius of a point
        /// </summary>
        public double Radius { get; internal set; }

        /// <summary>
        /// Gets the angle of a point
        /// </summary>
        public double Angle { get; internal set; }

        #endregion

        #region Appearance
        /// <summary>
        /// Gets the Fill brush of this point, this property overrides series Fill property 
        /// </summary>
        public object Fill { get; internal set; }
        /// <summary>
        /// Gets the Stroke brush of this point, this property overrides series Stroke property
        /// </summary>
        public object Stroke { get; internal set; }
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

        /// <summary>
        /// Gets the chart view.
        /// </summary>
        /// <value>
        /// The chart view.
        /// </value>
        public IChartView ChartView { get { return SeriesView.Model.Chart.View; } }

        internal double Gci { get; set; }
    }
}