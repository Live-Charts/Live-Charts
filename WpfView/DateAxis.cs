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
        public static readonly DependencyProperty InitialDateTimeProperty = DependencyProperty.Register("InitialDateTime", typeof(DateTime), typeof(DateAxis), new PropertyMetadata(DateTime.UtcNow, UpdateChart()));
        public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register("Period", typeof(PeriodUnits), typeof(DateAxis), new PropertyMetadata(PeriodUnits.Milliseconds, UpdateChart()));

        public DateAxis()
        {
            // Initialize the axis with date windows
            var collection = new AxisWindowCollection();
            collection.AddRange(DateAxisWindows.GetDateAxisWindows());
            SetCurrentValue(WindowsProperty, collection);
        }

        /// <summary>
        /// Gets or sets the Initial Date Time. Together with Period, this is used to calculate date times based opon the value of the axis.
        /// </summary>
        public DateTime InitialDateTime
        {
            get { return (DateTime)GetValue(InitialDateTimeProperty); }
            set { SetValue(InitialDateTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the period of the data. For each index, we increment the Initial dateTime with this period.
        /// </summary>
        public PeriodUnits Period
        {
            get { return (PeriodUnits)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
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

            ((DateAxisCore)Model).Windows = Windows.ToList();
            ((DateAxisCore)Model).Windows.ForEach(w => ((DateAxisWindow)w).DateAxisCore = (DateAxisCore)Model);
            return Model;
        }
    }
}