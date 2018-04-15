using System;
using System.Collections.ObjectModel;
using System.Drawing;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace Assets.ViewModels
{
    public class BasicHeat
    {
        public BasicHeat()
        {
            var r = new Random();

            var values = new ObservableCollection<WeightedModel>
            {
                // we will initialize the collection with the next set of point

                //X means sales man
                //Y is the day

                //"Jeremy Swanson"
                new WeightedModel(0, 0, r.Next(0, 10)),
                new WeightedModel(0, 1, r.Next(0, 10)),
                new WeightedModel(0, 2, r.Next(0, 10)),
                new WeightedModel(0, 3, r.Next(0, 10)),
                new WeightedModel(0, 4, r.Next(0, 10)),
                new WeightedModel(0, 5, r.Next(0, 10)),
                new WeightedModel(0, 6, r.Next(0, 10)),

                //"Lorena Hoffman"
                new WeightedModel(1, 0, r.Next(0, 10)),
                new WeightedModel(1, 1, r.Next(0, 10)),
                new WeightedModel(1, 2, r.Next(0, 10)),
                new WeightedModel(1, 3, r.Next(0, 10)),
                new WeightedModel(1, 4, r.Next(0, 10)),
                new WeightedModel(1, 5, r.Next(0, 10)),
                new WeightedModel(1, 6, r.Next(0, 10)),

                //"Robyn Williamson"
                new WeightedModel(2, 0, r.Next(0, 10)),
                new WeightedModel(2, 1, r.Next(0, 10)),
                new WeightedModel(2, 2, r.Next(0, 10)),
                new WeightedModel(2, 3, r.Next(0, 10)),
                new WeightedModel(2, 4, r.Next(0, 10)),
                new WeightedModel(2, 5, r.Next(0, 10)),
                new WeightedModel(2, 6, r.Next(0, 10)),

                //"Carole Haynes"
                new WeightedModel(3, 0, r.Next(0, 10)),
                new WeightedModel(3, 1, r.Next(0, 10)),
                new WeightedModel(3, 2, r.Next(0, 10)),
                new WeightedModel(3, 3, r.Next(0, 10)),
                new WeightedModel(3, 4, r.Next(0, 10)),
                new WeightedModel(3, 5, r.Next(0, 10)),
                new WeightedModel(3, 6, r.Next(0, 10)),

                //"Essie Nelson"
                new WeightedModel(4, 0, r.Next(0, 10)),
                new WeightedModel(4, 1, r.Next(0, 10)),
                new WeightedModel(4, 2, r.Next(0, 10)),
                new WeightedModel(4, 3, r.Next(0, 10)),
                new WeightedModel(4, 4, r.Next(0, 10)),
                new WeightedModel(4, 5, r.Next(0, 10)),
                new WeightedModel(4, 6, r.Next(0, 10))
            };

            // we will bind in XAML SeriesCollection to CartesianChart.YAxis property
            YAxis = new ChartingCollection<Plane>
            {
                new Axis
                {
                    Labels = new[]
                    {
                        "Monday",
                        "Tuesday",
                        "Wednesday",
                        "Thursday",
                        "Friday",
                        "Saturday",
                        "Sunday"
                    }
                }
            };

            // we will bind in XAML SeriesCollection to CartesianChart.XAxis property
            XAxis = new ChartingCollection<Plane>
            {
                new Axis
                {
                    Labels = new[]
                    {
                        "Jeremy Swanson",
                        "Lorena Hoffman",
                        "Robyn Williamson",
                        "Carole Haynes",
                        "Essie Nelson"
                    }
                }
            }; 

            // we will bind in XAML SeriesCollection to CartesianChart.Series property
            SeriesCollection = new ChartingCollection<ISeries>();
            SeriesCollection.Add(new HeatSeries<WeightedModel>
            {
                Values = values,
                Gradient = new[]
                {
                    new GradientStop
                    {
                        Color = Color.PaleVioletRed,
                        Offset = 0
                    },
                    new GradientStop
                    {
                        Color = Color.MediumVioletRed,
                        Offset = .3
                    },
                    new GradientStop
                    {
                        Color = Color.DarkRed,
                        Offset = 1
                    }
                }
            });
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
        public ChartingCollection<Plane> XAxis { get; set; }
        public ChartingCollection<Plane> YAxis { get; set; }
    }
}