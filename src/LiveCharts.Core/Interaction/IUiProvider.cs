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

using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Series;

#endregion

namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// Defines a drawing provider.
    /// </summary>
    public interface IUiProvider
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; }

        /// <summary>
        /// Gets the content of the chart.
        /// </summary>
        /// <returns></returns>
        IChartContent GetChartContent(IChartView chart);

        /// <summary>
        /// The axis separator provider.
        /// </summary>
        /// <returns></returns>
        IPlaneViewProvider GetNewPlane();

        /// <summary>
        /// Gets the new section.
        /// </summary>
        /// <returns></returns>
        IPlaneViewProvider GetNewSection();

        /// <summary>
        /// Gets the new label.
        /// </summary>
        /// <returns></returns>
        ILabel GetNewLabel(ChartModel context);

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns></returns>
        ICartesianPath GetNewPath(ChartModel context);

        /// <summary>
        /// Gets a new rectangle.
        /// </summary>
        /// <returns></returns>
        IRectangle GetNewRectangle(ChartModel context);

        /// <summary>
        /// Gets the new sg path.
        /// </summary>
        /// <returns></returns>
        ISvgPath GetNewSvgPath(ChartModel context);

        /// <summary>
        /// Gets the new bezier segment.
        /// </summary>
        /// <returns></returns>
        IBezierSegment GetNewBezierSegment(ChartModel context);

        /// <summary>
        /// Gets the new slice.
        /// </summary>
        /// <returns></returns>
        ISlice GetNewSlice(ChartModel context);

        /// <summary>
        /// Gets the new solid color brush.
        /// </summary>
        /// <returns></returns>
        ISolidColorBrush GetNewSolidColorBrush(byte alpha, byte red, byte green, byte blue);
    }
}
