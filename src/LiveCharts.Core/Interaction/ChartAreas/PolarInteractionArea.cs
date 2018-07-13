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
using System.Drawing;
using System.Net.Sockets;
using LiveCharts.Core.Interaction.Controls;

#endregion

namespace LiveCharts.Core.Interaction.ChartAreas
{
    /// <summary>
    /// The polar interaction area class.
    /// </summary>
    /// <seealso cref="InteractionArea" />
    public class PolarInteractionArea : InteractionArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarInteractionArea"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="innerRadius"></param>
        /// <param name="angleFrom">The angle from.</param>
        /// <param name="angleTo">The angle to.</param>
        /// <param name="chartCenter">The chart center.</param>
        public PolarInteractionArea(
            float radius, float innerRadius, float angleFrom, float angleTo, PointF chartCenter)
        {
            Radius = radius;
            InnerRadius = innerRadius;
            AngleFrom = angleFrom;
            AngleTo = angleTo;
            Center = chartCenter;
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public float Radius { get; }

        /// <summary>
        /// Gets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        public float InnerRadius { get; }

        /// <summary>
        /// Gets the angle start point in radians.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public float AngleFrom { get; }

        /// <summary>
        /// Gets the angle end point in radians.
        /// </summary>
        /// <value>
        /// The angle to.
        /// </value>
        public float AngleTo { get; }

        /// <summary>
        /// Gets the center of the chart.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public PointF Center { get; }

        /// <inheritdoc />
        public override bool Contains(PointF pointerLocation, ToolTipSelectionMode selectionMode)
        {
            float x = pointerLocation.X;
            float y = pointerLocation.Y;

            float dx = Math.Abs(x - Center.X);
            float dy = Math.Abs(y - Center.Y);

            double radius = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            double angle = Math.Atan(dy / dx) * 180 / Math.PI;

            // correction because ATan returns angles from 0-90

            if (x >= Center.X && y >= Center.Y)
            {
                angle = 90 - angle;
            }

            if (x >= Center.X && y <= Center.Y)
            {
                angle = 90 + angle;
            }

            if (x <= Center.X && y <= Center.Y)
            {
                angle = 270 - angle;
            }

            if (x <= Center.X && y >= Center.Y)
            {
                angle = 270 + angle;
            }

            return AngleFrom <= angle && AngleTo >= angle && radius <= Radius && radius >= InnerRadius;
        }

        /// <inheritdoc />
        public override float DistanceTo(PointF pointerLocation, ToolTipSelectionMode selectionMode)
        {
            float x = pointerLocation.X;
            float y = pointerLocation.Y;

            float dx = Math.Abs(x - Center.X);
            float dy = Math.Abs(y - Center.Y);

            double radius = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            double angle = Math.Atan(dy / dx);

            double a = (AngleFrom + AngleTo) / 2d;
            double r = (Math.PI / 180) * (InnerRadius + Radius) / 2d;

            return (float) Math.Sqrt(
                Math.Pow(Math.Sin(angle) * radius - Math.Sin(a) * r, 2) +
                Math.Pow(Math.Cos(angle) * radius - Math.Cos(a) * r, 2));
        }
    }
}