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

using LiveCharts.Charts;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction.Controls;
using System;

#endregion

namespace LiveCharts
{
    /// <summary>
    /// Defines the user interface factory.
    /// </summary>
    public static class UIFactory
    {
        internal static IChartCanvas GetNewChartContent(IChartView view) => WhenDrawingChartContents(view);

        internal static ILabel GetNewLabel(ChartModel context) => WhenDrawingLabels(context);

        internal static ICartesianPath GetNewCartesianPath(ChartModel context) => WhenDrawingCartesianPaths(context);

        internal static IRectangle GetNewRectangle(ChartModel context) => WhenDrawingRectangles(context);

        internal static IColoredShape GetNewColoredShape(ChartModel context) => WhenDrawingColoredShapes(context);

        internal static ISvgPath GetNewSvgPath(ChartModel context) => WhenDrawingSvgPaths(context);

        internal static IBezierSegment GetNewBezierSegment(ChartModel context) => WhenDrawingBezierSegments(context);

        internal static ISlice GetNewSlice(ChartModel context) => WhenDrawingSlices(context);

        internal static ISolidColorBrush GetNewSolidColorBrush(byte alpha, byte red, byte green, byte blue) => WhenPainting(alpha, red, green, blue);

        /// <summary>
        /// Called when a chart host is required in the UI.
        /// </summary>
        public static event Func<IChartView, IChartCanvas> WhenDrawingChartContents;

        /// <summary>
        /// Gets the new label.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, ILabel> WhenDrawingLabels;

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, ICartesianPath> WhenDrawingCartesianPaths;

        /// <summary>
        /// Gets a new rectangle.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, IRectangle> WhenDrawingRectangles;

        /// <summary>
        /// Called when a colored shape needs to be added to the UI.
        /// </summary>
        public static event Func<ChartModel, IColoredShape> WhenDrawingColoredShapes;

        /// <summary>
        /// Gets the new sg path.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, ISvgPath> WhenDrawingSvgPaths;

        /// <summary>
        /// Gets the new bezier segment.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, IBezierSegment> WhenDrawingBezierSegments;

        /// <summary>
        /// Gets the new slice.
        /// </summary>
        /// <returns></returns>
        public static event Func<ChartModel, ISlice> WhenDrawingSlices;

        /// <summary>
        /// Called when a solid color brush is required, the parameters are alpha, red, green and blue.
        /// </summary>
        public static event Func<byte, byte, byte, byte, ISolidColorBrush> WhenPainting;
    }
}
