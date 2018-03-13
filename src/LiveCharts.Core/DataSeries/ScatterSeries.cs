using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The scatter series class.
    /// </summary>
    public class ScatterSeries<TModel> 
        : CartesianSeries<TModel, WeightedPoint, ScatterViewModel, Point<TModel, WeightedPoint, ScatterViewModel>>, IScatterSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries{TModel}"/> class.
        /// </summary>
        public ScatterSeries()
        {
            MaxPointDiameter = 30f;
            MinPointDiameter = 12f;
            StrokeThickness = 1f;
            Geometry = Geometry.Circle;
            Charting.BuildFromSettings<IScatterSeries>(this);
            RangeByDimension = RangeByDimension = new[]
            {
                new RangeF(), // x
                new RangeF(), // y
                new RangeF()  // w
            };
            // ToDo: Check out if this is the best option... to avoid tooltip show throw...
            ScalesAt = new[] {0, 0, 0};
        }

        /// <inheritdoc />
        public Geometry PointGeometry { get; set; }

        /// <inheritdoc />
        public float MaxPointDiameter { get; set; }

        /// <inheritdoc />
        public float MinPointDiameter { get; set; }

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new []{0f,0f};

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.Dimensions[0][ScalesXAt];
            var y = cartesianChart.Dimensions[1][ScalesYAt];
            var uw = chart.Get2DUiUnitWidth(x, y);
            var p1 = new PointF(RangeByDimension[2].To, MinPointDiameter);
            var p2 = new PointF(RangeByDimension[2].From, MaxPointDiameter);

            Point<TModel, WeightedPoint, ScatterViewModel> previous = null;
            foreach (var current in Points)
            {
                var p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x),
                    chart.ScaleToUi(current.Coordinate[1][0], y),
                    cartesianChart.LinealScale(p1, p2, current.Coordinate.Weight)
                };

                var vm = new ScatterViewModel
                {
                    Location = Perform.Sum(new PointF(p[0], p[1]), new PointF(uw[0], uw[1])),
                    Diameter = p[2]
                };

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                current.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        vm.Location.X - p[2] * .5f,
                        vm.Location.Y - p[2] * .5f,
                        p[2],
                        p[2]));
                previous = current;
            }
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, WeightedPoint, ScatterViewModel>, WeightedPoint, ScatterViewModel> 
            DefaultPointViewProvider()
        {
            return Charting.Current.UiProvider.GetNewScatterView<TModel>();
        }
    }
}
