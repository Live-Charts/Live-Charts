//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.Linq;
using System.Windows;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.WindowAxis" />
    /// <seealso cref="LiveCharts.Definitions.Charts.IDateAxisView" />
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