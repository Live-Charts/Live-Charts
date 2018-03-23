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

using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.Providers;
using LiveCharts.Wpf.Views;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class UiProvider : IUiProvider
    {
        /// <inheritdoc />
        public IChartContent GetChartContent()
        {
            return new ChartContent();
        }

        /// <inheritdoc />
        public IPlaneLabelControl GetNewAxisLabel()
        {
            return new AxisLabel();
        }

        /// <inheritdoc />
        public IDataLabelControl GetNewDataLabel<TModel, TCoordinate, TViewModel>()
            where TCoordinate : ICoordinate
        {
            return new DataLabel();
        }

        /// <inheritdoc />
        public ICartesianAxisSeparator GetNewAxisSeparator()
        {
            return new CartesianAxisSeparatorView<AxisLabel>();
        }

        /// <inheritdoc />
        public ICartesianPath GetNewPath()
        {
            return new CartesianPath();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, Point, BarViewModel> BarViewProvider<TModel>()
        {
            return new BarSeriesViewProvider<TModel>();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, Point, BezierViewModel> BezierViewProvider<TModel>()
        {
            return new BezierSeriesViewProvider<TModel>();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, WeightedPoint, ScatterViewModel> ScatterViewProvider<TModel>()
        {
            return new ScatterSeriesViewProvider<TModel>();
        }
    }
}
