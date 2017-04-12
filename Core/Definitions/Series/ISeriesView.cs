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

using System;
using LiveCharts.Definitions.Points;

namespace LiveCharts.Definitions.Series
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISeriesView
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        SeriesAlgorithm Model { get; set; }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        IChartValues Values { get; set; }
        /// <summary>
        /// Gets a value indicating whether [data labels].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data labels]; otherwise, <c>false</c>.
        /// </value>
        bool DataLabels { get; }
        /// <summary>
        /// Gets or sets the scales x at.
        /// </summary>
        /// <value>
        /// The scales x at.
        /// </value>
        int ScalesXAt { get; set; }
        /// <summary>
        /// Gets or sets the scales y at.
        /// </summary>
        /// <value>
        /// The scales y at.
        /// </value>
        int ScalesYAt { get; set; }
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        object Configuration { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is series visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is series visible; otherwise, <c>false</c>.
        /// </value>
        bool IsSeriesVisible { get; }
        /// <summary>
        /// Gets or sets the label point.
        /// </summary>
        /// <value>
        /// The label point.
        /// </value>
        Func<ChartPoint, string> LabelPoint { get; set; }
        /// <summary>
        /// Gets the actual values.
        /// </summary>
        /// <value>
        /// The actual values.
        /// </value>
        IChartValues ActualValues { get; }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is first draw.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first draw; otherwise, <c>false</c>.
        /// </value>
        bool IsFirstDraw { get; }

        /// <summary>
        /// Gets the point view.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        IChartPointView GetPointView(ChartPoint point, string label);
        /// <summary>
        /// Called when [series update start].
        /// </summary>
        void OnSeriesUpdateStart();
        /// <summary>
        /// Erases the specified remove from view.
        /// </summary>
        /// <param name="removeFromView">if set to <c>true</c> [remove from view].</param>
        void Erase(bool removeFromView);
        /// <summary>
        /// Called when [series updated finish].
        /// </summary>
        void OnSeriesUpdatedFinish();
        /// <summary>
        /// Initializes the colors.
        /// </summary>
        void InitializeColors();
        /// <summary>
        /// Draws the specialized elements.
        /// </summary>
        void DrawSpecializedElements();
        /// <summary>
        /// Places the specialized elements.
        /// </summary>
        void PlaceSpecializedElements();
        /// <summary>
        /// Gets the label point formatter.
        /// </summary>
        /// <returns></returns>
        Func<ChartPoint, string> GetLabelPointFormatter();
    }
}