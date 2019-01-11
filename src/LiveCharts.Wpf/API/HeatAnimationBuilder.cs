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


#endregion

using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Wpf
{
    public class HeatAnimationBuilder : AnimationBuilder<ChartHeatShape>
    {
        private AnimatableArguments _animatableArguments;
        private ChartHeatShape _shape;

        public HeatAnimationBuilder(ChartHeatShape target, AnimatableArguments arguments)
            : base(target, arguments)
        {
            _animatableArguments = arguments;
            _shape = target;
        }

        public override IAnimationBuilder Property(
            string property, System.Drawing.Color from, System.Drawing.Color to, double delay = 0)
        {
            if (property == nameof(IHeatShape.Color))
            {
                var animation = new ColorAnimation
                {
                    RepeatBehavior = new RepeatBehavior(1),
                    Duration = _animatableArguments.Duration,
                    EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                    From = Color.FromArgb(from.A, from.R, from.G, from.B),
                    To = Color.FromArgb(to.A, to.R, to.G, to.B)
                };

                SetTarget(animation, SolidColorBrush.ColorProperty, _shape.Fill);

                return this;
            }

            return base.Property(property, from, to, delay);
        }
    }
}
