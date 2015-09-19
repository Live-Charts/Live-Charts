using System.Windows;
using System.Windows.Shapes;
using Charts.Series;

namespace Charts
{
    public class HoverableShape
    {
        /// <summary>
        /// Point of this area
        /// </summary>
        public Point Value { get; set; }
        /// <summary>
        /// Shape that fires hover
        /// </summary>
        public Shape Shape { get; set; }
        /// <summary>
        /// Shape that that changes style on hover
        /// </summary>
        public Shape Target { get; set; }
        /// <summary>
        /// serie that contains thos point
        /// </summary>
        public Serie Serie { get; set; }
        /// <summary>
        /// Point label
        /// </summary>
        public string Label { get; set; }
    }
}