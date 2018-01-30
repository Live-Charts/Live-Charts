using System;
using System.Windows;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.PointViews
{
    public class PointView<TModel, TPoint, TCoordinate, TViewModel, TShape, TLabel>
        : IPointView<TModel, TPoint, TCoordinate, TViewModel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
        where TShape : Shape, new()
        where TLabel : DependencyObject, IDataLabelControl, new()
    {
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public TShape Shape { get; protected set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public IDataLabelControl Label { get; protected set; }

        /// <inheritdoc cref="DrawShape"/>
        protected virtual void OnDraw(TPoint point, TPoint previous, IChartView chart, TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="DrawLabel"/>
        protected virtual void OnDrawLabel(TPoint point, Point location, IChartView chart)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="Dispose"/>
        protected virtual void OnDispose(IChartView chart)
        {
            throw new NotImplementedException();
        }

        #region ResourceViewImplementation


        /// <inheritdoc />
        object IPointView<TModel, TPoint, TCoordinate, TViewModel>.VisualElement => Shape;

        /// <inheritdoc />
        IDataLabelControl IPointView<TModel, TPoint, TCoordinate, TViewModel>.Label => Label;

        /// <inheritdoc />
        public void DrawShape(TPoint point, TPoint previous, IChartView chart, TViewModel viewModel)
        {
            OnDraw(point, previous, chart, viewModel);
        }

        /// <inheritdoc />
        public void DrawLabel(TPoint point, Point location, IChartView chart)
        {
            OnDrawLabel(point, location, chart);
        }

        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        public void Dispose(IChartView view)
        {
            OnDispose(view);
            Disposed?.Invoke(view);
        }

        #endregion
    }
}