using System.Windows.Media;

namespace Charts
{
    public class Separator
    {
        /// <summary>
        /// Indicates weather to draw separators or not.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets color separators color 
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Gets or sets separatos thickness
        /// </summary>
        public int Thickness { get; set; }
    }
}