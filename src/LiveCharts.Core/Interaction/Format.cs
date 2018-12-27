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
#region

using System;

#endregion

namespace LiveCharts.Interaction
{
    /// <summary>
    /// A helper class.
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// Converts a double number to a short string, based on the metric convention. 
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string AsMetricNumber(double number)
        {
            const double pico = 0.000000000001;
            const double nano = 0.000000001;
            const double micro = 0.000001;
            const double mili = 0.001;
            const double kilo = 1000;
            const double mega = 1000000;
            const double giga = 1000000000;
            const double tera = 1000000000000;

            double log = Math.Abs(number) < pico ? 0 : Math.Log10(Math.Abs(number));
            string Func(double @const) => Math.Round(number / @const, 2).ToString("N2");

            if (log >= 12) return $"{Func(tera)} T";
            if (log >= 9) return $"{Func(giga)} G";
            if (log >= 6) return $"{Func(mega)} M";
            if (log >= 3) return $"{Func(kilo)} k";
            if (log >= -3) return Func(1);
            if (log >= -6) return $"{Func(mili)} m";
            if (log >= -9) return $"{Func(micro)} µ";
            if (log >= -12) return $"{Func(nano)} n";
            return $"{Func(pico)} p";
        }
    }
}
