using System.Windows.Media;

namespace LiveCharts
{
    public static class DefaultAxes
    {
        /// <summary>
        /// Returns default axis
        /// </summary>
        public static Axis DefaultAxis
        {
            get
            {
                return new Axis
                {
                    Separator = new Separator()
                };
            }
        }

        /// <summary>
        /// Return an axis without separators at all
        /// </summary>
        public static Axis CleanAxis
        {
            get
            {
                return new Axis
                {
                    IsEnabled = false,
                    Separator = CleanSeparator
                };
            }
        }

        /// <summary>
        /// Returns an axis that only displays a line for zero
        /// </summary>
        public static Axis OnlyZerosAxis
        {
            get
            {
                return new Axis
                {
                    IsEnabled = true,
                    Separator = CleanSeparator
                };
            }
        }

        /// <summary>
        /// Returns an axis that highlights zeros.
        /// </summary>
        public static Axis HighlightZerosAxis
        {
            get
            {
                return new Axis
                {
                    IsEnabled = false,
                    StrokeThickness = 3,
                    Color = Color.FromRgb(218, 218, 218),
                    Separator = CleanSeparator
                };
            }
        }

        //Returns a clean separator
        public static Separator CleanSeparator
        {
            get
            {
                return new Separator
                {
                    IsEnabled = false
                };
            }
        }
    }
}