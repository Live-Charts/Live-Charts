﻿#region License
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

using System.Windows.Controls;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Wpf.Animations;
using Slice = LiveCharts.Wpf.Shapes.Slice;

#endregion

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The pie view provider.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel,TSeries}" />
    public class PieSeriesViewProvider<TModel>
        : ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, IPieSeries series, TimeLine timeLine)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> GetNewPoint()
        {
            return new PiePointView<TModel>();
        }

        /// <inheritdoc />
        public void OnPointHighlight(IChartPoint point, TimeLine timeLine)
        {
            PiePointView<TModel> view = (PiePointView<TModel>) point.View;
            view.Shape
                .Animate(timeLine)
                .Property(
                    Slice.PushOutProperty, view.Shape.PushOut, ((IPieSeries) point.Series).PushOut + 15)
                .Begin();
        }

        /// <inheritdoc />
        public void RemovePointHighlight(IChartPoint point, TimeLine timeLine)
        {
            PiePointView<TModel> view = (PiePointView<TModel>)point.View;
            view.Shape
                .Animate(timeLine)
                .Property(
                    Slice.PushOutProperty, view.Shape.PushOut, ((IPieSeries) point.Series).PushOut)
                .Begin();
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, IPieSeries series, TimeLine timeLine)
        {
        }
    }
}