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
using System.Drawing;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The bubble series class.
    /// </summary>
    public class BubbleSeries<TModel> 
        : CartesianStrokeSeries<TModel, WeightedCoordinate, GeometryPointViewModel, IBubbleSeries>, IBubbleSeries
    {
        private ISeriesViewProvider<TModel, WeightedCoordinate, GeometryPointViewModel, IBubbleSeries> _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleSeries{TModel}"/> class.
        /// </summary>
        public BubbleSeries()
        {
            ScalesAt = new[] {0, 0, 0};
            MaxGeometrySize = 30f;
            MinGeometrySize = 14f;
            StrokeThickness = 1f;
            Geometry = Geometry.Circle;
            Charting.BuildFromSettings<IBubbleSeries>(this);
        }

        /// <inheritdoc />
        public double MaxGeometrySize { get; set; }

        /// <inheritdoc />
        public double MinGeometrySize { get; set; }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IBubbleSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new []{0f,0f};

        /// <inheritdoc />
        public override float PointMargin => (float) MaxGeometrySize;

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, WeightedCoordinate, GeometryPointViewModel, IBubbleSeries>
            DefaultViewProvider => _provider ??
                                   (_provider = Charting.Current.UiProvider
                                       .GeometryPointViewProvider<TModel, WeightedCoordinate, IBubbleSeries>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.Dimensions[0][ScalesAt[0]];
            var y = cartesianChart.Dimensions[1][ScalesAt[1]];

            var uw = chart.Get2DUiUnitWidth(x, y);

            var p1 = new PointF(context.Ranges[2][ScalesAt[2]][0], (float) MinGeometrySize);
            var p2 = new PointF(context.Ranges[2][ScalesAt[2]][1], (float) MaxGeometrySize);

            int xi = 0, yi = 1;
            if (chart.InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            Point<TModel, WeightedCoordinate, GeometryPointViewModel, IBubbleSeries> previous = null;

            foreach (var current in Points)
            {
                if (current.View == null)
                {
                    current.View = ViewProvider.GetNewPoint();
                }

                var p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x),
                    chart.ScaleToUi(current.Coordinate[1][0], y),
                    cartesianChart.LinealScale(p1, p2, current.Coordinate.Weight)
                };

                var vm = new GeometryPointViewModel
                {
                    Location = Perform.Sum(new PointF(p[xi], p[yi]), new PointF(uw[0] * .5f, uw[1] * .5f)),
                    Diameter = p[2]
                };

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);
                if (DataLabels) current.View.DrawLabel(current, DataLabelsPosition, LabelsStyle);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);
                current.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        vm.Location.X,
                        vm.Location.Y,
                        p[2],
                        p[2]));

                previous = current;
            }
        }
    }
}
