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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Wpf.Animations;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.Drawing;
using LiveCharts.Wpf.Views;
using LiveCharts.Wpf.Views.Providers;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class UiProvider : IUiProvider
    {
        private string _name;
        private string _version;

        /// <inheritdoc />
        public string Name
        {
            get
            {
                if (_name != null) return _name;
                _name = Assembly.GetExecutingAssembly().GetName().Name;
                return _name;
            }
        }

        /// <inheritdoc />
        public string Version
        {
            get
            {
                if (_version != null) return _version;
                var assembly = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                _version = fvi.FileVersion;
                return _version;
            }
        }

        /// <inheritdoc />
        public IChartCanvas GetChartContent(IChartView view)
        {
            return new ChartContent(view);
        }

        /// <inheritdoc />
        public IPlaneViewProvider GetNewPlane()
        {
            return new PlaneViewProvider();
        }

        /// <inheritdoc />
        public IPlaneViewProvider GetNewSection()
        {
            return new PlaneViewProvider();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, TCoordinate, RectangleViewModel, TSeries, IRectangle, ILabel> 
            BarViewProvider<TModel, TCoordinate, TSeries>()
            where TCoordinate : ICoordinate
            where TSeries : ICartesianSeries, IStrokeSeries
        {
            return new BarSeriesViewProvider<TModel, TCoordinate, TSeries>();
        }

        public ILabel GetNewLabel()
        {
            return new Label();
        }

        public IRectangle GetNewRectangle()
        {
            return new Rectangle();
        }

        /// <inheritdoc />
        public IBezierSeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries> 
            BezierViewProvider<TModel>()
        {
            return new SelfDrawnBezierSeriesViewProvider<TModel>();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, TCoordinate, GeometryPointViewModel, TSeries> 
            GeometryPointViewProvider<TModel, TCoordinate, TSeries>()
            where TCoordinate : ICoordinate
            where TSeries : ICartesianSeries, IStrokeSeries
        {
            return new GeometryPointSeriesViewProvider<TModel, TCoordinate, TSeries>();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> 
            PieViewProvider<TModel>()
        {
            return new PieSeriesViewProvider<TModel>();
        }

        /// <inheritdoc />
        public ISeriesViewProvider<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> 
            HeatViewProvider<TModel>()
        {
            return new HeatViewSeriesProvider<TModel>();
        }
    }
}
