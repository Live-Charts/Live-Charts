using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Components;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class DateAxis : Axis, IDateAxisView
    {
        public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register("HeaderFontWeight", typeof(FontWeight), typeof(Axis), new PropertyMetadata(FontWeights.ExtraBold));
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(Axis), new PropertyMetadata(null));
        public static readonly DependencyProperty ReferenceDateTimeProperty = DependencyProperty.Register("ReferenceDateTime", typeof(DateTime), typeof(DateAxis), new PropertyMetadata(DateTime.UtcNow, UpdateChart()));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(SeriesResolution), typeof(DateAxis), new PropertyMetadata(SeriesResolution.Day, UpdateChart()));
        public static readonly DependencyProperty SeparatorResolutionProperty = DependencyProperty.Register("SeparatorResolution", typeof(SeparatorResolution), typeof(DateAxis), new PropertyMetadata(SeparatorResolution.Day, UpdateChart()));
                        
        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight HeaderFontWeight
        {
            get { return (FontWeight)GetValue(HeaderFontWeightProperty); }
            set { SetValue(HeaderFontWeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>
        /// The base.
        /// </value>
        public DateTime ReferenceDateTime
        {
            get { return (DateTime)GetValue(ReferenceDateTimeProperty); }
            set { SetValue(ReferenceDateTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>
        /// The resolution.
        /// </value>
        public SeriesResolution Resolution
        {
            get { return (SeriesResolution)GetValue(ResolutionProperty); }
            set { SetValue(ResolutionProperty, value); }
        }

        /// <value>
        /// The resolution.
        /// </value>
        public SeparatorResolution SeparatorResolution
        {
            get { return (SeparatorResolution)GetValue(SeparatorResolutionProperty); }
            set { SetValue(SeparatorResolutionProperty, value); }
        }

        public DateAxis()
        {
            SetCurrentValue(HeaderForegroundProperty, Brushes.CornflowerBlue);
        }

        public override AxisCore AsCoreElement(ChartCore chart, AxisOrientation source)
        {
            if (Model == null) Model = new DateAxisCore(this);
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
                        
        public override void RenderSeparator(SeparatorElementCore model, ChartCore chart)
        {
            base.RenderSeparator(model, chart);

            // Review whetehr hould we not implement this with a trigger instead of resetting property bindings
            var element = (AxisSeparatorElement)model.View;
            if (((DateSeparatorElementCore)model).IsHeader)
            {
                element.TextBlock.SetBinding(TextBlock.FontWeightProperty, new Binding
                {
                    Path = new PropertyPath(HeaderFontWeightProperty),
                    Source = this
                });
                element.TextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding
                {
                    Path = new PropertyPath(HeaderForegroundProperty),
                    Source = this
                });
            }
            else
            {
                element.TextBlock.SetBinding(TextBlock.FontWeightProperty, new Binding
                {
                    Path = new PropertyPath(FontWeightProperty),
                    Source = this
                });
                element.TextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding
                {
                    Path = new PropertyPath(ForegroundProperty),
                    Source = this
                });
            }
        }

        public void SetSeparatorResolution(SeparatorResolution resolution)
        {
            SeparatorResolution = resolution;
        }
    }
}