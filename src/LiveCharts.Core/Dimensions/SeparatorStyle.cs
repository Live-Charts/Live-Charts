using LiveCharts.Core.Drawing;
using System.Collections.Generic;

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a dimension style.
    /// </summary>
    public struct SeparatorStyle
    {
        private readonly bool _isEmpty;

        private SeparatorStyle(bool empty): this()
        {
            _isEmpty = empty;
            Fill = Color.Empty;
            StrokeThickness = double.NaN;
            Stroke = Color.Empty;
        }

        public SeparatorStyle(Color stroke, Color fill, double strokeThickness)
        {
            _isEmpty = false;
            Stroke = stroke;
            Fill = fill;
            StrokeThickness = strokeThickness;
        }

        /// <summary>
        /// The empty/
        /// </summary>
        public static SeparatorStyle Empty = new SeparatorStyle(true);

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Color Stroke { get; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get;}

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Color Fill { get; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SeparatorStyle))
            {
                return false;
            }

            var style = (SeparatorStyle)obj;
            return _isEmpty == style._isEmpty &&
                   EqualityComparer<Color>.Default.Equals(Stroke, style.Stroke) &&
                   StrokeThickness == style.StrokeThickness &&
                   EqualityComparer<Color>.Default.Equals(Fill, style.Fill);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(SeparatorStyle other)
        {
            return _isEmpty == other._isEmpty && Stroke.Equals(other.Stroke) && StrokeThickness.Equals(other.StrokeThickness) && Fill.Equals(other.Fill);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _isEmpty.GetHashCode();
                hashCode = (hashCode * 397) ^ Stroke.GetHashCode();
                hashCode = (hashCode * 397) ^ StrokeThickness.GetHashCode();
                hashCode = (hashCode * 397) ^ Fill.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(SeparatorStyle c1, SeparatorStyle c2)
        {
            return Equals(c1, c2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(SeparatorStyle c1, SeparatorStyle c2)
        {
            return !(c1 == c2);
        }
    }
}
