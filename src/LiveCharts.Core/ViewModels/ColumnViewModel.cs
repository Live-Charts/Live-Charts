namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// A column view model.
    /// </summary>
    public class ColumnViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnViewModel"/> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="t">The t.</param>
        /// <param name="h">The h.</param>
        /// <param name="w">The w.</param>
        /// <param name="zero">The zero.</param>
        /// <param name="initialState">The initial view state.</param>
        public ColumnViewModel(double l, double t, double h, double w, double zero, System.Drawing.RectangleF initialState)
        {
            Left = l;
            Top = t;
            Height = h;
            Width = w;
            Zero = zero;
            InitialState = initialState;
        }

        /// <summary>
        /// Gets the initial state.
        /// </summary>
        /// <value>
        /// The initial state.
        /// </value>
        public System.Drawing.RectangleF InitialState { get; }

        /// <summary>
        /// Specifies the column animation direction.
        /// </summary>
        /// <value>
        /// The animate to top.
        /// </value>
        public double Zero { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top { get; set; }
    }
}
