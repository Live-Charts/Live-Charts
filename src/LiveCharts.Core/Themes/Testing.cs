using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Dimensions;
using Font = LiveCharts.Core.Abstractions.Font;

namespace LiveCharts.Core.Themes
{
    /// <summary>
    /// Testing theme.
    /// </summary>
    public static class Testing
    {
        /// <summary>
        /// Uses the testing theme.
        /// </summary>
        /// <param name="charting">The charting.</param>
        /// <returns></returns>
        public static Charting UsingTestingTheme(this Charting charting)
        {
            var baseFont = new Font("Arial", 11, FontStyles.Regular, FontWeight.Regular);

            var sepStyle = new SeparatorStyle(Color.FromArgb(255, 230, 230, 230), Color.FromArgb(50, 245, 245, 245), 1);
            var altStyle =
                new SeparatorStyle(Color.FromArgb(255, 220, 220, 220), Color.FromArgb(50, 220, 220, 220), 1);

            charting
                .UseMaterialDesignColors()
                .UseMaterialDesignColors()
                .SetDefault<ISeries>(series =>
                {
                    series.StrokeThickness = 2;
                    series.DefaultFillOpacity = .3f;
                })
                .SetDefault<Axis>(axis =>
                {
                    axis.XSeparatorStyle = sepStyle;
                    axis.YSeparatorStyle = sepStyle;
                    axis.XAlternativeSeparatorStyle = altStyle;
                    axis.YAlternativeSeparatorStyle = altStyle;
                });

            return charting;
        }
    }
}