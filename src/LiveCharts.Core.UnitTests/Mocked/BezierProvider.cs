using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.ViewModels;
using Point = LiveCharts.Core.Coordinates.Point;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class BezierProvider<TModel> : IPointView<TModel, Point<TModel, Point, BezierViewModel>, Point, BezierViewModel>
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {

        }

        public object VisualElement { get; }
        public IDataLabelControl Label { get; }
        public void DrawShape(Point<TModel, Point, BezierViewModel> point, Point<TModel, Point, BezierViewModel> previous)
        {

        }

        public void DrawLabel(Point<TModel, Point, BezierViewModel> point, Drawing.Point location)
        {

        }
    }
}
