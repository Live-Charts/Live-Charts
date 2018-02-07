using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class ColumnView<TModel> : IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel>
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {
            Disposed?.Invoke(view, this);
        }

        public object VisualElement { get; }
        public IDataLabelControl Label { get; }
        public void DrawShape(Point<TModel, Point2D, ColumnViewModel> point, Point<TModel, Point2D, ColumnViewModel> previous, IChartView chart, ColumnViewModel viewModel)
        {
        }

        public void DrawLabel(Point<TModel, Point2D, ColumnViewModel> point, Point location, IChartView chart)
        {
        }
    }
}