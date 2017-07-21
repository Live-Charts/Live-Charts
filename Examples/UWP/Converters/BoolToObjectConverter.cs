using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWP.Converters
{
    /// <summary>
    /// This class converts a boolean value into an other object.
    /// Can be used to convert true/false to visibility, a couple of colors, couple of images, etc.
    /// </summary>
    public class BoolToObjectConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the value to be returned when the boolean is true
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the boolean is false
        /// </summary>
        public object FalseValue { get; set; }

        /// <summary>
        /// Convert a boolean value to an other object.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = value is bool && (bool)value;

            // Negate if needed
            if (TryParseBool(parameter))
            {
                boolValue = !boolValue;
            }

            return Convert(boolValue ? TrueValue : FalseValue, targetType);
        }

        /// <summary>
        /// Convert back the value to a boolean
        /// </summary>
        /// <remarks>If the <paramref name="value"/> parameter is a reference type, <see cref="TrueValue"/> must match its reference to return true.</remarks>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool result = Equals(value, Convert(TrueValue, targetType));

            if (TryParseBool(parameter))
            {
                result = !result;
            }

            return result;
        }

        /// <summary>
        /// Helper method to safely cast an object to a boolean
        /// </summary>
        /// <param name="parameter">Parameter to cast to a boolean</param>
        /// <returns>Bool value or false if cast failed</returns>
        private bool TryParseBool(object parameter)
        {
            var parsed = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out parsed);
            }

            return parsed;
        }

        /// <summary>
        /// Helper method to convert a value from a source type to a target type.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="targetType">The target type</param>
        /// <returns>The converted value</returns>
        internal static object Convert(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value))
            {
                return value;
            }
            else
            {
                return XamlBindingHelper.ConvertValue(targetType, value);
            }
        }
    }
}
