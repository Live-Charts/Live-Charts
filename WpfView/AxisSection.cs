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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Base;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// An Axis section highlights values or ranges in a chart.
    /// </summary>
    public class AxisSection : FrameworkElement, IAxisSectionView
    {
        private readonly Rectangle _rectangle;
        private TextBlock _label;
        internal static AxisSection Dragging;

        /// <summary>
        /// Initializes a new instance of AxisSection class
        /// </summary>
        public AxisSection()
        {
            _rectangle = new Rectangle();
            
            _rectangle.MouseDown += (sender, args) =>
            {
                if (!Draggable) return;
                Dragging = this;
                args.Handled = true;
                Chart.Ldsp = null;
            };

            SetCurrentValue(StrokeProperty, new SolidColorBrush(Color.FromRgb(131, 172, 191)));
            SetCurrentValue(FillProperty, new SolidColorBrush(Color.FromRgb(131, 172, 191)) {Opacity = .35});
            SetCurrentValue(StrokeThicknessProperty, 0d);
        }

        #region Properties
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
            "Label", typeof(string), typeof(AxisSection), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the name, the title of the section, a visual element will be added to the chart if this property is not null.
        /// </summary>
        [Obsolete("Use a VisualElement instead")]
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// From value property
        /// </summary>
        public static readonly DependencyProperty FromValueProperty = DependencyProperty.Register(
            "FromValue", typeof(double), typeof(AxisSection),
            new PropertyMetadata(double.NaN, UpdateSection));
        /// <summary>
        /// Gets or sets the value where the section starts
        /// </summary>
        [Obsolete("This property will be removed in future versions, instead use Value and SectionWidth properties")]
        public double FromValue
        {
            get { return (double)GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        /// <summary>
        /// To value property
        /// </summary>
        public static readonly DependencyProperty ToValueProperty = DependencyProperty.Register(
            "ToValue", typeof(double), typeof(AxisSection),
            new PropertyMetadata(double.NaN, UpdateSection));
        /// <summary>
        /// Gets or sets the value where the section ends
        /// </summary>
        [Obsolete("This property will be removed in future versions, instead use Value and SectionWidth properties")]
        public double ToValue
        {
            get { return (double)GetValue(ToValueProperty); }
            set { SetValue(ToValueProperty, value); }
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(AxisSection), new PropertyMetadata(default(double), UpdateSection));
        /// <summary>
        /// Gets or sets the value where the section is drawn
        /// </summary>
        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// The section width property
        /// </summary>
        public static readonly DependencyProperty SectionWidthProperty = DependencyProperty.Register(
            "SectionWidth", typeof(double), typeof(AxisSection), new PropertyMetadata(default(double), UpdateSection));
        /// <summary>
        /// Gets or sets the section width
        /// </summary>
        public double SectionWidth
        {
            get { return (double) GetValue(SectionWidthProperty); }
            set { SetValue(SectionWidthProperty, value); }
        }

        /// <summary>
        /// The section offset property
        /// </summary>
        public static readonly DependencyProperty SectionOffsetProperty = DependencyProperty.Register(
            "SectionOffset", typeof(double), typeof(AxisSection), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the section offset.
        /// </summary>
        /// <value>
        /// The section offset.
        /// </value>
        public double SectionOffset
        {
            get { return (double) GetValue(SectionOffsetProperty); }
            set { SetValue(SectionOffsetProperty, value); }
        }

        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(AxisSection), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets o sets the section stroke, the stroke brush will be used to draw the border of the section
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(AxisSection), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the section fill brush.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(AxisSection), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// The stroke dash array property
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof(DoubleCollection), typeof(AxisSection), new PropertyMetadata(default(DoubleCollection)));
        /// <summary>
        /// Gets or sets the stroke dash array collection, use this property to create dashed stroke sections
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
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
        /// The disable animations property
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof(bool), typeof(AxisSection), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets a value indicating whether the section is animated
        /// </summary>
        /// <value>
        /// <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        /// <summary>
        /// The data label property
        /// </summary>
        public static readonly DependencyProperty DataLabelProperty = DependencyProperty.Register(
            "DataLabel", typeof(bool), typeof(AxisSection), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets a value indicating whether the section should display a label that displays its current value.
        /// </summary>
        /// <value>
        /// <c>true</c> if [data label]; otherwise, <c>false</c>.
        /// </value>
        public bool DataLabel
        {
            get { return (bool) GetValue(DataLabelProperty); }
            set { SetValue(DataLabelProperty, value); }
        }

        /// <summary>
        /// The data label brush property
        /// </summary>
        public static readonly DependencyProperty DataLabelForegroundProperty = DependencyProperty.Register(
            "DataLabelForeground", typeof(Brush), typeof(AxisSection), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the data label brush.
        /// </summary>
        /// <value>
        /// The label brush.
        /// </value>
        public Brush DataLabelForeground
        {
            get { return (Brush) GetValue(DataLabelForegroundProperty); }
            set { SetValue(DataLabelForegroundProperty, value); }
        }
        #endregion

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
            Panel.SetZIndex(_rectangle, Panel.GetZIndex(this));
            BindingOperations.SetBinding(_rectangle, VisibilityProperty,
                new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});

            var ax = source == AxisOrientation.X ? Model.Chart.AxisX[axis] : Model.Chart.AxisY[axis];
            var uw = ax.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(source, Model.Chart, axis) / 2 : 0;

            if (Parent == null)
            {
                _label = ((Axis) ax.View).BindATextBlock();
                _label.Padding = new Thickness(5, 2, 5, 2);
                Model.Chart.View.AddToView(this);
                Model.Chart.View.AddToDrawMargin(_rectangle);
                Model.Chart.View.AddToView(_label);
                _rectangle.Height = 0;
                _rectangle.Width = 0;
                Canvas.SetLeft(_rectangle, 0d);
                Canvas.SetTop(_rectangle, Model.Chart.DrawMargin.Height);
                #region Obsolete
                Canvas.SetTop(_label, Model.Chart.DrawMargin.Height);
                Canvas.SetLeft(_label, 0d);
                #endregion
            }

            #pragma warning disable 618
            var from = ChartFunctions.ToDrawMargin(double.IsNaN(FromValue) ? Value + SectionOffset : FromValue, source, Model.Chart, axis) + uw;
#pragma warning restore 618
#pragma warning disable 618
            var to = ChartFunctions.ToDrawMargin(double.IsNaN(ToValue) ? Value  + SectionOffset + SectionWidth : ToValue, source, Model.Chart, axis) + uw;
#pragma warning restore 618

            if (from > to)
            {
                var temp = to;
                to = from;
                from = temp;
            }

            var anSpeed = Model.Chart.View.AnimationsSpeed;

            if (DataLabel)
            {
                if (DataLabelForeground != null) _label.Foreground = DataLabelForeground;
                _label.UpdateLayout();
                _label.Background = Stroke ?? Fill;
                PlaceLabel(ax.GetFormatter()(Value), ax, source);
            }

            if (source == AxisOrientation.X)
            {
                var w = to - from;
                w = StrokeThickness > w ? StrokeThickness : w;

                Canvas.SetTop(_rectangle, 0);
                _rectangle.Height = Model.Chart.DrawMargin.Height;

                if (Model.Chart.View.DisableAnimations || DisableAnimations)
                {
                    _rectangle.Width = w > 0 ? w : 0;
                    Canvas.SetLeft(_rectangle, from - StrokeThickness/2);
                }
                else
                {
                    _rectangle.BeginAnimation(WidthProperty, new DoubleAnimation(w > 0 ? w : 0, anSpeed));
                    _rectangle.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(from - StrokeThickness / 2, anSpeed));
                }
                return;
            }

            var h = to - from;
            h = StrokeThickness > h ? StrokeThickness : h;

            Canvas.SetLeft(_rectangle, 0d);
            _rectangle.Width = Model.Chart.DrawMargin.Width;

            if (Model.Chart.View.DisableAnimations || DisableAnimations)
            {
                Canvas.SetTop(_rectangle, from - StrokeThickness/2);
                _rectangle.Height = h > 0 ? h : 0;
            }
            else
            {
                _rectangle.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(from, anSpeed));
                _rectangle.BeginAnimation(HeightProperty, new DoubleAnimation(h, anSpeed));
            }
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            Model.Chart.View.RemoveFromDrawMargin(_label);
            Model.Chart.View.RemoveFromView(_label);
            Model.Chart.View.RemoveFromDrawMargin(_rectangle);
            Model.Chart.View.RemoveFromView(this);
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

        private static void UpdateSection(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var section = (AxisSection) dependencyObject;

            if (section.Model != null && section.Model.Chart != null)
            {
                if (!section.Model.Chart.AreComponentsLoaded) return;
                section.DrawOrMove(section.Model.Source, section.Model.AxisIndex);
            }
        }

        private void PlaceLabel(string text, AxisCore axis, AxisOrientation source)
        {
            _label.Text = text;

            var formattedText = new FormattedText(
                _label.Text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(_label.FontFamily, _label.FontStyle, _label.FontWeight, _label.FontStretch),
                _label.FontSize, Brushes.Black);

            var transform = new LabelEvaluation(axis.View.LabelsRotation,
                formattedText.Width + 10, formattedText.Height, axis, source);

            _label.RenderTransform = Math.Abs(transform.LabelAngle) > 1
                ? new RotateTransform(transform.LabelAngle)
                : null;

            var toLine = ChartFunctions.ToPlotArea(Value + SectionOffset + SectionWidth * .5, source, Model.Chart,
                axis);

            var direction = source == AxisOrientation.X ? 1 : -1;

            toLine += axis.EvaluatesUnitWidth ? direction * ChartFunctions.GetUnitWidth(source, Model.Chart, axis) / 2 : 0;
            var toLabel = toLine + transform.GetOffsetBySource(source);

            var chart = Model.Chart;

            if (axis.IsMerged)
            {
                const double padding = 4;

                if (source == AxisOrientation.Y)
                {
                    if (toLabel + transform.ActualHeight >
                        chart.DrawMargin.Top + chart.DrawMargin.Height)
                        toLabel -= transform.ActualHeight + padding;
                }
                else
                {
                    if (toLabel + transform.ActualWidth >
                        chart.DrawMargin.Left + chart.DrawMargin.Width)
                        toLabel -= transform.ActualWidth + padding;
                }
            }

            var labelTab = axis.Tab;
            labelTab += transform.GetOffsetBySource(source.Invert());

            if (source == AxisOrientation.Y)
            {
                labelTab += 8 * (axis.Position == AxisPosition.LeftBottom ? 1 : -1);

                if (Model.View.DisableAnimations || DisableAnimations)
                {
                    Canvas.SetLeft(_label, labelTab);
                    Canvas.SetTop(_label, toLabel);
                    return;
                }

                _label.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(toLabel, chart.View.AnimationsSpeed));
                _label.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(labelTab, chart.View.AnimationsSpeed));
            }
            else
            {
                if (Model.View.DisableAnimations || DisableAnimations)
                {
                    Canvas.SetLeft(_label, toLabel);
                    Canvas.SetTop(_label, labelTab);
                    return;
                }

                _label.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(toLabel, chart.View.AnimationsSpeed));
                _label.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(labelTab, chart.View.AnimationsSpeed));
            }

        }
    }
}
