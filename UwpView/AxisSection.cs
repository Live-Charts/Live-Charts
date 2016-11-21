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
using Windows.UI.Xaml.Shapes;
using LiveCharts.Definitions.Charts;
using LiveCharts.Uwp.Components;

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

            //_rectangle.MouseDown += (sender, args) =>
            //{
            //    if (!Draggable) return;
            //    Dragging = this;
            //    args.Handled = true;
            //    Chart.Ldsp = null;
            //};

            BindingOperations.SetBinding(_label, TextBlock.TextProperty,
                new Binding {Path = new PropertyPath(nameof(Label)), Source = this});
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public AxisSectionCore Model { get; set; }

        /// <summary>
        /// The label property
        /// </summary>
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

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(AxisSection), new PropertyMetadata(0d, UpdateSection));
        /// <summary>
        /// Gets or sets the value where the section is drawn
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// The section width property
        /// </summary>
        public static readonly DependencyProperty SectionWidthProperty = DependencyProperty.Register(
            "SectionWidth", typeof(double), typeof(AxisSection), new PropertyMetadata(0d, UpdateSection));
        /// <summary>
        /// Gets or sets the section width
        /// </summary>
        public double SectionWidth
        {
            get { return (double)GetValue(SectionWidthProperty); }
            set { SetValue(SectionWidthProperty, value); }
        }

        /// <summary>
        /// The draggable property
        /// </summary>
        public static readonly DependencyProperty DraggableProperty = DependencyProperty.Register(
            "Draggable", typeof(bool), typeof(AxisSection), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets if a user can drag the section
        /// </summary>
        public bool Draggable
        {
            get { return (bool) GetValue(DraggableProperty); }
            set { SetValue(DraggableProperty, value); }
        }

        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (AxisSection), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 131, 172, 191))));
        /// <summary>
        /// Gets o sets the section stroke, the stroke brush will be used to draw the border of the section
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof (Brush), typeof (AxisSection), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 131, 172, 191)) { Opacity = .35 }));
        /// <summary>
        /// Gets or sets the section fill brush.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (AxisSection), new PropertyMetadata(0d));
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// The stroke dash array property
        /// </summary>
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

        /// <summary>
        /// Draws the or move.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="axis">The axis.</param>
        public void DrawOrMove(AxisOrientation source, int axis)
        {
            _rectangle.Fill = Fill;
            _rectangle.Stroke = Stroke;
            _rectangle.StrokeDashArray = StrokeDashArray;
            _rectangle.StrokeThickness = StrokeThickness;
            Canvas.SetZIndex(_rectangle, Canvas.GetZIndex(this));

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

            var ax = source == AxisOrientation.X ? Model.Chart.AxisX[axis] : Model.Chart.AxisY[axis];
            var uw = ax.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(source, Model.Chart, axis) / 2 : 0;

            var from = ChartFunctions.ToDrawMargin(Value, source, Model.Chart, axis) + uw;
            var to = ChartFunctions.ToDrawMargin(Value + SectionWidth, source, Model.Chart, axis) + uw;

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
                    Canvas.SetLeft(_rectangle, from - StrokeThickness / 2);
                    Canvas.SetLeft(_label, (from + to)/2 - _label.ActualWidth/2);
                }
                else
                {
                    _rectangle.BeginDoubleAnimation("(Canvas.Left)", from - StrokeThickness/2, anSpeed);
                    _rectangle.BeginDoubleAnimation(nameof(Height), w > 0 ? w: 0, anSpeed);
                    _label.BeginDoubleAnimation("(Canvas.Left)", (from + to) / 2 - _label.ActualWidth / 2, anSpeed);
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
                _rectangle.BeginDoubleAnimation("(Canvas.Top)", from -StrokeThickness/2, anSpeed);
                _rectangle.BeginDoubleAnimation(nameof(Height), h > 0 ? h : 0, anSpeed);
                _label.BeginDoubleAnimation("(Canvas.Top)", (from+to)/2 - _label.ActualHeight/2, anSpeed);
            }
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            Model.Chart.View.RemoveFromView(this);
            Model.Chart.View.RemoveFromDrawMargin(_rectangle);
            Model.Chart.View.RemoveFromDrawMargin(_label);
        }

        /// <summary>
        /// Ases the core element.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public AxisSectionCore AsCoreElement(AxisCore axis, AxisOrientation source)
        {
            var model = new AxisSectionCore(this, axis.Chart);
            model.View.Model = model;
            return model;
        }

        private static void UpdateSection(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var section = (AxisSection) dependencyObject;

            if (section.Model != null && section.Model.Chart != null)
            {
                if (!section.Model.Chart.AreComponentsLoaded) return;
                section.DrawOrMove(section.Model.Source, section.Model.AxisIndex);
            }
        }
    }
}
