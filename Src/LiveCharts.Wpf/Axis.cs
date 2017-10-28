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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Events;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Converters;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// An Axis of a chart
    /// </summary>
    public class Axis : FrameworkElement, IAxisView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of Axis class
        /// </summary>
        public Axis()
        {
            TitleBlock = BindATextBlock();
            SetCurrentValue(SeparatorProperty, new Separator());
            SetCurrentValue(ShowLabelsProperty, true);
            SetCurrentValue(SectionsProperty, new SectionsCollection());
            SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(170, 170, 170)));

            TitleBlock.SetBinding(TextBlock.TextProperty,
                new Binding {Path = new PropertyPath(TitleProperty), Source = this});
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when an axis range changes by an user action (zooming or panning)
        /// </summary>
        public event RangeChangedHandler RangeChanged;

        /// <summary>
        /// The range changed command property
        /// </summary>
        public static readonly DependencyProperty RangeChangedCommandProperty = DependencyProperty.Register(
            "RangeChangedCommand", typeof(ICommand), typeof(Axis), new PropertyMetadata(default(ICommand)));
        /// <summary>
        /// Gets or sets the command to execute when an axis range changes by an user action (zooming or panning)
        /// </summary>
        /// <value>
        /// The range changed command.
        /// </value>
        public ICommand RangeChangedCommand
        {
            get { return (ICommand) GetValue(RangeChangedCommandProperty); }
            set { SetValue(RangeChangedCommandProperty, value); }
        }

        /// <summary>
        /// Occurs before an axis range changes by an user action (zooming or panning)
        /// </summary>
        public event PreviewRangeChangedHandler PreviewRangeChanged;

        /// <summary>
        /// The preview range changed command property
        /// </summary>
        public static readonly DependencyProperty PreviewRangeChangedCommandProperty = DependencyProperty.Register(
            "PreviewRangeChangedCommand", typeof(ICommand), typeof(Axis), new PropertyMetadata(default(ICommand)));
        /// <summary>
        /// Gets or sets the command to execute before an axis range changes by an user action (zooming or panning)
        /// </summary>
        /// <value>
        /// The preview range changed command.
        /// </value>
        public ICommand PreviewRangeChangedCommand
        {
            get { return (ICommand) GetValue(PreviewRangeChangedCommandProperty); }
            set { SetValue(PreviewRangeChangedCommandProperty, value); }
        }

        #endregion

        #region properties
        private TextBlock TitleBlock { get; set; }
        private FormattedText FormattedTitle { get; set; }
        /// <summary>
        /// Gets the Model of the axis, the model is used a DTO to communicate with the core of the library.
        /// </summary>
        public AxisCore Model { get; set; }
        /// <summary>
        /// Gets previous Min Value
        /// </summary>
        public double PreviousMinValue { get; internal set; }
        /// <summary>
        /// Gets previous Max Value
        /// </summary>
        public double PreviousMaxValue { get; internal set; }
        #endregion

        #region Dependency Properties

        /// <summary>
        /// The labels property
        /// </summary>
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof (IList<string>), typeof (Axis), 
            new PropertyMetadata(default(IList<string>), UpdateChart()));
        /// <summary>
        /// Gets or sets axis labels, labels property stores the array to map for each index and value, for example if axis value is 0 then label will be labels[0], when value 1 then labels[1], value 2 then labels[2], ..., value n labels[n], use this property instead of a formatter when there is no conversion between value and label for example names, if you are plotting sales vs salesman name.
        /// </summary>
        [TypeConverter(typeof(StringCollectionConverter))]
        public IList<string> Labels
        {
            get { return ThreadAccess.Resolve<IList<string>>(this, LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }

        /// <summary>
        /// The sections property
        /// </summary>
        public static readonly DependencyProperty SectionsProperty = DependencyProperty.Register(
            "Sections", typeof (SectionsCollection), typeof (Axis), new PropertyMetadata(default(SectionsCollection)));
        /// <summary>
        /// Gets or sets the axis sectionsCollection, a section is useful to highlight ranges or values in a chart.
        /// </summary>
        public SectionsCollection Sections
        {
            get { return (SectionsCollection) GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        /// <summary>
        /// The label formatter property
        /// </summary>
        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof (Func<double, string>), typeof (Axis),
            new PropertyMetadata(default(Func<double, string>), UpdateChart()));
        /// <summary>
        /// Gets or sets the function to convert a value to label, for example when you need to display your chart as currency ($1.00) or as degrees (10°), if Labels property is not null then formatter is ignored, and label will be pulled from Labels prop.
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>) GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        /// <summary>
        /// The separator property
        /// </summary>
        public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register(
            "Separator", typeof (Separator), typeof (Axis),
            new PropertyMetadata(default(Separator), UpdateChart()));
        /// <summary>
        /// Get or sets configuration for parallel lines to axis.
        /// </summary>
        public Separator Separator
        {
            get { return (Separator) GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }
        /// <summary>
        /// The show labels property
        /// </summary>
        public static readonly DependencyProperty ShowLabelsProperty = DependencyProperty.Register(
            "ShowLabels", typeof (bool), typeof (Axis), 
            new PropertyMetadata(default(bool), LabelsVisibilityChanged));
        /// <summary>
        /// Gets or sets if labels are shown in the axis.
        /// </summary>
        public bool ShowLabels
        {
            get { return (bool) GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); }
        }

        /// <summary>
        /// The maximum value property
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof (double), typeof (Axis), 
            new PropertyMetadata(double.NaN, UpdateChart()));
        /// <summary>
        /// Gets or sets axis max value, set it to double.NaN to make this property Auto, default value is double.NaN
        /// </summary>
        public double MaxValue
        {
            get { return (double) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// The minimum value property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof (double), typeof (Axis),
            new PropertyMetadata(double.NaN, UpdateChart()));
        /// <summary>
        /// Gets or sets axis min value, set it to double.NaN to make this property Auto, default value is double.NaN
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Gets the actual minimum value.
        /// </summary>
        /// <value>
        /// The actual minimum value.
        /// </value>
        public double ActualMinValue
        {
            get { return Model.BotLimit; }
        }

        /// <summary>
        /// Gets the actual maximum value.
        /// </summary>
        /// <value>
        /// The actual maximum value.
        /// </value>
        public double ActualMaxValue
        {
            get { return Model.TopLimit; }
        }

        /// <summary>
        /// The maximum range property
        /// </summary>
        public static readonly DependencyProperty MaxRangeProperty = DependencyProperty.Register(
            "MaxRange", typeof(double), typeof(Axis), new PropertyMetadata(double.MaxValue));
        /// <summary>
        /// Gets or sets the max range this axis can display, useful to limit user zooming.
        /// </summary>
        public double MaxRange
        {
            get { return (double) GetValue(MaxRangeProperty); }
            set { SetValue(MaxRangeProperty, value); }
        }

        /// <summary>
        /// The minimum range property
        /// </summary>
        public static readonly DependencyProperty MinRangeProperty = DependencyProperty.Register(
            "MinRange", typeof(double), typeof(Axis), new PropertyMetadata(double.MinValue));
        /// <summary>
        /// Gets or sets the min range this axis can display, useful to limit user zooming.
        /// </summary>
        public double MinRange
        {
            get { return (double) GetValue(MinRangeProperty); }
            set { SetValue(MinRangeProperty, value); }
        }

        /// <summary>
        /// The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Axis), 
            new PropertyMetadata(null, UpdateChart()));
        /// <summary>
        /// Gets or sets axis title, the title will be displayed only if this property is not null, default is null.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// The position property
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof (AxisPosition), typeof (Axis), 
            new PropertyMetadata(default(AxisPosition), UpdateChart()));
        /// <summary>
        /// Gets or sets the axis position, default is Axis.Position.LeftBottom, when the axis is at Y and Position is LeftBottom, then axis will be placed at left, RightTop position will place it at Right, when the axis is at X and position LeftBottom, the axis will be placed at bottom, if position is RightTop then it will be placed at top.
        /// </summary>
        public AxisPosition Position
        {
            get { return (AxisPosition) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// The is merged property
        /// </summary>
        public static readonly DependencyProperty IsMergedProperty = DependencyProperty.Register(
            "IsMerged", typeof (bool), typeof (Axis), 
            new PropertyMetadata(default(bool), UpdateChart()));
        /// <summary>
        /// Gets or sets if the axis labels should me placed inside the chart, this is useful to save some space.
        /// </summary>
        public bool IsMerged
        {
            get { return (bool) GetValue(IsMergedProperty); }
            set { SetValue(IsMergedProperty, value); }
        }

        /// <summary>
        /// The disable animations property
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof (bool), typeof (Axis), new PropertyMetadata(default(bool), UpdateChart(true)));
        /// <summary>
        /// Gets or sets if the axis is animated.
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        /// <summary>
        /// The font family property
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(Axis),
                new PropertyMetadata(new FontFamily("Calibri")));

        /// <summary>
        /// Gets or sets labels font family, font to use for any label in this axis
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(Axis), new PropertyMetadata(11.0));
        /// <summary>
        /// Gets or sets labels font size
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// The font weight property
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(Axis),
                new PropertyMetadata(FontWeights.Normal));
        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        /// <summary>
        /// The font style property
        /// </summary>
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(Axis),
                new PropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// The font stretch property
        /// </summary>
        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(Axis),
                new PropertyMetadata(FontStretches.Normal));

        /// <summary>
        /// Gets or sets labels font stretch
        /// </summary>
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        /// <summary>
        /// The foreground property
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Axis),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// The labels rotation property
        /// </summary>
        public static readonly DependencyProperty LabelsRotationProperty = DependencyProperty.Register(
            "LabelsRotation", typeof (double), typeof (Axis), new PropertyMetadata(default(double), UpdateChart()));
        /// <summary>
        /// Gets or sets the labels rotation in the axis, the angle starts as a horizontal line, you can use any angle in degrees, even negatives.
        /// </summary>
        public double LabelsRotation
        {
            get { return (double) GetValue(LabelsRotationProperty); }
            set { SetValue(LabelsRotationProperty, value); }
        }

        /// <summary>
        /// The bar unit property
        /// </summary>
        public static readonly DependencyProperty BarUnitProperty = DependencyProperty.Register(
            "BarUnit", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN));
        /// <summary>
        /// Gets or sets the bar's series unit width (rows and columns), this property specifies the value in the chart that any bar should take as width.
        /// </summary>
        [Obsolete("PThis property was renamed, please use Unit property instead.")]
        public double BarUnit
        {
            get { return (double) GetValue(BarUnitProperty); }
            set { SetValue(BarUnitProperty, value); }
        }

        /// <summary>
        /// The unit property
        /// </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(
            "Unit", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN));
        /// <summary>
        /// Gets or sets the axis unit, setting this property to your actual scale unit (seconds, minutes or any other scale) helps you to fix possible visual issues.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public double Unit
        {
            get { return (double) GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        /// <summary>
        /// The axis orientation property
        /// </summary>
        public static readonly DependencyProperty AxisOrientationProperty = DependencyProperty.Register(
            "AxisOrientation", typeof(AxisOrientation), typeof(Axis), new PropertyMetadata(default(AxisOrientation)));
        /// <summary>
        /// Gets or sets the element orientation ind the axis
        /// </summary>
        public AxisOrientation AxisOrientation
        {
            get { return (AxisOrientation) GetValue(AxisOrientationProperty); }
            internal set { SetValue(AxisOrientationProperty, value); }
        }

        #endregion

        #region Public Methods
        ///<summary>
        /// Cleans this instance.
        /// </summary>
        public void Clean()
        {
            if (Model == null) return;
            Model.ClearSeparators();
            Model.Chart.View.RemoveFromView(TitleBlock);
            Sections.Clear();
            TitleBlock = null;
        }

        /// <summary>
        /// Renders the separator.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="chart">The chart.</param>
        public virtual void RenderSeparator(SeparatorElementCore model, ChartCore chart)
        {
            AxisSeparatorElement ase;

            if (model.View == null)
            {
                ase = new AxisSeparatorElement(model)
                {
                    Line = BindALine(),
                    TextBlock = BindATextBlock()
                };

                model.View = ase;
                chart.View.AddToView(ase.Line);
                chart.View.AddToView(ase.TextBlock);
                Panel.SetZIndex(ase.Line, -1);
            }
            else
            {
                ase = (AxisSeparatorElement) model.View;
            }

            ase.Line.Visibility = !Separator.IsEnabled ? Visibility.Collapsed : Visibility.Visible;
            ase.TextBlock.Visibility = !ShowLabels ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Updates the title.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="rotationAngle">The rotation angle.</param>
        /// <returns></returns>
        public CoreSize UpdateTitle(ChartCore chart, double rotationAngle = 0)
        {
            if (TitleBlock.Parent == null)
            {
                if (Math.Abs(rotationAngle) > 1)
                    TitleBlock.RenderTransform = new RotateTransform(rotationAngle);

                chart.View.AddToView(TitleBlock);
            }

            FormattedTitle = new FormattedText(
                 TitleBlock.Text,
                 CultureInfo.CurrentUICulture,
                 FlowDirection.LeftToRight,
                 new Typeface(TitleBlock.FontFamily, TitleBlock.FontStyle, TitleBlock.FontWeight, TitleBlock.FontStretch),
                 TitleBlock.FontSize, Brushes.Black);

            return string.IsNullOrWhiteSpace(Title)
                ? new CoreSize()
                : new CoreSize(TitleBlock.Width, FormattedTitle.Height);
        }

        /// <summary>
        /// Sets the title top.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetTitleTop(double value)
        {
            Canvas.SetTop(TitleBlock, value);
        }

        /// <summary>
        /// Sets the title left.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetTitleLeft(double value)
        {
            Canvas.SetLeft(TitleBlock, value);
        }

        /// <summary>
        /// Gets the title left.
        /// </summary>
        /// <returns></returns>
        public double GetTitleLeft()
        {
            return Canvas.GetLeft(TitleBlock);
        }

        /// <summary>
        /// Gets the tile top.
        /// </summary>
        /// <returns></returns>
        public double GetTileTop()
        {
            return Canvas.GetTop(TitleBlock);
        }

        /// <summary>
        /// Gets the size of the label.
        /// </summary>
        /// <returns></returns>
        public CoreSize GetLabelSize()
        {
            return new CoreSize(FormattedTitle.Width, FormattedTitle.Height);
        }

        /// <summary>
        /// Ases the core element.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual AxisCore AsCoreElement(ChartCore chart, AxisOrientation source)
        {
            if (Model == null) Model = new AxisCore(this);

            Model.ShowLabels = ShowLabels;
            Model.Chart = chart;
            Model.IsMerged = IsMerged;
            Model.Labels = Labels;
            Model.LabelFormatter = LabelFormatter;
            Model.MaxValue = MaxValue;
            Model.MinValue = MinValue;
            Model.Title = Title;
            Model.Position = Position;
            Model.Separator = Separator.AsCoreElement(Model, source);
            Model.DisableAnimations = DisableAnimations;
            Model.Sections = Sections.Select(x => x.AsCoreElement(Model, source)).ToList();

            return Model;
        }

        /// <summary>
        /// Sets the range.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void SetRange(double min, double max)
        {
            var bMax = double.IsNaN(MaxValue) ? Model.TopLimit : MaxValue;
            var bMin = double.IsNaN(MinValue) ? Model.BotLimit : MinValue;

            var nMax = double.IsNaN(MaxValue) ? Model.TopLimit : MaxValue;
            var nMin = double.IsNaN(MinValue) ? Model.BotLimit : MinValue;

            var e = new RangeChangedEventArgs
            {
                Range = nMax - nMin,
                RightLimitChange = bMax - nMax,
                LeftLimitChange = bMin - nMin,
                Axis = this
            };

            var pe = new PreviewRangeChangedEventArgs(e)
            {
                PreviewMaxValue = max,
                PreviewMinValue = min
            };
            OnPreviewRangeChanged(pe);

            if (pe.Cancel) return;

            MaxValue = max;
            MinValue = min;

            Model.Chart.Updater.Run();

            OnRangeChanged(e);
        }

        #endregion

        internal TextBlock BindATextBlock()
        {
            var tb = new TextBlock();

            tb.SetBinding(TextBlock.FontFamilyProperty,
                new Binding { Path = new PropertyPath(FontFamilyProperty), Source = this });
            tb.SetBinding(TextBlock.FontSizeProperty,
                new Binding { Path = new PropertyPath(FontSizeProperty), Source = this });
            tb.SetBinding(TextBlock.FontStretchProperty,
                new Binding { Path = new PropertyPath(FontStretchProperty), Source = this });
            tb.SetBinding(TextBlock.FontStyleProperty,
                new Binding { Path = new PropertyPath(FontStyleProperty), Source = this });
            tb.SetBinding(TextBlock.FontWeightProperty,
                new Binding { Path = new PropertyPath(FontWeightProperty), Source = this });
            tb.SetBinding(TextBlock.ForegroundProperty,
                 new Binding { Path = new PropertyPath(ForegroundProperty), Source = this });

            return tb;
        }

        internal Line BindALine()
        {
            var l = new Line();

            var s = Separator as Separator;
            if (s == null) return l;

            l.SetBinding(Shape.StrokeProperty,
                new Binding {Path = new PropertyPath(Wpf.Separator.StrokeProperty), Source = s});
            l.SetBinding(Shape.StrokeDashArrayProperty,
                new Binding {Path = new PropertyPath(Wpf.Separator.StrokeDashArrayProperty), Source = s});
            l.SetBinding(Shape.StrokeThicknessProperty,
                new Binding {Path = new PropertyPath(Wpf.Separator.StrokeThicknessProperty), Source = s});
            l.SetBinding(VisibilityProperty,
                new Binding {Path = new PropertyPath(VisibilityProperty), Source = s});

            return l;
        }

        /// <summary>
        /// Updates the chart.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <param name="updateNow">if set to <c>true</c> [update now].</param>
        /// <returns></returns>
        protected static PropertyChangedCallback UpdateChart(bool animate = false, bool updateNow = false)
        {
            return (o, args) =>
            {
                var wpfAxis = o as Axis;
                if (wpfAxis == null) return;

                if (wpfAxis.Model != null)
                    wpfAxis.Model.Chart.Updater.Run(animate, updateNow);
            };
        }

        private static void LabelsVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var axis = (Axis) dependencyObject;
            if (axis.Model == null) return;
            
            foreach (var separator in axis.Model.CurrentSeparators)
            {
                var s = (AxisSeparatorElement) separator.View;
                s.TextBlock.Visibility = axis.ShowLabels
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            UpdateChart()(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        /// <summary>
        /// Raises the <see cref="E:RangeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RangeChangedEventArgs"/> instance containing the event data.</param>
        protected void OnRangeChanged(RangeChangedEventArgs e)
        {
            if (RangeChanged != null)
                RangeChanged.Invoke(e);
            if (RangeChangedCommand != null && RangeChangedCommand.CanExecute(e))
                RangeChangedCommand.Execute(e);
        }

        /// <summary>
        /// Raises the <see cref="E:PreviewRangeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PreviewRangeChangedEventArgs"/> instance containing the event data.</param>
        protected void OnPreviewRangeChanged(PreviewRangeChangedEventArgs e)
        {
            if (PreviewRangeChanged != null)
                PreviewRangeChanged.Invoke(e);
            if (PreviewRangeChangedCommand != null && PreviewRangeChangedCommand.CanExecute(e))
                PreviewRangeChangedCommand.Execute(e);
        }
    }
}