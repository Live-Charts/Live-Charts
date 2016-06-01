using System;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Brushes = System.Windows.Media.Brushes;

namespace Winforms.Cartesian.DateTime
{
    public class DateModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    public partial class DateTimeExample : Form
    {
        public DateTimeExample()
        {
            InitializeComponent();

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);

            //Notice you can also configure this type globally, so you don't need to configure every
            //SeriesCollection instance using the type.
            //more info at http://lvcharts.net/App/Index#/examples/v1/wpf/Types%20and%20Configuration

            cartesianChart1.Series = new SeriesCollection(dayConfig)
            {
                new LineSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 5
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 9
                        }
                    },
                    Fill = Brushes.Transparent
                },
                new ColumnSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 4
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(1),
                            Value = 6
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 8
                        }
                    }
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = value => new System.DateTime((long) (value*TimeSpan.FromHours(1).Ticks)).ToString("t")
            });
        }
    }
}
