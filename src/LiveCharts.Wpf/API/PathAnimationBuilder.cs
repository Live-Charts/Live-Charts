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

using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// An animation builder for WPF.
    /// </summary>
    public class PathAnimationBuilder : IAnimationBuilder
    {
        private readonly ChartPath _path;
        private readonly AnimatableArguments _animatableArguments;
        private readonly Storyboard _storyboard;

        public PathAnimationBuilder(ChartPath path, AnimatableArguments args)
        {
            _path = path;
            _animatableArguments = args;
            _storyboard = new Storyboard();
        }

        /// <inheritdoc></inheritdoc>
        public void Begin()
        {
            _storyboard.Begin();
            _storyboard.Completed += OnFinished;
        }

        /// <inheritdoc></inheritdoc>
        public void Dispose()
        {
            _storyboard.Completed -= OnFinished;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(string property, double from, double to, double delay = 0)
        {
            var p = DependencyPropertyDescriptor.FromName(property, typeof(Path), typeof(Path)).DependencyProperty;

            switch (property)
            {
                case nameof(IPath.Opacity):
                case nameof(IPath.ZIndex):
                    var a1 = new DoubleAnimation
                    {
                        RepeatBehavior = new RepeatBehavior(1),
                        Duration = _animatableArguments.Duration,
                        EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                        From = from,
                        To = to
                    };
                    SetTargetToFillPath(a1, p);
                    SetTargetToStrokePath(a1.Clone(), p);
                    break;

                case nameof(IPath.StrokeDashOffset):
                case nameof(IPath.StrokeThickness):
                    var a2 = new DoubleAnimation
                    {
                        RepeatBehavior = new RepeatBehavior(1),
                        Duration = _animatableArguments.Duration,
                        EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                        From = from,
                        To = to
                    };
                    SetTargetToStrokePath(a2, p);
                    break;
                case nameof(IPath.StartPoint):
                case nameof(IPath.StrokeDashArray):
                default:
                    throw new LiveChartsException(147, property);
            }

            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(string property, PointD from, PointD to, double delay = 0)
        {
            if (property == nameof(IPath.StartPoint))
            {
                var a = new PointAnimation
                {
                    RepeatBehavior = new RepeatBehavior(1),
                    Duration = _animatableArguments.Duration,
                    EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                    From = new Point(from.X, from.Y),
                    To = new Point(to.X, to.Y)
                };
                SetTargetToFillFigure(a, PathFigure.StartPointProperty);
                SetTargetToStrokeFigure(a.Clone(), PathFigure.StartPointProperty);
            }
            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(string property, System.Drawing.Color from, System.Drawing.Color to, double delay = 0)
        {
            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Then(EventHandler callback)
        {
            _storyboard.Completed += callback;
            return this;
        }

        private void OnFinished(object sender, EventArgs eventArgs)
        {
            Dispose();
        }

        private void SetTargetToFillPath(AnimationTimeline animation, DependencyProperty property)
        {
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _path._fillPath.Path);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
        }

        private void SetTargetToStrokePath(AnimationTimeline animation, DependencyProperty property)
        {
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _path._strokePath.Path);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
        }

        private void SetTargetToFillFigure(AnimationTimeline animation, DependencyProperty property)
        {
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _path._fillPath.Figure);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
        }

        private void SetTargetToStrokeFigure(AnimationTimeline animation, DependencyProperty property)
        {
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _path._strokePath.Figure);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
        }
    }
}
