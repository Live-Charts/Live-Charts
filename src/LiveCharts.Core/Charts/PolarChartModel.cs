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

using System.Drawing;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction;

#endregion

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Represents a chart with a polar system.
    /// </summary>
    /// <seealso cref="ChartModel" />
    public class PolarChartModel : ChartModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarChartModel"/> class.
        /// </summary>
        /// <param name="view">The chart view.</param>
        public PolarChartModel(IChartView view)
            : base(view)
        {
        }

        /// <inheritdoc />
        protected override int DimensionsCount => 2;

        /// <inheritdoc />
        public override float ScaleToUi(double dataValue, Plane plane, float[] sizeVector = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override float ScaleFromUi(float pixelsValue, Plane plane, float[] sizeVector = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override void ViewOnPointerMoved(TooltipSelectionMode selectionMode, PointF pointerLocation)
        {
            throw new System.NotImplementedException();
        }
    }
}