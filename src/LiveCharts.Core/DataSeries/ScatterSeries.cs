using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The scatter series class.
    /// </summary>
    public class ScatterSeries<TModel> 
        : CartesianSeries<TModel, Weighted2DPoint, ScatterViewModel, Point<TModel, Weighted2DPoint, ScatterViewModel>>, IScatterSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries{TModel}"/> class.
        /// </summary>
        public ScatterSeries()
        {
            LiveChartsSettings.Set<IScatterSeries>(this);
            RangeByDimension = RangeByDimension = new[]
            {
                new DoubleRange(), // x
                new DoubleRange(), // y
                new DoubleRange()  // w
            };
        }

        /// <inheritdoc />
        public override double[] DefaultPointWidth => new []{0d,0d};

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];
            var w = chart.Dimensions[2][ScalesAt[2]];
            var unitWidth = chart.Get2DUiUnitWidth(x, y);

            Point<TModel, Weighted2DPoint, ScatterViewModel> previous = null;
            foreach (var current in Points)
            {
                var p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x),
                    chart.ScaleToUi(current.Coordinate[1][0], y)
                };

                var dw = chart.ScaleToUi(current.Coordinate.Weight, w);

                var vm = new ScatterViewModel
                {
                    Location = new Point(p[0], p[1]) + unitWidth,
                    Radius = dw
                };

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                current.InteractionArea = new RectangleInteractionArea
                {
                    Top = vm.Location.Y,
                    Left = vm.Location.X,
                    Height = dw,
                    Width = dw
                };

                previous = current;
            }
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, Weighted2DPoint, ScatterViewModel>, Weighted2DPoint, ScatterViewModel> 
            DefaultPointViewProvider()
        {
            return LiveChartsSettings.Current.UiProvider.GetNewScatterView<TModel>();
        }
    }
}
