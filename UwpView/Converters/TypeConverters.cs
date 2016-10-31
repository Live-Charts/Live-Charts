//The MIT License(MIT)

//copyright(c) 2016 Greg Dennis & Alberto Rodríguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Globalization;
using System.Linq;
using LiveCharts.Helpers;

namespace LiveCharts.Uwp.Converters
{
    //public class StringCollectionConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        var valueString = value as string;
    //        if (valueString != null)
    //        {
    //            return valueString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
    //                .Select(x => x.Trim())
    //                .ToArray();
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }
    //}

    //public class NumericChartValuesConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        var valueString = value as string;

    //        if (valueString != null)
    //        {
    //            return valueString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
    //                .Select(d => double.Parse((string) d, CultureInfo.InvariantCulture))
    //                .AsChartValues();
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }
    //}
}