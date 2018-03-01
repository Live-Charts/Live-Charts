using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.ViewModels;
using Point = LiveCharts.Core.Coordinates.Point;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class ColumnView<TModel> : IPointView<TModel, Point<TModel, Point, ColumnViewModel>, Point, ColumnViewModel>
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {
            Disposed?.Invoke(view, this);
        }

        public object VisualElement { get; }
        public IDataLabelControl Label { get; }
        public void DrawShape(Point<TModel, Point, ColumnViewModel> point, Point<TModel, Point, ColumnViewModel> previous)
        {
        }

        public void DrawLabel(Point<TModel, Point, ColumnViewModel> point, Drawing.Point location)
        {
        }
    }
}