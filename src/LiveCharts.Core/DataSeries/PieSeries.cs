using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The Pie series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Core.DataSeries.Series{TModel, PieCoordinate, PieViewModel, TPoint}" />
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.IPieSeries" />
    public class PieSeries<TModel> :
        Series<TModel, PieCoordinate, PieViewModel, Point<TModel, PieCoordinate, PieViewModel>>, IPieSeries
    {
        private static ISeriesViewProvider<TModel, PieCoordinate, PieViewModel> _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries{TModel}"/> class.
        /// </summary>
        public PieSeries()
        {
            Charting.BuildFromSettings<IPieSeries>(this);
        }

        /// <inheritdoc />
        public double PushOut { get; set; }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IPieSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {0f, 0f};

        /// <inheritdoc />
        public override float[] PointMargin => new[] {0f, 0f};

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, PieCoordinate, PieViewModel>
            DefaultViewProvider => _provider ?? (_provider = Charting.Current.UiProvider.PieViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            foreach (var point in Points)
            {
                var vm = new 
            }
        }
    }
}
