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
        }

        /// <summary>
        /// Scales to pixels a data value according to an axis range and a given area, if the area is not present, the chart draw margin size will be used.
        /// </summary>
        /// <param name="dataValue">The value.</param>
        /// <param name="plane">The axis.</param>
        /// <param name="sizeVector">The draw margin, this param is optional, if not set, the current chart's draw margin area will be used.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override float ScaleToUi(float dataValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        protected override void ViewOnPointerMoved(PointF location, TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            throw new NotImplementedException();
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
                }

                CollectResources();
                OnUpdateFinished();
            });
        }
    }
}
