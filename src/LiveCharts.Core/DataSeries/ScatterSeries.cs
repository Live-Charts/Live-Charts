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
using System.Drawing;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The scatter series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianStrokeSeries{TModel,TCoordinate,TViewModel, TSeries}" />
    /// <seealso cref="IScatterSeries" />
    public class ScatterSeries<TModel>
        : CartesianStrokeSeries<TModel, PointCoordinate, GeometryPointViewModel, IScatterSeries>, IScatterSeries
    {
        private ISeriesViewProvider<TModel, PointCoordinate, GeometryPointViewModel, IScatterSeries> _provider;
        private double _geometrySize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries{TModel}"/> class.
        /// </summary>
        public ScatterSeries()
        {
            GeometrySize = 18;
            StrokeThickness = 1;
            Geometry = Geometry.Circle;
            DataLabelFormatter = coordinate =>
                $"{Format.AsMetricNumber(coordinate.X)}, {Format.AsMetricNumber(coordinate.Y)}";
            TooltipFormatter = DataLabelFormatter;
            Charting.BuildFromTheme<IScatterSeries>(this);
            Charting.BuildFromTheme<ISeries<PointCoordinate>>(this);
        }

        /// <summary>
        /// Gets or sets the size of the <see cref="P:LiveCharts.Core.Abstractions.DataSeries.ISeries.Geometry" /> property.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        public double GeometrySize
        {
            get => _geometrySize;
            set
            {
                _geometrySize = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(IScatterSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {0f, 0f};

        /// <inheritdoc />
        public override float PointMargin => (float) GeometrySize;

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, PointCoordinate, GeometryPointViewModel, IScatterSeries>
            DefaultViewProvider => _provider ??
                                   (_provider = Charting.Settings.UiProvider
                                       .GeometryPointViewProvider<TModel, PointCoordinate, IScatterSeries>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.Dimensions[0][ScalesAt[0]];
            var y = cartesianChart.Dimensions[1][ScalesAt[1]];

            int xi = 0, yi = 1;
            if (chart.InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            float r = (float) GeometrySize * .5f;

            ChartPoint<TModel, PointCoordinate, GeometryPointViewModel, IScatterSeries> previous = null;
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };
            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int i = 0;

            foreach (ChartPoint<TModel, PointCoordinate, GeometryPointViewModel, IScatterSeries> current in GetPoints(chart.View))
            {
                if (current.View == null)
                {
                    current.View = ViewProvider.GetNewPoint();
                }

                float[] p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x),
                    chart.ScaleToUi(current.Coordinate[1][0], y)
                };

                var vm = new GeometryPointViewModel
                {
                    Location = new PointF(p[xi] - r, p[yi] - r),
                    Diameter = (float) GeometrySize
                };

                if (DelayRule != DelayRules.None)
                {
                    timeLine = AnimationExtensions.Delay(
                        // ReSharper disable once PossibleMultipleEnumeration
                        originalDuration, originalAnimationLine, i / (float)PointsCount, DelayRule);
                }

                current.ViewModel = vm;
                current.View.DrawShape(current, previous, timeLine);
                if (DataLabels) current.View.DrawLabel(current, DataLabelsPosition, LabelsStyle, timeLine);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);
                current.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        vm.Location.X,
                        vm.Location.Y,
                        (float) GeometrySize,
                        (float) GeometrySize));
                previous = current;
                i++;
            }
        }
    }
}
