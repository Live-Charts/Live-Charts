//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

namespace LiveCharts.Dtos
{
    public struct LabelEvaluation
    {
        public LabelEvaluation(double angle, double w, double h, AxisCore axis, AxisOrientation source) : this()
        {
            const double padding = 4;

            ActualWidth = w;
            ActualHeight = h;

            // for now there is no support for rotated and merged labels.
            // the labels will be rotated but there is no warranty that they are displayed correctly
            if (axis.View.IsMerged)
            {
                Top = 0;
                Bottom = 0;
                Left = 0;
                Right = 0;

                if (source == AxisOrientation.Y)
                {
                    XOffset = padding;
                    YOffset = padding;
                }
                else
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        //Bot
                        XOffset = padding;
                        YOffset = -h * 2 - padding;
                    }
                    else
                    {
                        //Top
                        XOffset = padding;
                        YOffset = padding + h * 2;
                    }
                }

                return;
            }

            //OK now lets evaluate the rotation angle...

            // the rotation angle starts from an horizontal line, yes like this text
            // - 0°, | 90°, - 180°, | 270°
            // notice normally rotating a label from 90 to 270° will show the label
            // in a wrong orientation
            // we need to fix that angle

            const double toRadians = Math.PI / 180;

            // 1. width components
            // 2. height components

            WFromW = Math.Abs(Math.Cos(angle * toRadians) * w); // W generated from the width of the label
            WFromH = Math.Abs(Math.Sin(angle * toRadians) * h); // W generated from the height of the label

            HFromW = Math.Abs(Math.Sin(angle * toRadians) * w); // H generated from the width of the label
            HFromH = Math.Abs(Math.Cos(angle * toRadians) * h); // H generated from the height of the label

            LabelAngle = angle % 360;
            if (LabelAngle < 0) LabelAngle += 360;
            if (LabelAngle > 90 && LabelAngle < 270)
                LabelAngle = (LabelAngle + 180) % 360;

            //at this points angles should only exist in 1st and 4th quadrant
            //those are the only quadrants that generates readable labels
            //the other 2 quadrants display inverted labels

            var quadrant = ((int)(LabelAngle / 90)) % 4 + 1;

            if (source == AxisOrientation.Y)
            {
                // Y Axis
                if (quadrant == 1)
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        // 1, L
                        Top = HFromW + (HFromH / 2);      //space taken from separator to top
                        Bottom = TakenHeight - Top;          //space taken from separator to bottom
                        XOffset = -WFromW - padding;    //distance from separator to label origin in X
                        YOffset = -Top;                 //distance from separator to label origin in Y
                    }
                    else
                    {
                        // 1, R
                        Bottom = HFromW + (HFromH / 2);
                        Top = TakenHeight - Bottom;
                        XOffset = padding + WFromH;
                        YOffset = -Top;
                    }
                }
                else
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        // 4, L
                        Bottom = HFromW + (HFromH / 2);
                        Top = TakenHeight - Bottom;
                        XOffset = -TakenWidth - padding;
                        YOffset = HFromW - (HFromH / 2);
                    }
                    else
                    {
                        // 4, R
                        Top = HFromW + (HFromH / 2);
                        Bottom = TakenHeight - Top;
                        XOffset = padding;
                        YOffset = -Bottom;
                    }
                }
            }
            else
            {
                // X Axis

                //axis x has one exception, if labels rotation equals 0° then the label is centered
                if (Math.Abs(axis.View.LabelsRotation) < .01)
                {
                    Left = TakenWidth / 2;
                    Right = Left;
                    XOffset = -Left;
                    YOffset = axis.Position == AxisPosition.LeftBottom
                        ? padding
                        : -padding - TakenHeight;
                }
                else
                {
                    if (quadrant == 1)
                    {
                        if (axis.Position == AxisPosition.LeftBottom)
                        {
                            //1, B
                            Right = WFromW + (WFromH / 2);  //space taken from separator to right
                            Left = TakenWidth - Right;           //space taken from separator to left
                            XOffset = Left;                 //distance from separator to label origin in X
                            YOffset = padding;              //distance from separator to label origin in Y
                        }
                        else
                        {
                            //1, T
                            Left = WFromW + (WFromH / 2);
                            Right = TakenWidth - Left;
                            XOffset = -WFromW;
                            YOffset = -padding - TakenHeight;
                        }
                    }
                    else
                    {
                        if (axis.Position == AxisPosition.LeftBottom)
                        {
                            //4, B
                            Left = WFromW + (WFromH / 2);
                            Right = TakenWidth - Left;
                            XOffset = -Left;
                            YOffset = padding + HFromW;
                        }
                        else
                        {
                            //4, T
                            Right = WFromW + (WFromH / 2);
                            Left = TakenWidth - Right;
                            XOffset = -Left;
                            YOffset = -HFromH;
                        }
                    }
                }
            }

        }

        public double LabelAngle { get; set; }
        public double WFromW { get; set; }
        public double WFromH { get; set; }
        public double HFromW { get; set; }
        public double HFromH { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }

        public double TakenWidth { get { return WFromW + WFromH; } }
        public double TakenHeight { get { return HFromW + HFromH; } }

        public double ActualWidth { get; private set; }
        public double ActualHeight { get; private set; }

        public double GetOffsetBySource(AxisOrientation source)
        {
            return source == AxisOrientation.X
                ? XOffset
                : YOffset;
        }
    }
}