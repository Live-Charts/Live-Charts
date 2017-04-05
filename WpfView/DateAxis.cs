using System;
using System.Linq;
using System.Windows;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;

namespace LiveCharts.Wpf
{
    public class DateAxis : WindowAxis, IDateAxisView
    {
        public static readonly DependencyProperty ReferenceDateTimeProperty = DependencyProperty.Register("ReferenceDateTime", typeof(DateTime), typeof(DateAxis), new PropertyMetadata(DateTime.UtcNow, UpdateChart()));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(SeriesResolution), typeof(DateAxis), new PropertyMetadata(SeriesResolution.Ticks, UpdateChart()));

        public DateAxis()
        {
            // Initialize the axis with date windows
            var collection = new AxisWindowCollection();
            collection.AddRange(DateAxisWindows.GetDateAxisWindows());
            SetCurrentValue(WindowsProperty, collection);
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

            // TODO: come up with a better way to assing the core to the models
            ((DateAxisCore)Model).Windows = Windows.ToList();
            ((DateAxisCore)Model).Windows.ForEach(w => ((DateAxisWindow)w).DateAxisCore = (DateAxisCore)Model);
            return Model;
        }
    }
}