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
        public static Charting LearnPrimitiveAndDefaultTypes(this Charting charting)
        {
            // PointCoordinates

            charting.LearnType<short>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<ushort>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<int>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<long>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<ulong>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<double>((value, index) => new PointCoordinate(index, (float) value));
            charting.LearnType<float>((value, index) => new PointCoordinate(index, value));
            charting.LearnType<decimal>((value, index) => new PointCoordinate(index, (float) value));
            charting.LearnType<ObservableModel>((om, index) => new PointCoordinate(index, om.Value));
            charting.LearnType<PointModel>((opm, index) => new PointCoordinate(opm.X, opm.Y));

            // weighted coordinates

            charting.LearnType<WeightedModel, WeightedCoordinate>((model, index) =>
                new WeightedCoordinate(model.X, model.Y, model.Weight));
            
            // financial coordinates

            charting.LearnType<FinancialModel, FinancialCoordinate>(
                (fm, index) => new FinancialCoordinate(index, fm.Open, fm.High, fm.Low, fm.Close));

            //charting.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            charting.LearnType<WeightedCoordinate, WeightedCoordinate>((point, index) => new WeightedCoordinate(point.X, point.Y, point.Weight));

            return charting;
        }
    }
}