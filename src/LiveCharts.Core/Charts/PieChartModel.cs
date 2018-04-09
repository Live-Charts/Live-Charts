using System;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Charts
{
    /// <inheritdoc />
    public class PieChartModel : ChartModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        public PieChartModel(IChartView view) : base(view)
        {
            Charting.BuildFromSettings((IPieChartView) view);
        }

        public override float ScaleToUi(float dataValue, Plane plane, float[] sizeVector = null)
        {
            return 0;
            switch (plane.Dimension)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    throw new LiveChartsException(
                        $"A {nameof(PieChartModel)} does not know how to scale dimension '{plane.Dimension}'", 155);
            }
        }

        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            return 0f;
        }

        protected override void ViewOnPointerMoved(PointF location, TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            
        }

        /// <inheritdoc />
        protected override void Update(bool restart)
        {
            View.InvokeOnUiThread(() =>
            {
                OnUpdateStarted();

                base.Update(restart);

                if (DrawAreaSize[0] <= 0 || DrawAreaSize[1] <= 0)
                {
                    // skip update if the chart is too small.
                    // and lets delete its content...
                    CollectResources(true);
                    return;
                }

                View.Content.DrawArea = new RectangleF(
                    new PointF(DrawAreaLocation[0], DrawAreaLocation[1]),
                    new SizeF(DrawAreaSize[0], DrawAreaSize[1]));

                foreach (var series in Series.Where(x => x.IsVisible))
                {
                    if (!(series is IPieSeries))
                    {
                        throw new LiveChartsException($"{series.ResourceKey.Name} is not supported at a {nameof(ICartesianChartView)}", 110);
                    }
                    series.UpdateStarted(View);
                    series.UpdateView(this);
                    series.UpdateFinished(View);
                }

                CollectResources();
                OnUpdateFinished();
            });
        }
    }
}
