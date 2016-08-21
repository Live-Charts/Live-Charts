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
using System.Globalization;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Uwp.Components;
using LiveCharts.Uwp.Points;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The gauge chart is useful to display progress or completion.
    /// </summary>
    public class AngularGauge : UserControl
    {
        public AngularGauge()
        {
            Canvas = new Canvas(); //{ ClipToBounds = true};
            Content = Canvas;

            StickRotateTransform = new RotateTransform(180);
            Stick = new Path
            {
                Data = GeometryHelper.Parse("m0,90 a5,5 0 0 0 20,0 l-8,-88 a2,2 0 0 0 -4 0 z"),
                Fill = new SolidColorBrush(Colors.CornflowerBlue),
                Stretch = Stretch.Fill,
                RenderTransformOrigin = new Point(0.5, 0.9),
                RenderTransform = StickRotateTransform
            };
            Canvas.Children.Add(Stick);
            Canvas.SetZIndex(Stick, 1);

            Canvas.SetBinding(WidthProperty,
                new Binding { Path = new PropertyPath("ActualWidth"), Source = this });
            Canvas.SetBinding(HeightProperty,
                new Binding { Path = new PropertyPath("ActualHeight"), Source = this });

            SetCurrentValue(SectionsProperty, new List<AngularSection>());
            SetCurrentValue(NeedleFillProperty, new SolidColorBrush(Color.FromArgb(255, 69, 90, 100)));

            Stick.SetBinding(Shape.FillProperty,
                new Binding {Path = new PropertyPath("NeedleFill"), Source = this});
            
            SetCurrentValue(AnimationsSpeedProperty, TimeSpan.FromMilliseconds(500));
            SetCurrentValue(TicksForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 210, 210, 210)));
            Func<double, string> defaultFormatter = x => x.ToString(CultureInfo.InvariantCulture);
            SetCurrentValue(LabelFormatterProperty, defaultFormatter);
            //SetCurrentValue(LabelsEffectProperty,
            //    new DropShadowEffect {ShadowDepth = 2, RenderingBias = RenderingBias.Performance});

            SizeChanged += (sender, args) =>
            {
                IsControlLaoded = true;
                Draw();
            };
        }

        #region Properties

        private Canvas Canvas { get; set; }
        private Path Stick { get; set; }
        private RotateTransform StickRotateTransform { get; set; }
        private bool IsControlLaoded { get; set; }

        public static readonly DependencyProperty WedgeProperty = DependencyProperty.Register(
            "Wedge", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(300d, Redraw));
        /// <summary>
        /// Gets or sets the opening angle in the gauge
        /// </summary>
        public double Wedge
        {
            get { return (double) GetValue(WedgeProperty); }
            set { SetValue(WedgeProperty, value); }
        }

        public static readonly DependencyProperty TicksStepProperty = DependencyProperty.Register(
            "TicksStep", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(2d, Redraw));
        /// <summary>
        /// Gets or sets the separation between every tick
        /// </summary>
        public double TicksStep
        {
            get { return (double) GetValue(TicksStepProperty); }
            set { SetValue(TicksStepProperty, value); }
        }

        public static readonly DependencyProperty LabelsStepProperty = DependencyProperty.Register(
            "LabelsStep", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(10d, Redraw));
        /// <summary>
        /// Gets or sets the separation between every label
        /// </summary>
        public double LabelsStep
        {
            get { return (double) GetValue(LabelsStepProperty); }
            set { SetValue(LabelsStepProperty, value); }
        }

        public static readonly DependencyProperty FromValueProperty = DependencyProperty.Register(
            "FromValue", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(0d, Redraw));
        /// <summary>
        /// Gets or sets the minimum value of the gauge
        /// </summary>
        public double FromValue
        {
            get { return (double) GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        public static readonly DependencyProperty ToValueProperty = DependencyProperty.Register(
            "ToValue", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(100d, Redraw));
        /// <summary>
        /// Gets or sets the maximum value of the gauge
        /// </summary>
        public double ToValue
        {
            get { return (double) GetValue(ToValueProperty); }
            set { SetValue(ToValueProperty, value); }
        }

        public static readonly DependencyProperty SectionsProperty = DependencyProperty.Register(
            "Sections", typeof (List<AngularSection>), typeof (AngularGauge), 
            new PropertyMetadata(default(SectionsCollection), Redraw));
        /// <summary>
        /// Gets or sets a collection of sections
        /// </summary>
        public List<AngularSection> Sections
        {
            get { return (List<AngularSection>) GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(default(double), ValueChangedCallback));
        /// <summary>
        /// Gets or sets the current gauge value
        /// </summary>
        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof (Func<double, string>), typeof (AngularGauge), new PropertyMetadata(default(Func<double, string>)));
        /// <summary>
        /// Gets or sets the label formatter
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>) GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        public static readonly DependencyProperty DisableaAnimationsProperty = DependencyProperty.Register(
            "DisableaAnimations", typeof (bool), typeof (AngularGauge), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the chart is animated
        /// </summary>
        public bool DisableaAnimations
        {
            get { return (bool) GetValue(DisableaAnimationsProperty); }
            set { SetValue(DisableaAnimationsProperty, value); }
        }

        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan), typeof (AngularGauge), new PropertyMetadata(default(TimeSpan)));
        /// <summary>
        /// Gets or sets the animations speed
        /// </summary>
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        public static readonly DependencyProperty TicksForegroundProperty = DependencyProperty.Register(
            "TicksForeground", typeof (Brush), typeof (AngularGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the ticks foreground
        /// </summary>
        public Brush TicksForeground
        {
            get { return (Brush) GetValue(TicksForegroundProperty); }
            set { SetValue(TicksForegroundProperty, value); }
        }

        public static readonly DependencyProperty SectionsInnerRadiusProperty = DependencyProperty.Register(
            "SectionsInnerRadius", typeof (double), typeof (AngularGauge), 
            new PropertyMetadata(0.94d, Redraw));
        /// <summary>
        /// Gets or sets the inner radius of all the sections in the chart, the unit of this property is percentage, goes from 0 to 1
        /// </summary>
        public double SectionsInnerRadius
        {
            get { return (double) GetValue(SectionsInnerRadiusProperty); }
            set { SetValue(SectionsInnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty NeedleFillProperty = DependencyProperty.Register(
            "NeedleFill", typeof (Brush), typeof (AngularGauge), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets o sets the needle fill
        /// </summary>
        public Brush NeedleFill
        {
            get { return (Brush) GetValue(NeedleFillProperty); }
            set { SetValue(NeedleFillProperty, value); }
        }

        //public static readonly DependencyProperty LabelsEffectProperty = DependencyProperty.Register(
        //    "LabelsEffect", typeof (Effect), typeof (AngularGauge), new PropertyMetadata(default(Effect)));

        //public Effect LabelsEffect
        //{
        //    get { return (Effect) GetValue(LabelsEffectProperty); }
        //    set { SetValue(LabelsEffectProperty, value); }
        //}

        public static readonly DependencyProperty TicksStrokeThicknessProperty = DependencyProperty.Register(
            "TicksStrokeThickness", typeof (double), typeof (AngularGauge), new PropertyMetadata(2d));

        public double TicksStrokeThickness
        {
            get { return (double) GetValue(TicksStrokeThicknessProperty); }
            set { SetValue(TicksStrokeThicknessProperty, value); }
        }

        #endregion

        private static void ValueChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ag = (AngularGauge)o;
            ag.MoveStick();
        }

        private static void Redraw(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ag = (AngularGauge) o;
            ag.Draw();
        }

        private void MoveStick()
        {
            Wedge = Wedge > 360 ? 360 : (Wedge < 0 ? 0 : Wedge);

            var fromAlpha = (360 - Wedge) * .5;
            var toAlpha = 360 - fromAlpha;
            
            var alpha = LinearInterpolation(fromAlpha, toAlpha, FromValue, ToValue, Value) + 180;

            if (DisableaAnimations)
            {
                StickRotateTransform.Angle = alpha;
            }
            else
            {
                StickRotateTransform.BeginAnimation(RotateTransform.AngleProperty,
                    new DoubleAnimation(alpha, AnimationsSpeed));
            }
        }

        private void Draw()
        {
            if (!IsControlLaoded) return;

            //No cache for you gauge :( kill and redraw please
            foreach (var child in Canvas.Children.Cast<UIElement>()
                .Where(x => !Equals(x, Stick)).ToArray())
                Canvas.Children.Remove(child);

            Wedge = Wedge > 360 ? 360 : (Wedge < 0 ? 0 : Wedge);

            var fromAlpha = (360-Wedge)*.5;
            var toAlpha = 360 - fromAlpha;

            var d = ActualWidth < ActualHeight ? ActualWidth : ActualHeight;

            Stick.Height = d*.5*.8;
            Stick.Width = Stick.Height*.2;

            Canvas.SetLeft(Stick, ActualWidth*.5 - Stick.Width*.5);
            Canvas.SetTop(Stick, ActualHeight*.5 - Stick.Height*.9);

            var ticksHi = d*.5;
            var ticksHj = d*.47;
            var labelsHj = d*.44;

            if (Sections.Any())
            {
                SectionsInnerRadius = SectionsInnerRadius > 1
                    ? 1
                    : (SectionsInnerRadius < 0
                        ? 0
                        : SectionsInnerRadius);
                
                foreach (var section in Sections)
                {
                    var start = LinearInterpolation(fromAlpha, toAlpha, 
                        FromValue, ToValue, section.FromValue)+180;
                    var end = LinearInterpolation(fromAlpha, toAlpha, 
                        FromValue, ToValue, section.ToValue)+180;

                    var slice = new PieSlice
                    {
                        RotationAngle = start,
                        WedgeAngle = end - start,
                        Radius = d*.5,
                        InnerRadius = d*.5*SectionsInnerRadius,
                        Fill = section.Fill
                    };
                    Canvas.Children.Add(slice);
                    Canvas.SetTop(slice, ActualHeight*.5);
                    Canvas.SetLeft(slice, ActualWidth*.5);
                }
            }

            for (var i = FromValue; i <= ToValue; i += TicksStep)
            {
                var alpha = LinearInterpolation(fromAlpha, toAlpha, FromValue, ToValue, i) + 90;

                var tick = new Line
                {
                    X1 = ActualWidth*.5 + ticksHi*Math.Cos(alpha*Math.PI/180),
                    X2 = ActualWidth*.5 + ticksHj*Math.Cos(alpha*Math.PI/180),
                    Y1 = ActualHeight*.5 + ticksHi*Math.Sin(alpha*Math.PI/180),
                    Y2 = ActualHeight*.5 + ticksHj*Math.Sin(alpha*Math.PI/180)
                };
                Canvas.Children.Add(tick);
                tick.SetBinding(Shape.StrokeProperty,
                    new Binding {Path = new PropertyPath("TicksForeground"), Source = this});
                tick.SetBinding(Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath("TicksStrokeThickness"), Source = this });
            }

            for (var i = FromValue; i <= ToValue; i += LabelsStep)
            {
                var alpha = LinearInterpolation(fromAlpha, toAlpha, FromValue, ToValue, i) + 90;

                var tick = new Line
                {
                    X1 = ActualWidth*.5 + ticksHi*Math.Cos(alpha*Math.PI/180),
                    X2 = ActualWidth*.5 + labelsHj*Math.Cos(alpha*Math.PI/180),
                    Y1 = ActualHeight*.5 + ticksHi*Math.Sin(alpha*Math.PI/180),
                    Y2 = ActualHeight*.5 + labelsHj*Math.Sin(alpha*Math.PI/180)
                };

                Canvas.Children.Add(tick);
                var label = new TextBlock
                {
                    Text = LabelFormatter(i)
                };

                label.SetBinding(EffectProperty,
                    new Binding {Path = new PropertyPath("LabelsEffect"), Source = this});

                Canvas.Children.Add(label);
                label.UpdateLayout();
                Canvas.SetLeft(label, alpha < 270
                    ? tick.X2
                    : (Math.Abs(alpha - 270) < 4
                        ? tick.X2 - label.ActualWidth*.5
                        : tick.X2 - label.ActualWidth));
                Canvas.SetTop(label, tick.Y2);
                tick.SetBinding(Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("TicksForeground"), Source = this });
                tick.SetBinding(Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath("TicksStrokeThickness"), Source = this });
            }
            MoveStick();
        }

        private static double LinearInterpolation(double fromA, double toA, double fromB, double toB, double value)
        {
            var p1 = new Point(fromB, fromA);
            var p2 = new Point(toB, toA);

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);

            return m*(value - p1.X) + p1.Y;
        }

    }
}