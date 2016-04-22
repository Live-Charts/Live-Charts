
namespace LiveChartsCore
{
    public class LineModel : Series
    {
        public LineModel(ISeriesView view) : base(view)
        {
        }

        public LineModel(ISeriesView view, SeriesConfiguration configuration) : base(view, configuration)
        {
        }

        public override void Update()
        {
            ChartPoint previous = null;

            foreach (var chartPoint in Values.Points)
            {
                chartPoint.Coordinates = ChartFunctions.ToDrawMargin(chartPoint, ScalesXAt, ScalesYAt, Chart);

                if (chartPoint.View == null)
                    chartPoint.View = View.InitializePointView();

                chartPoint.View.Update(previous, chartPoint, Chart);
                

                previous = chartPoint;
            }
        }


    }
}
