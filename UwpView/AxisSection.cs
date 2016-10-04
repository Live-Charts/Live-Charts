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

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Charts;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// An Axis section highlights values or ranges in a chart.
    /// </summary>
    public class AxisSection : FrameworkElement, IAxisSectionView
    {
        private readonly Rectangle _rectangle;
        private readonly TextBlock _label;

        /// <summary>
        /// Initializes a new instance of AxisSection class
        /// </summary>
        public AxisSection()
        {
            _rectangle = new Rectangle();
            _label = new TextBlock();
            
            /*Current*/SetValue(StrokeProperty, new SolidColorBrush(Color.FromArgb(0, 131, 172, 191)));
            /*Current*/SetValue(FillProperty, new SolidColorBrush(Color.FromArgb(0, 131, 172, 191)) {Opacity = .35});
            /*Current*/SetValue(StrokeThicknessProperty, 0d);
            /*Current*/SetValue(FromValueProperty, 0d);
            /*Current*/SetValue(ToValueProperty, 0d);

            BindingOperations.SetBinding(_rectangle, Shape.FillProperty,
                    new Binding { Path = new PropertyPath(nameof(Fill)), Source = this });
            BindingOperations.SetBinding(_rectangle, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath(nameof(Stroke)), Source = this });
            BindingOperations.SetBinding(_rectangle, Shape.StrokeDashArrayProperty,
                    new Binding { Path = new PropertyPath(nameof(StrokeDashArray)), Source = this });
            BindingOperations.SetBinding(_rectangle, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath(nameof(StrokeThickness)), Source = this });

            BindingOperations.SetBinding(_label, TextBlock.TextProperty,
                new Binding {Path = new PropertyPath(nameof(Label)), Source = this});

            Canvas.SetZIndex(_rectangle, -1);
        }

        public AxisSectionCore Model { get; set; }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof (string), typeof (AxisSection), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the name, the title of the section, a visual element will be added to the chart if this property is not null.
        /// </summary>
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty FromValueProperty = DependencyProperty.Register(
            "FromValue", typeof (double), typeof (AxisSection), 
            new PropertyMetadata(default(double), CallChartUpdater));
        /// <summary>
        /// Gets or sets the value where the section starts
        /// </summary>
        public double FromValue
        {
            get { return (double) GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        public static readonly DependencyProperty ToValueProperty = DependencyProperty.Register(
            "ToValue", typeof (double), typeof (AxisSection), 
            new PropertyMetadata(default(double), CallChartUpdater));
        /// <summary>
        /// Gets or sets the value where the section ends
        /// </summary>
        public double ToValue
        {
            get { return (double) GetValue(ToValueProperty); }
            set { SetValue(ToValueProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (AxisSection), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets o sets the section stroke, the stroke brush will be used to draw the border of the section
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof (Brush), typeof (AxisSection), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the section fill brush.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (AxisSection), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof (DoubleCollection), typeof (AxisSection), new PropertyMetadata(default(DoubleCollection)));
        /// <summary>
        /// Gets or sets the stroke dash array collection, use this property to create dashed stroke sections
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection) GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public void DrawOrMove(AxisOrientation source, int axis)
        {
            if (Parent == null)
            {
                Model.Chart.View.AddToView(this);
                Model.Chart.View.AddToDrawMargin(_rectangle);
                Model.Chart.View.AddToDrawMargin(_label);
                _rectangle.Height = 0;
                _rectangle.Width = 0;
                Canvas.SetLeft(_rectangle, 0d);
                Canvas.SetTop(_rectangle, Model.Chart.DrawMargin.Height);
                Canvas.SetTop(_label, Model.Chart.DrawMargin.Height);
                Canvas.SetLeft(_label, 0d);
            }

            var from = ChartFunctions.ToDrawMargin(FromValue, source, Model.Chart, axis);
            var to = ChartFunctions.ToDrawMargin(ToValue, source, Model.Chart, axis);

            if (from > to)
            {
                var temp = to;
                to = from;
                from = temp;
            }

            var anSpeed = Model.Chart.View.AnimationsSpeed;
            _label.UpdateLayout();

            if (source == AxisOrientation.X)
            {
                var w = to - from;
                w = StrokeThickness > w ? StrokeThickness : w;

                Canvas.SetTop(_rectangle, 0);
                _rectangle.Height = Model.Chart.DrawMargin.Height;

                if (Model.Chart.View.DisableAnimations)
                {
                    _rectangle.Width = w > 0 ? w : 0;
                    Canvas.SetLeft(_rectangle, from);
                    Canvas.SetLeft(_label, (from + to)/2 - _label.ActualWidth/2);
                }
                else
                {
                    var storyBoard = new Storyboard();
                    var widthAnimation = new DoubleAnimation()
                    {
                        To = w > 0 ? w : 0,
                        Duration = anSpeed
                    };
                    var rectangleLeftAnimation = new DoubleAnimation()
                    {
                        To = from,
                        Duration = anSpeed
                    };
                    var labelLeftAnimation = new DoubleAnimation()
                    {
                        To = (from + to) / 2 - _label.ActualWidth / 2,
                        Duration = anSpeed
                    };
                    Storyboard.SetTarget(widthAnimation, _rectangle);
                    Storyboard.SetTargetProperty(widthAnimation, nameof(_rectangle.Width));

                    Storyboard.SetTarget(rectangleLeftAnimation, _rectangle);
                    Storyboard.SetTargetProperty(rectangleLeftAnimation, "(Canvas.Left)");

                    Storyboard.SetTarget(labelLeftAnimation, _label);
                    Storyboard.SetTargetProperty(labelLeftAnimation, "(Canvas.Left)");

                    storyBoard.Children.Add(widthAnimation);
                    storyBoard.Children.Add(rectangleLeftAnimation);
                    storyBoard.Children.Add(labelLeftAnimation);

                    storyBoard.Begin();
                }
                return;
            }

            var h = to - from;
            h = StrokeThickness > h ? StrokeThickness : h;

            Canvas.SetLeft(_rectangle, 0d);
            _rectangle.Width = Model.Chart.DrawMargin.Width;

            if (Model.Chart.View.DisableAnimations)
            {
                Canvas.SetTop(_rectangle, from);
                _rectangle.Height = h > 0 ? h : 0;
                Canvas.SetTop(_label, (from + to)/2 - _label.ActualHeight/2);
            }
            else
            {
                var storyBoard = new Storyboard();
                var rectangleTopAnimation = new DoubleAnimation()
                {
                    To = from,
                    Duration = anSpeed
                };
                var heightAnimation = new DoubleAnimation()
                {
                    To = h,
                    Duration = anSpeed
                };
                var labelTopAnimation = new DoubleAnimation()
                {
                    To = (from + to) / 2 - _label.ActualHeight / 2,
                    Duration = anSpeed
                };
                Storyboard.SetTarget(rectangleTopAnimation, _rectangle);
                Storyboard.SetTargetProperty(rectangleTopAnimation, "(Canvas.Top)");

                Storyboard.SetTarget(heightAnimation, _rectangle);
                Storyboard.SetTargetProperty(heightAnimation, nameof(Height));

                Storyboard.SetTarget(labelTopAnimation, _label);
                Storyboard.SetTargetProperty(labelTopAnimation, "(Canvas.Top)");

                storyBoard.Children.Add(rectangleTopAnimation);
                storyBoard.Children.Add(heightAnimation);
                storyBoard.Children.Add(labelTopAnimation);

                storyBoard.Begin();
            }
        }

        public void Remove()
        {
            Model.Chart.View.RemoveFromView(this);
            Model.Chart.View.RemoveFromDrawMargin(_rectangle);
            Model.Chart.View.RemoveFromDrawMargin(_label);
        }

        public AxisSectionCore AsCoreElement(AxisCore axis, AxisOrientation source)
        {
            var model = new AxisSectionCore(this, axis.Chart);
            model.View.Model = model;
            return model;
        }

        private static void CallChartUpdater(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var section = (AxisSection) dependencyObject;

            if (section.Model != null && section.Model.Chart != null) section.Model.Chart.Updater.Run();
        }
    }
}
