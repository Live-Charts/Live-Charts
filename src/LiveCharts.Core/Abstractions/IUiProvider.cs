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

using LiveCharts.Core.Coordinates;
using LiveCharts.Core.ViewModels;

#endregion

namespace LiveCharts.Core.Abstractions
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
        IChartContent GetChartContent();

        /// <summary>
        /// The axis label provider.
        /// </summary>
        /// <returns></returns>
        IPlaneLabelControl GetNewAxisLabel();

        /// <summary>
        /// The label provider.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        IDataLabelControl GetNewDataLabel<TModel, TCoordinate, TViewModel>()
            where TCoordinate : ICoordinate;

        /// <summary>
        /// The axis separator provider.
        /// </summary>
        /// <returns></returns>
        ICartesianAxisSeparator GetNewAxisSeparator();

        /// <summary>
        /// The path provider.
        /// </summary>
        /// <returns></returns>
        ICartesianPath GetNewPath();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a column view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, PointCoordinate, BarViewModel> BarViewProvider<TModel>();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a bezier view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, PointCoordinate, BezierViewModel> BezierViewProvider<TModel>();

        /// <summary>
        /// Defines the Scatter Ui Provider.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        ISeriesViewProvider<TModel, WeightedCoordinate, ScatterViewModel> ScatterViewProvider<TModel>();
    }
}
