using System.Collections.Generic;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Represts an object that contains one or more elements that the UI needs to paint.
    /// </summary>
    public interface IUIContent
    {
        /// <summary>
        /// Getds the paintables.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPaintable> GetPaintables();
    }
}