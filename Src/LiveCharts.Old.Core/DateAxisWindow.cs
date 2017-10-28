//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.Linq;

namespace LiveCharts
{
    public abstract class DateAxisWindow : AxisWindow
    {
        public DateAxisCore DateAxisCore { get; set; }

        public override bool IsHeader(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return IsHeader(date);
        }

        public override bool IsSeparator(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return IsSeparator(date);
        }

        public override string FormatAxisLabel(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return FormatAxisLabel(date);
        }

        protected bool IsSecond(DateTime x)
        {
            return x.Millisecond == 0;
        }
        protected bool IsMinute(DateTime x)
        {
            return x.Second == 0 && IsSecond(x);
        }
        protected bool IsHour(DateTime x)
        {
            return x.Minute == 0 && IsMinute(x);
        }
        protected bool IsDay(DateTime x)
        {
            return x.Hour == 0 && IsHour(x);
        }    
        protected bool IsYear(DateTime x)
        {
            return x.DayOfYear == 1 && IsDay(x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        public override bool TryGetSeparatorIndices(IEnumerable<double> indices, int maximumSeparatorCount, out IEnumerable<double> separators)
        {
            // First validate the interval between the indices
            // We expect always at least 2 indices to exist.
            var testdateTimes = indices.Take(2).Select(d => DateAxisCore.GetdateTime(d)).ToList();
            var distance = testdateTimes[1].Subtract(testdateTimes[0]);

            // Validate the distance between the separators.
            // This should be a sane value for this window.
            // I.e. Seconds displayed with intervals of hours, is not sane
            if (!Validate(distance))
            {
                // This date axis window does not validate the range between these 
                separators = Enumerable.Empty<double>();
                return false;
            }

            return base.TryGetSeparatorIndices(indices, maximumSeparatorCount, out separators);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seperatorDistance"></param>
        /// <returns></returns>
        protected virtual bool Validate(TimeSpan seperatorDistance)
        {
            return true;
        }

        /// <summary>
        /// Determines whether a dateTime is a header
        /// </summary>
        public abstract bool IsHeader(DateTime x);

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        public abstract bool IsSeparator(DateTime x);

        /// <summary>
        /// Gets or sets a function to format the label for the axis
        /// </summary>
        public abstract string FormatAxisLabel(DateTime x);

    }
}