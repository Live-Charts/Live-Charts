using System;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.HeatSeriesExample
{
    public partial class HeatSeriesExample : Form
    {
        public HeatSeriesExample()
        {
            InitializeComponent();

            var r = new Random();

            cartesianChart1.Series.Add(new HeatSeries
            {
                Values = new ChartValues<HeatPoint>
                {
                    //X means sales man
                    //Y is the day
                    //"Jeremy Swanson"
                    new HeatPoint(0, 0, r.Next(0, 10)),
                    new HeatPoint(0, 1, r.Next(0, 10)),
                    new HeatPoint(0, 2, r.Next(0, 10)),
                    new HeatPoint(0, 3, r.Next(0, 10)),
                    new HeatPoint(0, 4, r.Next(0, 10)),
                    new HeatPoint(0, 5, r.Next(0, 10)),
                    new HeatPoint(0, 6, r.Next(0, 10)),
                    //"Lorena Hoffman"
                    new HeatPoint(1, 0, r.Next(0, 10)),
                    new HeatPoint(1, 1, r.Next(0, 10)),
                    new HeatPoint(1, 2, r.Next(0, 10)),
                    new HeatPoint(1, 3, r.Next(0, 10)),
                    new HeatPoint(1, 4, r.Next(0, 10)),
                    new HeatPoint(1, 5, r.Next(0, 10)),
                    new HeatPoint(1, 6, r.Next(0, 10)),
                    //"Robyn Williamson"
                    new HeatPoint(2, 0, r.Next(0, 10)),
                    new HeatPoint(2, 1, r.Next(0, 10)),
                    new HeatPoint(2, 2, r.Next(0, 10)),
                    new HeatPoint(2, 3, r.Next(0, 10)),
                    new HeatPoint(2, 4, r.Next(0, 10)),
                    new HeatPoint(2, 5, r.Next(0, 10)),
                    new HeatPoint(2, 6, r.Next(0, 10)),
                    //"Carole Haynes"
                    new HeatPoint(3, 0, r.Next(0, 10)),
                    new HeatPoint(3, 1, r.Next(0, 10)),
                    new HeatPoint(3, 2, r.Next(0, 10)),
                    new HeatPoint(3, 3, r.Next(0, 10)),
                    new HeatPoint(3, 4, r.Next(0, 10)),
                    new HeatPoint(3, 5, r.Next(0, 10)),
                    new HeatPoint(3, 6, r.Next(0, 10)),
                    //"Essie Nelson"
                    new HeatPoint(4, 0, r.Next(0, 10)),
                    new HeatPoint(4, 1, r.Next(0, 10)),
                    new HeatPoint(4, 2, r.Next(0, 10)),
                    new HeatPoint(4, 3, r.Next(0, 10)),
                    new HeatPoint(4, 4, r.Next(0, 10)),
                    new HeatPoint(4, 5, r.Next(0, 10)),
                    new HeatPoint(4, 6, r.Next(0, 10))
                },
                DataLabels = true,

                //The GradientStopCollection is optional
                //If you do not set this property, LiveCharts will set a gradient
                GradientStopCollection = new GradientStopCollection
                {
                    new GradientStop(Color.FromRgb(0, 0, 0), 0),
                    new GradientStop(Color.FromRgb(0, 255, 0), .25),
                    new GradientStop(Color.FromRgb(0, 0, 255), .5),
                    new GradientStop(Color.FromRgb(255, 0, 0), .75),
                    new GradientStop(Color.FromRgb(255, 255, 255), 1)
                }
            });

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelsRotation = -15,
                Labels = new[]
                {
                    "Jeremy Swanson",
                    "Lorena Hoffman",
                    "Robyn Williamson",
                    "Carole Haynes",
                    "Essie Nelson"
                },
                Separator = new Separator {Step = 1}
            });

            cartesianChart1.AxisY.Add(new Axis
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
            });

        }
    }
}
