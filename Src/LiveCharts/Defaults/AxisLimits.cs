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

namespace LiveCharts.Defaults
{
    internal static class AxisLimits
    {
        internal static double StretchMax(AxisCore axis)
        {
            return axis.TopLimit; //Math.Ceiling(axis.TopLimit/axis.Magnitude)*axis.Magnitude;
        }

        internal static double StretchMin(AxisCore axis)
        {
            return axis.BotLimit; //Math.Floor(axis.BotLimit/axis.Magnitude)*axis.Magnitude;
        }

        internal static double UnitRight(AxisCore axis)
        {
            return Math.Ceiling(axis.TopLimit/axis.Magnitude)*axis.Magnitude + 1;
        }

        internal static double UnitLeft(AxisCore axis)
        {
            return Math.Floor(axis.BotLimit/axis.Magnitude)*axis.Magnitude - 1;
        }

        internal static double SeparatorMax(AxisCore axis)
        {
            return (Math.Floor(axis.TopLimit/axis.S) + 1.0)*axis.S;
        }

        internal static double SeparatorMaxRounded(AxisCore axis)
        {
            return Math.Round((axis.TopLimit/axis.S) + 1.0, 0)*axis.S;
        }

        internal static double SeparatorMin(AxisCore axis)
        {
            return ((Math.Floor(axis.BotLimit/axis.S)) - 1.0)*axis.S;
        }
    }
}
