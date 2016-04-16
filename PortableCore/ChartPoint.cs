using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiveChartsCore
{
    public class ChartPoint : IChartPointView
    {
        /// <summary>
        /// Gets the X point value
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Gets the Y point value
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Gets the coordinate where the value is placed at chart
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Gets the index of this point in the chart
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// Gets the object where the chart pulled the point
        /// </summary>
        public object Instance { get; set; }

        public virtual void UpdateView()
        {
            throw new NotImplementedException("You must build the view of a chart point before caling this method");
        }
    }

    public interface IChartPointView
    {
        void UpdateView();
    }
}