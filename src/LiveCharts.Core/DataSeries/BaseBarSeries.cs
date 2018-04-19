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
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Styles;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;
#if NET45
using Font = LiveCharts.Core.Interaction.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The base bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    public abstract class BaseBarSeries<TModel, TCoordinate, TSeries>
        : CartesianStrokeSeries<TModel, TCoordinate, RectangleViewModel, TSeries>, IBarSeries
        where TCoordinate : ICoordinate
        where TSeries : class, ISeries
    {
        private double _pivot;
        private double _barPadding;
        private double _maxColumnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBarSeries{TModel,TCoordinate, TSeries}"/> class.
        /// </summary>
        protected BaseBarSeries()
        {
            MaxColumnWidth = 45f;
            BarPadding = 6f;
            Charting.BuildFromSettings<IBarSeries>(this);
        }

        /// <inheritdoc />
        public double BarPadding
        {
            get => _barPadding;
            set
            {
                _barPadding = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double MaxColumnWidth
        {
            get => _maxColumnWidth;
            set
            {
                _maxColumnWidth = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double Pivot
        {
            get => _pivot;
            set
            {
                _pivot = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IBarSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 1f, 0f };

        /// <inheritdoc />
        public override float[] PointMargin => new[] { 0f, 0f };

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            int w = 0, h = 1;
            var orientation = Orientation.Horizontal;

            var directionAxis = chart.Dimensions[0][ScalesAt[0]];
            var scaleAxis = chart.Dimensions[1][ScalesAt[1]];

            var uw = chart.Get2DUiUnitWidth(directionAxis, scaleAxis);

            var barsCount = context.GetBarsCount(ScalesAt[1]);

            var cw = (uw[0] - ((float) BarPadding) * barsCount) / barsCount;
            var position = context.GetBarIndex(ScalesAt[1], this);

            if (cw > MaxColumnWidth)
            {
                cw = (float) MaxColumnWidth;
            }

            var bp = (float) BarPadding;

            var byBarOffset = new[]
            {
                -cw * .5f + uw[0] * .5f,
                0f
            };

            var positionOffset = new float[2];

            if (chart.InvertXy)
            {
                w = 1;
                h = 0;
                orientation = Orientation.Vertical;
                byBarOffset[0] = 0;
                byBarOffset[1] = cw * .5f - uw[0] * .5f;
            }

            positionOffset[w] = (bp + cw) * position - (bp + cw) * ((barsCount - 1) * .5f);
            
            var pivot = GetColumnStart(chart, scaleAxis, directionAxis);


            Point<TModel, TCoordinate, RectangleViewModel, TSeries> previous = null;

            foreach (var current in Points)
            {
                if (current.View == null)
                {
                    current.View = PointViewProvider.GetNewPoint();
                }

                BuildModel(
                    current, context, chart, directionAxis,
                    scaleAxis, cw, pivot, byBarOffset,
                    positionOffset, orientation, h, w);

                current.InteractionArea = new RectangleInteractionArea(current.ViewModel.To);
                current.View.DrawShape(current, previous);
                if (DataLabels) current.View.DrawLabel(current, DataLabelsPosition, LabelsStyle);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        /// <summary>
        /// Offsets the by stack.
        /// </summary>
        /// <returns></returns>
        protected abstract void BuildModel(
            Point<TModel, TCoordinate, RectangleViewModel, TSeries> current, UpdateContext context, ChartModel chart, Plane directionAxis, Plane scaleAxis,
            float cw, float columnStart, float[] byBarOffset, float[] positionOffset, Orientation orientation,
            int h, int w);

        private float GetColumnStart(ChartModel chart, Plane target, Plane complementary)
        {
            var p = (float) Pivot;

            var value = target.ActualMinValue >= p && complementary.ActualMaxValue > p
                ? target.ActualMinValue
                : (target.ActualMinValue < p && complementary.ActualMaxValue <= p
                    ? target.ActualMaxValue
                    : p);
            return chart.ScaleToUi(value, target);
        }
    }
}