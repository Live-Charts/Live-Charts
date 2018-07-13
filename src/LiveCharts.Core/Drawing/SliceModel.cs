﻿#region License
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

#endregion

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// The slice model class.
    /// </summary>
    public static class Slice
    {
        /// <summary>
        /// Builds the specified angle.
        /// </summary>
        /// <param name="wedge">The angle.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="innerRadius">The inner radius.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="center">The center.</param>
        /// <param name="forceAngle">if set to <c>true</c> [force angle].</param>
        /// <param name="pushOut">The push out.</param>
        /// <returns></returns>
        public static S‪liceBuilderViewModel Build(
            double wedge, double radius, double innerRadius, 
            double cornerRadius, PointF center, bool forceAngle, double pushOut)
        {
            return GetPoints(
                wedge, radius, innerRadius, cornerRadius, center, forceAngle, pushOut);
        }

        private static S‪liceBuilderViewModel GetPoints(
            double wedge, double outerRadius, double innerRadius,
            double cornerRadius, PointF center, bool forceAngle, double pushOut)
        {
            // See docs/resources/slice.png
            // if you require to know more about the formulas in the geometry

            PointF[] pts = new PointF[8];

            const double toRadians = Math.PI / 180d;
            double a = wedge;

            if (a > 359.9f)
            {
                // workaround..
                // for some reason, large arc does not work whether true or false if angle eq 360...
                // so lets just make it look like the circle is complete.
                a = 359.99f;
            }

            double angle = a * toRadians;

            double cr = (outerRadius - innerRadius) / 2 > cornerRadius ? cornerRadius : (outerRadius - innerRadius) / 2;

            double temporal = Math.Atan(
                cr / Math.Sqrt(
                    Math.Pow(innerRadius + cr, 2) + Math.Pow(cr, 2)));

            if (angle < temporal * 2)
            {
                if (!forceAngle)
                {
                    angle = temporal * 2;
                }
                else
                {
                    cr = 0;
                }
            }

            double innerRoundingAngle = Math.Atan(
                cr / Math.Sqrt(
                    Math.Pow(innerRadius + cr, 2) + Math.Pow(cr, 2)));
            double outerRoundingAngle = Math.Atan(
                cr / Math.Sqrt(
                    Math.Pow(outerRadius - cr, 2) + Math.Pow(cr, 2)));

            if (double.IsNaN(innerRoundingAngle)) innerRoundingAngle = 0d;
            if (double.IsNaN(outerRoundingAngle)) outerRoundingAngle = 0d;
            
            double o1 = (innerRadius + cr) * Math.Cos(innerRoundingAngle);
            double o2 = (outerRadius - cr) * Math.Cos(outerRoundingAngle);
            double o3 = Math.Sqrt(Math.Pow(outerRadius - cr, 2) - Math.Pow(cr, 2));
            double o4 = Math.Sqrt(Math.Pow(innerRadius + cr, 2) - Math.Pow(cr, 2));

            double xp = pushOut * Math.Sin(angle / 2);
            double yp = pushOut * Math.Cos(angle / 2);

            unchecked
            {
                pts[0] = new PointF((float) (center.X + xp), (float) (center.Y + o1 + yp));
                pts[1] = new PointF((float) (center.X + xp), (float) (center.Y + o2 + yp));
                pts[2] = new PointF(
                    (float) (center.X + outerRadius * Math.Sin(outerRoundingAngle) + xp),
                    (float) (center.Y + outerRadius * Math.Cos(outerRoundingAngle) + yp));
                pts[3] = new PointF(
                    (float) (center.X + outerRadius * Math.Sin(angle - outerRoundingAngle) + xp),
                    (float) (center.Y + outerRadius * Math.Cos(angle - outerRoundingAngle) + yp));
                pts[4] = new PointF(
                    (float) (center.X + o3 * Math.Sin(angle) + xp),
                    (float) (center.Y + o3 * Math.Cos(angle) + yp));
                pts[5] = new PointF(
                    (float) (center.X + o4 * Math.Sin(angle) + xp),
                    (float) (center.Y + o4 * Math.Cos(angle) + yp));
                pts[6] = new PointF(
                    (float) (center.X + innerRadius * Math.Sin(angle - innerRoundingAngle) + xp),
                    (float) (center.Y + innerRadius * Math.Cos(angle - innerRoundingAngle) + yp));
                pts[7] = new PointF(
                    (float) (center.X + innerRadius * Math.Sin(innerRoundingAngle) + xp),
                    (float) (center.Y + innerRadius * Math.Cos(innerRoundingAngle) + yp));

                return new S‪liceBuilderViewModel(
                    pts,
                    (float) cr,
                    angle - outerRoundingAngle * 2 >= Math.PI,
                    angle - innerRoundingAngle * 2 >= Math.PI);
            }
        }
    }
}
