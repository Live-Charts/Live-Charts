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

using LiveCharts.Core.Coordinates;

#endregion

namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class PlotTypesConfig
    {
        /// <summary>
        /// Configures LiveCharts to plot the default types.
        /// </summary>
        /// <param name="charting">The configuration.</param>
        /// <returns></returns>
        public static Charting ForPrimitiveAndDefaultTypes(this Charting charting)
        {
            charting.For<short>((value, index) => new Point(index, value));
            charting.For<ushort>((value, index) => new Point(index, value));
            charting.For<int>((value, index) => new Point(index, value));
            charting.For<long>((value, index) => new Point(index, value));
            charting.For<ulong>((value, index) => new Point(index, value));
            charting.For<double>((value, index) => new Point(index, (float) value));
            charting.For<float>((value, index) => new Point(index, value));

            charting.For<short, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));
            charting.For<ushort, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));
            charting.For<int, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));
            charting.For<long, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));
            charting.For<ulong, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));
            charting.For<double, WeightedPoint>((value, index) => new WeightedPoint(index, (float) value, 0));
            charting.For<float, WeightedPoint>((value, index) => new WeightedPoint(index, value, 0));

            charting.For<decimal>((value, index) => new Point(index, (float) value));
            charting.For<decimal, WeightedPoint>((value, index) => new WeightedPoint(index, (float) value, 0));

            charting.For<ObservableModel>((om, index) => new Point(index, om.Value));
            charting.For<ObservablePointModel>((opm, index) => new Point(opm.X, opm.Y));

            charting.For<FinancialModel, FinancialPoint>(
                (fm, index) => new FinancialPoint(index, fm.Open, fm.High, fm.Low, fm.Close));

            charting.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            charting.For<WeightedPoint, WeightedPoint>((point, index) => new WeightedPoint(point.X, point.Y, point.Weight));

            return charting;
        }
    }
}