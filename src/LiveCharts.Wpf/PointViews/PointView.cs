using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.PointViews
{
    public class PointView<TModel, TPoint, TCoordinate, TViewModel, TShape, TLabel>
        : IPointView<TModel, TPoint, TCoordinate, TViewModel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
        where TShape : Shape, new()
        where TLabel : FrameworkElement
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
        public TLabel Label { get; protected set; }

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

        /// <inheritdoc cref="Erase"/>
        protected virtual void OnDispose(IChartView chart)
        {
            throw new NotImplementedException();
        }

        #region OPointView implementation

        /// <inheritdoc />
        object IPointView<TModel, TPoint, TCoordinate, TViewModel>.VisualElement => Shape;

        /// <inheritdoc />
        object IPointView<TModel, TPoint, TCoordinate, TViewModel>.Label => Label;

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

        /// <inheritdoc />
        public void Dispose(IChartView view)
        {
            OnDispose(view);
        }

        #endregion
    }
}