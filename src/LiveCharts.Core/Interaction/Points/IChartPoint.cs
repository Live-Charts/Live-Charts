using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Interaction.ChartAreas;

namespace LiveCharts.Core.Interaction.Points
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <seealso cref="LiveCharts.Core.Interaction.Points.IChartPoint" />
    public interface IChartPoint<out TModel, out TCoordinate> : IChartPoint
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets the instance represented by this point.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        new TModel Model { get; }

        /// <summary>
        /// Gets the point coordinate.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        new TCoordinate Coordinate { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Interaction.Points.IChartPoint" />
    public interface IChartPoint
    {
        /// <summary>
        /// Gets the key of the point, a key is used internally as a unique identifier in 
        /// in a <see cref="Series"/> 
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        int Key { get; }

        /// <summary>
        /// Gets the instance represented by this point.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        object Model { get;  }

        /// <summary>
        /// Gets the shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        IShape Shape { get; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        ILabel Label { get; }

        // /// <summary>
        // /// Gets the view.
        // /// </summary>
        // /// <value>
        // /// The view.
        // /// </value>
        // object View { get; }

        /// <summary>
        /// Gets the point coordinate.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        ICoordinate Coordinate { get;  }

        /// <summary>
        /// Gets the hover area.
        /// </summary>
        /// <value>
        /// The hover area.
        /// </value>
        InteractionArea InteractionArea { get; }

        /// <summary>
        /// Gets the series that owns the point.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        ISeries Series { get; }

        /// <summary>
        /// Gets the chart that owns the point.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        IChartView Chart { get; }

        /// <summary>
        /// Gets the tool tip text.
        /// </summary>
        /// <value>
        /// The tool tip text.
        /// </value>
        string ToolTipText { get; }
    }
}