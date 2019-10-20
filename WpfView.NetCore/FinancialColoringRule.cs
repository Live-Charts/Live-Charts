using System.Windows.Media;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Defines a condition that decides the fill and stroke to use in a CandleStick series
    /// </summary>
    public class FinancialColoringRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialColoringRule"/> class.
        /// </summary>
        public FinancialColoringRule()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialColoringRule"/> class.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        public FinancialColoringRule(FinancialDelegate condition, Brush stroke, Brush fill)
        {
            Condition = condition;
            Stroke = stroke;
            Fill = fill;
        }

        /// <summary>
        /// Gets or sets the condition, if the condition returns true, the point will use the defined Stroke/Fill properties in the FinancialColoringRule object
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public FinancialDelegate Condition { get; set; }

        /// <summary>
        /// Gets or sets the stroke to use when the condition returns true.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke { get; set; }

        /// <summary>
        /// Gets or sets the fill to use when the condition returns true.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill { get; set; }
    }
}
