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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using LiveCharts.Maps;
using LiveCharts.Wpf.Maps;

namespace LiveCharts.Wpf
{
    public class GeoMap : UserControl
    {
        #region Constructors
        public GeoMap()
        {
            Canvas = new Canvas {ClipToBounds = true};
            Map = new Canvas {ClipToBounds = true};
            Canvas.Children.Add(Map);
            Content = Canvas;

            Canvas.SetBinding(WidthProperty,
                new Binding { Path = new PropertyPath(ActualWidthProperty), Source = this });
            Canvas.SetBinding(HeightProperty,
                new Binding { Path = new PropertyPath(ActualHeightProperty), Source = this });

            Cache = new Dictionary<string, MapData>();

            SetCurrentValue(DefaultLandFillProperty, new SolidColorBrush(Color.FromRgb(247,247,247)));
            SetCurrentValue(LandStrokeProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)));
            SetCurrentValue(LandStrokeThicknessProperty, 1d);
            SetCurrentValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(500));
            SetCurrentValue(BackgroundProperty, 
                new SolidColorBrush(Color.FromRgb(120,143,155)));

            SizeChanged += (sender, e) =>
            {
                Update();
            };

            MouseWheel += (sender, e) =>
            {
                e.Handled = true;
                var rt = Map.RenderTransform as ScaleTransform;
                var p = rt == null ? 1 : rt.ScaleX;
                p += e.Delta > 0 ? .05 : -.05;
                p = p < 1 ? 1 : p;
                var o = e.GetPosition(this);
                if (e.Delta > 0) Map.RenderTransformOrigin = new Point(o.X/ActualWidth,o.Y/ActualHeight);
                Map.RenderTransform = new ScaleTransform(p, p);
            };

            MouseDown += (sender, e) =>
            {
                DragOrigin = e.GetPosition(this);
            };

            MouseUp += (sender, e) =>
            {
                var end = e.GetPosition(this);
                var delta = new Point(DragOrigin.X - end.X, DragOrigin.Y - end.Y);

                var l = Canvas.GetLeft(Map) - delta.X;
                var t = Canvas.GetTop(Map) - delta.Y;

                if (DisableAnimations)
                {
                    Canvas.SetLeft(Map, l);
                    Canvas.SetTop(Map, t);
                }
                else
                {
                    Map.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(l, AnimationsSpeed));
                    Map.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(t, AnimationsSpeed));
                }
            };
        }

        #endregion

        #region Events

        public event Action<object, MapData> LandClick;

        #endregion

        #region Properties

        private Canvas Canvas { get; set; }
        private Canvas Map { get; set; }
        private Point DragOrigin { get; set; }
        private Point OriginalPosition { get; set; }
        private bool IsWidthDominant { get; set; }
        private Dictionary<string, MapData> Cache { get; set; }

        public static readonly DependencyProperty DefaultLandFillProperty = DependencyProperty.Register(
            "DefaultLandFill", typeof (Brush), typeof (GeoMap), new PropertyMetadata(default(Brush)));

        public Brush DefaultLandFill
        {
            get { return (Brush) GetValue(DefaultLandFillProperty); }
            set { SetValue(DefaultLandFillProperty, value); }
        }

        public static readonly DependencyProperty LandStrokeThicknessProperty = DependencyProperty.Register(
            "LandStrokeThickness", typeof (double), typeof (GeoMap), new PropertyMetadata(default(double)));

        public double LandStrokeThickness
        {
            get { return (double) GetValue(LandStrokeThicknessProperty); }
            set { SetValue(LandStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty LandStrokeProperty = DependencyProperty.Register(
            "LandStroke", typeof (Brush), typeof (GeoMap), new PropertyMetadata(default(Brush)));

        public Brush LandStroke
        {
            get { return (Brush) GetValue(LandStrokeProperty); }
            set { SetValue(LandStrokeProperty, value); }
        }

        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof (bool), typeof (GeoMap), new PropertyMetadata(default(bool)));

        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan), typeof (GeoMap), new PropertyMetadata(default(TimeSpan)));

        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof (bool), typeof (GeoMap), new PropertyMetadata(default(bool)));

        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        #endregion

        #region Publics

        public void MoveTo(MapData data)
        {
            var s = (Path) data.Shape;
            var area = s.Data.Bounds;
            var t = (ScaleTransform) s.RenderTransform;
            


            //if (DisableAnimations)
            //{
            
            double scale;
            double cx = 0;
            double cy = 0;

            if (IsWidthDominant)
            {
                scale = data.SvgMap.DesiredWidth / area.Width;
            }
            else
            {
                scale = data.SvgMap.DesiredHeight / area.Height;
            }

            Map.RenderTransformOrigin = new Point(0, 0);
            Map.RenderTransform = new ScaleTransform(scale, scale);

            Canvas.SetLeft(Map, -area.X*t.ScaleX*scale + cx);
            Canvas.SetTop(Map, -area.Y*t.ScaleY*scale + cy);

            //}
            //else
            //{
            //    Map.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(-area.X, AnimationsSpeed));
            //    //Map.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(area.Y, AnimationsSpeed));
            //}
        }

        public void Restart()
        {
            Map.RenderTransform = new ScaleTransform(1, 1);
            if (DisableAnimations)
            {
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }
            else
            {
                Map.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(OriginalPosition.X, TimeSpan.FromMilliseconds(1)));
                Map.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(OriginalPosition.Y, TimeSpan.FromMilliseconds(1)));
            }
        }

        #endregion
        

        #region Privates

        private void Update()
        {
            Map.Children.Clear();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Map.Children.Add(new TextBlock
                {
                    Text = "Designer preview is not currently available",
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    Effect = new DropShadowEffect
                    {
                        ShadowDepth = 2,
                        RenderingBias = RenderingBias.Performance
                    }
                });
                return;
            }

            var map = MapResolver.Get(LiveCharts.Maps.Maps.World);

            var desiredSize = new Size(map.DesiredWidth, map.DesiredHeight);
            var r = desiredSize.Width/desiredSize.Height;

            var wr = ActualWidth/desiredSize.Width;
            var hr = ActualHeight/desiredSize.Height;
            double s;

            if (wr < hr)
            {
                IsWidthDominant = true;
                Map.Width = ActualWidth;
                Map.Height = Map.Width/r;
                s = wr;
                OriginalPosition = new Point(0, (ActualHeight - Map.Height)*.5);
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }
            else
            {
                IsWidthDominant = false;
                Map.Height = ActualHeight;
                Map.Width = r*ActualHeight;
                s = hr;
                OriginalPosition = new Point((ActualWidth - Map.Width)*.5, 0d);
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }

            var t = new ScaleTransform(s, s);

            foreach (var land in map.Data)
            {
                var p = new Path
                {
                    Data = Geometry.Parse(land.Data),
                    RenderTransform = t
                };

                land.Shape = p;
                Cache[land.Name] = land;
                Map.Children.Add(p);

                p.MouseEnter += POnMouseEnter;
                p.MouseLeave += POnMouseLeave;
                p.MouseDown += POnMouseDown;

                p.SetBinding(Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath(LandStrokeProperty), Source = this});
                p.SetBinding(Shape.StrokeThicknessProperty,
                    new Binding {Path = new PropertyPath(LandStrokeThicknessProperty), Source = this});
                p.SetBinding(Shape.FillProperty,
                    new Binding {Path = new PropertyPath(DefaultLandFillProperty), Source = this});
            }      
        }

        private void POnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var map = Cache.Values.FirstOrDefault(x => x.Shape == sender);
            if (map == null) return;
            if (LandClick != null) LandClick.Invoke(sender, map);
        }

        private void POnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            var path = (Path)sender;
            path.Opacity = 1;
        }

        private void POnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            var path = (Path) sender;
            path.Opacity = .8;
        }

        #endregion
    }
}
