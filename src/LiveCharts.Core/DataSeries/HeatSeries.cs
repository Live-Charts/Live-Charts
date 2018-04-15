using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The heat series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianStrokeSeries{TModel,TCoordinate,TViewModel, TSeries}" />
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.IHeatSeries" />
    public class HeatSeries<TModel>
        : CartesianStrokeSeries<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries>, IHeatSeries
    {
        private static ISeriesViewProvider<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> _provider;
        private IEnumerable<GradientStop> _gradient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatSeries{TModel}"/> class.
        /// </summary>
        public HeatSeries()
        {
            ScalesAt = new[] { 0, 0, 0 };
            DefaultFillOpacity = .2f;
            Charting.BuildFromSettings<IHeatSeries>(this);
        }

        /// <inheritdoc />
        public IEnumerable<GradientStop> Gradient
        {
            get => _gradient;
            set
            {
                _gradient = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IHeatSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 1f, 0f };

        /// <inheritdoc />
        public override float[] PointMargin => new[] { 0f, 0f };

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries>
            DefaultViewProvider => _provider ?? (_provider = Charting.Current.UiProvider.HeatViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.Dimensions[0][ScalesAt[0]];
            var y = cartesianChart.Dimensions[1][ScalesAt[1]];
            
            var uw = chart.Get2DUiUnitWidth(x, y);

            int xi = 0, yi = 1;
            if (chart.InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator

            var wd = x.ActualMaxValue - x.ActualMinValue == 0
                ? double.MaxValue
                : x.ActualMaxValue - x.ActualMinValue;

            var hd = y.ActualMaxValue - y.ActualMinValue == 0
                ? double.MaxValue
                : y.ActualMaxValue - y.ActualMinValue;

            // ReSharper restore CompareOfFloatsByEqualityOperator

            var w = cartesianChart.DrawAreaSize[xi] / wd;
            var h = cartesianChart.DrawAreaSize[yi] / hd;

            Point<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> previous;

            foreach (var current in Points)
            {
                if (current.View == null)
                {
                    current.View = ViewProvider.Getter();
                }



                previous = current;
            }
        }
    }
}