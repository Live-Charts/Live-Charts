using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class BezierProvider<TModel> : IPointView<TModel, Point<TModel, Point2D, BezierViewModel>, Point2D, BezierViewModel>
    {
        public event DisposingResourceHandler Disposed;
        public object UpdateId { get; set; }
        public void Dispose(IChartView view)
        {

        }

        public object VisualElement { get; }
        public IDataLabelControl Label { get; }
        public void DrawShape(Point<TModel, Point2D, BezierViewModel> point, Point<TModel, Point2D, BezierViewModel> previous)
        {

        }

        public void DrawLabel(Point<TModel, Point2D, BezierViewModel> point, Point location)
        {

        }
    }
}
