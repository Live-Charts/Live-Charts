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
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
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
        /// Provides LiveCharts with a builder that returns a column view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TSeries">The type of the series.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, TCoordinate, RectangleViewModel, TSeries>
            BarViewProvider<TModel, TCoordinate, TSeries>()
            where TCoordinate : ICoordinate
            where TSeries : ICartesianSeries, IStrokeSeries;

        /// <summary>
        /// Provides LiveCharts with a builder that returns a bezier view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IBezierSeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries> 
            BezierViewProvider<TModel>();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a scatter view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TSeries">The type of the series.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, TCoordinate, GeometryPointViewModel, TSeries> 
            GeometryPointViewProvider<TModel, TCoordinate, TSeries>()
            where TCoordinate : ICoordinate
            where TSeries : ICartesianSeries, IStrokeSeries;

        /// <summary>
        /// Provides LiveCharts with a builder that returns a pie view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> 
            PieViewProvider<TModel>();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a heat view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> 
            HeatViewProvider<TModel>();
    }
}
