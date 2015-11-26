using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace LiveCharts.TypeConverters
{
    public class ColorCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var valueString = value as string;
            if (valueString != null)
            {
                var values = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return values
                    .Select(x => (Color) (ColorConverter.ConvertFromString(x.Trim())
                                          ?? Colors.Transparent))
                    .ToArray();
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
