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
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The Pie series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Core.DataSeries.Series{TModel, PieCoordinate, PieViewModel, TSeries}" />
    /// <seealso cref="IPieSeries" />
    public class PieSeries<TModel> :
        StrokeSeries<TModel, StackedPointCoordinate, PieViewModel, IPieSeries>, IPieSeries
    {
        private ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> _provider;
        private double _pushOut;
        private double _cornerRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries{TModel}"/> class.
        /// </summary>
        public PieSeries()
        {
            Charting.BuildFromSettings<IPieSeries>(this);
        }

        /// <inheritdoc />
        public double PushOut
        {
            get => _pushOut;
            set
            {
                _pushOut = value; 
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IPieSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {0f, 0f};

        /// <inheritdoc />
        public override float PointMargin => 0f;

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, StackedPointCoordinate, PieViewModel, IPieSeries>
            DefaultViewProvider => _provider ?? (_provider = Charting.Current.UiProvider.PieViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var pieChart = (IPieChartView) chart.View;
            
            var maxPushOut = context.GetMaxPushOut();

            var innerRadius = pieChart.InnerRadius;
            var outerDiameter = pieChart.ControlSize[0] < pieChart.ControlSize[1]
                ? pieChart.ControlSize[0]
                : pieChart.ControlSize[1];

            outerDiameter -= (float) maxPushOut *2f;

            var centerPoint = new PointF(pieChart.ControlSize[0] / 2f, pieChart.ControlSize[1] / 2f);

            var startsAt = pieChart.StartingRotationAngle > 360f
                ? 360f
                : (pieChart.StartingRotationAngle < 0
                    ? 0f
                    : (float) pieChart.StartingRotationAngle);

            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> previous = null;

            foreach (var current in Points)
            {
                var range = current.Coordinate.To - current.Coordinate.From;

                float stacked;

                unchecked
                {
                    stacked = context.GetStack((int) current.Coordinate.Key, 0, true);
                }

                var vm = new PieViewModel
                {
                    To = new SliceViewModel
                    {
                        Wedge = range * 360f / stacked,
                        InnerRadius = (float) innerRadius,
                        OuterRadius = outerDiameter / 2,
                        Rotation = startsAt + current.Coordinate.From * 360f / stacked
                    },
                    ChartCenter = centerPoint
                };

                if (current.View == null)
                {
                    current.View = ViewProvider.GetNewPoint();
                }

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);
                current.InteractionArea = new PolarInteractionArea(
                    vm.To.OuterRadius, vm.To.Rotation, vm.To.Rotation + vm.To.Wedge, centerPoint);

                previous = current;
            }
        }
    }
}
