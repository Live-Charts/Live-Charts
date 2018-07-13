#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodr�guez Orozco & LiveCharts contributors
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

using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Styles;

#endregion

namespace LiveCharts.Core.Interaction.Points
{
    /// <summary>
    /// Defines a point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the point model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    public interface IPointView<TModel, TCoordinate, TViewModel, TSeries> : IResource
        where TSeries : ISeries
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets the visual element.
        /// </summary>
        /// <value>
        /// The visual.
        /// </value>
        object VisualElement { get; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        object Label { get; }

        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="chartPoint">The point.</param>
        /// <param name="previous">The previous.</param>
        /// <param name="timeLine">The animation.</param>
        void DrawShape(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> previous,
            TimeLine timeLine);

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="chartPoint">The point.</param>
        /// <param name="position">The labels position.</param>
        /// <param name="style">The data label style.</param>
        /// <param name="timeLine">The animation.</param>
        void DrawLabel(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint, 
            DataLabelsPosition position, 
            LabelStyle style,
            TimeLine timeLine);
    }
}
