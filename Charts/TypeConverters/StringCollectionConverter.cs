using System;
using System.ComponentModel;
using System.Globalization;

namespace LiveCharts.TypeConverters
{
    public class StringCollectionConverter : TypeConverter
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
                return values;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
