using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;

namespace Assets.ViewModels
{
    public class SharedAxes
    {
        public SharedAxes()
        {
            SeriesCollection1 = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double>
                    {
                        8,
                        7,
                        3,
                        9,
                        3
                    }
                }
            };
            
            SeriesCollection2 = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double>
                    {
                        800000,
                        700000,
                        300000,
                        900000,
                        300000
                    }
                }
            };

            // we will explicitly build our axes
            // he library normally creates this automatically
            // but now we need a custom behavior, lets override the default...

            // we create an axis instance
            var firstAxis = new Axis();
            YAxis1 = new ObservableCollection<Plane>();
            // and we will use this instance in the first chart Y axis
            YAxis1.Add(firstAxis);

            // now our second instance to use in the second chart.
            var secondAxis = new Axis();
            YAxis2 = new ObservableCollection<Plane>();
            YAxis2.Add(secondAxis);

            // finally we will let both instances know that they depend on each other.
            // so they will always share its size
            firstAxis.SharedAxes.Add(secondAxis);
            secondAxis.SharedAxes.Add(firstAxis);

            XAxis = new ObservableCollection<Plane>
            {
                new Axis
                {
                    Step = 1,
                    XSeparatorStyle = new ShapeStyle(Brushes.Gray, Brushes.Transparent, 2, null)
                }
            };
        }

        public ObservableCollection<ISeries> SeriesCollection1 { get; set; }
        public ObservableCollection<ISeries> SeriesCollection2 { get; set; }
        public ObservableCollection<Plane> XAxis { get; set; }
        public ObservableCollection<Plane> YAxis1 { get; set; }
        public ObservableCollection<Plane> YAxis2 { get; set; }
    }
}
