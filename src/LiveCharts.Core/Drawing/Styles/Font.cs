#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
namespace LiveCharts.Core.Drawing.Styles
{
    /// <summary>
    /// A font abstraction.
    /// </summary>
    public struct Font
    {
        private readonly bool _isEmpty;

        private Font(bool isEmpty) : this()
        {
            _isEmpty = isEmpty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> struct.
        /// </summary>
        /// <param name="familyName">Name of the family.</param>
        /// <param name="size">The size.</param>
        /// <param name="style">The style.</param>
        /// <param name="weight">The weight.</param>
        public Font(string familyName, float size, FontStyle style, FontWeight weight)
        {
            _isEmpty = false;
            FamilyName = familyName;
            Size = size;
            Style = style;
            Weight = weight;
        }

        /// <summary>
        /// An empty font.
        /// </summary>
        public static Font Empty = new Font(true);

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        public string FamilyName { get; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public float Size { get; }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        public FontStyle Style { get; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public FontWeight Weight { get; }

        /// <summary>
        /// Get the default font.
        /// </summary>
        public static Font Default => new Font("Arial", 10, FontStyle.Regular, FontWeight.Regular);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Font))
            {
                return false;
            }

            var font = (Font)obj;
            return _isEmpty == font._isEmpty &&
                   FamilyName == font.FamilyName &&
                   // ReSharper disable once CompareOfFloatsByEqualityOperator
                   Size == font.Size &&
                   Style == font.Style &&
                   Weight == font.Weight;
        }

        /// <summary>
        /// Compares with the specified element.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Font other)
        {
            return _isEmpty == other._isEmpty && string.Equals(FamilyName, other.FamilyName) && Size.Equals(other.Size) && Style == other.Style && Weight == other.Weight;
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
                int hashCode = _isEmpty.GetHashCode();
                hashCode = (hashCode * 397) ^ (FamilyName != null ? FamilyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Style;
                hashCode = (hashCode * 397) ^ (int) Weight;
                return hashCode;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="f1">The f1.</param>
        /// <param name="f2">The f2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Font f1, Font f2)
        {
            return Equals(f1, f2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="f1">The f1.</param>
        /// <param name="f2">The f2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Font f1, Font f2)
        {
            return !(f1 == f2);
        }
    }
}
