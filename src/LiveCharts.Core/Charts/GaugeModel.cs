using System;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// The gauge model.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Charts.ChartModel" />
    public class GaugeModel : ChartModel
    {
        public GaugeModel(IChartView view) : base(view)
        {
        }

        protected override int DimensionsCount => 2;

        public override float ScaleToUi(float dataValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        protected override void ViewOnPointerMoved(PointF location, TooltipSelectionMode selectionMode,
            params double[] dimensions)
        {
        }

        /// <inheritdoc />
        protected override void Update(bool restart, UpdateContext context)
        {
            base.Update(restart, context);

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
                    throw new LiveChartsException(
                        $"{series.ResourceKey.Name} is not supported at a {nameof(ICartesianChartView)}", 110);
                }

                series.UpdateStarted(View);
                series.UpdateView(this, context);
                series.UpdateFinished(View);
            }

            CollectResources();
            OnUpdateFinished();
        }
    }
}
