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

using LiveCharts.Coordinates;
using LiveCharts.Defaults;

#endregion

namespace LiveCharts
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The tracker constant.
        /// </summary>
        public const string TrackerKey = "Tracker";

        /// <summary>
        /// The scales pie constant
        /// </summary>
        public static int[] ScalesPieConst = {0, 0};

        /// <summary>
        /// Configures LiveCharts to plot the default types.
        /// </summary>
        /// <param name="settings">The configuration.</param>
        /// <returns></returns>
        public static Settings LearnPrimitiveAndDefaultTypes(this Settings settings)
        {
            // PointCoordinates
            settings.LearnMap<short, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<ushort, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<int, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<long, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<ulong, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<double, PointCoordinate>((value, index) => new PointCoordinate(index, (float) value));
            settings.LearnMap<float, PointCoordinate>((value, index) => new PointCoordinate(index, value));
            settings.LearnMap<decimal, PointCoordinate>((value, index) => new PointCoordinate(index, (float) value));
            settings.LearnMap<ObservableModel, PointCoordinate>((om, index) => new PointCoordinate(index, om.Value));
            settings.LearnMap<PointModel, PointCoordinate>((opm, index) => new PointCoordinate((float) opm.X, (float) opm.Y));

            // Stacked Coordinates
            settings.LearnMap<short, StackedPointCoordinate>((value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<ushort, StackedPointCoordinate>((value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<int, StackedPointCoordinate>((value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<long, StackedPointCoordinate>((value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<ulong, StackedPointCoordinate>((value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<double, StackedPointCoordinate>(
                (value, index) => new StackedPointCoordinate(index, (float) value));
            settings.LearnMap<float, StackedPointCoordinate>(
                (value, index) => new StackedPointCoordinate(index, value));
            settings.LearnMap<decimal, StackedPointCoordinate>(
                (value, index) => new StackedPointCoordinate(index, (float) value));
            settings.LearnMap<ObservableModel, StackedPointCoordinate>(
                (om, index) => new StackedPointCoordinate(index, om.Value));
            settings.LearnMap<PointModel, StackedPointCoordinate>(
                (opm, index) => new StackedPointCoordinate((float) opm.X, (float) opm.Y));

            // weighted coordinates
            settings.LearnMap<WeightedModel, WeightedCoordinate>((model, index) =>
                new WeightedCoordinate((float) model.X, (float) model.Y, model.Weight));
            
            // financial coordinates
            settings.LearnMap<FinancialModel, FinancialCoordinate>(
                (fm, index) => new FinancialCoordinate(index, fm.Open, fm.High, fm.Low, fm.Close));

            //charting.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            settings.LearnMap<WeightedCoordinate, WeightedCoordinate>((point, index) => new WeightedCoordinate(point.X, point.Y, point.Weight));

            return settings;
        }
    }
}
