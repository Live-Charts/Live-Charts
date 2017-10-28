using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.UIElements
{
    public class UiElementsViewModel
    {
        private DateTimePoint _selectedPoint;

        public UiElementsViewModel()
        {
            Values = DataProvider.Points.AsChartValues();
            Formatter = x => new DateTime((long) x).ToString("dd MMM");
            Step = TimeSpan.FromDays(1).Ticks * 2;

            //lets get some random points to add an even in the chart.
            var e1 = Values.Skip(15).Take(1).First();
            var e2 = Values.Skip(35).Take(1).First();

            SelectedVisualElement = new VisualElement
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                UIElement = new SelectedItemControl()
            };

            Visuals = new VisualElementsCollection
            {
                new VisualElement
                {
                    X = e1.DateTime.Ticks,
                    Y = e1.Value,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    UIElement = new EventAControl()
                },
                new VisualElement
                {
                    X = e2.DateTime.Ticks,
                    Y = e2.Value,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    UIElement = new EventBControl()
                },
                SelectedVisualElement
            };
        }

        public ChartValues<DateTimePoint> Values { get; set; }
        public VisualElementsCollection Visuals { get; set; }
        public VisualElement EventA { get; set; }
        public VisualElement EventB { get; set; }
        public VisualElement SelectedVisualElement { get; set; }
        public Func<double, string> Formatter { get; set; }
        public double Step { get; set; }
        public DateTimePoint SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                UpdateSelectedVisual();
            }
        }

        private void UpdateSelectedVisual()
        {
            SelectedVisualElement.X = SelectedPoint.DateTime.Ticks;
            SelectedVisualElement.Y = SelectedPoint.Value;
        }
    }
}
