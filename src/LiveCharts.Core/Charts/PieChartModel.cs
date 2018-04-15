using System;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.Charts
{
    /// <inheritdoc />
    public class PieChartModel : ChartModel
    {
        private PointF _previousTooltipLocation = PointF.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        public PieChartModel(IChartView view) : base(view)
        {
            Charting.BuildFromSettings((IPieChartView) view);
        }

        /// <inheritdoc />
        protected override int DimensionsCount => 2;

        public override float ScaleToUi(float dataValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void ViewOnPointerMoved(
            TooltipSelectionMode selectionMode, params double[] mouseLocation)
        {
            if (Series == null) return;
            var query = GetInteractedPoints(mouseLocation).ToArray();

            if (selectionMode == TooltipSelectionMode.Auto)
            {
                // ToDo: guess what the user meant here ...
            }

            ToolTip = View.DataToolTip;

            // ReSharper disable once PossibleMultipleEnumeration
            if (!query.Any())
            {
                ToolTipTimeoutTimer.Start();
                return;
            }

            ToolTipTimeoutTimer.Stop();

            View.DataToolTip.ShowAndMeasure(query, View);

            var p = query.First();

            var model = (PieViewModel) p.ViewModel;
            var angle = ((IPieChartView) View).StartingRotationAngle + model.To.Rotation + model.To.Wedge * .5;
            var radius = model.To.OuterRadius;

            var sx = model.ChartCenter.X + radius * Math.Sin(angle * Math.PI / 180);
            var sy = model.ChartCenter.Y + radius * Math.Cos(angle * Math.PI / 180);

            var newTooltipLocation = new PointF((float) sx, (float) sy);

            if (_previousTooltipLocation != newTooltipLocation)
            {
                View.DataToolTip.Move(newTooltipLocation, View);
            }

            OnDataPointerEnter(query);
            var leftPoints = PreviousHoveredPoints?.ToArray()
                .Where(x => !x.InteractionArea.Contains(mouseLocation));
            // ReSharper disable once PossibleMultipleEnumeration
            if (leftPoints != null && leftPoints.Any())
            {
                // ReSharper disable once PossibleMultipleEnumeration
                OnDataPointerLeave(leftPoints);
            }
            PreviousHoveredPoints = query;

            _previousTooltipLocation = newTooltipLocation;
        }

        /// <inheritdoc />
        protected override void Update(bool restart, UpdateContext context)
        {
            OnUpdateStarted();

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
