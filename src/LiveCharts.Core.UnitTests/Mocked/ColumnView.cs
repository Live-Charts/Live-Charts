using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.ViewModels;
using Point = LiveCharts.Core.Coordinates.Point;
using Rectangle = LiveCharts.Core.ViewModels.Rectangle;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class ColumnView<TModel> : IPointView<TModel, Point<TModel, Point, Rectangle>, Point, Rectangle>
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {
            Disposed?.Invoke(view, this);
        }

        public object VisualElement { get; }
        public IDataLabelControl Label { get; }
        public void DrawShape(Point<TModel, Point, Rectangle> point, Point<TModel, Point, Rectangle> previous)
        {
        }

        public void DrawLabel(Point<TModel, Point, Rectangle> point, Drawing.Point location)
        {
        }
    }
}