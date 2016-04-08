//The MIT License(MIT)

//copyright(c) 2016 Greg Dennis & Beto Rodríguez

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace LiveCharts.TypeConverters
{
	internal class DefaultValuesConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var valueString = value as string;
            
            //for arrays like "2,4,7,2" pharses as double
			if (valueString != null && Regex.IsMatch(valueString, @"^\s*-?\d*(\.\d*)?\s*(,\s*-?\d*(\.\d*)?\s*)*$"))
			{
			    var v = valueString
			        .Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
			        .Select(d => double.Parse(d, CultureInfo.InvariantCulture))
			        .AsChartValues();
                return v;
			}

            //arrays like "[0,1], [5,2]" pharses as point
		    if (valueString != null &&
		        Regex.IsMatch(valueString,
		            @"\s*\[\s*-?\d*(\.\d*)?\s*,\s*-?\d*(\.\d*)?\s*\](,\s*\[\s*-?\d*(\.\d*)?\s*,\s*-?\d*(\.\d*)?\s*\]\s*)*$"))
		    {
                var sb = new StringBuilder();
		        var isOpen = false;
                var points = new List<Point>();
		        foreach (var s in valueString)
		        {
                    if (isOpen && s != ']') sb.Append(s);
		            if (s == '[') isOpen = true;
		            if (s == ']')
		            {
		                isOpen = false;
		                var xy = sb.ToString().Split(new[] {","}, StringSplitOptions.None);
		                points.Add(new Point(double.Parse(xy[0], CultureInfo.InvariantCulture),
		                    double.Parse(xy[1], CultureInfo.InvariantCulture)));
		                sb.Clear();
		            }
		        }
                
                return points.AsChartValues();
            }

		    throw new FormatException("The Values pattern contains an error.");
		}
	}
}
