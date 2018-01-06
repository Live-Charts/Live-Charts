using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data.Points;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// Column point view.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.IPointView" />
    public class ColumnPointView : IPointView
    {
        /// <summary>
        /// Gets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rectangle Rectangle { get; protected set; }

        public void Draw(ChartPoint point, ChartPoint previous, IChartView chart)
        {
            var wpfChart = (CartesianChart) chart;

            throw new NotImplementedException();
        }

        public void Erase()
        {
            throw new NotImplementedException();
        }
    }
}
