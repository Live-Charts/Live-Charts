using System;
using System.Globalization;
using System.Windows.Data;
using LiveCharts.Core.Data;

namespace LiveCharts.Wpf.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public abstract class PointAsTooltipData : IValueConverter
    {
        private readonly int _dimension;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointAsTooltipData"/> class.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        protected PointAsTooltipData(int dimension)
        {
            _dimension = dimension;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid null value is used.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (PackedPoint) value;
            return point?.AsTooltipData()[_dimension];
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid null value is used.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}