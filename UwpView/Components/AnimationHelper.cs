using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace LiveCharts.Uwp.Components
{
    public static class AnimationHelper
    {
        public static DoubleAnimation CreateDouble(double? to, Duration duration, string targetProperty)
        {
            var animation = new DoubleAnimation()
            {
                To = to,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation, targetProperty);
            return animation;
        }

        public static PointAnimation CreatePoint(Point from, Point to, Duration duration, string targetProperty)
        {
            var animation = new PointAnimation()
            {
                From = from,
                To = to,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation, targetProperty);
            return animation;
        }

        public static void BeginDoubleAnimation(this DependencyObject target, string path, double? to,
            Duration duration)
        {
            var animation = new DoubleAnimation()
            {
                To = to,
                Duration = duration
            };
            target.BeginDoubleAnimation(animation, path);
        }

        public static void BeginDoubleAnimation(this DependencyObject target, string path, double? from, double? to,
            Duration duration)
        {
            var animation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = duration
            };
            target.BeginDoubleAnimation(animation, path);
        }

        public static void BeginDoubleAnimation(this DependencyObject target, DoubleAnimation animation, string path)
        {
            var sb = new Storyboard();
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, path);
            sb.Children.Add(animation);
            sb.Begin();
        }

        public static void CreateStoryBoardAndBegin(DependencyObject target, params Timeline[] animation)
        {
            var sb = new Storyboard();
            foreach (var timeline in animation)
            {
                Storyboard.SetTarget(timeline, target);
                sb.Children.Add(timeline);
            }
            sb.Begin();
        }

        public static void CreateCanvasStoryBoardAndBegin(this DependencyObject target, double? toX, double? toY, Duration duration)
        {
            var xAnimation = CreateDouble(toX, duration, "Canvas.Left");
            var yAnimation = CreateDouble(toY, duration, "Canvas.Top");
            CreateStoryBoardAndBegin(target, xAnimation, yAnimation);
        }

        public static void CreateY1Y2StoryBoardAndBegin(this DependencyObject target, double? toY1, double? toY2,
            Duration duration)
        {
            var y1Animation = CreateDouble(toY1, duration, nameof(Line.Y1));
            var y2Animation = CreateDouble(toY2, duration, nameof(Line.Y2));
            CreateStoryBoardAndBegin(target, y1Animation, y2Animation);
        }

        public static Storyboard CreateStoryBoard(DependencyObject target,params Timeline[] animation)
        {
            var sb = new Storyboard();
            foreach (var timeline in animation)
            {
                Storyboard.SetTarget(timeline, target);
                sb.Children.Add(timeline);
            }
            return sb;
        }
    }
}
