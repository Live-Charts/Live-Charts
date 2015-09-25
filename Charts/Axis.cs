using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Charts
{
    public class Axis
    {
        public Axis()
        {
            CleanFactor = 3;
            Color = Color.FromRgb(205, 205, 205);
            Thickness = 3;
            Enabled = true;
            FontFamily = new FontFamily("Calibri");
            FontSize = 11;
            FontWeight = FontWeights.Normal;
            FontStyle = FontStyles.Normal;
            FontStretch = FontStretches.Normal;
            PrintLabels = true;
            TextColor = Color.FromRgb(150,150,150);
            Separator = new Separator
            {
                Enabled = true,
                Color = Color.FromRgb(205, 205, 205),
                Thickness = 1
            };
        }

        /// <summary>
        /// Get or sets configuration for parallel lines to axis.
        /// </summary>
        public Separator Separator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Func<double, string> LabelFormatter { get; set; }
        /// <summary>
        /// Hardcoded labels, this property overrides LabelFormatter
        /// </summary>
        public IEnumerable<string> Labels { get; set; }
        /// <summary>
        /// Indicates weather to draw axis or not.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets axis color.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Gets or sets axis thickness.
        /// </summary>
        public int Thickness { get; set; }
        /// <summary>
        /// Gets or sets labels font family
        /// </summary>
        public FontFamily FontFamily { get; set; }
        /// <summary>
        /// Gets or sets labels font size
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight FontWeight { get; set; }
        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
        public FontStyle FontStyle { get; set; }
        /// <summary>
        /// Gets or sets labels font strech
        /// </summary>
        public FontStretch FontStretch { get; set; }
        /// <summary>
        /// Gets or sets if labels should be printed.
        /// </summary>
        public bool PrintLabels { get; set; }
        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
        public Color TextColor { get; set; }
        /// <summary>
        /// Factor used to calculate label separations. default is 3. increase it to make it 'cleaner'
        /// initialSeparations = Graph.Heigth / (label.Height * cleanFactor)
        /// </summary>
        public int CleanFactor { get; set; }
        /// <summary>
        /// Gets or sets chart max value, set it to null to make this property Auto, default value is null
        /// </summary>
        public double? MaxValue { get; set; }
        /// <summary>
        /// Gets or sets chart min value, set it to null to make this property Auto, default value is null
        /// </summary>
        public double? MinValue { get; set; }
    }
}