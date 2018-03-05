namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// A column view model.
    /// </summary>
    public struct Column
    {
        private bool _isEmpty;

        /// <summary>
        /// The empty column instance.
        /// </summary>
        public static Column Empty = new Column(true);

        private Column(bool isEmpty)
        {
            _isEmpty = isEmpty;
            Top = 0f;
            Left = 0f;
            Height = 0f;
            Width = 0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="t">The t.</param>
        /// <param name="h">The h.</param>
        /// <param name="w">The w.</param>
        public Column(float l, float t, float h, float w)
        {
            _isEmpty = false;
            Left = l;
            Top = t;
            Height = h;
            Width = w;
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public float Height { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float Width { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public float Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public float Top { get; set; }
    }
}