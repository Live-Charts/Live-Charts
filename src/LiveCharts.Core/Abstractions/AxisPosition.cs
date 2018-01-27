using LiveCharts.Core.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Tooltip selection mode.
    /// </summary>
    public enum TooltipSelectionMode
    {
        /// <summary>
        /// LiveCharts will decide the selection mode based on the current data.
        /// </summary>
        Auto,
        /// <summary>
        /// Displays the first <see cref="Point{TModel,TCoordinate,TViewModel}"/> that is in the pointer position.
        /// </summary>
        First,
        /// <summary>
        /// Displays all the points that share the X dimension.
        /// </summary>
        SharedX,
        /// <summary>
        /// Displays all the points that share the Y dimension.
        /// </summary>
        SharedY
    }

    /// <summary>
    /// Axis available positions.
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// The position will be set according to the predefined data.
        /// </summary>
        Auto,
        /// <summary>
        /// The top position.
        /// </summary>
        Top,
        /// <summary>
        /// The left position.
        /// </summary>
        Left,
        /// <summary>
        /// The right position.
        /// </summary>
        Right,
        /// <summary>
        /// The bottom position.
        /// </summary>
        Bottom
    }
}