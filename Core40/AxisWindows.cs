namespace LiveCharts
{
    public static class AxisWindows
    {
        /// <summary>
        /// 
        /// </summary>
        public static AxisWindowCore EmptyWindow
        {
            get { return new EmptyAxisWindow(); }
        }

        /// <inheritdoc />
        public sealed class EmptyAxisWindow : AxisWindowCore
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 0; }
            }

            /// <inheritdoc />
            public override bool IsHeader(double x)
            {
                return false;
            }

            /// <inheritdoc />
            public override bool IsSeparator(double x)
            {
                return false;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(double x)
            {
                return "";
            }
        }
    }
}